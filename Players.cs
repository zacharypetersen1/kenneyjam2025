using System.Collections.Generic;
using Godot;

public static class Players
{
    public static bool Win = false;
    public static List<int> PlayerIds = [];

    public static readonly Color[] Colors = [PlayerColors.Red, PlayerColors.Green, PlayerColors.Blue, PlayerColors.Yellow];

    public static class PlayerColors
    {
        public static readonly Color Red = new(1, 0, 0);
        public static readonly Color Green = new(0, 1, 0);
        public static readonly Color Blue = new(0, 0, 1);
        public static readonly Color Yellow = new(1, 1, 0);
    }
}
