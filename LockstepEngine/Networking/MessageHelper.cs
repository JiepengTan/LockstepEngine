using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lockstep.Logging;
using Lockstep.Serialization;


namespace Lockstep.Networking {
    /// <summary>
    ///     Helper class, that uses <see cref="IMessageFactory" /> implementation
    ///     to help create messages
    /// </summary>
    public static class MessageHelper {
        private static IMessageFactory _factory;


        static MessageHelper(){
            _factory = new MessageFactory();
        }

        /// <summary>
        ///     Changes current message factory.
        /// </summary>
        /// <param name="factory"></param>
        public static void SetFactory(IMessageFactory factory){
            _factory = factory;
        }

        /// <summary>
        ///     Writes data into a provided packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data, T packet) where T : ISerializablePacket{
            return packet; // SerializablePacket.FromBytes(data, packet);
        }


        /// <summary>
        ///     Creates an empty message
        /// </summary>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static IMessage Create(short opCode){
            return _factory.Create(opCode);
        }

        /// <summary>
        ///     Creates a message with data
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IMessage Create(short opCode, byte[] data){
            return _factory.Create(opCode, data);
        }

        /// <summary>
        ///     Creates a message from string
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IMessage Create(short opCode, string message){
            return _factory.Create(opCode, Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        ///     Creates a message from int
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IMessage Create(short opCode, int value){
            var bytes = new byte[4];
            ByteHelper.CopyBytes(value, bytes, 0);
            return _factory.Create(opCode, bytes);
        }


        public static IMessage Create(short opCode, ISerializablePacket packet){
            return Create(opCode, packet.ToBytes());
        }

        /// <summary>
        ///     Reconstructs message data into <see cref="IIncommingMessage" />
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="peer"></param>
        /// <returns></returns>
        public static IIncommingMessage FromBytes(byte[] buffer, int start, IPeer peer){
            return _factory.FromBytes(buffer, start, peer);
        }
    }
}