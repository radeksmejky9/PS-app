using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnappingPoint
{
    public string Building;
    public string Room;
    public PositionData Position;
    public RotationData Rotation;

    public string Url;

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
public class PositionData
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
public class RotationData
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
