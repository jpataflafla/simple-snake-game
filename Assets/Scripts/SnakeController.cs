using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Right, Left };

public class SnakeController : MonoBehaviour
{
    public Vector2Int HeadPositionIdices { get; private set; }
    public Direction HeadDirection { get; private set; }

    [SerializeField] private SpriteRenderer _head;
    [SerializeField] private SpriteRenderer _tail;

    private SpriteRenderer[,] _boardTiles;

    public void Initialize(SpriteRenderer[,] boardTiles, int startTileRowIndex, int startTileColumnIndex, Direction startHeadDirection)
    {
        _boardTiles = boardTiles;

        // set head size
        var currentSize = _head.bounds.size.x;
        var desiredSize = boardTiles[0, 0].bounds.size.x;
        var spriteScale = currentSize != 0 ? desiredSize / currentSize : currentSize;
        
        _head.transform.localScale = new Vector3(spriteScale, spriteScale, 1f);

        // set initial position
        _head.transform.position = boardTiles[startTileRowIndex, startTileColumnIndex].transform.position;
        HeadPositionIdices = new Vector2Int(startTileRowIndex, startTileColumnIndex);

        UpdateHeadOrientation(startHeadDirection);
    }

    public void MoveHead(Direction headDirection)
    {
        Vector2Int newHeadPositionIndices = HeadPositionIdices;

        // (0, 0) -> left bottom corner
        switch (headDirection)
        {
            case Direction.Up:
                newHeadPositionIndices.y++;
                if(newHeadPositionIndices.y >= _boardTiles.GetLength(1))
                {
                    newHeadPositionIndices.y -= _boardTiles.GetLength(1);
                }
                break;
            case Direction.Down:
                newHeadPositionIndices.y--;
                if (newHeadPositionIndices.y < 0)
                {
                    newHeadPositionIndices.y += _boardTiles.GetLength(1);
                }
                break;
            case Direction.Left:
                newHeadPositionIndices.x--;
                if (newHeadPositionIndices.x < 0)
                {
                    newHeadPositionIndices.x += _boardTiles.GetLength(0);
                }
                break;
            case Direction.Right:
                newHeadPositionIndices.x++;
                if (newHeadPositionIndices.x >= _boardTiles.GetLength(0))
                {
                    newHeadPositionIndices.x -= _boardTiles.GetLength(0);
                }
                break;
        }


        HeadPositionIdices = newHeadPositionIndices;
        HeadDirection = headDirection;
        _head.transform.position = _boardTiles[newHeadPositionIndices.x, newHeadPositionIndices.y].transform.position;

    }

    private void UpdateHeadOrientation(Direction newDirection)
    {
        switch (newDirection)
        {
            case Direction.Up:
                _head.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Direction.Down:
                _head.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case Direction.Left:
                _head.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case Direction.Right:
                _head.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

}
