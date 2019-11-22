// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.BehaviourTree
{
    public class TAny
    {
        public T As<T>() where T : TAny
        {
            return (T)this;
        }
    }
}