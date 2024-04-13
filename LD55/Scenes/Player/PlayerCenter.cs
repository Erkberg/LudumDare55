using Godot;
using System;

public partial class PlayerCenter : Interactable
{
    [Export] public HealthComponent health;

    public override void _Ready()
    {
        health.Died += OnDied;
    }

    private void OnDied()
    {
        GD.Print("GAME OVER!");
    }


    public void Damage(float amount)
    {
        health.Damage(amount);
    }
}
