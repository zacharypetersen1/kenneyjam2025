using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Flash : MeshInstance3D
{
    StandardMaterial3D FlashMaterial = new()
    {
        Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
        AlbedoColor = new(0, 0, 0, 0.5f),
        EmissionEnabled = true,
        Emission = new(0, 1, 1, 1),
    };

    HashSet<Player> interactablePlayers = new HashSet<Player>();

    public override void _Process(double delta)
    {
        if(IsFlashing())
        {
            MaterialOverlay = FlashMaterial;
        }
    }

    public void StackFlash(Player player)
    {
        interactablePlayers.Add(player);
    }

    public void UnstackFlash(Player player)
    {
        interactablePlayers.Remove(player);
        if (interactablePlayers.Count == 0)
        {
            MaterialOverlay = null;
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
