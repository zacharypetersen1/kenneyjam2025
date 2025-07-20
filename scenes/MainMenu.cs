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
            if (Players.PlayerIds.Count < Players.Colors.Length)
            {
                if (!Players.PlayerIds.Contains(@event.Device))
                {
                    GetNode("VBoxContainer/PlayerStatuses").GetChild(Players.PlayerIds.Count).GetNode<Label>("Label").Text = $"Player {Players.PlayerIds.Count + 1} Joined";
                    Players.PlayerIds.Add(@event.Device);
                    GetNode<Label>("VBoxContainer/CenterContainer/Label").Text = "Press Start";
                }
            }
        }
        // else if (@event.IsMatch(new InputEventJoypadButton() { Device = -1, ButtonIndex = JoyButton.B, Pressed = true }))
        // {
        //     var playerIndex = Players.PlayerIds.IndexOf(@event.Device);
        //     if (playerIndex != -1)
        //     {
        //         GetNode("VBoxContainer/PlayerStatuses").GetChild(playerIndex).GetNode<Label>("Label").Text = "Press A to Join";
        //         Players.PlayerIds.Remove(@event.Device);
        //     }
        // }
        else if (@event.IsMatch(new InputEventJoypadButton() { Device = -1, ButtonIndex = JoyButton.Start, Pressed = true }))
        {
            if (Players.PlayerIds.Count > 0) Play();
        }
    }
}
