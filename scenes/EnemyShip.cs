using Godot;
using System;

public partial class EnemyShip : Node3D
{
    float lerpSpeed = .1f;
    override public void _PhysicsProcess(double delta)
    {
        Position = Position.Lerp(new Vector3(), lerpSpeed);
    }
}
