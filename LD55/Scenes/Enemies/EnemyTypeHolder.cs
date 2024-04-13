using Godot;
using System;

public partial class EnemyTypeHolder : Node
{
    [Export] private EnemyType type;
    [Export] private PackedScene enemyScene;
    [Export] private Timer spawnTimer;
    [Export] private float baseSpawnTime = 1;

    public override void _Ready()
    {
        spawnTimer.Timeout += SpawnEnemy;
        Enable();
    }

    public void Enable()
    {
        spawnTimer.Start();
    }

    private void SpawnEnemy()
    {
        BaseEnemy enemyInstance = enemyScene.Instantiate() as BaseEnemy;
        AddChild(enemyInstance);
        enemyInstance.GlobalPosition = Main.inst.GetRandomSpawnPosition();
    }
}
