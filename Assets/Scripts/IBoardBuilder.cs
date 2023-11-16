﻿using UnityEngine;

public interface IBoardBuilder
{
    public SpriteRenderer[,] InitializeBoard(SpriteRenderer gameBoard, int boardSize,
        Sprite lightTile, Sprite darkTile, float lightTileAlpha, float darkTileAlpha);
}
