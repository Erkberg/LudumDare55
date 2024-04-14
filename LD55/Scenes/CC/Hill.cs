using Godot;
using System;

public partial class Hill : CsgSphere3D
{
    public void Init()
    {
        Vector3 scale = Scale;
        float zMultiplier = Mathf.Sqrt(Mathf.Sqrt(Mathf.Abs(GlobalPosition.Z)));
        scale.X *= (float)GD.RandRange(0.8f, zMultiplier);
        scale.Y *= (float)GD.RandRange(0.8f, zMultiplier * 2.4f);
        scale.Z *= (float)GD.RandRange(0.8f, zMultiplier);
        Scale = scale;
    }
}
