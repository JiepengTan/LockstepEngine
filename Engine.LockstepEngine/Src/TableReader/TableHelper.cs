// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;

public class TableHelper
{
    public static string GetLanguageValue(string contentCN, string tableName)
    {
        return "";
    }

    public static ulong GetKey(int key1)
    {
        return Convert.ToUInt64(key1);
    }

    public static ulong GetKey(int key1, int key2)
    {
        return (((ulong)key1 & 0xffffffff) | (((ulong)key2 & 0xffffffff) << 32));
    }

    public static ulong GetKey(int key1, int key2, int key3)
    {
        short shortKey2 = Convert.ToInt16(key2);
        short shortKey3 = Convert.ToInt16(key3);
        return (((ulong)key1 & 0xffffffff) | (((ulong)shortKey2 & 0xffff) << 32) | (((ulong)shortKey3 & 0xffff) << 48));
    }


    public static ulong GetKey(int key1, int key2, int key3, int key4)
    {
        short shortKey1 = Convert.ToInt16(key1);
        short shortKey2 = Convert.ToInt16(key2);
        short shortKey3 = Convert.ToInt16(key3);
        short shortKey4 = Convert.ToInt16(key4);
        return (((ulong)shortKey1 & 0xffff) | (((ulong)shortKey2 & 0xffff) << 16) | (((ulong)shortKey3 & 0xffff) << 32) | (((ulong)shortKey4 & 0xffff) << 48));
    }
}