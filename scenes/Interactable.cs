using Godot;
using System;

public partial class Interactable : RigidBody3D
{
    public bool isPlaced = false;
    public Compartment compartment = null;

    public void Place(Compartment compartment)
    {
        GD.Print("Placed in compartment");
        isPlaced = true;
        this.compartment = compartment;
        compartment.isFilled = true;
    }

    public void Remove()
    {
        compartment.RemoveChild(this);
        isPlaced = false;
        compartment.isFilled = false;
    }
}
