using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SnakeGame
{
    [RequireComponent (typeof (SpriteRenderer))]
    public class InteractiveItem : MonoBehaviour
    {
        [SerializeField] private SnakeInteraction _snakeInteraction;
        public SnakeInteraction SnakeInteraction => _snakeInteraction;
        public Vector2Int ItemPositionIndieces => _thisSegment.Indices;

        [SerializeField] private Sprite[] _itemSprites;

        private IAudioController _audioController;

        private SpriteRenderer _spriteRenderer;
        private TileSizedMovableSegment _thisSegment;

        private bool _interactionCompleted;
        private bool _isInitialized;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void InitializeItem(Vector2Int indices,
            Direction initialOrientation, SpriteRenderer[,] boardTiles, IAudioController audioController)
        {
            if (_isInitialized)
            {
                Debug.LogError("InteractiveItem is already initialized.");
                return;
            }
            _audioController = audioController;
            _thisSegment = new TileSizedMovableSegment(_spriteRenderer, indices,
                initialOrientation, boardTiles);

            _spriteRenderer.sprite = _itemSprites[UnityEngine.Random.Range(0, _itemSprites.Length)];

            _isInitialized = true;
            _interactionCompleted = false;
        }

        public void SetPosition(Vector2Int position)
        {
            if (!_isInitialized)
            {
                Debug.LogError("InteractiveItem is not initialized. Call InitializeItem first.");
                return;
            }

            _thisSegment.SetPosition(position);
        }

        public void HandleSnakeInteraction(SnakeController snakeController)
        {
            if (_interactionCompleted) return;

            switch (SnakeInteraction)
            {
                case SnakeInteraction.EdibleItem:
                    snakeController.AddSegment();
                    _audioController.PlayEdibleItemSound();
                    break;
                case SnakeInteraction.InedibleItem:
                    snakeController.RemoveSegment();
                    _audioController.PlayInedibleItemSound();
                    break;
                case SnakeInteraction.SpeedUpItem:
                    snakeController.SpeedUpAction();
                    _audioController.PlaySpeedUpItemSound();
                    break;
                case SnakeInteraction.SlowDownItem:
                    snakeController.SlowDownAction();
                    _audioController.PlaySlowDownItemSound();
                    break;
                case SnakeInteraction.HeadTailSwapItem:
                    snakeController.HeadTailSwapAction();
                    _audioController.PlayHeadTailSwapItemSound();
                    break;
                // Add more cases as needed
                default:
                    Debug.LogError($"Unhandled SnakeInteraction: {SnakeInteraction}");
                    break;
            }

            _interactionCompleted = true;
        }

        public void MakeItemDisappear() // could be returned to a pool
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.InOutQuad))
                    .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack))
                    .OnComplete(() =>
                    {
                     Destroy(gameObject);
                    });
        }
    }
}
