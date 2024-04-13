using Godot;
using System;

public partial class Progress : Node
{
    [Export] public float currency;

    public void AddCurrency(float amount)
    {
        currency += amount;
        Main.inst.ingameUi.SetCurrency(currency);
    }

    public void SubtractCurrency(float amount)
    {
        currency -= amount;
        Main.inst.ingameUi.SetCurrency(currency);
    }

    public bool CanAfford(float amount)
    {
        return currency >= amount;
    }
}
