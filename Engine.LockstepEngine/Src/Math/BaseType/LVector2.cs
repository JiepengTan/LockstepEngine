// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Runtime.CompilerServices;
using Lockstep.Math;

namespace Lockstep.Math {
    [Serializable]
    public struct LVector2 {
        public LFloat x {
            get { return new LFloat(true, _x); }
            set { _x = value._val; }
        }

        public LFloat y {
            get { return new LFloat(true, _y); }
            set { _y = value._val; }
        }

        public long _x;
        public long _y;
        public static readonly LVector2 zero = new LVector2(true, 0, 0);
        public static readonly LVector2 one = new LVector2(true, LFloat.Precision, LFloat.Precision);
        public static readonly LVector2 half = new LVector2(true, LFloat.Precision / 2, LFloat.Precision / 2);
        public static readonly LVector2 up = new LVector2(true, 0, LFloat.Precision);
        public static readonly LVector2 down = new LVector2(true, 0, -LFloat.Precision);
        public static readonly LVector2 right = new LVector2(true, LFloat.Precision, 0);
        public static readonly LVector2 left = new LVector2(true, -LFloat.Precision, 0);

        private static readonly int[] Rotations = new int[] {
            1,
            0,
            0,
            1,
            0,
            1,
            -1,
            0,
            -1,
            0,
            0,
            -1,
            0,
            -1,
            1,
            0
        };

        /// <summary>
        /// 顺时针旋转90Deg 参数
        /// </summary>
        public const int ROTATE_CW_90 = 1;

