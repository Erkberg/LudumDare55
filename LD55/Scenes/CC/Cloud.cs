using Godot;
using System;

public partial class Cloud : Area3D
{
    [Export] public float moveSpeed = 1;
    [Export] private Node3D visuals;
    [Export] private Vector3 direction = Vector3.Right;
    [Export] private HealthComponent health;
    [Export] private Timer dodgeTimer;
    [Export] private float maxDodgeOffset = 2f;

    private float initialX;
    private MoveMode moveMode;
    private Vector3 moveCenter;
    private float amplitude;
    private float frequency;
    private Vector3 targetPosition;

    public enum MoveMode
    {
        Straight,
        SineUpDown,
        SineFrontBack,
        SineForwardBackward,
        Circle
    }

    public void Init()
    {
        direction = GlobalPosition.X < 0 ? Vector3.Right : Vector3.Left;
        initialX = Mathf.Abs(GlobalPosition.X);
        float newHealth = GD.RandRange(0, 3);
        health.MaxHealth = newHealth;
        health.CurrentHealth = newHealth;
        health.HealthChanged += OnHealthChanged;
        RandomizeMoveMove();
    }

    private void OnHealthChanged()
    {
        Dodge();
    }

    private void RandomizeMoveMove()
    {
        moveCenter = GlobalPosition;
        amplitude = (float)GD.RandRange(32f, 128f);
        frequency = (float)GD.RandRange(0.5f, 8f);
        float rand = GD.Randf();
        float step = 1f / 5f;
        if (rand < step * 1)
            moveMode = MoveMode.Straight;
        else if (rand < step * 2)
            moveMode = MoveMode.SineUpDown;
        else if (rand < step * 3)
            moveMode = MoveMode.SineFrontBack;
        else if (rand < step * 4)
            moveMode = MoveMode.SineForwardBackward;
        else if (rand < step * 5)
            moveMode = MoveMode.Circle;
    }

    private void Dodge()
    {
        if (GD.Randf() < 0.33f)
        {
            direction.X *= -1;
        }

        dodgeTimer.Start();
        Tween tween = CreateTween();
        Vector3 offset = new Vector3((float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset), (float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset), (float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset));
        offset *= Mathf.Sqrt(Mathf.Sqrt(Mathf.Abs(GlobalPosition.Z)));
        if (GlobalPosition.Y + offset.Y < 0)
        {
            offset.Y *= -1;
        }
        tween.TweenProperty(this, "global_position", GlobalPosition + offset, GD.RandRange(0.2f, 0.4f)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenCallback(Callable.From(() => RandomizeMoveMove()));
    }

    public override void _Process(double delta)
    {
        if (dodgeTimer.IsStopped())
        {
            Move((float)delta);
        }

        CheckDeath();
    }

    private void Move(float delta)
    {
        GlobalPosition += direction * moveSpeed * delta;

        targetPosition = GlobalPosition;
        switch (moveMode)
        {
            case MoveMode.Straight:
                break;
            case MoveMode.SineUpDown:
                targetPosition.Y = moveCenter.Y + GetSine() * delta;
                break;
            case MoveMode.SineFrontBack:
                targetPosition.Z = moveCenter.Z + GetSine() * delta;
                break;
            case MoveMode.SineForwardBackward:
                targetPosition.X = moveCenter.X + GetSine() * delta + direction.X * moveSpeed * delta;
                break;
            case MoveMode.Circle:
                break;
        }
        GlobalPosition = GlobalPosition.MoveToward(targetPosition, delta);
    }

    private float GetSine()
    {
        return Mathf.Sin(ChasingClouds.inst.time * frequency) * amplitude;
    }

    private void CheckDeath()
    {
        if (Mathf.Abs(GlobalPosition.X) > initialX * 1.5f)
        {
            Disappear();
        }
    }

    public void Appear()
    {
        visuals.Visible = true;
    }

    public bool OnMouseOver()
    {
        if (dodgeTimer.IsStopped())
        {
            if (health.CurrentHealth > 0)
            {
                health.Damage(1);
            }
            else
            {
                Disappear();
                return true;
            }
        }

        return false;
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
