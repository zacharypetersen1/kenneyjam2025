using Godot;

public partial class MainMenu : Control
{
    [Export]
    public PackedScene GameScene;

    public void Play()
    {
        GetTree().ChangeSceneToPacked(GameScene);
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
                GetNode<Button>("CenterContainer/Play").Text = "Press Start";
            }
        }
        if (@event.IsMatch(new InputEventJoypadButton() { Device = -1, ButtonIndex = JoyButton.Start, Pressed = true }))
        {
            if (Players.PlayerIds.Count > 0) Play();
        }
    }
}
