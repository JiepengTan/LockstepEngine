// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using Lockstep.Logging;
using Lockstep.Math;
using Random = Lockstep.Math.Random;

namespace Lockstep.Game {
    public partial class RandomService : IRandomService ,ITimeMachine{
        Random _i = new Math.Random();
        public LFloat value => _i.value;

        public uint Next(){
            return _i.Next();
        }

        public uint Next(uint max){
            return _i.Next(max);
        }

        public int Next(int max){
            return _i.Next(max);
        }

        public uint Range(uint min, uint max){
            return _i.Range(min, max);
        }

        public int Range(int min, int max){
            return _i.Range(min, max);
        }

        public LFloat Range(LFloat min, LFloat max){
            return _i.Range(min, max);
        }

        public int CurTick { get; set; }
        Dictionary<int, ulong> _tick2Id = new Dictionary<int, ulong>();

        public void RollbackTo(int tick){
            _i.randSeed = _tick2Id[tick];
        }

        public void Backup(int tick){
            _tick2Id[tick] = _i.randSeed;
        }

        public void Clean(int maxVerifiedTick){
            
        }
    }
    
}