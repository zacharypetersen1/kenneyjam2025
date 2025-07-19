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
            var heldObject = holdLocation.GetChildren().FirstOrDefault() as Interactable;
            if (heldObject is null)
            {
                var target = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Interactable);
                if (target is Interactable interactable)
                {
                    if (interactable.Compartment is not null)
                    {
                        interactable.Remove();
                    }
                    else
                    {
                        interactable.GetParent().RemoveChild(interactable);
                    }
                    interactable.Freeze = true;
                    interactable.Rotation = new();
                    holdLocation.AddChild(interactable);
                    interactable.GlobalPosition = ToGlobal(holdLocation.Position);
                }
            }
            else
            {
                var target = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Compartment or Window);
                if(target is Compartment compartment)
                {
                    heldObject.Place(compartment);
                    holdLocation.RemoveChild(heldObject);
                    compartment.AddChild(heldObject);
                    heldObject.Position = new();
                }
                else
                {
                    if (heldObject is RigidBody3D body) body.Freeze = false;
                    holdLocation.RemoveChild(heldObject);
                    GetTree().CurrentScene.AddChild(heldObject);
                    if(target is Window window)
                    {
                        var relativePos = window.ToLocal(Position);
                        float offset = relativePos.Z > 0 ? -1 : 1;
                        heldObject.GlobalPosition = window.ToGlobal(new Vector3(0, 0, offset));
                    }
                    else
                    {
                        heldObject.GlobalPosition = ToGlobal(GetNode<Area3D>("Area3D").GetChild<Node3D>(0).Position);
                    }
                }
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
