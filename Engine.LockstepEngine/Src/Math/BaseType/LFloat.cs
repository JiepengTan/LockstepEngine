// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Runtime.CompilerServices;
using Lockstep.Math;

namespace Lockstep.Math {
    [Serializable]
    public struct LFloat : IEquatable<LFloat>, IComparable<LFloat> {
        public const long Precision = 1000000;
        public const long RateOfOldPrecision = Precision/1000;
        public const long HalfPrecision = Precision / 2;
        public const float PrecisionFactor = 0.000001f;

        public long _val;

        public static readonly LFloat zero = new LFloat( true,0L);
        public static readonly LFloat one = new LFloat(true, LFloat.Precision);
        public static readonly LFloat negOne = new LFloat(true, -LFloat.Precision);
        public static readonly LFloat half = new LFloat(true, LFloat.Precision / 2L);
        public static readonly LFloat FLT_MAX = new LFloat(true, long.MaxValue);
        public static readonly LFloat FLT_MIN = new LFloat(true, long.MinValue);
        public static readonly LFloat EPSILON = new LFloat(true, 1L);
        public static readonly LFloat INTERVAL_EPSI_LON = new LFloat(true, 1L);

        public static readonly LFloat MaxValue = new LFloat(true, long.MaxValue);
        public static readonly LFloat MinValue = new LFloat(true, long.MinValue);
      
        /// ! 传入的是正常数放大1000 的数值</summary>
        public LFloat(string isUseRawVal1000,long rawVal1000){
            this._val = rawVal1000 * RateOfOldPrecision;
        }
        
        public LFloat(bool isUseRawVal,long rawVal){
            this._val =  rawVal;
        }
       
        public LFloat(int val){
            this._val = val * LFloat.Precision;
        }
        public LFloat(long val){
            this._val = val * LFloat.Precision;
        }

        #if UNITY_EDITOR
        /// <summary>
        /// 直接使用浮点型 进行构造 警告!!! 仅应该在Editor模式下使用，不应该在正式代码中使用,避免出现引入浮点的不确定性
        /// </summary>
        public LFloat(bool shouldOnlyUseInEditor,float val)
        {
            this._val = (long)(val * LFloat.Precision);
        }
        #endif

