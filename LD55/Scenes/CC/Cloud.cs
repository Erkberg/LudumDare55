using Godot;
using System;

public partial class Cloud : Area3D
{
    [Export] public float moveSpeed = 1;
    [Export] private Node3D visuals;
    [Export] private Vector3 direction = Vector3.Right;

    public void Init()
    {
        direction = GlobalPosition.X < 0 ? Vector3.Right : Vector3.Left;
    }

    public override void _Process(double delta)
    {
        Move((float)delta);
    }

    private void Move(float delta)
    {
        GlobalPosition += direction * moveSpeed * delta;
    }

    public void Appear()
    {
        visuals.Visible = true;
    }

    public void OnClicked()
    {
        Disappear();
    }

    public void Disappear()
    {
        QueueFree();
    }
}
