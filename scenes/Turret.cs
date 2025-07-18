using Godot;
using System;

public partial class Turret : Node3D
{
    [Export]
    public float fireRate = 1;
    [Export]
    public ThreatDir aimDir;
    float t = 0;
    public bool isPowered = false;
    [Export]
    public Compartment compartment;
    [Export]
    public MeshInstance3D light1;
    [Export]
    public MeshInstance3D light2;
    StandardMaterial3D mat;
    float cooldown;


    override public void _Ready()
    {
        StandardMaterial3D material = light1.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
        mat = material.Duplicate() as StandardMaterial3D;
        light1.MaterialOverride = mat;
        light2.MaterialOverride = mat;
    }

    override public void _Process(double delta)
    {
        t += (float)delta * 5;
        isPowered = compartment.Inserted != null && compartment.Inserted.Charge > 0;
        if(isPowered)
        {
            mat.Emission = new Color(0,1,0,1);
            mat.EmissionEnergyMultiplier = 1;
        }
        else
        {
            mat.Emission = new Color(1,0,0,1);
            mat.EmissionEnergyMultiplier = Mathf.Sin(t) > 0 ? 1 : 0;
        }

        if(isPowered)
        {
            cooldown = Mathf.Max(0, cooldown - (float)delta);
            if(cooldown <= 0)
            {
                EnemyShip target = GameManager.inst.GetLowestHPShipInDir(aimDir);
                if(target is not null)
                {
                    target.TakeDamage(1);
                    cooldown = fireRate;
                }
            }
        }
    }
}
