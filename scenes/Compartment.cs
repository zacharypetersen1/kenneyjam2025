using Godot;
using System;

public partial class Compartment : StaticBody3D
{
    public Interactable Inserted { get; set; }

    [Export]
    public int PowerDraw = 1;

    public double PowerRequired = 0;

    public bool Active = false;

    public void Queue(int delay, int powerRequired)
    {
        var timer = GetNode<Timer>("Timer");
        timer.Start(delay);
        PowerRequired = powerRequired;
    }

    public void Activate()
    {
        Active = true;
        GD.Print("Die Humans!");
    }

    public void Deactivate()
    {
        Active = false;
        GD.Print("All Done!");
    }

    public bool CanDrain() => Inserted is not null && Inserted.Charge > 0;

    public bool Drain(double delta)
    {
        PowerRequired -= PowerDraw * delta;
        return PowerRequired <= 0;
    }
}
