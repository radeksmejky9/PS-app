using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SnappingPoint
{
    public string Building;
    public string Room;
    public PositionData Position;
    public RotationData Rotation;

    public string Url;

    public SnappingPoint(string building = "", string room = "", float x = 0, float y = 0, float z = 0, float xRotation = 0, float yRotation = 0, float zRotation = 0, string url = "")
    {
        Building = building;
        Room = room;
        Position = new PositionData(x, y, z);
        Rotation = new RotationData(xRotation, yRotation, zRotation);
        Url = url;
    }
    public SnappingPoint(string building = "", string room = "", string url = "")
    {
        Building = building;
        Room = room;
        Position = new PositionData(0, 0, 0);
        Rotation = new RotationData(0, 0, 0);
        Url = url;
    }
    public SnappingPoint(string building, string name, PositionData posistion, RotationData rotation, string url)
    {
        Building = building;
        Room = name;
        Position = posistion;
        Rotation = rotation;
        Url = url;
    }


}
[System.Serializable]
public struct PositionData
{
    public float X;
    public float Y;
    public float Z;

    public PositionData(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

[System.Serializable]
public struct RotationData
{
    public float X;
    public float Y;
    public float Z;

    public RotationData(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
