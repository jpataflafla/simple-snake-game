using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : IBoardBuilder
{
    private SpriteRenderer _gameBoard;
    private int _boardSize;

    private SpriteRenderer[,] _board;

    //checkerboard sprites
    private Sprite _lightTile;
    private float _lightTileAlpha;
    private Sprite _darkTile;
    private float _darkTileAlpha;

    private const int _minimumBoardSize = 2;

    public SpriteRenderer[,] InitializeBoard(SpriteRenderer gameBoard, int boardSize, 
        Sprite lightTile, Sprite darkTile, float lightTileAlpha, float darkTileAlpha)
    {
        _gameBoard = gameBoard;
        if (_gameBoard == null)
        {
            throw new Exception("Game board sprite renderer not assigned!");
        }

        _boardSize = boardSize;
        if (_boardSize < _minimumBoardSize)
        {
            throw new Exception($"Game board size should be at least {_minimumBoardSize}!");
        }

        _lightTile = lightTile;
        _darkTile = darkTile;
        if( _darkTile == null || _lightTile == null)
        {
            throw new Exception($"Board tile sprites were not provided!");
        }
        _lightTileAlpha = lightTileAlpha;
        _darkTileAlpha = darkTileAlpha;

        _board = CreateBoard(gameBoard);

        return _board;
    }

    private SpriteRenderer[,] CreateBoard(SpriteRenderer boardSpriteRenderer)
    {
        SpriteRenderer[,] board = new SpriteRenderer[_boardSize, _boardSize];

        // Calculate the size of each tile
        float tileSizeX = boardSpriteRenderer.bounds.size.x / _boardSize;
        float tileSizeY = boardSpriteRenderer.bounds.size.y / _boardSize;

        for (int row = 0; row < _boardSize; row++)
        {
            for (int col = 0; col < _boardSize; col++)
            {
                // Calculate the position of each tile
                float posX = 
                    boardSpriteRenderer.transform.position.x - boardSpriteRenderer.bounds.size.x * 0.5f 
                    + col * tileSizeX + tileSizeX * 0.5f;
                float posY = 
                    boardSpriteRenderer.transform.position.y - boardSpriteRenderer.bounds.size.y * 0.5f
                    + row * tileSizeY + tileSizeY * 0.5f;

                // Instantiate a GameObject with SpriteRenderer for each tile
                var tileSpriteRenderer = CreateTile(row, col, posX, posY, parent: boardSpriteRenderer.transform,
                    tileSizeX, tileSizeY, boardSpriteRenderer.transform);

                // Store the sprite in the board array
                board[row, col] = tileSpriteRenderer;
            }
        }

        return board;
    }

    private SpriteRenderer CreateTile(int row, int col, float posX, float posY, Transform parent, 
        float tileSizeX, float tileSizeY, Transform board)
    {
        GameObject tile = new GameObject("tile_" + row + "_" + col);
        tile.transform.position = new Vector2(posX, posY);
        tile.transform.localScale = Vector2.one;
        tile.transform.parent = parent;

        SpriteRenderer tileSpriteRenderer = AddSpriteRenderer(tile, row, col);

        var spriteSizeX = tileSpriteRenderer.bounds.size.x;
        var spriteSizeY = tileSpriteRenderer.bounds.size.y;
        // bound x of new tle should be tileSizeX, and the same for y
        var scaleX = spriteSizeX != 0 ? tileSizeX / spriteSizeX : 1f;
        var scaleY = spriteSizeY != 0 ? tileSizeY / spriteSizeY : 1f;

        tileSpriteRenderer.transform.localScale = new Vector2(scaleX/board.localScale.x, scaleY/ board.localScale.y);

        return tileSpriteRenderer;
    }

    private SpriteRenderer AddSpriteRenderer(GameObject tile, int row, int col)
    {
        SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();

        Color spriteColor = spriteRenderer.color;

        bool isEven = (row + col) % 2 == 0;

        if (isEven)
        {
            spriteColor.a = _lightTileAlpha;
            spriteRenderer.sprite = _lightTile;
        }
        else
        {
            spriteColor.a = _darkTileAlpha; ;
            spriteRenderer.sprite = _darkTile;
        }
        spriteRenderer.color = spriteColor;

        return spriteRenderer;
    }
}
