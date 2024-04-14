using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class ChasingClouds : Node3D
{
    public static ChasingClouds inst;

    [ExportGroup("Refs")]
    [Export] public Clouds clouds;
    [Export] public Hills hills;
    [Export] public Camera3D mainCam;
    [Export] private RayCast3D mouseCast;
    [Export] private Timer cloudSpawnTimer;
    [Export] private GpuParticles3D cloudsAbove;
    [Export] private GpuParticles3D rain;

    [ExportGroup("World")]
    [Export] public float minZ = 2, maxZ = 32;
    [Export] public float minY = 0, maxY = 2;
    [Export] public float maxX = 2;
    [Export] public float xPerZ = 1.5f;

    [ExportGroup("Progress")]
    [Export] private float cloudsAmountExponent = 1.33f;
    [Export] private int cloudsDuplicateThreshold = 4;

    [ExportGroup("Audio")]
    [Export] private AudioStreamPlayer atmoPlayer;

    [ExportGroup("UI")]
    [Export] private Control introHolder;
    [Export] private Array<Label> introLabels;

    public float time;
    private int cloudsCollected;


    override public void _EnterTree()
    {
        inst = this;

        cloudsAbove.Emitting = false;
        rain.Emitting = false;

        cloudSpawnTimer.Timeout += OnCloudSpawnTimerTimeout;
    }

    public override void _Ready()
    {
        IntroSequence();
    }

    private async void IntroSequence()
    {
        GetTree().Paused = true;
        await Task.Delay(1000);
        for (int i = 0; i < introLabels.Count; i++)
        {
            Tween tween = CreateTween();
            tween.TweenProperty(introLabels[i], "modulate", Colors.White, 2f);
            await Task.Delay(4000);
        }

        Tween tweenOut = CreateTween();
        tweenOut.TweenProperty(introHolder, "modulate", Colors.Transparent, 2f);
        cloudSpawnTimer.Start();
        GetTree().Paused = false;
    }

    private void OnCloudSpawnTimerTimeout()
    {
        clouds.SpawnCloud();
    }


    public override void _Process(double delta)
    {
        time += (float)delta;
        UpdateMouseCast();
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if (mouseCast.IsColliding())
        {
            if (mouseCast.GetCollider() is Cloud cloud)
            {
                bool collected = cloud.OnMouseOver();

                if (collected)
                {
                    OnCloudCollected();
                    cloud.OnClicked();
                }
            }
            else if (mouseCast.GetCollider() is Hill hill)
            {

            }
            else
            {

            }
        }
    }

    private void UpdateMouseCast()
    {
        Vector2 mousePosition = mainCam.GetViewport().GetMousePosition();
        Vector3 target = mainCam.ProjectRayNormal(mousePosition) * 64;

        mouseCast.GlobalPosition = mainCam.GlobalPosition;
        mouseCast.TargetPosition = target;
        mouseCast.ForceRaycastUpdate();
    }

    public void OnCloudCollected()
    {
        cloudsCollected++;

        if (cloudsCollected % cloudsDuplicateThreshold == 0)
        {
            GpuParticles3D newCloudsAbove = cloudsAbove.Duplicate() as GpuParticles3D;
            newCloudsAbove.Emitting = true;
            cloudsAbove.GetParent().AddChild(newCloudsAbove);

            GpuParticles3D newRain = rain.Duplicate() as GpuParticles3D;
            newRain.Emitting = true;
            rain.GetParent().AddChild(newRain);
        }

        cloudSpawnTimer.WaitTime *= 0.99f;

        atmoPlayer.VolumeDb = -16 * (1f - GetFinishPercentage()) - 8f;
    }

    private float GetFinishPercentage()
    {
        return (float)cloudsCollected / 100f;
    }
}
