using Godot;
using System;

public partial class BaseSummon : Area3D
{
    [Export] public SummonType type;
    [Export] public float moveSpeed = 1;
    [Export] public float damage = 1;

    private Vector3 playerCenterPosition;

    public override void _Ready()
    {
        playerCenterPosition = Main.inst.playerCenter.GlobalPosition;

        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area3D area)
    {

    }


    public override void _Process(double delta)
    {
        Move(delta);
    }

    protected virtual void Move(double delta)
    {
        Vector3 direction = GlobalPosition - playerCenterPosition;
        GlobalPosition = direction.Rotated(Vector3.Back, moveSpeed * (float)delta);
    }
}
