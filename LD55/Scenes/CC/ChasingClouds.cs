using Godot;
using System;

public partial class ChasingClouds : Node3D
{
    public static ChasingClouds inst;

    [Export] public Clouds clouds;
    [Export] public Hills hills;
    [Export] public Camera3D mainCam;
    [Export] private RayCast3D mouseCast;
    [Export] private GpuParticles3D cloudsAbove;
    [Export] private GpuParticles3D rain;
    [Export] private float cloudsAmountExponent = 1.33f;
    [Export] private int rainPerCloud = 4;

    private int cloudsCollected;


    override public void _EnterTree()
    {
        inst = this;

        cloudsAbove.Emitting = false;
        rain.Emitting = false;
    }

    public override void _Process(double delta)
    {
        UpdateMouseCast();
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if (mouseCast.IsColliding())
        {
            if (mouseCast.GetCollider() is Cloud cloud)
            {
                if (Input.IsActionJustPressed(Inputs.Interact))
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

        cloudsAbove.Emitting = true;
        rain.Emitting = true;

        cloudsAbove.Amount = Mathf.RoundToInt(Mathf.Pow(cloudsCollected, cloudsAmountExponent));
        rain.Amount = cloudsAbove.Amount * rainPerCloud;

        GD.Print($"Collected: {cloudsCollected}, cloud particles: {cloudsAbove.Amount}, rain particles: {rain.Amount}");
    }
}
