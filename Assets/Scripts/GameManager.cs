using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

namespace GS.PongFootball
{
    public enum DifficultyLevel
    {
        EASY = 0, MEDIUM, HARD
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsVibrateModeOn;
        public bool IsSoundModeOn;

        public bool IsPlay;

        public bool IsAllowedToQuit = true;

        public DifficultyLevel GameDifficultyLevel;

        public Ball ball;

        public Paddle playerPaddle;
        public int playerScore { get; private set; }
        public Text playerScoreText;

        public Paddle computerPaddle;
        public int computerScore { get; private set; }
        public Text computerScoreText;

        [HideInInspector] public Vector2 tempVelocityStore = Vector2.zero;

        [Header("Puds")]
        [SerializeField] private SpriteRenderer RightSidePudOrPudOne, LeftSidePudOrPudTwo;
        [SerializeField] public PudContainer pudContainer;
        [HideInInspector] public string CurrentlySelectedPudOneHitAnimation = "USAHit";
        [HideInInspector] public string CurrentlySelectedPudTwoHitAnimation = "BRAHit";

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
            IsVibrateModeOn = true;
            IsSoundModeOn = true;
            IsPlay = false;
            ball.gameObject.SetActive(false);
            UpdatePlayerPud(GameData.Instance.CurrentlySelectedPudIndex);
            // StartCountDown();
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
            UIManager.Instance.SetUIState(UI_State.Null);
            ball.gameObject.SetActive(false);
            countDownObject.SetActive(false);
            countDownObject.SetActive(true);
            SetPlayerScore(0);
            SetComputerScore(0);
            UIManager.Instance.ActivatePauseButtonUI();
            UIManager.Instance.DeActivatePauseButtonInterectable();
        }
        public void NewGame()
        {
            SetPlayerScore(0);
            SetComputerScore(0);
            StartRound();
        }

        public void StartRound()
        {
            if (playerScore < TargetGoal && computerScore < TargetGoal)
            {
                IsPlay = true;
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
                    UIManager.Instance.ActivatePauseButtonInterectable();
                }, 1.5f));
            }
            else
            {
                IsPlay = false;
                UIManager.Instance.ActivateResultMenuCanvas(playerScore > computerScore, true);
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
            playerScore = score;
            // playerScoreText.text = score.ToString();
            ScoreGraphicsUpdate(rightSidePlayerScoreRenderer, score);
        }

        private void SetComputerScore(int score)
        {
            computerScore = score;
            // computerScoreText.text = score.ToString();
            ScoreGraphicsUpdate(leftSidePlayerScoreRenderer, score);
        }

        private void ScoreGraphicsUpdate(SpriteRenderer _sr, int score)
        {
            if (score >= 0 && score < scoreTextSprites.Count)
                _sr.sprite = scoreTextSprites[score];
        }

        public void DeActivateBallToPause()
        {
            Vector2 _velocity = ball.GetComponent<Rigidbody2D>().velocity;
            if (_velocity != Vector2.zero)
            {
                tempVelocityStore = ball.GetComponent<Rigidbody2D>().velocity;
            }
            ball.gameObject.SetActive(false);
        }
        public void ActivateBallFromPause()
        {
            ball.gameObject.SetActive(true);
            if (tempVelocityStore != Vector2.zero)
            {
                ball.GetComponent<Rigidbody2D>().velocity = tempVelocityStore;
            }
            else
            {
                StartRound();
            }
        }

        #region Pud Selection

        public void UpdatePlayerPud(int pudIndex)
        {
            Debug.Log(pudIndex);
            SetPud(pudContainer.container[pudIndex].Pud,pudContainer.container[pudIndex].PudAnimation,true);
        }
        public void SetPud(Sprite pudSprite, string pudAnimationString, bool isPudOne)
        {
            Debug.Log(pudAnimationString);
            if(isPudOne)
            {
                CurrentlySelectedPudOneHitAnimation = pudAnimationString;
                RightSidePudOrPudOne.gameObject.SetActive(false);
                RightSidePudOrPudOne.sprite = pudSprite;
                RightSidePudOrPudOne.gameObject.SetActive(true);
            }
            else
            {
                CurrentlySelectedPudTwoHitAnimation = pudAnimationString;
                LeftSidePudOrPudTwo.gameObject.SetActive(false);
                LeftSidePudOrPudTwo.sprite = pudSprite;
                LeftSidePudOrPudTwo.gameObject.SetActive(true);
            }
        }

        #endregion


        IEnumerator Delay(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }
    
    
    
    }
}