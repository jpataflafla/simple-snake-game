using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public enum Direction { Up, Down, Right, Left };

    public enum SnakeInteraction
    {
        EdibleItem,
        InedibleItem,
        SpeedUpItem,
        SlowDownItem,
        HeadTailSwapItem,
    };
}