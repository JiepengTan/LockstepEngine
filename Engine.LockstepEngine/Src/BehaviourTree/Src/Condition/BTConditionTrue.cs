// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.BehaviourTree {
    [BuildInNode(typeof(BTConditionTrue),EBTBuildInTypeIdx.BTConditionTrue)]
    public partial class  BTConditionTrue : BTCondition {
        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return true;
        }
    }
}