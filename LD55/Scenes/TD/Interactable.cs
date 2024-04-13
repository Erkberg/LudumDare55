using Godot;
using System;

public partial class Interactable : Area3D
{
    public bool mouseOver;
    protected bool isPressed;

    public override void _Process(double delta)
    {
        if (mouseOver)
        {
            if (Input.IsActionJustPressed(Inputs.Interact))
            {
                isPressed = true;
                OnMousePressed();
            }
            else if (Input.IsActionJustReleased(Inputs.Interact))
            {
                isPressed = false;
                OnMouseReleased();
            }

            if (isPressed)
            {
                OnMouseHeld();
            }
        }
    }

    public override void _MouseEnter()
    {
        mouseOver = true;
        OnMouseEnter();
    }

    public override void _MouseExit()
    {
        mouseOver = false;
        isPressed = false;
        OnMouseExit();
    }

    protected virtual void OnMouseEnter() { }
    protected virtual void OnMouseExit() { }
    protected virtual void OnMousePressed() { }
    protected virtual void OnMouseReleased() { }
    protected virtual void OnMouseHeld() { }
}
