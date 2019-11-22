// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;

namespace Lockstep {
    public partial interface INeedBackup { }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false,
        Inherited = true)]
    public class NoBackupAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false,
        Inherited = true)]
    public class BackupAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false,
        Inherited = true)]
    public class ReRefBackupAttribute : Attribute { }
}
namespace Lockstep {
    public partial interface IBaseEntity:INeedBackup {}
    public partial interface IBaseContexts { }
    public partial interface IBaseComponent :INeedBackup{ }
}