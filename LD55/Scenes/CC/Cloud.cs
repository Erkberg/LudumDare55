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

    public enum MoveMove
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
        float newHealth = (float)GD.RandRange(0, 3);
        health.MaxHealth = newHealth;
        health.CurrentHealth = newHealth;
        health.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        Dodge();
    }

    private void Dodge()
    {
        dodgeTimer.Start();
        Tween tween = CreateTween();
        Vector3 offset = new Vector3((float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset), (float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset), (float)GD.RandRange(-maxDodgeOffset, maxDodgeOffset));
        offset *= Mathf.Sqrt(Mathf.Sqrt(Mathf.Abs(GlobalPosition.Z)));
        if (GlobalPosition.Y + offset.Y < 0)
        {
            offset.Y *= -1;
        }
        tween.TweenProperty(this, "global_position", GlobalPosition + offset, GD.RandRange(0.2f, 0.4f)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
    }

    public override void _Process(double delta)
    {
        Move((float)delta);
        CheckDeath();
    }

    private void Move(float delta)
    {
        GlobalPosition += direction * moveSpeed * delta;
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

    public void OnMouseOver()
    {
        if (health.CurrentHealth > 0 && dodgeTimer.IsStopped())
        {
            health.Damage(1);
        }
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
