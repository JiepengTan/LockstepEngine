// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

#if UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.UnsafeCollision2D;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;
using Random = System.Random;

namespace Lockstep.Collision2D {
    public partial class CollisionSystem {
        public static ColliderPrefab CreateColliderPrefab(GameObject fab, ColliderData data){
            Debug.Trace("CreateColliderPrefab " + fab.name);
        
            if (data == null) {
                Debug.LogError(fab.name + " Miss ColliderDataMono ");
                return null;
            }

            Debug.Trace($"{fab.name} !!!CreateCollider  deg: {data.deg} up:{data.size} radius:{data.radius}");
            return CreateColliderPrefab(data);
        }

        public static ColliderPrefab CreateColliderPrefab(ColliderData data){  
            CBaseShape collider = null;
            if (LMath.Abs(data.deg - 45) < 1) {
                int i = 09;
            }

            //warning data.deg is unity deg
            //changed unity deg to ccw deg
            var collisionDeg = -data.deg + 90;
            if (data.radius > 0) {
                //circle
                collider = new CCircle(data.radius);
            }
            else {
                //obb
                collider = new COBB(data.size, collisionDeg);
            }

            collider.high = data.high;
            var colFab = new ColliderPrefab();
            colFab.parts.Add(new ColliderPart() {
                transform = new CTransform2D(data.pos,data.y,data.deg),
                collider = collider
            });
            return colFab;
        }
    }
}
#endif