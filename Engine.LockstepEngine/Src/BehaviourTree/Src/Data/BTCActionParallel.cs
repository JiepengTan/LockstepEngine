// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

#pragma warning disable 0169
using System;
using System.Runtime.InteropServices;
using Lockstep.Util;

namespace Lockstep.BehaviourTree {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe partial struct BTCActionParallel:IBTContent {
        public struct BTCParallelStatusEval {
            public const int Size = 16;
            public fixed bool status[Size];

            public void Init(bool val){
                for (int i = 0; i < Size; i++) {
                   fixed(bool* p = status)  p[i] = val;
                }
            }

            void CheckIdx(Int32 index){
                if (index < 0 || index >= Size) {
                    NativeHelper.ArrayOutOfRange();
                }
            }

            public bool this[int index] {
                get {
                    CheckIdx(index);
                    fixed (bool* p = status) return p[index];
                }
                set {
                    CheckIdx(index);
                    fixed(bool* p = status)  p[index] = value;
                }
            }
        }

        public unsafe struct BTCParallelStatusRunning {
            public const int Size = 16;
            public fixed byte status[Size];

            public void Init(byte val){
                for (int i = 0; i < Size; i++) {
                    fixed(byte* p = status)  p[i] = val;
                }
            }

            void CheckIdx(Int32 index){
                if (index < 0 || index >= Size) {
                    NativeHelper.ArrayOutOfRange();
                }
            }

            public byte this[int index] {
                get {
                    CheckIdx(index);
                    fixed (byte* p = status) return p[index];
                }
                set {
                    CheckIdx(index);
                    fixed(byte* p = status)  p[index] = value;
                }
            }
        }

        internal BTCParallelStatusEval evaluationStatus;
        internal BTCParallelStatusRunning StatusRunning;
    }
}