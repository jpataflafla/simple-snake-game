using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        [Header("Board Setup")]
        [SerializeField] private SpriteRenderer _board;
        [SerializeField] private int _boardSize; //it is a sqare sizeXsize
        [SerializeField] private Sprite _lightTile;
        [SerializeField] private float _lightTileAlpha = .5f;
        [SerializeField] private Sprite _darkTile;
        [SerializeField] private float _darkTileAlpha = .8f;
        private IBoardBuilder _boardBuilder;
        private SpriteRenderer[,] _boardTiles;

        [Space(10)]
        [Header("Snake Controller Setup")]
        [SerializeField] private SnakeController _snakeController; // not interface - main game idea will not change
        [SerializeField] private int _startTileRowIndex;
        [SerializeField] private int _startTileColumnIndex;
        [SerializeField] private Direction _startHeadDirection;

        [Space(10)]
        [Header("Input System")]
        [SerializeField] private InputController _inputController;


        private void Start()
        {
            _boardBuilder = new BoardBuilder();
            _boardTiles = _boardBuilder.InitializeBoard(
                _board, _boardSize, _lightTile, _darkTile, _lightTileAlpha, _darkTileAlpha);

            _snakeController.Initialize(_boardTiles, _startTileRowIndex, _startTileColumnIndex, _startHeadDirection);

            _inputController.OnDirectionKeyDown += DirectionInput;
            _inputController.OnDirectionUIButtonDown += DirectionUIButtonInput;
        }

        /// <summary>
        /// When the left/right UI buttons are used, the direction changes 
        /// relative to the current rotation of the head, not the orientation of the map.
        /// </summary>
        /// <param name="direction"></param>
        private void DirectionUIButtonInput(Direction direction)
        {
            Dictionary<(Direction, Direction), Direction> rotationMapping = 
                new Dictionary<(Direction, Direction), Direction>
            {
                { (Direction.Up, Direction.Left), Direction.Left },
                { (Direction.Left, Direction.Left), Direction.Down },
                { (Direction.Down, Direction.Left), Direction.Right },
                { (Direction.Right, Direction.Left), Direction.Up },

                { (Direction.Up, Direction.Right), Direction.Right },
                { (Direction.Left, Direction.Right), Direction.Up },
                { (Direction.Down, Direction.Right), Direction.Left },
                { (Direction.Right, Direction.Right), Direction.Down },
            };

            var currentOrientation = _snakeController.HeadDirection;

            if(rotationMapping.TryGetValue((currentOrientation, direction), out var newDirection))
            {
                _snakeController.SetHeadOrientation(newDirection);
            }

            AddSegment();
        }

        /// <summary>
        /// Direction changes according to the orientation of the map.
        /// </summary>
        /// <param name="direction"></param>
        private void DirectionInput(Direction direction)
        {
             _snakeController.MoveHead(direction);
             //_snakeController.SetHeadOrientation(direction);
        }

        // DEBUG
        private void AddSegment()
        {
            _snakeController.AddSegment(add: 1);
        }
    }
}