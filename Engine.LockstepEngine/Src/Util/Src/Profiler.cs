// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System.Diagnostics;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Profiling;
#endif
namespace Lockstep.Util {
    public class Profiler {
        [Conditional("DEBUG")]
        public static void BeginSample(object obj){
#if UNITY_5_3_OR_NEWER
            UnityEngine.Profiling.Profiler.BeginSample(obj.GetType().Name);
#endif
        }
        
        [Conditional("DEBUG")]
        public static void EndSample(object obj){
#if UNITY_5_3_OR_NEWER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
        
        [Conditional("DEBUG")]
        public static void BeginSample(string tag){
#if UNITY_5_3_OR_NEWER
            UnityEngine.Profiling.Profiler.BeginSample(tag);
#endif
        }

        [Conditional("DEBUG")]
        public static void EndSample(){
#if UNITY_5_3_OR_NEWER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
    }
}