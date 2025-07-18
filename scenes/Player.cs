using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed = 10;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            var holdLocation = GetNode<Node3D>("HoldLocation");
            var heldObject = holdLocation.GetChildren().FirstOrDefault() as Node3D;
            if (heldObject is null)
            {
                var target = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Interactable);
                if (target is Interactable interactable)
                {
                    interactable.Freeze = true;
                    interactable.GetParent().RemoveChild(interactable);
                    interactable.Rotation = new();
                    interactable.Position = holdLocation.Position;
                    holdLocation.AddChild(interactable);
                }
            }
            else
            {
                if (heldObject is RigidBody3D body) body.Freeze = false;
                holdLocation.RemoveChild(heldObject);
                GetTree().Root.AddChild(heldObject);
                heldObject.GlobalPosition = holdLocation.ToGlobal(holdLocation.Position);
            }
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        var direction = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        var velocity = direction * Speed;
        Velocity = new Vector3(velocity.X, 0, velocity.Y) + GetGravity();
        if (direction.LengthSquared() > 0) Rotation = new Vector3(0, -direction.Angle(), 0);
        MoveAndSlide();
    }
}
