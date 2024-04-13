using Godot;
using System;

public partial class IngameUi : Control
{
    [Export] private Label currencyLabel;
    [Export] private ProgressBar centerHealthBar;

    public void SetCurrency(float amount)
    {
        currencyLabel.Text = amount.ToString();
    }

    public void SetHealth(float current, float max)
    {
        centerHealthBar.Value = current;
    }
}
