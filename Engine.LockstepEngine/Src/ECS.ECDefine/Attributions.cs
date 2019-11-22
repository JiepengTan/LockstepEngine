// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;

namespace Lockstep.UnsafeECSDefine {

    [AttributeUsage(AttributeTargets.Method)]
    public class SignalAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class AbstractAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class InitEntityCountAttribute : System.Attribute {
        public int count;

        public InitEntityCountAttribute(int count){
            this.count = count;
        }
    }

    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Field,AllowMultiple = true)]
    public class AttributeAttribute : System.Attribute {
        public string name;
        public AttributeAttribute(string name){
            this.name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Enum)]
    public class IgnoreAttribute : System.Attribute {
    }
    
    /// 不导出Excel
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class NoExcelAttribute : System.Attribute {
    }
    
    /// 不导出Excel
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class KeyIdAttribute : System.Attribute {
    }
}