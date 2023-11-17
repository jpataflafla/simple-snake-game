using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegment
    {
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Vector2Int Indices { get; private set; }
        public Direction SegmentOrientation { get; private set; }

        private readonly SpriteRenderer[,] _boardTiles;

        public SnakeSegment(SpriteRenderer spriteRenderer, Vector2Int indices, 
            Direction initialOrientation, SpriteRenderer[,] boardTiles)
        {
            SpriteRenderer = spriteRenderer;
            _boardTiles = boardTiles;
            ScaleTile(_boardTiles, this.SpriteRenderer);
            SetOrientation(initialOrientation);
            SetPosition(indices);
        }

        public void MoveSegment(Direction movementDirection)
        {
            Vector2Int currentIndieces = Indices;

            // (0, 0) -> left bottom corner
            switch (movementDirection)
            {
                case Direction.Right:
                    currentIndieces.y++;
                    if (currentIndieces.y >= _boardTiles.GetLength(1))
                    {
                        currentIndieces.y -= _boardTiles.GetLength(1);
                    }
                    break;
                case Direction.Left:
                    currentIndieces.y--;
                    if (currentIndieces.y < 0)
                    {
                        currentIndieces.y += _boardTiles.GetLength(1);
                    }
                    break;
                case Direction.Down:
                    currentIndieces.x--;
                    if (currentIndieces.x < 0)
                    {
                        currentIndieces.x += _boardTiles.GetLength(0);
                    }
                    break;
                case Direction.Up:
                    currentIndieces.x++;
                    if (currentIndieces.x >= _boardTiles.GetLength(0))
                    {
                        currentIndieces.x -= _boardTiles.GetLength(0);
                    }
                    break;
            }

            SetPosition(currentIndieces);
            SetOrientation(movementDirection);
        }

        public void SetOrientation(Direction newOrientation)
        {
            var newRotation = Quaternion.Euler(0f, 0f, 0f);
            switch (newOrientation)
            {
                case Direction.Up:
                    newRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case Direction.Down:
                    newRotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Direction.Left:
                    newRotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                case Direction.Right:
                    newRotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
            }
            this.SegmentOrientation = newOrientation;
            this.SpriteRenderer.transform.rotation = newRotation;
        }

        public void SetPosition(Vector2Int indices)
        {
            this.SpriteRenderer.transform.position =
                _boardTiles[indices.x, indices.y].transform.position;
            Indices = indices;
        }

        private void ScaleTile(SpriteRenderer[,] boardTiles, SpriteRenderer tileToScale)
        {
            // set head size
            var currentSize = tileToScale.bounds.size.x;
            var desiredSize = boardTiles[0, 0].bounds.size.x;
            var spriteScale = currentSize != 0 ? desiredSize / currentSize : currentSize;

            tileToScale.transform.localScale = new Vector3(spriteScale, spriteScale, 1f);
        }
    }
}