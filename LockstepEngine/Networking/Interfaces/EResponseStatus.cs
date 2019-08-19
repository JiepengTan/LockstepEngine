namespace Lockstep.Networking
{
    public enum EResponseStatus
    {
        Default = 0,
        Success = 1,
        Timeout = 2,
        Error = 3,
        Unauthorized = 4,
        Invalid = 5,
        Failed = 6,
        NotConnected = 7,
        NotHandled = 8,
    }
}