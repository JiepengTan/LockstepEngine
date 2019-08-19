namespace Lockstep.Networking
{
    /// <summary>
    ///     Represents functionality of a basic outgoing message
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        ///     Operation code, a.k.a message type
        /// </summary>
        short OpCode { get; }

        /// <summary>
        ///     Content of the message
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        ///     Returns true if data is not empty
        /// </summary>
        bool HasData { get; }

        /// <summary>
        ///     An id of ack request. It's set when we send a message,
        ///     and expect a response. This is how we tell which message we got a response to
        /// </summary>
        int? AckRequestId { get; set; }

        /// <summary>
        ///     Used to identify what message we are responsing to
        /// </summary>
        int? AckResponseId { get; set; }

        /// <summary>
        ///     Internal flags, used to help identify what kind of message we've received
        /// </summary>
        byte Flags { get; set; }

        /// <summary>
        ///     Status code of the message
        /// </summary>
        EResponseStatus Status { get; set; }

        /// <summary>
        ///     Overrides current data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IMessage SetBinary(byte[] data);

        /// <summary>
        ///     Serializes message to byte array
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();
    }
}