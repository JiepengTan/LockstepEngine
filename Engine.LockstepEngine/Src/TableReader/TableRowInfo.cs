// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using Lockstep.Serialization;

public struct TableRowInfo {
    public ushort len;
    public int offset;

    public TableRowInfo(ushort len, int offset){
        this.len = len;
        this.offset = offset;
    }
}


public interface ITableInfo {
    void Init(string name, byte[] bytes);
    void UnLoad();
}


public interface ITableData {
    string Name();
    void ParseData(Deserializer reader, int offset, ushort len);
}