using ErkbergsGodotLibrary;
using Godot;
using System;

public partial class Player : FirstPersonCharacter3D
{
    [Export] public Shake3D camShake;

    [ExportGroup("Timers")]
    [Export] private Timer savePositionTimer;
    [Export] private Timer walkTimer;

    private Vector3 savedPosition;

    public override void _Ready()
    {
        base._Ready();

        savedPosition = GlobalPosition;
        savePositionTimer.Timeout += OnSavePositionTimerTimeout;
        walkTimer.Timeout += OnWalkTimer;
    }

    private void OnSavePositionTimerTimeout()
    {
        if (IsOnFloor())
        {
            savedPosition = GlobalPosition;
        }
    }

    private void OnWalkTimer()
    {
        if (IsOnFloor() && Velocity != Vector3.Zero)
        {
            // play walk sound
        }
    }

    protected override void JustLanded()
    {
        CamBopOnLand(Mathf.Abs(previousVelocity.Y));
    }

    private void CamBopOnLand(float veloY)
    {
        Vector3 initialPosition = camPivot.Position;
        Tween tween = CreateTween();
        float offsetY = Mathf.Pow(veloY, 1f / 16f) - 1f;
        Vector3 offset = Vector3.Down * offsetY;
        tween.TweenProperty(camPivot, "position", initialPosition + offset, 0.08f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(camPivot, "position", initialPosition, 0.16f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    }
}
