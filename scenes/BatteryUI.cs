using Godot;
using System;

public partial class BatteryUI : Node3D
{
    [Export]
    public Node3D scaler;
    [Export]
    public Sprite3D barFG;
    [Export]
    public Battery battery;
    [Export]
    public Sprite3D arrowUp;
    [Export]
    public Sprite3D arrowDown;

    override public void _Process(double delta)
    {
        Vector3 cameraPos = GetViewport().GetCamera3D().GlobalPosition;
        GlobalPosition = battery.GlobalPosition + new Vector3(0, 0.6f, 0);
        Vector3 lookDir = (cameraPos - GlobalPosition).Normalized();
        LookAt(cameraPos, lookDir.Cross(new Vector3(1, 0, 0)));
        float chargePercent = (float) (battery.Charge / battery.MaxCharge);
        scaler.Scale = new Vector3(chargePercent, 1, 1);
        if(chargePercent > 0.35f)
        {
            barFG.Modulate = new Color(1,1,0,1);
        }
        else
        {
            barFG.Modulate = new Color(1,0.5f,0.5f,1);
        }
        arrowUp.Visible = false;
        arrowDown.Visible = false;
        if(battery.Charge > 0 && battery.DischargeRate > 0)
        {
            arrowDown.Visible = true;
        }
        if(battery.Charge < battery.MaxCharge && battery.DischargeRate < 0)
        {
            arrowUp.Visible = true;
        }
    }
}
