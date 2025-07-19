using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed = 10;
    Interactable heldBattery = null;
    Interactable targetBattery = null;
    Compartment targetCompartment = null;
    Window targetWindow = null;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            var holdLocation = GetNode<Node3D>("HoldLocation");
            if (heldBattery is null)
            {
                if (targetBattery is not null)
                {
                    if (targetBattery.Compartment is not null)
                    {
                        targetBattery.Remove();
                    }
                    targetBattery.GetParent().RemoveChild(targetBattery);
                    targetBattery.Freeze = true;
                    targetBattery.Rotation = new();
                    holdLocation.AddChild(targetBattery);
                    targetBattery.GlobalPosition = ToGlobal(holdLocation.Position);
                    heldBattery = targetBattery;
                }
            }
            else
            {
                if(targetCompartment is not null)
                {
                    heldBattery.Place(targetCompartment);
                    holdLocation.RemoveChild(heldBattery);
                    targetCompartment.AddChild(heldBattery);
                    heldBattery.Position = new();
                }
                else
                {
                    if (heldBattery is RigidBody3D body) body.Freeze = false;
                    holdLocation.RemoveChild(heldBattery);
                    GetTree().CurrentScene.AddChild(heldBattery);
                    if(targetWindow is not null)
                    {
                        var relativePos = targetWindow.ToLocal(Position);
                        float offset = relativePos.Z > 0 ? -1 : 1;
                        heldBattery.GlobalPosition = targetWindow.ToGlobal(new Vector3(0, 0, offset));
                    }
                    else
                    {
                        heldBattery.GlobalPosition = ToGlobal(GetNode<Area3D>("Area3D").GetChild<Node3D>(0).Position);
                    }
                }
                heldBattery = null;
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

        Interactable newTargetBattery = null;
        if (heldBattery is null)
        {
            newTargetBattery = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Interactable) as Interactable;
        }
        if(newTargetBattery != targetBattery)
        {
            if(targetBattery != null)
            {
                GD.Print("UnstackFlash");
                Flash.SetFlashOnThisAndChildren(targetBattery as Node3D, false, this);
            }                
            if(newTargetBattery != null)
            {
                GD.Print("StackFlash");
                Flash.SetFlashOnThisAndChildren(newTargetBattery as Node3D, true, this);
            }
            targetBattery = newTargetBattery;
        }

        Compartment newTargetCompartment = null;
        if (heldBattery is not null)
        {
            newTargetCompartment = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Compartment) as Compartment;
        }
        if(newTargetCompartment != targetCompartment)
        {
            if(targetCompartment != null)
            {
                Flash.SetFlashOnThisAndChildren(targetCompartment as Node3D, false, this);
            }                
            if(newTargetCompartment != null)
            {
                Flash.SetFlashOnThisAndChildren(newTargetCompartment as Node3D, true, this);
            }
            targetCompartment = newTargetCompartment;
        }

        Window newTargetWindow = null;
        if (heldBattery != null && targetCompartment == null)
        {
            newTargetWindow = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Window) as Window;
        }
        if(newTargetWindow != targetWindow)
        {
            if(targetWindow != null)
            {
                Flash.SetFlashOnThisAndChildren(targetWindow as Node3D, false, this);
            }                
            if(newTargetWindow != null)
            {
                Flash.SetFlashOnThisAndChildren(newTargetWindow as Node3D, true, this);
            }
            targetWindow = newTargetWindow;
        }
    }
}
