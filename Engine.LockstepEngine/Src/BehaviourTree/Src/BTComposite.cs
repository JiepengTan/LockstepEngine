// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.BehaviourTree {
    public partial class  BTComposite : BTAction {
        public BTComposite(int maxChildCount)
            : base(maxChildCount){ }
        public BTComposite( )
            : this(int.MaxValue){ }
    }
}