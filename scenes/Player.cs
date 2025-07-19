using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
    private readonly struct PlayerInputMap(int deviceId)
    {
        public string Left { get; } = $"Left_{deviceId}";
        public string Right { get; } = $"Right_{deviceId}";
        public string Up { get; } = $"Up_{deviceId}";
        public string Down { get; } = $"Down_{deviceId}";
        public string Interact { get; } = $"Interact_{deviceId}";
    }

    [Export]
    public int Speed = 10;
    Battery heldBattery = null;
    Battery targetBattery = null;
    Compartment targetCompartment = null;
    Window targetWindow = null;

    private int DeviceId;
    private PlayerInputMap PlayerInput;

    public static Player SpawnPlayer(int deviceId, Color color)
    {
        var player = GD.Load<PackedScene>("res://scenes/Player.tscn").Instantiate() as Player;
        player.DeviceId = deviceId;
        player.MapInput();
        player.GetNode<Sprite3D>("Marker").Modulate = color;
        return player;
    }

    public void MapInput()
    {
        PlayerInput = new PlayerInputMap(DeviceId);

        InputMap.AddAction(PlayerInput.Interact);
        InputMap.ActionAddEvent(PlayerInput.Interact, new InputEventJoypadButton() { Device = DeviceId, ButtonIndex = JoyButton.A, Pressed = true });

        InputMap.AddAction(PlayerInput.Left);
        InputMap.ActionAddEvent(PlayerInput.Left, new InputEventJoypadMotion() { Device = DeviceId, Axis = JoyAxis.LeftX, AxisValue = -1 });

        InputMap.AddAction(PlayerInput.Right);
        InputMap.ActionAddEvent(PlayerInput.Right, new InputEventJoypadMotion() { Device = DeviceId, Axis = JoyAxis.LeftX, AxisValue = 1 });

        InputMap.AddAction(PlayerInput.Up);
        InputMap.ActionAddEvent(PlayerInput.Up, new InputEventJoypadMotion() { Device = DeviceId, Axis = JoyAxis.LeftY, AxisValue = -1 });

        InputMap.AddAction(PlayerInput.Down);
        InputMap.ActionAddEvent(PlayerInput.Down, new InputEventJoypadMotion() { Device = DeviceId, Axis = JoyAxis.LeftY, AxisValue = 1 });
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(PlayerInput.Interact))
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
                if (targetCompartment is not null)
                {
                    heldBattery.Place(targetCompartment);
                    holdLocation.RemoveChild(heldBattery);
                    targetCompartment.AddChild(heldBattery);
                    heldBattery.Position = new Vector3(0, 0.5f, 0);
                    heldBattery.Rotation = new Vector3(Mathf.DegToRad(90), 0, 0);
                }
                else
                {
                    if (heldBattery is RigidBody3D body) body.Freeze = false;
                    holdLocation.RemoveChild(heldBattery);
                    GetTree().CurrentScene.AddChild(heldBattery);
                    if (targetWindow is not null)
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

    public override void _Process(double delta)
    {
        GetNode<Sprite3D>("Marker").RotateY((float)(Math.PI * delta));
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Input.GetVector(PlayerInput.Left, PlayerInput.Right, PlayerInput.Up, PlayerInput.Down);
        var velocity = direction * Speed;
        Velocity = new Vector3(velocity.X, 0, velocity.Y) + GetGravity();
        if (direction.LengthSquared() > 0) Rotation = new Vector3(0, -direction.Angle(), 0);
        MoveAndSlide();

        Battery newTargetBattery = null;
        if (heldBattery is null)
        {
            newTargetBattery = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Battery) as Battery;
        }
        if (newTargetBattery != targetBattery)
        {
            if (targetBattery != null)
            {
                Flash.SetFlashOnThisAndChildren(targetBattery as Node3D, false, this);
            }
            if (newTargetBattery != null)
            {
                Flash.SetFlashOnThisAndChildren(newTargetBattery as Node3D, true, this);
            }
            targetBattery = newTargetBattery;
        }

        Compartment newTargetCompartment = null;
        if (heldBattery is not null)
        {
            newTargetCompartment = GetNode<Area3D>("Area3D").GetOverlappingBodies().FirstOrDefault(x => x is Compartment) as Compartment;
        }
        if (newTargetCompartment != targetCompartment)
        {
            if (targetCompartment != null)
            {
                Flash.SetFlashOnThisAndChildren(targetCompartment as Node3D, false, this);
            }
            if (newTargetCompartment != null)
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
        if (newTargetWindow != targetWindow)
        {
            if (targetWindow != null)
            {
                Flash.SetFlashOnThisAndChildren(targetWindow as Node3D, false, this);
            }
            if (newTargetWindow != null)
            {
                Flash.SetFlashOnThisAndChildren(newTargetWindow as Node3D, true, this);
            }
            targetWindow = newTargetWindow;
        }
    }
}
