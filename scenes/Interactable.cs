using Godot;
using System;

public partial class Interactable : RigidBody3D
{
    public Compartment Compartment { get; set; }

    public double MaxCharge = 100;
    public double Charge = 100;
    public double DischargeRate = 0;

    public void Place(Compartment compartment)
    {
        this.Compartment = compartment;
        compartment.Inserted = this;
        DischargeRate = compartment.PowerDraw;
    }

    public void Remove()
    {
        Compartment.Inserted = null;
        DischargeRate = 0;
        GD.Print(Charge);
    }

    public override void _Process(double delta)
    {
        if (DischargeRate != 0) Charge = Math.Max(0, Charge - DischargeRate * delta);
    }
}
