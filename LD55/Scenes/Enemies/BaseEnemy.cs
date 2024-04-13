using Godot;
using System;

public partial class BaseEnemy : Interactable
{
    [Export] public EnemyType type;
    [Export] public float moveSpeed = 1;
    [Export] public float damage = 1;
    [Export] public float value = 1;
    [Export] public HealthComponent health;

    private Vector3 playerCenterPosition;

    public override void _Ready()
    {
        playerCenterPosition = Main.inst.playerCenter.GlobalPosition;

        AreaEntered += OnAreaEntered;
        health.Died += OnDied;
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

    protected override void OnMouseEnter()
    {
        health.Damage(1);
    }

    protected virtual void OnDied()
    {
        Main.inst.progress.AddCurrency(value);
        QueueFree();
    }

    private void OnAreaEntered(Area3D area)
    {
        if (area is PlayerCenter playerCenter)
        {
            playerCenter.Damage(damage);
            OnDied();
        }
    }

}
