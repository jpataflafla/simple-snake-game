using SnakeGame;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private int _score = 0;

    private const string BestScorePlayerPrefsKey = "BestScore";

    public void AddPoints(int points)
    {
        _score = Mathf.Max(0, _score + points);

        UpdateScoreText();

        if (_score > GetBestScore())
        {
            SetBestScore(_score);
        }
    }

    public void InitializeScoreBoard()
    {
        LoadBestScore();
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _score.ToString();
    }

    private void LoadBestScore()
    {
        int bestScore = PlayerPrefs.GetInt(BestScorePlayerPrefsKey, 0);
        SetBestScore(bestScore);
    }

    private int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScorePlayerPrefsKey, 0);
    }

    private void SetBestScore(int newBestScore)
    {
        PlayerPrefs.SetInt(BestScorePlayerPrefsKey, newBestScore);
        _bestScoreText.text = newBestScore.ToString();
    }

    // Saving best score to PlayerPrefs is a temporary solution
    // and a database /account system should be considered for a more robust solution.
}
