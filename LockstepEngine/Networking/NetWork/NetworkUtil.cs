using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lockstep.Networking {

    public static class NetworkUtil {
        
        public static T CreateDelegateFromMethodInfo<T>(System.Object instance, MethodInfo method) where T : Delegate{
            return Delegate.CreateDelegate(typeof(T), instance, method) as T;
        }

        public static void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen,
            Action<TEnum, TDelegate> callBack, object obj)
            where TDelegate : Delegate
            where TEnum : struct{
            if (callBack == null) return;
            var methods = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var method in methods) {
                var methodName = method.Name;
                if (methodName.StartsWith(prefix)) {
                    var eventTypeStr = methodName.Substring(ignorePrefixLen);
                    if (Enum.TryParse(eventTypeStr, out TEnum eType)) {
                        try {
                            var handler = CreateDelegateFromMethodInfo<TDelegate>(obj, method);
                            callBack(eType, handler);
                        }
                        catch (Exception e) {
                            Console.WriteLine("CreateDelegate failed " + eventTypeStr + " " + e);
                            throw;
                        }
                    }
                }
            }
        }

        
        
        public static bool InitNetServer<TMsgType>(ref NetServer<TMsgType> refServer, int port,
            object target,string key = "") where TMsgType : struct{
            if (refServer != null) return true;
            var maxIdx = (short) (object) (TMsgType) Enum.Parse(typeof(TMsgType), "EnumCount");
            HashSet<string> tags = new HashSet<string>();
            for (short i = 0; i < maxIdx; i++) {
                var enumStr = ((TMsgType) (object) i).ToString();
                tags.Add(enumStr.Split('_')[0]);
            }

            refServer = new NetServer<TMsgType>(key, maxIdx, tags.ToArray(), target);
            refServer.Listen(port);
            return false;
        }


        public static bool InitNetClient<TMsgType>(ref NetClient<TMsgType> refClient, string ip, int port,
            Action onConnCallback, object target,string key = "")
            where TMsgType : struct{
            if (refClient != null) return true;
            var maxIdx = (short) (object) (TMsgType) Enum.Parse(typeof(TMsgType), "EnumCount");
            HashSet<string> tags = new HashSet<string>();
            for (short i = 0; i < maxIdx; i++) {
                var enumStr = ((TMsgType) (object) i).ToString();
                tags.Add(enumStr.Split('_')[0]);
            }

            refClient = new NetClient<TMsgType>(maxIdx, tags.ToArray(), target);
            if (onConnCallback != null) {
                refClient.OnConnected += onConnCallback;
            }

            refClient.Connect(ip, port, key);
            return false;
        }
    }
    
    
}