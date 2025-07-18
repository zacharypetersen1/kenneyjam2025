using Godot;
using System;

public partial class PlayerCharacter : CharacterBody3D
{
    [Export]
    public int Speed = 10;

    public override void _PhysicsProcess(double delta)
    {
        var direction = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        var velocity = direction * Speed;
        Velocity = new Vector3(velocity.X, 0, velocity.Y);
        Rotation = new Vector3(direction.X, 0, direction.Y);
        MoveAndSlide();
    }

}