        public const int ROTATE_CW_180 = 2;
        public const int ROTATE_CW_270 = 3;
        public const int ROTATE_CW_360 = 4;

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LVector2(bool isUseRawVal, long x, long y){
            this._x = x;
            this._y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LVector2(LFloat x, LFloat y){
            this._x = x._val;
            this._y = y._val;
        }

        public LVector2(long x, long y){
            this._x = x * LFloat.Precision;
            this._y = y * LFloat.Precision;
        }


        /// <summary>
        /// clockwise 顺时针旋转  
        /// 1表示顺时针旋转 90 degree
        /// 2表示顺时针旋转 180 degree
        /// </summary>
        public static LVector2 Rotate(LVector2 v, int r){
            r %= 4;
            return new LVector2(true,
                v._x * LVector2.Rotations[r * 4] + v._y * LVector2.Rotations[r * 4 + 1],
                v._x * LVector2.Rotations[r * 4 + 2] + v._y * LVector2.Rotations[r * 4 + 3]);
        }

        public LVector2 Rotate( LFloat deg){
            var rad = LMath.Deg2Rad * deg;
            LFloat cos, sin;
            LMath.SinCos(out sin, out cos, rad);
            return new LVector2(x * cos - y * sin, x * sin + y * cos);
        }

        public static LVector2 Min(LVector2 a, LVector2 b){
            return new LVector2(true, LMath.Min(a._x, b._x), LMath.Min(a._y, b._y));
        }

        public static LVector2 Max(LVector2 a, LVector2 b){
            return new LVector2(true, LMath.Max(a._x, b._x), LMath.Max(a._y, b._y));
        }

        public void Min(ref LVector2 r){
            this._x = LMath.Min(this._x, r._x);
            this._y = LMath.Min(this._y, r._y);
        }

        public void Max(ref LVector2 r){
            this._x = LMath.Max(this._x, r._x);
            this._y = LMath.Max(this._y, r._y);
        }


        public void Normalize(){
            long sqr = _x * _x + _y * _y;
            if (sqr == 0L) {
                return;
            }

            long b = (long) LMath.Sqrt(sqr);
            this._x = (_x * LFloat.Precision / b);
            this._y = (_y * LFloat.Precision / b);
        }

        public LFloat sqrMagnitude {
            get {
                return new LFloat(true, (_x * _x + _y * _y) / LFloat.Precision);
            }
        }

        public long rawSqrMagnitude {
            get {
                return _x * _x + _y * _y;
            }
        }

        public LFloat magnitude {
            get {
                return new LFloat(true, LMath.Sqrt(_x * _x + _y * _y));
            }
        }

        public LVector2 normalized {
            get {
                LVector2 result = new LVector2(true, this._x, this._y);
                result.Normalize();
                return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector2 operator +(LVector2 a, LVector2 b){
            return new LVector2(true, a._x + b._x, a._y + b._y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector2 operator -(LVector2 a, LVector2 b){
            return new LVector2(true, a._x - b._x, a._y - b._y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector2 operator -(LVector2 lhs){
            lhs._x = -lhs._x;
            lhs._y = -lhs._y;
            return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector2 operator *(LFloat rhs, LVector2 lhs){
            lhs._x = (int) (((long) (lhs._x * rhs._val)) / LFloat.Precision);
            lhs._y = (int) (((long) (lhs._y * rhs._val)) / LFloat.Precision);
            return lhs;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector2 operator *(LVector2 lhs, LFloat rhs){
            lhs._x = (int) (((long) (lhs._x * rhs._val)) / LFloat.Precision);
            lhs._y = (int) (((long) (lhs._y * rhs._val)) / LFloat.Precision);
            return lhs;
        }
        public static LVector2 operator *(int rhs, LVector2 lhs){
            lhs._x = lhs._x * rhs;
            lhs._y = lhs._y * rhs;
            return lhs;
        }

        public static LVector2 operator *(LVector2 lhs, int rhs){
            lhs._x = lhs._x * rhs;
            lhs._y = lhs._y * rhs;
            return lhs;
        }
        public static LVector2 operator /(LVector2 lhs, LFloat rhs){
            lhs._x = (int) (((long) lhs._x * LFloat.Precision) / rhs._val);
            lhs._y = (int) (((long) lhs._y * LFloat.Precision) / rhs._val);
            return lhs;
        }
        public static LVector2 operator /(LVector2 lhs, int rhs){
            lhs._x = lhs._x / rhs;
            lhs._y = lhs._y / rhs;
            return lhs;
        }
        public static bool operator ==(LVector2 a, LVector2 b){
            return a._x == b._x && a._y == b._y;
        }

        public static bool operator !=(LVector2 a, LVector2 b){
            return a._x != b._x || a._y != b._y;
        }

        public static implicit operator LVector2(LVector3 v){
            return new LVector2(true, v._x, v._y);
        }

        public static implicit operator LVector3(LVector2 v){
            return new LVector3(true, v._x, v._y, 0);
        }

        public override bool Equals(object o){
            if (o == null) {
                return false;
            }

            LVector2 vInt = (LVector2) o;
            return this._x == vInt._x && this._y == vInt._y;
        }

        public override int GetHashCode(){
            return unchecked((int)(this._x * 49157 + this._y * 98317));
        }

        public override string ToString(){
            return string.Format("({0},{1})", _x * LFloat.PrecisionFactor, _y * LFloat.PrecisionFactor);
        }

        public LVector3 ToInt3 {
            get { return new LVector3(true, _x, 0, _y); }
        }

        public LFloat this[int index] {
            get {
                switch (index) {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException("vector idx invalid" + index);
                }
            }

            set {
                switch (index) {
                    case 0:
                        _x = value._val;
                        break;
                    case 1:
                        _y = value._val;
                        break;
                    default: throw new IndexOutOfRangeException("vector idx invalid" + index);
                }
            }
        }


        public static LFloat Dot(LVector2 u, LVector2 v){
            return new LFloat(true, ((long) u._x * v._x + (long) u._y * v._y) / LFloat.Precision);
        }

        public static LFloat Cross(LVector2 a, LVector2 b){
            return new LFloat(true, ((long) a._x * (long) b._y - (long) a._y * (long) b._x) / LFloat.Precision);
        }

        public static LVector2 Lerp(LVector2 a, LVector2 b, LFloat f){
            return new LVector2(true,
                (int) (((long) (b._x - a._x) * f._val) / LFloat.Precision) + a._x,
                (int) (((long) (b._y - a._y) * f._val) / LFloat.Precision) + a._y);
        }
    }
}