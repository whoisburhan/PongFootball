using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

namespace GS.PongFootball
{
    public enum PudSideView
    {
        Right = 0, Left
    }
    public enum DifficultyLevel
    {
        EASY = 0, MEDIUM, HARD
    }

    public enum GamePlayMode
    {
        OFFLINE = 0, LOCAL_MULTIPLAYER, GLOBAL_MULTIPLAYER
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsVibrateModeOn;
        public bool IsSoundModeOn;

        public bool IsPlay;

        public bool IsAllowedToQuit = true;

        public DifficultyLevel GameDifficultyLevel;

        public float[] DifficultySpeed = new float[3] { 15f, 8f, 11f };

        public GamePlayMode PlayMode;

        private PudSideView sideView = PudSideView.Right;
        public PudSideView SideView
        {
            get { return sideView;}
            set { 
                sideView = value;
                ChangeSideView(value == PudSideView.Right ? true : false);
            }
        }

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
        [SerializeField] public PudContainer localMultiplayerPudContainer;
        [HideInInspector] public string CurrentlySelectedPudOneHitAnimation = "USAHit";
        [HideInInspector] public string CurrentlySelectedPudTwoHitAnimation = "BRAHit";

        public int TargetGoal = 3;

        [Header("Goal Score Sptries")]
        [SerializeField] private List<Sprite> scoreTextSprites;
        [Space]
        [SerializeField] private SpriteRenderer leftSidePlayerScoreRenderer;
        [SerializeField] private SpriteRenderer rightSidePlayerScoreRenderer;

        [Header("CountDownObject")]
        [SerializeField] private GameObject countDownObject;

        [Header("Side View Objs")]
        [SerializeField] private Transform gameWorldObj;
        [SerializeField] private Transform leftSideScorePanel;
        [SerializeField] private Transform rightSideScorePanel;

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

        public void SetPaddleMode()
        {
            switch (PlayMode)
            {
                case GamePlayMode.OFFLINE:
                    playerPaddle.paddleType = PaddleType.PLAYER;
                    playerPaddle.speed = 15f;
                    computerPaddle.paddleType = PaddleType.AI;
                    computerPaddle.speed = DifficultySpeed[(int)GameDifficultyLevel];
                    break;
                case GamePlayMode.GLOBAL_MULTIPLAYER:
                    playerPaddle.paddleType = PaddleType.PLAYER;
                    playerPaddle.speed = 15f;
                    computerPaddle.paddleType = PaddleType.AI;
                    computerPaddle.speed = 10f;
                    break;
                case GamePlayMode.LOCAL_MULTIPLAYER:
                    playerPaddle.paddleType = PaddleType.PLAYER;
                    playerPaddle.speed = 15f;
                    computerPaddle.paddleType = PaddleType.PLAYER;
                    computerPaddle.speed = 15f;
                    break;
            }
        }
        public void StartCountDown()
        {
            //UIManager.Instance.SetUIState(UI_State.Null);
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
            SetPaddleMode();
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

        public void UpdateLocalMultiplayerPud()
        {
            // Set Blue Pud For Left Side
            SetPud(localMultiplayerPudContainer.container[0].Pud, localMultiplayerPudContainer.container[0].PudAnimation, true);
            //Set Purple Pud For Right Side
            SetPud(localMultiplayerPudContainer.container[1].Pud, localMultiplayerPudContainer.container[1].PudAnimation, false);
        }

        public void UpdateRandomOpponentPud()
        {
            int _index = UnityEngine.Random.Range(0, pudContainer.container.Count);

            while (_index == GameData.Instance.CurrentlySelectedPudIndex)
            {
                _index = UnityEngine.Random.Range(0, pudContainer.container.Count);
            }

            SetPud(pudContainer.container[_index].Pud, pudContainer.container[_index].PudAnimation, false);
        }
        public void UpdatePlayerPud(int pudIndex)
        {
            Debug.Log(pudIndex);
            SetPud(pudContainer.container[pudIndex].Pud, pudContainer.container[pudIndex].PudAnimation, true);
        }
        public void SetPud(Sprite pudSprite, string pudAnimationString, bool isPlayerPud)
        {
            Debug.Log(pudAnimationString);
            if (isPlayerPud)
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

        #region  Side View Change

        public void ChangeSideView(bool changeToRightSide)
        {
            if (changeToRightSide)
            {
                gameWorldObj.localScale = Vector3.one;
                leftSideScorePanel.localScale = Vector3.one;
                rightSideScorePanel.localScale = Vector3.one;
            }
            else
            {
                gameWorldObj.localScale = new Vector3(-1f, 1f, 1f);
                leftSideScorePanel.localScale = new Vector3(-1f, 1f, 1f);
                rightSideScorePanel.localScale = new Vector3(-1f, 1f, 1f);
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