using Godot;
using System;

public partial class EnemyShip : Node3D
{
    [Export]
    public float hp = 5;
    public ThreatDir dir;
    public int spot = -1;
    float lerpSpeed = .1f;
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
}
