using Godot;
using System;

public partial class Summon : RigidBody3D
{
    public void Recall()
    {
        QueueFree();
    }
}
