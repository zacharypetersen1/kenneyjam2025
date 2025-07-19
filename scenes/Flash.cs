using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Flash : MeshInstance3D
{
    float t = 0;
    StandardMaterial3D m;
    HashSet<Player> interactablePlayers = new HashSet<Player>();

    public override void _Ready()
    {
        StandardMaterial3D material = GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
        m = material.Duplicate() as StandardMaterial3D;
        m.EmissionEnabled = true;
        m.Emission = new Color(1,1,1,1);
        m.EmissionEnergyMultiplier = 0;
        MaterialOverride = m;
    }

    public override void _Process(double delta)
    {
        if(IsFlashing())
        {
            //t += (float)delta * 10;
            //float energy = Mathf.Sin(t) * 0.5f + 0.5f;
            //m.EmissionEnergyMultiplier = energy *.5f;
            m.EmissionEnergyMultiplier = .4f;
        }
    }

    public void StackFlash(Player player)
    {
        interactablePlayers.Add(player);
    }

    public void UnstackFlash(Player player)
    {
        interactablePlayers.Remove(player);
        if(interactablePlayers.Count == 0)
        {
            t = 0;
            m.EmissionEnergyMultiplier = 0;
        }
    }

    public bool IsFlashing()
    {
        return interactablePlayers.Count > 0;
    }

    public static void SetFlashOnThisAndChildren(Node3D node, bool value, Player player)
    {
        if(node is Flash flash)
        {
            if(value)
            {
                flash.StackFlash(player);
            }
            else
            {
                flash.UnstackFlash(player);
            }
        }                    
        foreach (Node3D child in node.GetChildren().Where(x => x is Node3D))
        {
            SetFlashOnThisAndChildren(child, value, player);
        }
    }
}
