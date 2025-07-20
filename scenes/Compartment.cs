using Godot;
using System;

public partial class Compartment : StaticBody3D
{
    public Battery Inserted { get; set; }

    [Export]
    public Node3D BatteryLocation;

    [Export]
    public int PowerDraw = 1;

    public bool HasPower() => Inserted is not null && Inserted.Charge > 0;

    public void Insert(Battery battery)
    {
        Inserted = battery;
        battery.GetParent().RemoveChild(battery);
        BatteryLocation.AddChild(battery);
        battery.Position = new();
    }

    public Battery Remove()
    {
        var battery = Inserted;
        Inserted = null;
        return battery;
    }
}
