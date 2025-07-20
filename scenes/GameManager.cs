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
    public static GameManager inst;

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
    [Export]
    public Node3D[] PlayerSpawnPoints = [];

    Dictionary<ThreatDir, Node3D[]> enemySpots = new Dictionary<ThreatDir, Node3D[]>();
    Dictionary<ThreatDir, EnemyShip[]> enemyShips = new Dictionary<ThreatDir, EnemyShip[]>();

    public double MaxHealth = 100;
    public double Health;

    public void Initialize()
    {
        inst = this;
        Health = MaxHealth;
        SpawnPlayers();
        enemySpots.Add(ThreatDir.North, northEnemySpots);
        enemySpots.Add(ThreatDir.East, eastEnemySpots);
        enemySpots.Add(ThreatDir.South, southEnemySpots);
        enemySpots.Add(ThreatDir.West, westEnemySpots);

        for (int i = 0; i < 4; i++)
        {
            enemyShips.Add((ThreatDir)i, new EnemyShip[3]);
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                SpawnEnemyShip((ThreatDir)i);
            }
        }
    }

    public void SpawnPlayers()
    {
        for (var i = 0; i < Players.PlayerIds.Count; i++)
        {
            var player = Player.SpawnPlayer(Players.PlayerIds[i], Players.Colors[i]);
            player.Position = PlayerSpawnPoints[i].Position;
            GetTree().CurrentScene.AddChild(player);
        }
    }

    public override void _Process(double delta)
    {
        if (Health > 0)
        {

        }
        else
        {
            GD.Print("Game Over!");
            SetProcess(false);
        }
    }

    public bool IsSpotAvailable(ThreatDir dir)
    {
        for (int i = 0; i < enemyShips[dir].Count(); i++)
        {
            if (enemyShips[dir][i] is null)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnEnemyShip(ThreatDir dir)
    {
        if (!IsSpotAvailable(dir))
            return;

        int chosenSpot = -1;
        for (int i = 0; i < enemyShips[dir].Count(); i++)
        {
            if (enemyShips[dir][i] is null)
            {
                chosenSpot = i;
                break;
            }
        }
        Node3D chosenNode = enemySpots[dir][chosenSpot];
        EnemyShip enemyShip = enemyShipBlueprint.Instantiate() as EnemyShip;
        enemyShip.dir = dir;
        enemyShip.spot = chosenSpot;
        enemyShips[dir][chosenSpot] = enemyShip;
        chosenNode.AddChild(enemyShip);
        enemyShip.Position = new Vector3(0, 0, -200);
        enemyShip.Rotation = new();
    }

    public EnemyShip GetLowestHPShipInDir(ThreatDir dir)
    {
        EnemyShip res = null;
        float lowestHp = float.MaxValue;
        for(int i = 0; i < enemyShips[dir].Count(); i++)
        {
            if(enemyShips[dir][i] != null && enemyShips[dir][i].hp < lowestHp)
            {
                res = enemyShips[dir][i];
                lowestHp = enemyShips[dir][i].hp;
            }
        }
        return res;
    }

    public void ClearEnemyShipSpot(ThreatDir dir, int spot)
    {
        enemyShips[dir][spot] = null;
    }

    public void TakeDamage(float dmg)
    {
        Health -= (double)dmg;
    }
}
