namespace Lockstep.Networking
{
    /// <summary>
    ///     Generic packet handler
    /// </summary>
    public class PacketHandler : IPacketHandler
    {
        private readonly IncommingMessageHandler _handler;
        private readonly short _opCode;

        public PacketHandler(short opCode, IncommingMessageHandler handler)
        {
            _opCode = opCode;
            _handler = handler;
        }

        public short OpCode
        {
            get { return _opCode; }
        }

        public void Handle(IIncommingMessage message)
        {

            _handler.Invoke(message);
        }
    }
}