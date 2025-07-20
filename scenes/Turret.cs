using Godot;
using System;

public partial class Turret : Node3D
{
    int rocketsFired = 0;
    [Export]
    public float fireRate = 1;
    float t = 0;
    public bool isPowered = false;
    [Export]
    public Compartment compartment;
    [Export]
    public MeshInstance3D light1;
    [Export]
    public MeshInstance3D light2;
    public Node3D[] muzzles = new Node3D[2];
    [Export]
    public PackedScene rocketBlueprint; 
    StandardMaterial3D mat;
    float cooldown;


    override public void _Ready()
    {
        muzzles[0] = GetNode<Node3D>("Muzzle1");
        muzzles[1] = GetNode<Node3D>("Muzzle2");
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
            EnemyShip target = GameManager.inst.GetLowestHPShip();
            if(target is not null)
            {
                LookAt(target.GlobalPosition, new Vector3(0, 1, 0));
                if(cooldown <= 0)
                {
                    Rocket rocket = rocketBlueprint.Instantiate() as Rocket;
                    GetTree().CurrentScene.AddChild(rocket);
                    Node3D muzzle = muzzles[rocketsFired % 2];
                    rocketsFired++;
                    rocket.GlobalPosition = muzzle.GlobalPosition;
                    rocket.GlobalRotation = muzzle.GlobalRotation;
                    rocket.initialPos = muzzle.GlobalPosition;
                    rocket.targetPos = target.GetParent<Node3D>().GlobalPosition;
                    float dist = (rocket.initialPos - rocket.targetPos).Length();
                    rocket.travelTime = dist / rocket.speed;
                    rocket.target = target;
                    rocket.targetSlot = target.index;
                    cooldown = fireRate;
                }
            }
            else
            {
                //Rotation = new();
            }
        }
        else
        {
            //Rotation = new();
        }
    }
}
