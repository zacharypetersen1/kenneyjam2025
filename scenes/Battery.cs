using Godot;
using System;

public partial class Battery : RigidBody3D
{
    [Export]
    public float MoveSpeedModifier = 1;

    public Compartment Compartment { get; set; }

    public double MaxCharge = 100;
    public double Charge = 100;
    public double DischargeRate = 0;

    public void Place(Compartment compartment)
    {
        this.Compartment = compartment;
        compartment.Insert(this);
        DischargeRate = compartment.PowerDraw;
    }

    public void Remove()
    {
        Compartment.Remove();
        Compartment = null;
        DischargeRate = 0;
    }

    public override void _Process(double delta)
    {
        if (DischargeRate != 0) Charge = Math.Clamp(Charge - DischargeRate * delta, 0, MaxCharge);
    }
}
