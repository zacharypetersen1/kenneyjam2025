using Godot;
using System;

public partial class Compartment : StaticBody3D
{
    public Battery Inserted { get; set; }

    [Export]
    public int PowerDraw = 1;

    public bool HasPower() => Inserted is not null && Inserted.Charge > 0;

    public void Insert(Battery battery)
    {
        Inserted = battery;
    }

    public Battery Remove()
    {
        var battery = Inserted;
        Inserted = null;
        return battery;
    }
}