        #region override operator 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(LFloat a, LFloat b){
            return a._val < b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(LFloat a, LFloat b){
            return a._val > b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(LFloat a, LFloat b){
            return a._val <= b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(LFloat a, LFloat b){
            return a._val >= b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(LFloat a, LFloat b){
            return a._val == b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(LFloat a, LFloat b){
            return a._val != b._val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat operator +(LFloat a, LFloat b){
            return new LFloat(true, a._val + b._val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat operator -(LFloat a, LFloat b){
            return new LFloat(true, a._val - b._val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat operator *(LFloat a, LFloat b){
            long val = (long) (a._val) * b._val;
            return new LFloat(true, val/LFloat.Precision);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat operator /(LFloat a, LFloat b){
            long val = (long) (a._val * LFloat.Precision) / b._val;
            return new LFloat( true,val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat operator -(LFloat a){
            return new LFloat(true, -a._val);
        }

        #region adapt for int

        public static LFloat operator +(LFloat a, int b){
            return new LFloat(true, a._val + b * Precision);
        }

        public static LFloat operator -(LFloat a, int b){
            return new LFloat(true, a._val - b * Precision);
        }

        public static LFloat operator *(LFloat a, int b){
            return new LFloat(true, (a._val * b));
        }

        public static LFloat operator /(LFloat a, int b){
            return new LFloat(true, (a._val) / b);
        }


        public static LFloat operator +(int a, LFloat b){
            return new LFloat(true, b._val + a * Precision);
        }

        public static LFloat operator -(int a, LFloat b){
            return new LFloat(true, a * Precision - b._val);
        }

        public static LFloat operator *(int a, LFloat b){
            return new LFloat(true, (b._val * a));
        }

        public static LFloat operator /(int a, LFloat b){
            return new LFloat(true, ((long) (a * Precision * Precision) / b._val));
        }


        public static bool operator <(LFloat a, int b){
            return a._val < (b * Precision);
        }

        public static bool operator >(LFloat a, int b){
            return a._val > (b * Precision);
        }

        public static bool operator <=(LFloat a, int b){
            return a._val <= (b * Precision);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(LFloat a, int b){
            return a._val >= (b * Precision);
        }

        public static bool operator ==(LFloat a, int b){
            return a._val == (b * Precision);
        }

        public static bool operator !=(LFloat a, int b){
            return a._val != (b * Precision);
        }


        public static bool operator <(int a, LFloat b){
            return (a * Precision) < (b._val);
        }

        public static bool operator >(int a, LFloat b){
            return (a * Precision) > (b._val);
        }

        public static bool operator <=(int a, LFloat b){
            return (a * Precision) <= (b._val);
        }

        public static bool operator >=(int a, LFloat b){
            return (a * Precision) >= (b._val);
        }

        public static bool operator ==(int a, LFloat b){
            return (a * Precision) == (b._val);
        }

        public static bool operator !=(int a, LFloat b){
            return (a * Precision) != (b._val);
        }

        #endregion
        #region adapt for long

        public static LFloat operator +(LFloat a, long b){
            return new LFloat(true, a._val + b * Precision);
        }

        public static LFloat operator -(LFloat a, long b){
            return new LFloat(true, a._val - b * Precision);
        }

        public static LFloat operator *(LFloat a, long b){
            return new LFloat(true, (a._val * b));
        }

        public static LFloat operator /(LFloat a, long b){
            return new LFloat(true, (a._val) / b);
        }


        public static LFloat operator +(long a, LFloat b){
            return new LFloat(true, b._val + a * Precision);
        }

        public static LFloat operator -(long a, LFloat b){
            return new LFloat(true, a * Precision - b._val);
        }

        public static LFloat operator *(long a, LFloat b){
            return new LFloat(true, (b._val * a));
        }

        public static LFloat operator /(long a, LFloat b){
            return new LFloat(true, ((long) (a * Precision * Precision) / b._val));
        }


        public static bool operator <(LFloat a, long b){
            return a._val < (b * Precision);
        }

        public static bool operator >(LFloat a, long b){
            return a._val > (b * Precision);
        }

        public static bool operator <=(LFloat a, long b){
            return a._val <= (b * Precision);
        }

        public static bool operator >=(LFloat a, long b){
            return a._val >= (b * Precision);
        }

        public static bool operator ==(LFloat a, long b){
            return a._val == (b * Precision);
        }

        public static bool operator !=(LFloat a, long b){
            return a._val != (b * Precision);
        }


        public static bool operator <(long a, LFloat b){
            return (a * Precision) < (b._val);
        }

        public static bool operator >(long a, LFloat b){
            return (a * Precision) > (b._val);
        }

        public static bool operator <=(long a, LFloat b){
            return (a * Precision) <= (b._val);
        }

        public static bool operator >=(long a, LFloat b){
            return (a * Precision) >= (b._val);
        }

        public static bool operator ==(long a, LFloat b){
            return (a * Precision) == (b._val);
        }

        public static bool operator !=(long a, LFloat b){
            return (a * Precision) != (b._val);
        }

        #endregion

        #endregion

        #region override object func 

        public override bool Equals(object obj){
            return obj is LFloat && ((LFloat) obj)._val == _val;
        }

        public bool Equals(LFloat other){
            return _val == other._val;
        }

        public int CompareTo(LFloat other){
            return _val.CompareTo(other._val);
        }

        public override int GetHashCode(){
            return (int)_val;
        }

        public override string ToString(){
            return (_val * LFloat.PrecisionFactor).ToString();
        }

        #endregion

        #region override type convert 
        public static implicit operator LFloat(short value){
            return new LFloat(true, value * Precision);
        }

        public static explicit operator short(LFloat value){
            return (short)(value._val / Precision);
        }
        
        public static implicit operator LFloat(int value){
            return new LFloat(true, value * Precision);
        }

        public static implicit operator int(LFloat value){
            return (int)(value._val / Precision);
        }

        public static explicit operator LFloat(long value){
            return new LFloat(true, value * Precision);
        }

        public static implicit operator long(LFloat value){
            return value._val / Precision;
        }


        public static explicit operator LFloat(float value){
            return new LFloat(true, (long) (value * Precision));
        }

        public static explicit operator float(LFloat value){
            return (float) value._val *LFloat.PrecisionFactor;
        }

        public static explicit operator LFloat(double value){
            return new LFloat(true, (long) (value * Precision));
        }

        public static explicit operator double(LFloat value){
            return (double) value._val *LFloat.PrecisionFactor;
        }

        #endregion


        public int ToInt(){
            return (int)(_val / LFloat.Precision);
        }

        public long ToLong(){
            return _val / LFloat.Precision;
        }

        public float ToFloat(){
            return _val *LFloat.PrecisionFactor;
        }

        public double ToDouble(){
            return _val *LFloat.PrecisionFactor;
        }

        public int Floor(){
            var x = this._val;
            if (x > 0) {
                x /= LFloat.Precision;
            }
            else {
                if (x % LFloat.Precision == 0) {
                    x /= LFloat.Precision;
                }
                else {
                    x = x / LFloat.Precision - 1;
                }
            }

            return (int)x;
        }

        public int Ceil(){
            var x = this._val;
            if (x < 0) {
                x /= LFloat.Precision;
            }
            else {
                if (x % LFloat.Precision == 0) {
                    x /= LFloat.Precision;
                }
                else {
                    x = x / LFloat.Precision + 1;
                }
            }

            return (int)x;
        }
    }
}