using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private BoardItemsManager _boardItemsManager;
        [SerializeField] private int _initialNumberOfInteractiveItems = 5;
        [SerializeField] private int _minDistanceFromNewItemsToSnakesHead = 1;

        [Space(10)]
        [Header("Snake Controller Setup")]
        [SerializeField] private SnakeController _snakeController; // not interface - main game idea will not change
        [SerializeField] private int _startTileRowIndex;
        [SerializeField] private int _startTileColumnIndex;
        [SerializeField] private Direction _startHeadDirection;
        [SerializeField] private float _snakeInitialSpeed = 3f;
        [SerializeField] private float _snakeSpeedChangeTime = .5f;
        [SerializeField] private float _snakeSpeedChangeAmount = 1.5f; // x times faster

        [Space(10)]
        [Header("Input System")]
        [SerializeField] private InputController _inputController;


        [Space(10)]
        [Header("Audio System")]
        [SerializeField] private AudioController _audioControllerImplementation;
        private IAudioController _audioController;
        [SerializeField] private float _backgroundMusicVolume = .5f;

        [Space(10)]
        [Header("Score Manager")]
        [SerializeField] private ScoreManager _scoreManagerImplementation;
        private IScoreInfoManager _scoreManager;

        private void Start()
        {
            _scoreManager = _scoreManagerImplementation ?? throw new System.NullReferenceException("ScoreManagerImplementation is not assigned!");
            _audioController = _audioControllerImplementation ?? throw new System.NullReferenceException("AudioControllerImplementation is not assigned!");
            _audioController.PlayBackgroundLoop(_backgroundMusicVolume);

            _boardBuilder = new BoardBuilder();
            _boardTiles = _boardBuilder.InitializeBoard(
                _board, _boardSize, _lightTile, _darkTile, _lightTileAlpha, _darkTileAlpha);


            _snakeController.Initialize(_boardTiles, _startTileRowIndex,
                _startTileColumnIndex, _startHeadDirection,
                _snakeSpeedChangeTime, _snakeSpeedChangeAmount);

            _boardItemsManager.InitializeBoardItems(_boardTiles, _snakeController,
                _initialNumberOfInteractiveItems, _minDistanceFromNewItemsToSnakesHead, _audioController);

            _snakeController.StartSnakeMovement(_snakeInitialSpeed);

            _inputController.OnDirectionKeyDown += DirectionInput;
            _inputController.OnDirectionUIButtonDown += DirectionUIButtonInput;

            _scoreManager.InitializeScoreBoard();

            _snakeController.OnSizeChange += AddPoints;
            _snakeController.OnSnakeDead += GameOver;
            _snakeController.SetActionMessenger(_scoreManager.ShowAction);
        }

        /// <summary>
        /// When the left/right UI buttons are used, the direction changes 
        /// relative to the current rotation of the head, not the orientation of the map.
        /// </summary>
        /// <param name="pressedDirection"></param>
        private void DirectionUIButtonInput(Direction pressedDirection)
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

            if(rotationMapping.TryGetValue((currentOrientation, pressedDirection), out var correctDirection))
            {
                //_snakeController.SetHeadOrientation(correctDirection);
                _snakeController.MoveHead(correctDirection);

            }
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

        private void AddPoints(int points)
        {
            _scoreManager.AddPoints(points);
        }

        private void GameOver()
        {
            _audioController.PlayGameOverSound();

            StartCoroutine(ReloadSceneAfterDelay(delay: 2f));
        }

        private IEnumerator ReloadSceneAfterDelay(float delay)
        {
            // Temporary solution until a menu is implemented

            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("GameScene");
        }

    }
}