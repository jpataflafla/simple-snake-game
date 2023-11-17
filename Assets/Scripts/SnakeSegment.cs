using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegment : TileSizedMovableSegment
    {
        public SnakeSegment(SpriteRenderer spriteRenderer, Vector2Int indices,
                            Direction initialOrientation, SpriteRenderer[,] boardTiles) :
            base(spriteRenderer, indices, initialOrientation, boardTiles)
        {
            // Additional constructor logic for snake segment
        }
    }

}