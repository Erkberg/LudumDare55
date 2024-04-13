using Godot;
using System;

public partial class IngameUi : Control
{
    [Export] private Label currencyLabel;

    public void SetCurrency(float amount)
    {
        currencyLabel.Text = amount.ToString();
    }
}
