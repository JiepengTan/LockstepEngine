namespace Lockstep.Networking
{
    public enum EMessageFlag : byte
    {
        AckRequest = 1, // 0000 0001
        AckResponse = 2 // 0000 0010
    }
}