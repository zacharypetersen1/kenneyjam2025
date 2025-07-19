using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public enum ThreatDir
{
    North = 0, East = 1, South = 2, West = 3
}

public partial class GameManager : Node
{
    [Export]
    public PackedScene enemyShipBlueprint;
    [Export]
    public Node3D[] northEnemySpots = new Node3D[3];
    [Export]
    public Node3D[] eastEnemySpots = new Node3D[3];
    [Export]
    public Node3D[] southEnemySpots = new Node3D[3];
    [Export]
    public Node3D[] westEnemySpots = new Node3D[3];

    Dictionary<ThreatDir, Node3D[]> enemySpots = new Dictionary<ThreatDir, Node3D[]>();
    Dictionary<ThreatDir, EnemyShip[]> enemyShips = new Dictionary<ThreatDir, EnemyShip[]>();

    public List<Compartment> InactivateCompartments = [];

    public List<Compartment> ActiveCompartments = [];

    public double Health = 100;

    public void Initialize()
    {
        InactivateCompartments = [.. GetTree().CurrentScene.GetChildren().Where(x => x is Compartment).Select(x => x as Compartment)];
        enemySpots.Add(ThreatDir.North, northEnemySpots);
        enemySpots.Add(ThreatDir.East, eastEnemySpots);
        enemySpots.Add(ThreatDir.South, southEnemySpots);
        enemySpots.Add(ThreatDir.West, westEnemySpots);

        for(int i = 0; i < 4; i++)
        {
            enemyShips.Add((ThreatDir)i, new EnemyShip[3]);
        }

        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                SpawnEnemyShip((ThreatDir)i);
            }
        }
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

    public bool IsSpotAvailable(ThreatDir dir)
    {
        for(int i = 0; i < enemyShips[dir].Count(); i++)
        {
            if(enemyShips[dir][i] is null)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnEnemyShip(ThreatDir dir)
    {
        if(!IsSpotAvailable(dir))
            return;

        int chosenSpot = -1;
        for(int i = 0; i < enemyShips[dir].Count(); i++)
        {
            if(enemyShips[dir][i] is null)
            {
                chosenSpot = i;
                break;
            }
        }
        Node3D chosenNode = enemySpots[dir][chosenSpot];
        EnemyShip enemyShip = enemyShipBlueprint.Instantiate() as EnemyShip;
        enemyShips[dir][chosenSpot] = enemyShip;
        chosenNode.AddChild(enemyShip);
        enemyShip.Position = new Vector3(0, 0, -200);
        enemyShip.Rotation = new();
    }
}
