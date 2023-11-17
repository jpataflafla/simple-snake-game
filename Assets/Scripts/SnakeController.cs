using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        public int SnakeLength => _snake.Count;
        public Vector2Int HeadPositionIndices => _snake[0].Indices;
        public Direction HeadDirection => _snake[0].SegmentOrientation;

        private Direction _nextMoveHeadDirection;

        [SerializeField] private SpriteRenderer _headSpriteRenderer;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private SpriteRenderer _tailSpriteRenderer;

        private List<SnakeSegment> _snake;
        private SpriteRenderer[,] _boardTiles;


        private Coroutine _snakeMovementLoop;
        private bool _snakeShouldMove;
        private float _snakeSpeed = 1f;


        public void Initialize(SpriteRenderer[,] boardTiles, int startTileRowIndex, 
            int startTileColumnIndex, Direction startHeadDirection)
        {
            _boardTiles = boardTiles;
            var headPositionIndices = new Vector2Int(startTileRowIndex, startTileColumnIndex);

            // build snake and setup initial position
            _snake = new List<SnakeSegment>() {
                new SnakeSegment(_headSpriteRenderer, indices: headPositionIndices,
                                startHeadDirection, boardTiles),
                new SnakeSegment(_bodySpriteRenderer, indices: headPositionIndices, 
                                startHeadDirection, boardTiles),
                new SnakeSegment(_tailSpriteRenderer, indices: headPositionIndices, 
                                startHeadDirection, boardTiles),
            };

        }

        public void MoveHead(Direction headDirection)
        {
            _nextMoveHeadDirection = headDirection;
        }

        private void MoveSnake(Direction headDirection)
        {
            var previousIndices = _snake[0].Indices;
            var previousOrientation = _snake[0].SegmentOrientation;
            _snake[0].MoveSegment(headDirection);

            for (int i = 1; i < _snake.Count; i++)
            {
                var currentIndices = _snake[i].Indices;
                var currentOrientation = _snake[i].SegmentOrientation;

                if (previousIndices == currentIndices) break;

                if (i == _snake.Count - 1) // tail
                {
                    _snake[i].SetPosition(previousIndices);
                    _snake[i].SetOrientation(_snake[i - 1].SegmentOrientation);
                    continue;
                }

                // Move the current segment to the position of the segment before it
                _snake[i].SetPosition(previousIndices);
                _snake[i].SetOrientation(previousOrientation);


                previousIndices = currentIndices;
                previousOrientation = currentOrientation;
            }
        }

        //public void SetHeadOrientation(Direction newOrientation)
        //{
        //    //SetSnakeSegmentOrientation(newOrientation, _head);
        //    _snake[0].SetOrientation(newOrientation);
        //}

        #region SnakeAutomaticMovement
        public void ChangeSnakeSpeed(float newSpeed)
        {
            _snakeSpeed = Mathf.Max(newSpeed, 0f);
        }

        public void StartSnakeMovement(float initialSpeed)
        {
            StopSnakeMovement(forceStopCoroutine: true);
            _snakeSpeed = initialSpeed;
            _snakeMovementLoop = StartCoroutine(SnakeMovementLoop());
        }

        private void StopSnakeMovement(bool forceStopCoroutine = false)
        {
            _snakeShouldMove = false;

            if (forceStopCoroutine && _snakeMovementLoop != null)
            {
                StopCoroutine(_snakeMovementLoop);
            }
        }

        private IEnumerator SnakeMovementLoop()
        {
            _snakeShouldMove = true;
            while (_snakeShouldMove)
            {
                yield return new WaitForSeconds(_snakeSpeed <= 0 ? 1f : 1f/_snakeSpeed);
                yield return new WaitForEndOfFrame();//wait for direction changes

                MoveSnake(_nextMoveHeadDirection);
                _nextMoveHeadDirection = HeadDirection;
                //yield return null;
            }

            // things to do after stop
        }
        #endregion SnakeAutiomaticMovement


        public void AddSegment(int add)
        {
            var tail = _snake[^1];
            var body = _snake[^2];

            var newSegment = new SnakeSegment(GetBodySegmentSpriteRenderer(), 
                body.Indices, body.SegmentOrientation, _boardTiles);

            _snake[^1] = newSegment;
            _snake.Add(tail);
            tail.SpriteRenderer.transform.SetAsLastSibling();
        }

        // TODO pool
        private SpriteRenderer GetBodySegmentSpriteRenderer()
        {
            var spriteRenderer = Instantiate(_bodySpriteRenderer.transform.gameObject, parent: _bodySpriteRenderer.transform.parent).GetComponent<SpriteRenderer>();
            spriteRenderer.transform.localScale = Vector3.one;
            return spriteRenderer;
        }
    }
}