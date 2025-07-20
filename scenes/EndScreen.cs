using Godot;
using System;

public partial class EndScreen : CenterContainer
{
    public override void _Ready()
    {
        GetNode<Label>("Label").Text = Players.Win ? "You Win!" : "You Died!";
    }
}
