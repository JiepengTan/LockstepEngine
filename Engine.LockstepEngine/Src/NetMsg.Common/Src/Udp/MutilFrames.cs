// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Diagnostics;
using Lockstep.Serialization;

namespace NetMsg.Common {
    [SelfImplement]
    [Udp]
    [Serializable]
    public partial class MutilFrames : BaseMsg {
        public int startTick;
        public ServerFrame[] frames;

        public override void Serialize(Serializer writer){
            writer.Write(startTick);
            var count = (ushort) frames.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++) {
                Debug.Assert(frames[i].Tick == startTick + i, "Frame error");
                frames[i].BeforeSerialize();
                writer.Write(frames[i].InputDatas);
            }
        }

        public override void Deserialize(Deserializer reader){
            startTick = reader.ReadInt32();
            var tickCount = reader.ReadUInt16();
            frames = new ServerFrame[tickCount];
            for (int i = 0; i < tickCount; i++) {
                var frame = new ServerFrame();
                frame.Tick = startTick + i;
                frame.InputDatas = reader.ReadBytes();
                frame.AfterDeserialize();
                frames[i] = frame;
            }
        }
    }
}