namespace Lockstep.Networking
{
    /// <summary>
    ///     Represents an object that can handle packets
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        ///     Operation code of the message to be handled
        /// </summary>
        short OpCode { get; }

        /// <summary>
        ///     Handling of the message
        /// </summary>
        /// <param name="message"></param>
        void Handle(IIncommingMessage message);
    }
}