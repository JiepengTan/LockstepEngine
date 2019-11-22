// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using Lockstep.Game;
using Lockstep.UnsafeCollision2D;
using Lockstep.Math;
#if UNITY_EDITOR
using UnityEngine;

#endif
namespace Lockstep.Collision2D {
    public delegate void FuncOnTriggerEvent(ColliderProxy other, ECollisionEvent type);

    public partial class ColliderProxy : ILPCollisionEventHandler, ILPTriggerEventHandler {
        public object EntityObject;
        public int Id;
        public int LayerType { get; set; }
        public ColliderPrefab Prefab;
        public CTransform2D Transform2D;
        public LFloat Height;
        public bool IsTrigger = true;
        public bool IsStatic = false;


        private LVector2 _prePos;
        private LFloat _preDeg;
        public static LFloat DegGap = new LFloat(null, 100);

        private LRect _bound;

        public FuncOnTriggerEvent OnTriggerEvent;

        private BoundsQuadTree _quadTree;

        private static int autoIncId = 0;

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y){
            Init(prefab, pos, y, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos){
            Init(prefab, pos, LFloat.zero, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y, LFloat deg){
            Init(prefab, new CTransform2D(pos, y, deg));
        }

        public LFloat MaxSideSize = 0;
        public void Init(ColliderPrefab prefab, CTransform2D trans){
            this.Prefab = prefab;
            _bound = prefab.GetBounds();
            MaxSideSize = LMath.Max(_bound.halfSize.x, _bound.halfSize.y);
            Transform2D = trans;
            _prePos = Transform2D.pos;
            _preDeg = Transform2D.deg;
            unchecked {
                Id = autoIncId++;
            }
        }

        public void DoUpdate(LFloat deltaTime){
            var curPos = Transform2D.pos;
            if (_prePos != curPos) {
                _prePos = curPos;
                IsMoved = true;
            }

            var curDeg = Transform2D.deg;
            if (LMath.Abs(curDeg - _preDeg) > DegGap) {
                _preDeg = curDeg;
                IsMoved = true;
            }
        }


        public bool IsMoved = true;

        public LVector2 pos {
            get => Transform2D.pos;
            set {
                IsMoved = true;
                Transform2D.pos = value;
            }
        }
        public LVector3 Pos3 {
            get => Transform2D.Pos3;
        }
        public LFloat y {
            get => Transform2D.y;
            set {
                IsMoved = true;
                Transform2D.y = value;
            }
        }

        public LFloat deg {
            get => Transform2D.deg;
            set {
                IsMoved = true;
                Transform2D.deg = value;
            }
        }


        public LRect GetBounds(){
            return new LRect(_bound.position + pos, _bound.size);
        }

        public virtual void OnLPTriggerEnter(ColliderProxy other){ }
        public virtual void OnLPTriggerStay(ColliderProxy other){ }
        public virtual void OnLPTriggerExit(ColliderProxy other){ }
        public virtual void OnLPCollisionEnter(ColliderProxy other){ }
        public virtual void OnLPCollisionStay(ColliderProxy other){ }
        public virtual void OnLPCollisionExit(ColliderProxy other){ }
    }

    public interface ILPCollisionEventHandler {
        void OnLPTriggerEnter(ColliderProxy other);
        void OnLPTriggerStay(ColliderProxy other);
        void OnLPTriggerExit(ColliderProxy other);
    }

    public interface ILPTriggerEventHandler {
        void OnLPTriggerEnter(ColliderProxy other);
        void OnLPTriggerStay(ColliderProxy other);
        void OnLPTriggerExit(ColliderProxy other);
    }
}