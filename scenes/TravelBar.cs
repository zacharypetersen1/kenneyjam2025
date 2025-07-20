using Godot;
using System;

public partial class TravelBar : TextureProgressBar
{
    float t;
    [Export]
    public Control noThrusterText;

    override public void _Process(double delta)
    {
        t += (float)delta * 5;
        Value = GameManager.inst.CurTravelDist / GameManager.inst.MaxTravelDist;

        if(GameManager.inst.CurTravelSpeed <= 0)
        {
            noThrusterText.Visible = Mathf.Sin(t) > 0;
        }
        else
        {
            noThrusterText.Visible = false;
        }
    }
}
