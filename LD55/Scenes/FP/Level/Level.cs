using Godot;
using System;

public partial class Level : Node3D
{
    public static Level inst;

    [Export] public Player player;

    public override void _EnterTree()
    {
        inst = this;
    }
}
