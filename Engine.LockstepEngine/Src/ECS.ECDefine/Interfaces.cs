namespace Lockstep.UnsafeECSDefine {


    
    
    public interface ISystem { }
    public interface ISystemWithIdx { }
    public interface ISystemWithEntity { }

    /// System 定义
    public interface IPureSystem : ISystem { }

    public interface IPureSystemWithEntity : IPureSystem, ISystemWithEntity { }

    public interface IJobSystem : ISystem { }

    public interface IJobSystemWithEntity : IJobSystem, ISystemWithEntity { }

    public interface IJobForEachSystem : IJobSystem { }

    public interface IJobForEachSystemWithEntity : IJobForEachSystem, ISystemWithEntity { }

    public interface IJobHashMapSystem : ISystem { }


    /// 全局数据定义
    public interface IGlobal { }
    /// Enitty目标集
    public interface IAsset { }

    public interface IComponent { }

    public interface IEntity { }

    public interface IBitset { }

    public interface IFields { }

    public interface IServiceState { }

    public interface IEvent{}
    //碰撞事件定义
    public interface ICollisionEvent { }

}