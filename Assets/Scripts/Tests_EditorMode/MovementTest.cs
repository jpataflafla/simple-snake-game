using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SnakeGame;

public class MovementTest
{
    [Test]
    public void Snake__When_move_in_direction__Should_move_snake_head()
    {
        // Arrange
        GameObject snakeObject = new GameObject();
        SnakeController snakeController = snakeObject.AddComponent<SnakeController>();
        SpriteRenderer snakeRenderer = snakeObject.AddComponent<SpriteRenderer>();
        snakeRenderer.sprite = CreateWhiteTestSprite(1000, 1000);

        var boardBuilder = new BoardBuilder();
        var gameBoard = boardBuilder.InitializeBoard(snakeRenderer, boardSize: 4,
            CreateWhiteTestSprite(100, 100), CreateWhiteTestSprite(100, 100), .2f, .2f);

        GameObject headObject = new GameObject();
        SpriteRenderer headRenderer = headObject.AddComponent<SpriteRenderer>();
        headRenderer.sprite = CreateWhiteTestSprite(1000, 1000);
        SetPrivateField(snakeController, "_headSpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_bodySpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_tailSpriteRenderer", headRenderer);

        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: Direction.Right);

        // Act
        snakeController.MoveHead(Direction.Right);

        // Assert
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 1));
    }

    [Test]
    public void Snake__When_move_in_direction__Should_change_head_orientation()
    {
        // Arrange
        GameObject snakeGameObject = new GameObject();
        SnakeController snakeController = snakeGameObject.AddComponent<SnakeController>();
        SpriteRenderer snakeRenderer = snakeGameObject.AddComponent<SpriteRenderer>();
        snakeRenderer.sprite = CreateWhiteTestSprite(1000, 1000);

        var boardBuilder = new BoardBuilder();
        var gameBoard = boardBuilder.InitializeBoard(snakeRenderer, boardSize: 4,
            CreateWhiteTestSprite(100, 100), CreateWhiteTestSprite(100, 100), .2f, .2f);

        GameObject headGameObject = new GameObject();
        SpriteRenderer headRenderer = headGameObject.AddComponent<SpriteRenderer>();
        headRenderer.sprite = CreateWhiteTestSprite(1000, 1000);
        SetPrivateField(snakeController, "_headSpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_bodySpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_tailSpriteRenderer", headRenderer);

        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: Direction.Up); // Set initial orientation and direction differently than next movement

        // Act
        snakeController.MoveHead(Direction.Right);

        // Assert
        Assert.AreEqual(snakeController.HeadDirection, Direction.Right);

    }

    [UnityTest]
    public IEnumerator Snake__When_moves_too_far_right_or_up__Should_appear_from_left_or_down()
    {
        // Arrange
        GameObject snakeObject = new GameObject();
        SnakeController snakeController = snakeObject.AddComponent<SnakeController>();
        SpriteRenderer snakeRenderer = snakeObject.AddComponent<SpriteRenderer>();
        snakeRenderer.sprite = CreateWhiteTestSprite(1000, 1000);

        var boardBuilder = new BoardBuilder();
        var gameBoard = boardBuilder.InitializeBoard(snakeRenderer, boardSize: 4,
            CreateWhiteTestSprite(100, 100), CreateWhiteTestSprite(100, 100), .2f, .2f);

        GameObject headObject = new GameObject();
        SpriteRenderer headRenderer = headObject.AddComponent<SpriteRenderer>();
        headRenderer.sprite = CreateWhiteTestSprite(1000, 1000);
        SetPrivateField(snakeController, "_headSpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_bodySpriteRenderer", headRenderer);
        SetPrivateField(snakeController, "_tailSpriteRenderer", headRenderer);

        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: Direction.Right);

        // Act and assert

        // Move right to the edge every frame
        snakeController.MoveHead(Direction.Right);
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 1));
        yield return null;

        // Move right again
        snakeController.MoveHead(Direction.Right);
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 2));
        yield return null;

        // Move right again
        snakeController.MoveHead(Direction.Right);
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 3));
        yield return null;

        // Move right to loop back to the left
        snakeController.MoveHead(Direction.Right);
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 0));
        yield return null;

        // Move up to the top and one time more - in a one frame
        snakeController.MoveHead(Direction.Up);
        snakeController.MoveHead(Direction.Up);
        snakeController.MoveHead(Direction.Up);
        snakeController.MoveHead(Direction.Up);
        Assert.AreEqual(snakeController.HeadPositionIndices, new Vector2Int(0, 0));
    }


    private Sprite CreateWhiteTestSprite(int width = 100, int height = 100)
    {
        // Create a texture for the sprite
        Texture2D texture = new Texture2D(width, height);

        // Fill the texture with a white color
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        // Create a sprite using the texture
        return Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
    }

    // Helper method to set the value of a private field using reflection
    private void SetPrivateField(object instance, string fieldName, object value)
    {
        System.Type type = instance.GetType();
        System.Reflection.FieldInfo field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(instance, value);
    }

    // Helper method to get the value of a private field using reflection
    private T GetPrivateField<T>(object instance, string fieldName)
    {
        System.Type type = instance.GetType();
        System.Reflection.FieldInfo field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field.GetValue(instance);
    }
}
