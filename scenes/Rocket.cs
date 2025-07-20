using Godot;
using System;

public partial class Rocket : Node3D
{
    [Export]
    public GpuParticles3D smoke, explTrail, explCore;
    [Export]
    public Node3D visual;
    [Export]
    public float speed = 5;
    public float travelTime = 0.45f;
    float curTime;
    float curPercent = 0;
    public Vector3 initialPos;
    public Vector3 targetPos;
    public EnemyShip target;
    bool destroyTimerOn = false;
    float destroyTimer = 1f;
    public int targetSlot = -1;

    override public void _Process(double delta)
    {
        curTime += (float) delta;
        curPercent = Mathf.Min(curTime / travelTime, 1);
        if(curPercent >= 1 && !destroyTimerOn)
        {
            Explode();
        }

        GlobalPosition = initialPos.Lerp(targetPos, curPercent);

        if(destroyTimerOn)
        {
            destroyTimer -= (float)delta;
            if(destroyTimer <= 0)
            {
                QueueFree();
            }
        }
    }

    void Explode()
    {
        if(GameManager.inst.enemyShips[targetSlot] != null)
        {
            GameManager.inst.enemyShips[targetSlot].TakeDamage(1);
        }
        visual.Visible = false;
        smoke.Emitting = false;
        explTrail.Emitting = true;
        explCore.Emitting = true;
        destroyTimerOn = true;
    }
}
