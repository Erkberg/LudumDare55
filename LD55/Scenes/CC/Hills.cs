using Godot;
using System;

public partial class Hills : Node3D
{
    [Export] private PackedScene hillScene;
    [Export] private int minZ, maxZ;
    [Export] private int maxX;
    [Export] private int xPerZ = 1;
    [Export] private int step = 2;
    [Export] private float maxSpawnRand = 0.33f;

    public override void _Ready()
    {
        CreateHills();
    }

    private void CreateHills()
    {
        for (int z = minZ; z <= maxZ; z += step)
        {
            for (int x = -maxX - z * xPerZ; x <= maxX + z * xPerZ; x += step)
            {
                Hill hillInstance = hillScene.Instantiate<Hill>();
                AddChild(hillInstance);
                hillInstance.Name = $"Hill_{x}_{z}";
                hillInstance.GlobalPosition = new Vector3((float)(x + GD.RandRange(-maxSpawnRand, maxSpawnRand)), -0.2f, (float)(-z + GD.RandRange(-maxSpawnRand, maxSpawnRand)));
            }
        }
    }
}
