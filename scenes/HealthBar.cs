using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
    override public void _Process(double delta)
    {
        Value = (float)(GameManager.inst.Health / GameManager.inst.MaxHealth);
    }
}
