// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using Lockstep.Math;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Lockstep.Collision2D {
    [Serializable]
    public partial class ColliderData :IBaseComponent{
#if UNITY_5_3_OR_NEWER
        [Header("Offset")]
#endif
        public LFloat y;
        public LVector2 pos;
#if UNITY_5_3_OR_NEWER
        [Header("Collider data")]
#endif
        public LFloat high;
        public LFloat radius;
        public LVector2 size;
        public LVector2 up;
        public LFloat deg;

        public bool IsCircle(){
            return radius > 0;
        }
    }
}