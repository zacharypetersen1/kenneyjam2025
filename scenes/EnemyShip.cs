using Godot;
using System;

public partial class EnemyShip : Node3D
{
    [Export]
    public float attackSpeed = 10;
    [Export]
    public float attackDmg = 5;
    [Export]
    public float hp = 5;
    [Export]
    public UIProgressBar attackProgressBar;
    [Export]
    public GpuParticles3D[] playOnAttack = new GpuParticles3D[2];
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
            for(int i = 0; i < playOnAttack.Length; i++)
            {
                playOnAttack[i].Emitting = true;
            }
            GameManager.inst.TakeDamage(attackDmg);
            curAttackProgress -= attackSpeed;
        }
        attackProgressBar.SetProgress(curAttackProgress / attackSpeed);
    }
}
