// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.Network {
    public interface IMessageDispatcher {
        void Dispatch(Session session, Packet packet);
    }
}