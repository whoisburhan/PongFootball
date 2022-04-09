using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Ball ball;

    public Paddle playerPaddle;
    public int playerScore { get; private set; }
    public Text playerScoreText;

    public Paddle computerPaddle;
    public int computerScore { get; private set; }
    public Text computerScoreText;

    [Header("Goal Score Sptries")]
    [SerializeField] private List<Sprite> scoreTextSprites;
    [Space]
    [SerializeField] private SpriteRenderer leftSidePlayerScoreRenderer;
    [SerializeField] private SpriteRenderer rightSidePlayerScoreRenderer;


    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
    }

    public void NewGame()
    {
      //  SetPlayerScore(0);
      //  SetComputerScore(0);
        StartRound();
    }

    public void StartRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();
        ball.AddStartingForce();
    }

    public void PlayerScores()
    {
        SetPlayerScore(playerScore + 1);
        StartRound();
    }

    public void ComputerScores()
    {
        SetComputerScore(computerScore + 1);
        StartRound();
    }

    private void SetPlayerScore(int score)
    {
        Debug.Log("A");
        playerScore = score;
       // playerScoreText.text = score.ToString();
       ScoreGraphicsUpdate(rightSidePlayerScoreRenderer, score);
    }

    private void SetComputerScore(int score)
    {
        Debug.Log("COMPUTER");
        computerScore = score;
       // computerScoreText.text = score.ToString();
       ScoreGraphicsUpdate(leftSidePlayerScoreRenderer, score);
    }

    private void ScoreGraphicsUpdate(SpriteRenderer _sr, int score)
    {
        if(score >= 0 && score<scoreTextSprites.Count)
            _sr.sprite = scoreTextSprites[score];
    }

}
