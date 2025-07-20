using Godot;
using System;

public partial class Thruster : Node3D
{
    [Export]
    public GpuParticles3D thrusterEffect;
    [Export]
    public Compartment Compartment;
    [Export]
    public float ThrustAmount = 2;
    public bool isActive = false; 

    override public void _Ready()
    {
        thrusterEffect.Emitting = false;
    }

    override public void _Process(double delta)
    {
        bool hasPower = Compartment.HasPower();
        if(!isActive && hasPower)
        {
            GameManager.inst.CurTravelSpeed += ThrustAmount;
            isActive = true;
            thrusterEffect.Emitting = true;
        }
        else if(isActive && !hasPower)
        {
            GameManager.inst.CurTravelSpeed -= ThrustAmount;
            isActive = false;
            thrusterEffect.Emitting = false;
        }
    }
}
