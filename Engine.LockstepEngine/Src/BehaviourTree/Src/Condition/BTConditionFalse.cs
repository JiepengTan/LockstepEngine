// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.BehaviourTree {
    [BuildInNode(typeof(BTConditionFalse),EBTBuildInTypeIdx.BTConditionFalse)]
    public partial class BTConditionFalse : BTCondition {
        public override bool IsTrue( /*in*/ BTWorkingData wData){
            return false;
        }
    }
}