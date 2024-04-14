using Godot;
using System;

public partial class Hills : Node3D
{
    [Export] private PackedScene hillScene;
    [Export] private int step = 2;
    [Export] private float maxSpawnRand = 0.33f;

    private ChasingClouds cc;

    public override void _Ready()
    {
        cc = ChasingClouds.inst;
        CreateHills();
    }

    private void CreateHills()
    {
        for (float z = cc.minZ; z <= cc.maxZ; z += step)
        {
            for (float x = -cc.maxX - z * cc.xPerZ; x <= cc.maxX + z * cc.xPerZ; x += step)
            {
                Hill hillInstance = hillScene.Instantiate<Hill>();
                AddChild(hillInstance);
                hillInstance.Name = $"Hill_{x}_{z}";
                hillInstance.GlobalPosition = new Vector3((float)(x + GD.RandRange(-maxSpawnRand, maxSpawnRand)), -0.2f, (float)(-z + GD.RandRange(-maxSpawnRand, maxSpawnRand)));
            }
        }
    }
}
