using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class BoardItemsManager : MonoBehaviour
    {
        [SerializeField] private Transform _parentForGeneratedItems;
        [SerializeField] private List<InteractiveItem> _interactiveItemsPrefabs;
        private List<InteractiveItem> _interactiveItems;

        private SpriteRenderer[,] _boardTiles;
        private SnakeController _snakeController;
        private IAudioController _audioController;
        private bool _isInitialized;
        private int _maxNumberOfItemsOnBoard, _minDistanceFromSnakesHead;

        public void InitializeBoardItems(SpriteRenderer[,] boardTiles, 
            SnakeController snakeController, 
            int maxNumberOfItemsOnBoard, int minDistanceFromSnakesHead,
            IAudioController audioController)
        {
            _boardTiles = boardTiles;
            _snakeController = snakeController;
            _interactiveItems = new List<InteractiveItem>();
            _audioController = audioController;

            _maxNumberOfItemsOnBoard = maxNumberOfItemsOnBoard;
            _minDistanceFromSnakesHead = minDistanceFromSnakesHead;
            //TryToGenerateInteractiveItems(maxNumberOfItemsOnBoard, minDistanceFromSnakesHead);

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            TryToGenerateInteractiveItems(_maxNumberOfItemsOnBoard, _minDistanceFromSnakesHead);
            InteractiveItem itemToDispose = null;
            foreach (var item in _interactiveItems)
            {
                if (_snakeController.IsSnakeHeadAtPosition(item.ItemPositionIndieces))
                {
                    item.HandleSnakeInteraction(_snakeController);
                    itemToDispose = item;
                }
            }
            _interactiveItems.Remove(itemToDispose);
            itemToDispose?.MakeItemDisappear(); // could be returned to a pool
        }

        private void TryToGenerateInteractiveItems(int maxNumberOfItemsOnBoard, int minDistanceFromSnakesHead = 1)
        {
            var numberOfTries = 0;
            int maxNumberOfTries = 2 * maxNumberOfItemsOnBoard;

            // Regenerate items if there are less than the minimum number
            while (_interactiveItems.Count < maxNumberOfItemsOnBoard
                && numberOfTries < maxNumberOfTries)
            {
                Vector2Int randomPosition = GetRandomEmptyPosition();

                if (!IsTooCloseToSnakesHead(randomPosition, minDistanceFromSnakesHead))
                {
                    InteractiveItem newItem = GetRandomInteractiveItem();

                    _interactiveItems.Add(newItem);

                    // Set the interactive item's position on the board
                    newItem.SetPosition(randomPosition);
                }
                maxNumberOfTries++;
            }
        }

        private bool IsTooCloseToSnakesHead(Vector2Int randomPosition, int minDistanceFromSnakesHead)
        {
            Vector2Int headPosition = _snakeController.HeadPositionIndices;

            int distanceX = Mathf.Abs(randomPosition.x - headPosition.x);
            int distanceY = Mathf.Abs(randomPosition.y - headPosition.y);

            return distanceX < minDistanceFromSnakesHead && distanceY < minDistanceFromSnakesHead;
        }


        private InteractiveItem GetRandomInteractiveItem()
        {
            if (_interactiveItemsPrefabs.Count == 0)
            {
                Debug.LogError("No interactive item prefabs available.");
                return null;
            }

            int randomIndex = Random.Range(0, _interactiveItemsPrefabs.Count);
            InteractiveItem newItem = Instantiate(_interactiveItemsPrefabs[randomIndex]);
            newItem.gameObject.SetActive(true);
            newItem.transform.parent = _parentForGeneratedItems;
            newItem.InitializeItem(Vector2Int.zero, Direction.Up, _boardTiles, _audioController);

            return newItem;
        }


        private Vector2Int GetRandomEmptyPosition()
        {
            // Generate a random position that is not occupied by the snake
            Vector2Int randomPosition;
            do
            {
                randomPosition = new Vector2Int(
                    Random.Range(0, _boardTiles.GetLength(0)),
                    Random.Range(0, _boardTiles.GetLength(1))
                );
            } while (_snakeController.IsAnySnakeSegmentAtPosition(randomPosition));

            return randomPosition;
        }
    }

}