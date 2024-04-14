using Godot;
using System;

public partial class Cloud : Area3D
{
    [Export] private Node3D visuals;

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
