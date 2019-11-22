// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.Network {
    public interface IMessagePacker {
        object DeserializeFrom(ushort type, byte[] bytes, int index, int count);
        byte[] SerializeToByteArray(IMessage msg);
    }
}