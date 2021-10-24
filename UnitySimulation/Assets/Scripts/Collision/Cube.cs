using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cube
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
}

[Serializable]
public class CubeSerializer
{
    public CubeSerializer(List<Cube> cubes)
    {
        this.Cubes = cubes;
    }
    public List<Cube> Cubes;
}