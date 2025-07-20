using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameManager : Node
{
    public static GameManager inst;

    [Export]
    public PackedScene enemyShipBlueprint;
    [Export]
    public Node3D[] enemyWaypoints = new Node3D[5];
    public EnemyShip[] enemyShips = new EnemyShip[5];

    [Export]
    public Node3D[] PlayerSpawnPoints = [];

    public double MaxHealth = 100;
    public double Health;

    public float MaxTravelDist = 1000;
    public float CurTravelDist = 0;
    public float CurTravelSpeed = 0;

    [Export]
    float MinSpawnTime = 6;
    [Export]
    float MaxSpawnTime = 17;
    bool spawning = false;
    float spawnTime = 0;

    public void Initialize()
    {
        inst = this;
        Health = MaxHealth;
        SpawnPlayers();
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
        if(CurTravelSpeed > 0)
        {
            spawning = true;
        }

        if(spawning)
        {
            spawnTime -= (float)delta;
            if(spawnTime < 0)
            {
                SpawnEnemyShip();
                spawnTime += Random.Shared.Next((int)MinSpawnTime, (int)MaxSpawnTime);
            }
        }

        if (Health > 0)
        {
            CurTravelDist += CurTravelSpeed * (float)delta;
            if (CurTravelDist >= MaxTravelDist)
            {
                GD.Print("You win!");
                SetProcess(false);
            }
        }
        else
        {
            GD.Print("Game Over!");
            SetProcess(false);
        }
    }

    public bool IsSpotAvailable()
    {
        for (int i = 0; i < enemyShips.Length; i++)
        {
            if (enemyShips[i] is null)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnEnemyShip()
    {
        if (!IsSpotAvailable())
            return;

        int chosenIndex = -1;
        for (int i = 0; i < enemyShips.Length; i++)
        {
            if (enemyShips[i] is null)
            {
                chosenIndex = i;
                break;
            }
        }
        Node3D chosenNode = enemyWaypoints[chosenIndex];
        EnemyShip enemyShip = enemyShipBlueprint.Instantiate() as EnemyShip;
        enemyShip.index = chosenIndex;
        enemyShips[chosenIndex] = enemyShip;
        chosenNode.AddChild(enemyShip);
        enemyShip.Position = new Vector3(0, 0, -200);
        enemyShip.Rotation = new();
    }

    public EnemyShip GetLowestHPShip()
    {
        EnemyShip res = null;
        float lowestHp = float.MaxValue;
        for(int i = 0; i < enemyShips.Length; i++)
        {
            if(enemyShips[i] != null && enemyShips[i].hp < lowestHp)
            {
                res = enemyShips[i];
                lowestHp = enemyShips[i].hp;
            }
        }
        return res;
    }

    public void ClearEnemyShipIndex(int index)
    {
        enemyShips[index] = null;
    }

    public void TakeDamage(float dmg)
    {
        Health -= (double)dmg;
    }
}
