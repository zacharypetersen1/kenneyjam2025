using Godot;
using System;

public partial class TravelBar : TextureProgressBar
{
    override public void _Process(double delta)
    {
        Value = GameManager.inst.CurTravelDist / GameManager.inst.MaxTravelDist;
    }
}
