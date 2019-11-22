// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lockstep.Logging;
using Lockstep.Serialization;


public class TableInfo<T> : ITableInfo where T : ITableData, new() {
    public string Name { get; private set; }
    protected Dictionary<ulong, TableRowInfo> key2RowInfo;
    protected Dictionary<ulong, T> key2Data;
    protected Deserializer reader;
    public ushort RowsCount;

    public Dictionary<ulong, T> GetAllData(){
        return key2Data;
    }

    public void Init(string name, byte[] bytes){
        Name = name;
        key2Data = new Dictionary<ulong, T>();
        reader = new Deserializer(bytes);
        RowsCount = reader.ReadUInt16();
        var headOffset = reader.ReadInt32();
        key2RowInfo = new Dictionary<ulong, TableRowInfo>(RowsCount);
        reader.SetPosition(headOffset);
        for (int i = 0; i < RowsCount; i++) {
            var key = reader.ReadUInt64();
            var offset = reader.ReadInt32();
            var len = reader.ReadUInt16();
            key2RowInfo.Add(key, new TableRowInfo(len, offset));
        }
    }

    public void UnLoad(){
        reader = null;
        key2RowInfo = null;
        key2Data = null;
    }

    public void LoadAll(){
        if (RowsCount == key2Data.Count) return;
        foreach (var key in key2RowInfo.Keys) {
            if (!key2Data.ContainsKey(key)) {
                Load(key);
            }
        }
    }

    public bool HasData(ulong key){
        return key2RowInfo.ContainsKey(key);
    }

    public T GetOrLoad(ulong key){
        if (key2Data.TryGetValue(key, out T val)) {
            return val;
        }
        return Load(key);
    }

    protected T Load(ulong key){
        Debug.Assert(key2RowInfo.ContainsKey(key), "Table do not have key= " + key);
        if (key2RowInfo.TryGetValue(key, out var data)) {
            T val = new T();
            val.ParseData(reader, data.offset, data.len);
            key2Data.Add(key, val);
            return val;
        }

        return default(T);
    }
}