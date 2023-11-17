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
        public event Action<int> OnSizeChange;
        public event Action OnSnakeDead;

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
        private readonly int _minSnakeSegments = 3;

        private Coroutine speedChange;
        private float _initialSnakeSpeed;
        private float _snakeSpeed = 1f;
        private float _snakeSpeedChangeTime = 1f;
        private float _snakeSpeedChangeAmount = 1.5f;

        public void Initialize(SpriteRenderer[,] boardTiles, int startTileRowIndex, 
            int startTileColumnIndex, Direction startHeadDirection,
            float snakeSpeedChangeTime, float snakeSpeedChangeAmount)
        {
            _snakeSpeedChangeTime = snakeSpeedChangeTime;
            _snakeSpeedChangeAmount = snakeSpeedChangeAmount;

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

            MoveHead(startHeadDirection);
            _snake.ForEach(i => MoveSnake(startHeadDirection));
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

        public bool IsAnySnakeSegmentAtPosition(Vector2Int position)
        {
            foreach(var segment in _snake)
            {
                if(segment.Indices == position) return true;
            }
            return false;
        }

        public bool IsSnakeHeadAtPosition(Vector2Int position)
        {
            return HeadPositionIndices == position;
        }

        #region SnakeAutomaticMovement
        public void ChangeSnakeSpeed(float newSpeed)
        {
            _snakeSpeed = Mathf.Max(newSpeed, 0f);
        }

        public void StartSnakeMovement(float initialSpeed)
        {
            _initialSnakeSpeed = initialSpeed;
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

        #region SnakeActions
        public void AddSegment()
        {
            var tail = _snake[^1];
            var body = _snake[^2];

            var newSegment = new SnakeSegment(GetBodySegmentSpriteRenderer(), 
                body.Indices, body.SegmentOrientation, _boardTiles);

            _snake[^1] = newSegment;
            _snake.Add(tail);
            tail.SpriteRenderer.transform.SetAsLastSibling();

            OnSizeChange?.Invoke(1);
        }

        public void RemoveSegment()
        {
            if(_snake.Count <= _minSnakeSegments)
            {
                GameOver();
                return;
            }
            var tail = _snake[^1];
            var body = _snake[^2];
            _snake[^2] = tail;
            Destroy(body.SpriteRenderer.gameObject);
            _snake.RemoveAt(_snake.Count - 1);           
            tail.SpriteRenderer.transform.SetAsLastSibling();

            OnSizeChange?.Invoke(-1);
        }

        private void GameOver()
        {
            StopSnakeMovement( forceStopCoroutine: true);
            OnSnakeDead?.Invoke();
        }

        public void SpeedUpAction()
        {
            if(speedChange != null)
            {
                StopCoroutine(speedChange);
            }
            ChangeSnakeSpeed(_initialSnakeSpeed);
            speedChange = StartCoroutine(SpeedChange(_snakeSpeedChangeTime, _snakeSpeedChangeAmount));
        }

        public void SlowDownAction()
        {
            if (speedChange != null)
            {
                StopCoroutine(speedChange);
            }
            ChangeSnakeSpeed(_initialSnakeSpeed);
            speedChange = StartCoroutine(SpeedChange(_snakeSpeedChangeTime, 1f/_snakeSpeedChangeAmount));
        }

        private IEnumerator SpeedChange(float snakeSpeedChangeTime, float snakeSpeedChangeAmount)
        {
            ChangeSnakeSpeed(snakeSpeedChangeAmount * _snakeSpeed);
            yield return new WaitForSeconds(snakeSpeedChangeTime);
            ChangeSnakeSpeed(_initialSnakeSpeed);
        }

        public void HeadTailSwapAction()
        {
            int n = _snake.Count;
            
            for (int i = 0; i < n / 2; i++)
            {
                if(i==0) //head and tail
                {
                    var tmp = _snake[0].Indices;
                    _snake[0].SetPosition(_snake[^1].Indices);
                    _snake[^1].SetPosition(tmp);
                    continue;
                }

                // Swap body elements to reverse the list
                (_snake[n - 1 - i], _snake[i]) = (_snake[i], _snake[n - 1 - i]);
            }

            MoveHead(GetOppositeDirection(_snake[^1].SegmentOrientation));
            _snake[^1].SetOrientation(HeadDirection);

        }


        #endregion SnakeActions

        static Direction GetOppositeDirection(Direction currentDirection)
        {
            // Return the opposite direction without modifying the input
            switch (currentDirection)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new ArgumentException("Invalid direction");
            }
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