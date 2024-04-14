using Godot;
using System;

public partial class Clouds : Node3D
{
    [Export] private PackedScene cloudScene;

    private int cloudsSpawned = 0;
    private ChasingClouds cc;

    public override void _Ready()
    {
        cc = ChasingClouds.inst;
    }

    public void SpawnCloud()
    {
        Cloud cloudInstance = cloudScene.Instantiate<Cloud>();
        AddChild(cloudInstance);
        cloudInstance.Name = $"Cloud_{cloudsSpawned}";
        cloudInstance.GlobalPosition = GetRandomSpawnPosition();
        cloudInstance.moveSpeed *= Mathf.Sqrt(Mathf.Abs(cloudInstance.GlobalPosition.Z)) * (float)GD.RandRange(0.8f, 1.6f);
        cloudInstance.Scale /= Mathf.Sqrt(Mathf.Sqrt(Mathf.Abs(cloudInstance.GlobalPosition.Z)));
        cloudInstance.Scale *= (float)GD.RandRange(0.8f, 1.6f);
        cloudInstance.Init();
        cloudsSpawned++;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = Vector3.Zero;
        pos.Z = (float)-GD.RandRange(cc.minZ + 4, cc.maxZ);
        pos.Y = (float)GD.RandRange(cc.minY, cc.maxY);
        pos.X = cc.maxX + pos.Z * cc.xPerZ;
        pos.X = GD.Randf() < 0.5f ? -pos.X : pos.X;

        return pos;
    }
}
