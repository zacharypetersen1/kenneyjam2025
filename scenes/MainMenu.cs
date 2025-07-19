using Godot;

public partial class MainMenu : Control
{
    public void Play()
    {
        var gameScene = GD.Load<PackedScene>("res://scenes/MovementPlayground.tscn");
        GetTree().ChangeSceneToPacked(gameScene);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsMatch(new InputEventJoypadButton() { Device = -1, ButtonIndex = JoyButton.A, Pressed = true }))
        {
            if (!Players.PlayerIds.Contains(@event.Device))
            {
                GetNode("PlayerStatuses").GetChild(Players.PlayerIds.Count).GetNode<Label>("Label").Text = $"Player {Players.PlayerIds.Count + 1} Joined";
                Players.PlayerIds.Add(@event.Device);
                GetNode<Button>("CenterContainer/Play").Disabled = false;
            }
        }
    }
}
