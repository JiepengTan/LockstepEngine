using System;
using Lockstep.Logging;
using Lockstep.Serialization;

namespace Lockstep.Networking {
    public class MessageFactory : IMessageFactory {
        public IMessage Create(short opCode){
            return new Message(opCode);
        }

        public IMessage Create(short opCode, byte[] data){
            return new Message(opCode, data);
        }
   
        ///     Used raw byte data to create an <see cref="IIncommingMessage" />
        public IIncommingMessage FromBytes(byte[] buffer, int start, IPeer peer){
            try {
                //var converter = EndianBitConverter.Big;
                var flags = buffer[start];
                var opCode = ByteHelper.ToInt16(buffer, start + 1);
                var pointer = start + 3;

                var dataLength = ByteHelper.ToInt32(buffer, pointer);
                pointer += 4;
                var data = new byte[dataLength];
                Array.Copy(buffer, pointer, data, 0, dataLength);
                pointer += dataLength;

                var message = new IncommingMessage(opCode, flags, data, EDeliveryMethod.ReliableUnordered, peer) {
                    SequenceChannel = 0
                };

                if ((flags & (byte) EMessageFlag.AckRequest) > 0) {
                    // We received a message which requests a response
                    message.AckResponseId = ByteHelper.ToInt32(buffer, pointer);
                    pointer += 4;
                }

                if ((flags & (byte) EMessageFlag.AckResponse) > 0) {
                    // We received a message which is a response to our ack request
                    var ackId = ByteHelper.ToInt32(buffer, pointer);
                    message.AckRequestId = ackId;
                    pointer += 4;

                    var statusCode = buffer[pointer];

                    message.Status =
                        (EResponseStatus) statusCode; // TODO look into not exposing status code / ackRequestId
                    pointer++;
                }

                return message;
            }
            catch (Exception e) {
                Debug.LogError("WS Failed parsing an incoming message " + e);
            }

            return null;
        }

    }
}