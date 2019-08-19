namespace Lockstep.Networking
{
    public enum EDeliveryMethod
    {
        /// <summary>
        /// Unreliable. Packets can be dropped, duplicated or arrive without order
        /// </summary>
        Unreliable,
        /// <summary>
        /// Reliable. All packets will be sent and received, but without order
        /// </summary>
        ReliableUnordered,
        /// <summary>
        /// Unreliable. Packets can be dropped, but never duplicated and arrive in order
        /// </summary>
        Sequenced,
        /// <summary>
        /// Reliable and ordered. All packets will be sent and received in order
        /// </summary>
        ReliableOrdered,
        /// <summary>Reliable only last packet</summary>
        ReliableSequenced,
    }
}