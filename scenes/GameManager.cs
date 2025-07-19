using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameManager : Node
{
    public List<Compartment> InactivateCompartments = [];

    public List<Compartment> ActiveCompartments = [];

    public double Health = 100;

    public void Initialize()
    {
        InactivateCompartments = [.. GetTree().CurrentScene.GetChildren().Where(x => x is Compartment).Select(x => x as Compartment)];
    }

    public override void _Process(double delta)
    {
        //GD.Print(Health);
        if (Health > 0)
        {
            if (ActiveCompartments.Count == 0 && InactivateCompartments.Count > 0) ActivateNext();
            for (var i = 0; i < ActiveCompartments.Count; i++)
            {
                var compartment = ActiveCompartments[i];
                if (compartment.Active) {
                    if (!compartment.CanDrain()) Health = Math.Max(0, Health - delta * compartment.PowerDraw);
                    else if (compartment.Drain(delta))
                    {
                        Deactivate(compartment);
                        i--;
                    }
                }
            }
        }
        else
        {
            GD.Print("Game Over!");
            SetProcess(false);
        }
    }

    public void ActivateNext()
    {
        var next = InactivateCompartments[Random.Shared.Next(0, InactivateCompartments.Count)];
        InactivateCompartments.Remove(next);
        ActiveCompartments.Add(next);
        next.Queue(5, Random.Shared.Next(5, 10));
    }

    public void Deactivate(Compartment compartment)
    {
        ActiveCompartments.Remove(compartment);
        InactivateCompartments.Add(compartment);
        compartment.Deactivate();
    }
}
