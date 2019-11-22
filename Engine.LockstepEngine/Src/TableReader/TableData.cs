// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using Lockstep.Serialization;

public class TableData<T> : ITableData where T : ITableData, new() {
    private static readonly T _instance = new T();
    private static TableInfo<T> _tableInfo;

    public static int RowCount => _tableInfo.RowsCount;

    public static void Load(){
        if (_tableInfo != null) return;
        _tableInfo = TableManager.GetTableInfo<TableInfo<T>>(_instance.Name());
    }

    public static void LoadAll(){
        Load();
        _tableInfo.LoadAll();
    }

    public static void UnLoad(bool isRemove = true){
        if (_tableInfo == null) return;
        _tableInfo.UnLoad();
        _tableInfo = null;
    }

    public static T GetData(int key1){
        ulong keyID = TableHelper.GetKey(key1);
        return _tableInfo.GetOrLoad(keyID);
    }

    public static T GetData(int key1, int key2){
        ulong keyID = TableHelper.GetKey(key1, key2);
        return _tableInfo.GetOrLoad(keyID);
    }

    public static T GetData(int key1, int key2, int key3){
        ulong keyID = TableHelper.GetKey(key1, key2, key3);
        return _tableInfo.GetOrLoad(keyID);
    }

    public static T GetData(int key1, int key2, int key3, int key4){
        ulong keyID = TableHelper.GetKey(key1, key2, key3, key4);
        return _tableInfo.GetOrLoad(keyID);
    }

    public static bool HasData(int key1){
        ulong keyID = TableHelper.GetKey(key1);
        return _tableInfo.HasData(keyID);
    }

    public static bool HasData(int key1, int key2){
        ulong keyID = TableHelper.GetKey(key1, key2);
        return _tableInfo.HasData(keyID);
    }

    public static bool HasData(int key1, int key2, int key3){
        ulong keyID = TableHelper.GetKey(key1, key2, key3);
        return _tableInfo.HasData(keyID);
    }

    public static bool HasData(int key1, int key2, int key3, int key4){
        ulong keyID = TableHelper.GetKey(key1, key2, key3, key4);
        return _tableInfo.HasData(keyID);
    }

    public static Dictionary<ulong, T> GetAllData(){
        _tableInfo.LoadAll();
        return _tableInfo.GetAllData();
    }

    public void ParseData(Deserializer reader, int offset, ushort len){
        reader.SetPosition(offset);
        try {
            DoParseData(reader);
        }
        catch (Exception e) {
            throw new Exception(
                $" {Name()} DoParseData Data Error  offset+Len:{offset} + {len} != Position:{reader.Position}");
        }

        if (reader.Position != offset + len) {
            throw new Exception(
                $" {Name()} Parse Data Error  offset+Len:{offset} + {len} != Position:{reader.Position}");
        }
    }

    public virtual string Name(){
        return "";
    }

    protected virtual void DoParseData(Deserializer reader){ }
}