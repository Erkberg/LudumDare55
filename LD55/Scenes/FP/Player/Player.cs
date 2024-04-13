using ErkbergsGodotLibrary;
using Godot;
using System;

public partial class Player : FirstPersonCharacter3D
{
    [Export] public Shake3D camShake;

    [Export] private RayCast3D forwardRaycast;
    [Export] private Node3D summonTarget;
    [Export] private float maxSummonDistance = 4;
    [Export] private float summonDistanceSafetyMultiplier = 0.9f;

    [ExportGroup("Timers")]
    [Export] private Timer savePositionTimer;
    [Export] private Timer walkTimer;

    [ExportGroup("Summons")]
    [Export] private PackedScene cubeSummonScene;

    private Vector3 savedPosition;
    private int maxSummons = 2;
    private int summonsAvailable = 2;

    public override void _Ready()
    {
        base._Ready();

        savedPosition = GlobalPosition;
        summonsAvailable = maxSummons;
        savePositionTimer.Timeout += OnSavePositionTimerTimeout;
        walkTimer.Timeout += OnWalkTimer;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        UpdateRaycast();
        UpdateSummonTarget();
        CheckSummon();
    }

    private void UpdateRaycast()
    {
        forwardRaycast.TargetPosition = GetCamForward() * maxSummonDistance;
        forwardRaycast.ForceRaycastUpdate();
    }

    private void UpdateSummonTarget()
    {
        if (forwardRaycast.IsColliding())
        {
            Vector3 dir = forwardRaycast.GetCollisionPoint() - camPivot.GlobalPosition;
            summonTarget.GlobalPosition = camPivot.GlobalPosition + dir * summonDistanceSafetyMultiplier;
        }
        else
        {
            summonTarget.GlobalPosition = camPivot.GlobalPosition + GetCamForward() * maxSummonDistance * summonDistanceSafetyMultiplier;
        }
    }

    private void CheckSummon()
    {
        if (Input.IsActionJustPressed(Inputs.Interact))
        {
            if (summonsAvailable > 0)
            {
                SpawnCubeSummon();
            }
        }

        if (Input.IsActionJustPressed(Inputs.Recall))
        {
            if (forwardRaycast.IsColliding() && forwardRaycast.GetCollider() is Summon summon)
            {
                summon.Recall();
                summonsAvailable++;
            }
        }
    }

    private void SpawnCubeSummon()
    {
        Summon cubeSummon = cubeSummonScene.Instantiate() as Summon;
        Level.inst.summonsHolder.AddChild(cubeSummon);
        cubeSummon.GlobalPosition = summonTarget.GlobalPosition;
        summonsAvailable--;
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
