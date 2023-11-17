using SnakeGame;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreInfoManager
{
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private readonly float _actionTextTime = 1f;
    private int _score = 0;

    private const string BestScorePlayerPrefsKey = "BestScore";

    private Coroutine _actionTextAnim;

    public void ShowAction(string action)
    {
        if(_actionTextAnim != null)
        {
            StopCoroutine(_actionTextAnim);
        }
        _actionTextAnim = StartCoroutine(DisplayTextCoroutine(action, _actionTextTime));
    }

    private IEnumerator DisplayTextCoroutine(string newText, float displayTime)
    {
        _actionText.text = newText;
        _actionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        _actionText.text = "";
        _actionText.gameObject.SetActive(false);
    }


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
