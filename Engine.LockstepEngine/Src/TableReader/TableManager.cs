// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using System.IO;

public class TableManager {
    private static string _relPath;
    private static string _postfix = ".bytes";
    private static Dictionary<string, ITableInfo> _tableInfoDic = new Dictionary<string, ITableInfo>();

    public static void Init(string relPath,string postfix = ".bytes"){
        _relPath = relPath;
        _postfix = postfix;
    }

    public static T GetTableInfo<T>(string tableName) where T : ITableInfo, new(){
        T tableInfo;
        if (_tableInfoDic.ContainsKey(tableName)) {
            tableInfo = (T) _tableInfoDic[tableName];
        }
        else {
            tableInfo = new T();
            byte[] bytes = GetBytes(tableName);
            if (bytes == null) {
                return default(T);
            }
            tableInfo.Init(tableName, bytes);
            _tableInfoDic.Add(tableName, tableInfo);
        }

        return tableInfo;
    }

    private static byte[] GetBytes(string tableName){
        return File.ReadAllBytes(_relPath + tableName + _postfix);
    }

    public static void UnLoadData(string tableName){
        if (_tableInfoDic.ContainsKey(tableName)) {
            _tableInfoDic[tableName].UnLoad();
            _tableInfoDic.Remove(tableName);
        }
    }

    public static void UnLoadALlTable(){
        foreach (var key in _tableInfoDic.Keys) {
            UnLoadData(key);
        }
        _tableInfoDic.Clear();
    }
}