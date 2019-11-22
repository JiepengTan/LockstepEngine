// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;

namespace Lockstep.BehaviourTree {
    //---------------------------------------------------------------
    public abstract unsafe partial class BTPrecondition : BTNode {
        public BTPrecondition(int maxChildCount)
            : base(maxChildCount){ }

        public abstract bool IsTrue( /*in*/ BTWorkingData wData);
    }
}