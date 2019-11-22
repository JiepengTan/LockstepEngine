// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.Game {
    public interface IECSFactoryService : IService {
        object CreateContexts();
        object CreateSystems(object contexts, IServiceContainer services);
    }
}