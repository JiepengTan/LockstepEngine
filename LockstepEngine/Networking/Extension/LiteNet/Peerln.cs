using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using LiteNetLib;
using Lockstep.Logging;
using Lockstep.Networking;
using Lockstep.Util;


namespace Lockstep.Networking {
    public class PeerLn : BasePeer {
        public override IPEndPoint EndPoint => _peer?.EndPoint;
        protected NetPeer _peer;
        public const float Delay = 0.2f;
        private Queue<byte[]> _delayedMessages;

        public PeerLn(NetPeer socket){
            _peer = socket;

            _delayedMessages = new Queue<byte[]>();
        }

        private bool isConn = false;

        public void SetConnectedState(bool isConn){
            this.isConn = isConn;
        }

        public override bool IsConnected {
            get { return isConn; }
        }

        public IEnumerator SendDelayedMessages(){
            yield return new WaitForSeconds(Delay);

            if (_delayedMessages == null) {
                yield break;
            }

            lock (_delayedMessages) {
                if (_delayedMessages == null)
                    yield break;

                var copy = _delayedMessages;
                _delayedMessages = null;

                foreach (var data in copy) {
                    _peer.Send(data, DeliveryMethod.ReliableSequenced);
                }
            }
        }

        public override void SendMessage(IMessage message, EDeliveryMethod eDeliveryMethod){
            _peer.Send(message.ToBytes(), (DeliveryMethod) (int) eDeliveryMethod);
        }

        public override void Disconnect(string reason){
            _peer.Disconnect();
        }
    }
}