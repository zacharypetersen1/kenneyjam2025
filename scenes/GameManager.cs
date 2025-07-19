using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameManager : Node
{
    public List<Compartment> Compartments = [];

    public double Health = 100;

    public void Initialize()
    {
        Compartments = [.. GetTree().CurrentScene.GetChildren().Where(x => x is Compartment).Select(x => x as Compartment)];
    }

    public override void _Process(double delta)
    {
        GD.Print(Health);
        if (Health > 0)
        {
            foreach (var compartment in Compartments)
            {
                if (!compartment.Drain()) Health = Math.Max(0, Health - delta * compartment.PowerDraw); ;
            }
        }
        else
        {
            GD.Print("Game Over!");
            SetProcess(false);
        }
    }
}
