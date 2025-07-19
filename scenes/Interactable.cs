using Godot;
using System;

public partial class Interactable : RigidBody3D
{
    public bool isPlaced = false;
    public Compartment compartment = null;

    public double MaxCharge = 100;
    public double Charge = 100;
    public double DischargeRate = 0;

    public void Place(Compartment compartment)
    {
        isPlaced = true;
        this.compartment = compartment;
        compartment.Inserted = this;
        DischargeRate = compartment.PowerDraw;
    }

    public void Remove()
    {
        compartment.RemoveChild(this);
        isPlaced = false;
        compartment.Inserted = null;
        DischargeRate = 0;
        GD.Print(Charge);
    }

    public override void _Process(double delta)
    {
        if (DischargeRate != 0) Charge = Math.Max(0, Charge - DischargeRate * delta);
    }
}
