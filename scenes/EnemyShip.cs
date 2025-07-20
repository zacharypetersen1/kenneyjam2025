using Godot;
using System;

public partial class EnemyShip : Node3D
{
    [Export]
    public float attackSpeed = 10;
    [Export]
    public float hp = 5;
    [Export]
    public UIProgressBar attackProgressBar;
    public ThreatDir dir;
    public int spot = -1;
    float lerpSpeed = .1f;
    float curAttackProgress = 0;
    override public void _PhysicsProcess(double delta)
    {
        Position = Position.Lerp(new Vector3(), lerpSpeed);
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            GameManager.inst.ClearEnemyShipSpot(dir, spot);
            QueueFree();
        }
    }

    override public void _Process(double delta)
    {
        curAttackProgress += (float)delta;
        if(curAttackProgress > attackSpeed)
        {
            // do attack
            curAttackProgress -= attackSpeed;
        }
        attackProgressBar.SetProgress(curAttackProgress / attackSpeed);
    }
}
