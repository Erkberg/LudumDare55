using ErkbergsGodotLibrary;
using Godot;
using System;

public partial class Main : Node3D
{
    public static Main inst;

    [Export] public Vector2 worldBounds;
    [Export] public PlayerCenter playerCenter;
    [Export] public Progress progress;
    [Export] public IngameUi ingameUi;
    [Export] public Shake3D camShake;

    public override void _EnterTree()
    {
        inst = this;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = Vector3.Zero;

        if (GD.Randf() < 0.5f)
        {
            // left / right
            pos.X = GD.Randf() < 0.5f ? -worldBounds.X : worldBounds.X;
            pos.Y = GD.Randf() * worldBounds.Y;
        }
        else
        {
            // top / bottom
            pos.X = GD.Randf() * worldBounds.X;
            pos.Y = GD.Randf() < 0.5f ? -worldBounds.Y : worldBounds.Y;
        }

        return pos;
    }
}
