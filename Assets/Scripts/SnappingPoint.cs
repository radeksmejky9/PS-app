using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SnappingPoint
{
    public string Name;
    public PositionData Position;
    public RotationData Rotation;

    public SnappingPoint(string name = "", float x = 0, float y = 0, float z = 0, float xRotation = 0, float yRotation = 0, float zRotation = 0)
    {
        Name = name;
        Position = new PositionData(x, y, z);
        Rotation = new RotationData(xRotation, yRotation, zRotation);
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
}
