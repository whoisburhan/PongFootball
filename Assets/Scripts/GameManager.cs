using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

namespace GS.PongFootball
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Ball ball;

        public Paddle playerPaddle;
        public int playerScore { get; private set; }
        public Text playerScoreText;

        public Paddle computerPaddle;
        public int computerScore { get; private set; }
        public Text computerScoreText;

        public int TargetGoal = 9;

        [Header("Goal Score Sptries")]
        [SerializeField] private List<Sprite> scoreTextSprites;
        [Space]
        [SerializeField] private SpriteRenderer leftSidePlayerScoreRenderer;
        [SerializeField] private SpriteRenderer rightSidePlayerScoreRenderer;

        [Header("CountDownObject")]
        [SerializeField] private GameObject countDownObject;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
        private void Start()
        {
            StartCountDown();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCountDown();
            }
        }

        public void StartCountDown()
        {
            countDownObject.SetActive(false);
            countDownObject.SetActive(true);
        }
        public void NewGame()
        {
            SetPlayerScore(0);
            SetComputerScore(0);
            StartRound();
        }

        public void StartRound()
        {
            if (playerScore < 9 && computerScore < 9)
            {
                StartCoroutine(Delay(() =>
                {
                    AudioManager.Instance.Play(AudioName.FREE_KICK);
                    playerPaddle.ResetPosition();
                    computerPaddle.ResetPosition();

                    ball.gameObject.SetActive(true);


                    ball.ResetPosition();
                    ball.AddStartingForce();
                    ball.GetComponent<TrailRenderer>().enabled = true;
                    ball.GetComponent<TrailRenderer>().Clear();
                }, 1.5f));
            }
            else
            {
                Debug.Log("GAME OVER");
            }

        }

        public void PlayerScores()
        {
            SetPlayerScore(playerScore + 1);
            //StartRound();
        }

        public void ComputerScores()
        {
            SetComputerScore(computerScore + 1);
            //StartRound();
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
            if (score >= 0 && score < scoreTextSprites.Count)
                _sr.sprite = scoreTextSprites[score];
        }

        IEnumerator Delay(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }
    }
}