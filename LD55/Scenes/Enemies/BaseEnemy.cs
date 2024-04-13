using Godot;
using System;

public partial class BaseEnemy : Area3D
{
    [Export] public EnemyType type;
    [Export] public float moveSpeed = 1;

    private Vector3 playerCenterPosition;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        playerCenterPosition = Main.inst.playerCenter.GlobalPosition;
    }

    public override void _Process(double delta)
    {
        Move(delta);
    }

    protected virtual void Move(double delta)
    {
        Vector3 direction = (playerCenterPosition - GlobalPosition).Normalized();
        GlobalPosition += direction * moveSpeed * (float)delta;
    }

    private void OnAreaEntered(Area3D area)
    {
        if (area is PlayerCenter)
        {
            GD.Print("damage!");
            QueueFree();
        }
    }

}
