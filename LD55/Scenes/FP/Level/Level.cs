using Godot;
using System;

public partial class Level : Node3D
{
    public static Level inst;

    [Export] public Player player;
    [Export] public Node3D summonsHolder;

    public override void _EnterTree()
    {
        inst = this;
    }
}
