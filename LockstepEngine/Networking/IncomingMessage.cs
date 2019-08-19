using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lockstep.Serialization;


namespace Lockstep.Networking {
    /// <summary>
    ///     Default implementation of incomming message
    /// </summary>
    public class IncommingMessage : IIncommingMessage {
        private readonly byte[] _data;

        public IncommingMessage(short opCode, byte flags, byte[] data, EDeliveryMethod eDeliveryMethod, IPeer peer){
            OpCode = opCode;
            Peer = peer;
            _data = data;
        }

        /// <summary>
        ///     Message flags
        /// </summary>
        public byte Flags { get; private set; }

        /// <summary>
        ///     Operation code (message type)
        /// </summary>
        public short OpCode { get; private set; }

        /// <summary>
        ///     Sender
        /// </summary>
        public IPeer Peer { get; private set; }

        /// <summary>
        ///     Ack id the message is responding to
        /// </summary>
        public int? AckResponseId { get; set; }

        /// <summary>
        ///     We add this to a packet to so that receiver knows
        ///     what he responds to
        /// </summary>
        public int? AckRequestId { get; set; }

        /// <summary>
        ///     Returns true, if sender expects a response to this message
        /// </summary>
        public bool IsExpectingResponse {
            get { return AckResponseId.HasValue; }
        }

        /// <summary>
        ///     For ordering
        /// </summary>
        public int SequenceChannel { get; set; }

        /// <summary>
        ///     Message status code
        /// </summary>
        public EResponseStatus Status { get; set; }

        /// <summary>
        ///     Respond with a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public void Respond(IMessage message, EResponseStatus statusCode = EResponseStatus.Default){
            message.Status = statusCode;

            if (AckResponseId.HasValue)
                message.AckResponseId = AckResponseId.Value;

            Peer.SendMessage(message, EDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        ///     Respond with data (message is created internally)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        public void Respond(byte[] data, EResponseStatus statusCode = EResponseStatus.Default){
            Respond(MessageHelper.Create(OpCode, data), statusCode);
        }

        /// <summary>
        ///     Respond with data (message is created internally)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        public void Respond(ISerializablePacket packet, EResponseStatus statusCode = EResponseStatus.Default){
            Respond(MessageHelper.Create(OpCode, packet.ToBytes()), statusCode);
        }

        /// <summary>
        ///     Respond with empty message and status code
        /// </summary>
        /// <param name="statusCode"></param>
        public void Respond(EResponseStatus statusCode = EResponseStatus.Default){
            Respond(MessageHelper.Create(OpCode), statusCode);
        }

        public void Respond(string message, EResponseStatus statusCode = EResponseStatus.Default){
            Respond(message.ToBytes(), statusCode);
        }

        public void Respond(int response, EResponseStatus statusCode = EResponseStatus.Default){
            Respond(MessageHelper.Create(OpCode, response), statusCode);
        }

        /// <summary>
        ///     Returns true if message contains any data
        /// </summary>
        public bool HasData {
            get { return _data.Length > 0; }
        }

        /// <summary>
        ///     Returns contents of this message. Mutable
        /// </summary>
        /// <returns></returns>
        public byte[] AsBytes(){
            return _data;
        }

        /// <summary>
        ///     Decodes content into a string
        /// </summary>
        /// <returns></returns>
        public string AsString(){
            return Encoding.UTF8.GetString(_data);
        }

        /// <summary>
        ///     Decodes content into a string. If there's no content,
        ///     returns the <see cref="defaultValue"/>
        /// </summary>
        /// <returns></returns>
        public string AsString(string defaultValue){
            return HasData ? AsString() : defaultValue;
        }

        /// <summary>
        ///     Decodes content into an integer
        /// </summary>
        /// <returns></returns>
        public int AsInt(){
            return ByteHelper.ToInt32(_data, 0);
        }

        /// <summary>
        ///     Writes content of the message into a packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packetToBeFilled"></param>
        /// <returns></returns>
        public T Deserialize<T>(T packetToBeFilled) where T : ISerializablePacket{
            return MessageHelper.Deserialize(_data, packetToBeFilled);
        }


        public override string ToString(){
            return AsString(base.ToString());
        }

        public T Parse<T>() where T : BaseFormater, new(){
            Deserializer reader = new Deserializer(_data);
            var val = new T();
            val.Deserialize(reader);
            return val;
        }

        public Deserializer GetData(){
            return _data == null ? null : new Deserializer(_data);
        }
        public byte[] GetRawBytes(){
            return _data ;
        }
        public void Respond(object type, BaseFormater msg, EResponseStatus responseStatus = EResponseStatus.Default){
            Respond(MessageHelper.Create((short) type, msg.ToBytes()), responseStatus);
        }
    }
}