using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private PlayerInput _playerInput;


    private void Start()
    {
        _boardBuilder = new BoardBuilder();
        _boardTiles = _boardBuilder.InitializeBoard(
            _board, _boardSize, _lightTile, _darkTile, _lightTileAlpha, _darkTileAlpha);

        _snakeController.Initialize(_boardTiles, _startTileRowIndex, _startTileColumnIndex, _startHeadDirection);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // onKeyDown
        {
            Vector2 moveValue = context.ReadValue<Vector2>();
            Debug.Log("Move callback. Value: " + moveValue);

            //MOVE SNAKE
        }
    }
}
