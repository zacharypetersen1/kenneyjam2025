using Godot;
using System;

public partial class Flash : MeshInstance3D
{
    float t = 0;
    StandardMaterial3D m;
    int flashCount = 0;

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
            t += (float)delta * 10;
            float energy = Mathf.Sin(t) * 0.5f + 0.5f;
            GD.Print(t);
            GD.Print(energy);
            m.EmissionEnergyMultiplier = energy *.5f;
        }
    }

    public void StackFlash()
    {
        flashCount++;
    }

    public void UnstackFlash()
    {
        flashCount--;
        if(flashCount == 0)
        {
            t = 0;
            m.EmissionEnergyMultiplier = 0;
        }
    }

    public bool IsFlashing()
    {
        return flashCount > 0;
    }
}
