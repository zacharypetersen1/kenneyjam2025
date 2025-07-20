using Godot;
using System;

public partial class UIProgressBar : Node3D
{
    [Export]
    public Color color;
    [Export]
    public Node3D scaler;
    [Export]
    public Sprite3D barFG;

    override public void _Ready()
    {
        barFG.Modulate = color;
    }

    public void SetProgress(float progress)
    {
        scaler.Scale = new Vector3(progress, 1, 1);
    }

    override public void _Process(double delta)
    {
        Vector3 cameraPos = GetViewport().GetCamera3D().GlobalPosition;
        Vector3 lookDir = (cameraPos - GlobalPosition).Normalized();
        LookAt(cameraPos, lookDir.Cross(new Vector3(1, 0, 0)));
    }
}
