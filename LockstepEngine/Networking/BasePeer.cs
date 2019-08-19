using System;
using System.Collections.Generic;
using System.Net;
using Lockstep.Logging;
using Lockstep.Serialization;
using Lockstep.Util;

namespace Lockstep.Networking {
    /// <summary>
    ///     This is an abstract implementation of <see cref="IPeer" /> interface,
    ///     which handles acknowledgements and SendMessage overloads.
    ///     Extend this, if you want to implement custom protocols
    /// </summary>
    public abstract class BasePeer : IPeer {
        public abstract IPEndPoint EndPoint { get; }
        public static bool DontCatchExceptionsInEditor = true;

        private static readonly object _idGenerationLock = new object();
        private static int _peerIdGenerator;

        /// <summary>
        ///     Default timeout, after which response callback is invoked with
        ///     timeout status.
        /// </summary>
        public static int DefaultTimeoutSecs = 60;

        private readonly Dictionary<int, ResponseCallback> _acks;
        protected readonly List<long[]> _ackTimeoutQueue;
        private readonly Dictionary<int, object> _data;
        private int _id = -1;
        private int _nextAckId = 1;
        private IIncommingMessage _timeoutMessage;
        private Dictionary<Type, object> _extensions;

        protected BasePeer(){
            _data = new Dictionary<int, object>();
            _acks = new Dictionary<int, ResponseCallback>(30);
            _ackTimeoutQueue = new List<long[]>();
            _extensions = new Dictionary<Type, object>();
            _id = -1;
            LTimer.OnTickPerSeconds += HandleAckDisposalTick;

            _timeoutMessage = new IncommingMessage(-1, 0, "Time out".ToBytes(), EDeliveryMethod.ReliableUnordered, this) {
                Status = EResponseStatus.Timeout
            };
        }

        public event Action<IIncommingMessage> MessageReceived;
        public event PeerActionHandler Disconnected;

        public event Action<IIncommingMessage> OnMessage {
            add { MessageReceived += value; }
            remove { MessageReceived -= value; }
        }

        public IPeer Peer { get; private set; }

        /// <summary>
        ///     Sends a message to peer
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="eDeliveryMethod">Delivery method</param>
        /// <returns></returns>
        public abstract void SendMessage(IMessage message, EDeliveryMethod eDeliveryMethod);


        ///     Saves data into peer
        public void SetProperty(int id, object data){
            if (_data.ContainsKey(id))
                _data[id] = data;
            else
                _data.Add(id, data);
        }

        ///     Retrieves data from peer, which was stored with <see cref="SetProperty" />
        public object GetProperty(int id){
            object value;

            _data.TryGetValue(id, out value);

            return value;
        }

        ///     Retrieves data from peer, which was stored with <see cref="SetProperty" />
        public object GetProperty(int id, object defaultValue){
            var obj = GetProperty(id);
            return obj ?? defaultValue;
        }

        public T AddExtension<T>(T extension){
            _extensions[typeof(T)] = extension;
            return extension;
        }
        public void CleanExtension(){
            _extensions.Clear();
        }
        public T GetExtension<T>(){
            object extension;
            _extensions.TryGetValue(typeof(T), out extension);
            if (extension == null)
                return default(T);

            return (T) extension;
        }

        public bool HasExtension<T>(){
            return _extensions.ContainsKey(typeof(T));
        }

        public void Dispose(){
            LTimer.OnTickPerSeconds -= HandleAckDisposalTick;
        }


        /// <summary>
        ///     True, if connection is stil valid
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        ///     Force disconnect
        /// </summary>
        /// <param name="reason"></param>
        public abstract void Disconnect(string reason);

        public void NotifyDisconnectEvent(){
            if (Disconnected != null)
                Disconnected(this);
        }

        protected void NotifyMessageEvent(IIncommingMessage message){
            if (MessageReceived != null)
                MessageReceived(message);
        }

        protected int RegisterAck(IMessage message, ResponseCallback responseCallback,
            int timeoutSecs){
            int id;

            lock (_acks) {
                id = _nextAckId++;
                _acks.Add(id, responseCallback);
            }

            message.AckRequestId = id;

            StartAckTimeout(id, timeoutSecs);
            return id;
        }

        protected void TriggerAck(int ackId, EResponseStatus statusCode, IIncommingMessage message){
            lock (_acks) {
                ResponseCallback ackCallback;
                _acks.TryGetValue(ackId, out ackCallback);

                if (ackCallback == null) return;

                _acks.Remove(ackId);
                ackCallback(statusCode, message);
            }
        }

        private void StartAckTimeout(int ackId, int timeoutSecs){
            // +1, because it might be about to tick in a few miliseconds
            _ackTimeoutQueue.Add(new[] {ackId, LTimer.CurrentTick + timeoutSecs + 1});
        }

        public virtual void HandleMessage(IIncommingMessage message){
            if (MessageReceived != null)
                MessageReceived(message);
        }

        public void HandleDataReceived(byte[] buffer, int start){
            IIncommingMessage message = null;

            try {
                message = MessageHelper.FromBytes(buffer, start, this);

                if (message.AckRequestId.HasValue) {
                    // We received a message which is a response to our ack request
                    TriggerAck(message.AckRequestId.Value, message.Status, message);
                    return;
                }
            }
            catch (Exception e) {
#if UNITY_EDITOR
                if (DontCatchExceptionsInEditor)
                    throw e;
#endif

                Debug.LogError("Failed parsing an incomming message: " + e);

                return;
            }

            HandleMessage(message);
        }

