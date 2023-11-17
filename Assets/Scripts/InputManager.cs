using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SnakeGame
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;

        public event Action<Direction> OnDirectionKeyDown;
        public event Action<Direction> OnDirectionUIButtonDown;

        private void Awake()
        {
            _leftArrow.onClick.AddListener(()=>OnDirectionUIButtonDown?.Invoke(Direction.Left));
            _rightArrow.onClick.AddListener(()=>OnDirectionUIButtonDown?.Invoke(Direction.Right));
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) // like onKeyDown
            {
                Vector2 moveValue = context.ReadValue<Vector2>();
#if UNITY_EDITOR
                Debug.Log("Move callback. Value: " + moveValue);
#endif

                OnDirectionKeyDown?.Invoke(GetDirectonFromVector(moveValue));
            }
        }

        private Direction GetDirectonFromVector(Vector2 headDirection, 
            Direction defaultDirection = Direction.Right)
        {
            if (Mathf.Approximately(headDirection.y, 1f))
            {
                return Direction.Up;
            }
            else if (Mathf.Approximately(headDirection.y, -1f))
            {
                return Direction.Down;
            }
            else if (Mathf.Approximately(headDirection.x, -1f))
            {
                return Direction.Left;
            }
            else if (Mathf.Approximately(headDirection.x, 1f))
            {
                return Direction.Right;
            }
            else
            {
                return defaultDirection;
            }
        }
    }
}