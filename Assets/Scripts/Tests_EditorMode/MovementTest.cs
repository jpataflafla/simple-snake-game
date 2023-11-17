using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SnakeGame;
using UnityEngine.Accessibility;

public class MovementTest
{
    [Test]
    public void Snake__When_Initialize__Should_Have_Correct_Initial_State()
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

        Direction startHeadDirection = Direction.Up;
        // Act
        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: startHeadDirection);

        // Assert
        Assert.NotNull(snakeController.HeadPositionIndices);
        Assert.AreEqual(snakeController.HeadDirection, startHeadDirection);
    }

    [Test]
    public void Snake__When_MoveHead__Should_Change_Head_Direction()
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

        Direction startHeadDirection = Direction.Up;
        // Act
        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: startHeadDirection);

        // Act
        snakeController.MoveHead(Direction.Left);

        // Assert
        Assert.AreEqual(GetPrivateField<Direction>(snakeController, "_nextMoveHeadDirection"), Direction.Left);
    }

    [UnityTest]
    public IEnumerator Snake__When_AddSegment__Should_Increase_Snake_Length()
    {
        // Arrange
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

        Direction startHeadDirection = Direction.Up;
        snakeController.Initialize(gameBoard, startTileRowIndex: 0, startTileColumnIndex: 0,
            startHeadDirection: startHeadDirection);

        // Act
        snakeController.AddSegment(1);
        yield return null; // Wait for the next frame

        // Assert
        Assert.AreEqual(snakeController.SnakeLength, 4); // Assuming initial length is 3
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

    // Helper method to get the value of a private field
    private T GetPrivateField<T>(object instance, string fieldName)
    {
        System.Type type = instance.GetType();
        System.Reflection.FieldInfo field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field.GetValue(instance);
    }
}