        #region Ack Disposal Stuff

        /// <summary>
        ///     Unique id
        /// </summary>
        public int Id {
            get {
                if (_id < 0) {
                    lock (_idGenerationLock) {
                        if (_id < 0) {
                            _id = _peerIdGenerator++;
                        }
                    }
                }

                return _id;
            }
        }

        /// <summary>
        ///     Called when ack disposal thread ticks
        /// </summary>
        private void HandleAckDisposalTick(long currentTick){
            // TODO test with ordered queue, might be more performant
            _ackTimeoutQueue.RemoveAll(a => {
                if (a[1] > currentTick) return false;

                try {
                    CancelAck((int) a[0], EResponseStatus.Timeout);
                }
                catch (Exception e) {
                    Debug.LogError(e);
                }

                return true;
            });
        }

        private void CancelAck(int ackId, EResponseStatus eResponseCode){
            lock (_acks) {
                ResponseCallback ackCallback;
                _acks.TryGetValue(ackId, out ackCallback);

                if (ackCallback == null) return;

                _acks.Remove(ackId);
                ackCallback(eResponseCode, _timeoutMessage);
            }
        }

        #endregion


        #region SendMessage

        public void SendMessage(short opCode){
            SendMessage(MessageHelper.Create(opCode), EDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(short opCode, ISerializablePacket packet){
            SendMessage(MessageHelper.Create(opCode, packet), EDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(short opCode, ISerializablePacket packet, EDeliveryMethod method){
            SendMessage(MessageHelper.Create(opCode, packet), method);
        }

        public void SendMessage(short opCode, ISerializablePacket packet, ResponseCallback responseCallback){
            var message = MessageHelper.Create(opCode, packet.ToBytes());
            SendMessage(message, responseCallback);
        }

        public void SendMessage(short opCode, ISerializablePacket packet, ResponseCallback responseCallback,
            int timeoutSecs){
            var message = MessageHelper.Create(opCode, packet.ToBytes());
            SendMessage(message, responseCallback, timeoutSecs, EDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(short opCode, ResponseCallback responseCallback){
            SendMessage(MessageHelper.Create(opCode), responseCallback);
        }

        public void SendMessage(short opCode, byte[] data,
            EDeliveryMethod deliveryMethod = EDeliveryMethod.ReliableOrdered){
            SendMessage(MessageHelper.Create(opCode, data),deliveryMethod);
        }

        public void SendMessage(short opCode, byte[] data, ResponseCallback ackCallback){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, ackCallback);
        }

        public void SendMessage(short opCode, byte[] data, ResponseCallback responseCallback, int timeoutSecs){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, responseCallback, timeoutSecs);
        }

        public void SendMessage(short opCode, string data){
            SendMessage(MessageHelper.Create(opCode, data), EDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(short opCode, string data, ResponseCallback responseCallback){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, responseCallback);
        }

        public void SendMessage(short opCode, string data, ResponseCallback responseCallback, int timeoutSecs){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, responseCallback, timeoutSecs);
        }

        public void SendMessage(short opCode, int data){
            SendMessage(MessageHelper.Create(opCode, data), EDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(short opCode, int data, ResponseCallback responseCallback){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, responseCallback);
        }

        public void SendMessage(short opCode, int data, ResponseCallback responseCallback, int timeoutSecs){
            var message = MessageHelper.Create(opCode, data);
            SendMessage(message, responseCallback, timeoutSecs);
        }

        public void SendMessage(IMessage message){
            SendMessage(message, EDeliveryMethod.ReliableOrdered);
        }

        void IMsgDispatcher.SendMessage(IMessage message, ResponseCallback responseCallback){
            SendMessage(message, responseCallback);
        }

        void IMsgDispatcher.SendMessage(IMessage message, ResponseCallback responseCallback, int timeoutSecs){
            SendMessage(message, responseCallback, timeoutSecs);
        }

        /// <summary>
        ///     Sends a message to peer
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="responseCallback">Callback method, which will be invoked when peer responds</param>
        /// <returns></returns>
        public int SendMessage(IMessage message, ResponseCallback responseCallback){
            return SendMessage(message, responseCallback, DefaultTimeoutSecs);
        }

        /// <summary>
        ///     Sends a message to peer
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="responseCallback">Callback method, which will be invoked when peer responds</param>
        /// <param name="timeoutSecs">If peer fails to respons within this time frame, callback will be invoked with timeout status</param>
        /// <returns></returns>
        public int SendMessage(IMessage message, ResponseCallback responseCallback,
            int timeoutSecs){
            return SendMessage(message, responseCallback, timeoutSecs, EDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        ///     Sends a message to peer
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="responseCallback">Callback method, which will be invoked when peer responds</param>
        /// <param name="timeoutSecs">If peer fails to respons within this time frame, callback will be invoked with timeout status</param>
        /// <param name="eDeliveryMethod">Delivery method</param>
        /// <returns></returns>
        public int SendMessage(IMessage message, ResponseCallback responseCallback,
            int timeoutSecs, EDeliveryMethod eDeliveryMethod){
            if (!IsConnected) {
                responseCallback.Invoke(EResponseStatus.NotConnected, null);
                return -1;
            }

            var id = RegisterAck(message, responseCallback, timeoutSecs);

            SendMessage(message, eDeliveryMethod);

            return id;
        }

        #endregion
    }
}