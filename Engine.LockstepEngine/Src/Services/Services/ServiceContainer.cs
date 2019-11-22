// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lockstep.Game {
    public class ServiceContainer : IServiceContainer {
        protected Dictionary<Type, IService> _allServices = new Dictionary<Type, IService>();

        public IService[] GetAllServices(){
            return _allServices.Values.ToArray();
        }

        public void RegisterService(IService service, bool overwriteExisting = true){
            var interfaceTypes = service.GetType().FindInterfaces((type, criteria) =>
                    type.GetInterfaces().Any(t => t == typeof(IService)), service)
                .ToArray();
            if (service is IDoInit svc) {
                svc.DoInit(null);
            }

            foreach (var type in interfaceTypes) {
                if (!_allServices.ContainsKey(type))
                    _allServices.Add(type, service);
                else if (overwriteExisting) {
                    _allServices[type] = service;
                }
            }
        }


        public T GetService<T>() where T : IService{
            var key = typeof(T);
            if (!_allServices.ContainsKey(key)) {
                return default(T);
            }
            else {
                return (T) _allServices[key];
            }
        }
    }
}