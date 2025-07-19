using Godot;
using System;

public partial class Compartment : StaticBody3D
{
    public Interactable Inserted { get; set; }

    [Export]
    public int PowerDraw = 1;

    public bool Drain() => Inserted is not null && Inserted.Charge > 0;
}
