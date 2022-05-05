using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GS.PongFootball
{
    public enum UI_State
    {
        Null = 0, StartMenu, PauseMenu, OptionsMenu, ResultMenu, QuitMenu, RestartConfimationMenu, ShopMenu, LanguageMenu, SetGamePointAndDifficulty
    }

    [Serializable]
    class StartMenuCanvasClass
    {
        [SerializeField] public CanvasGroup startMenuCanvasGroup;

        [Header("Elements of Start Menu Canvas Group")]
        public Transform titleImgTransform;
        public Transform titleImgStartPos;
        public Transform titleImgEndPos;
        [Space]
        public Transform startMenuButtonsParentTransform;

        [Header("Buttons")]
        public Button playButton;
        public Button rateButton, optionsButton, shareButton, moreGamesButton, selectLanguageButton;

        [Header("Multiplayers")]
        public Button localButton;
        public Button globalButton;
        public Transform localPos, globalPos;

        public void MultiplayerButtonFunc()
        {
            if (localButton.interactable) DeActivateMultiplayerOptions();
            else ActivateMultiplayerOptions();
        }
        private void ActivateMultiplayerOptions()
        {
            shareButton.interactable = false;
            localButton.transform.DOMove(localPos.position, 0.5f);
            localButton.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f).OnComplete(() =>
            {
                localButton.interactable = true;
                shareButton.interactable = true;
            });

            globalButton.transform.DOMove(globalPos.position, 0.5f);
            globalButton.transform.DOScale(new Vector3(0.8f, 0.8f), 0.5f).OnComplete(() =>
            {
                globalButton.interactable = true;
            });
        }

        private void DeActivateMultiplayerOptions()
        {
            shareButton.interactable = false;
            localButton.transform.DOMove(shareButton.transform.position, 0.5f);
            localButton.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                localButton.interactable = false;
                shareButton.interactable = true;
            });

            globalButton.transform.DOMove(shareButton.transform.position, 0.5f);
            globalButton.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                globalButton.interactable = false;
            });
        }

    }

    [Serializable]
    class PauseMenuCanvasClass
    {
        public CanvasGroup pauseMenuCanvasGroup;

        public Transform pauseMenuButtonsParentTransform;

        [Header("Buttons")]
        public Button homeButtons;
        public Button restartButton, optionsButton, continueButton;
    }

    [Serializable]
    class OptionsMenuCanvasClass
    {
        public CanvasGroup optionsMenuCanvasGroup;

        public Transform optionsButtonsParentTransform;
        public Button confirmationButton;

        [Header("Common Sprites for Sound & Vibration Settings")]

        public Sprite OnButtonSprite;
        public Sprite OffButtonSprite;

        [Header("Sound Settings")]
        public Button soundButton;
        public Image SoundButtonImg;

        [Header("Vibration Settings")]
        public Button vibrationButton;
        public Image vibrationButtonImg;

        [Header("Difficulty Settings")]
        public Text difficultyLevelText;
        [Space]
        public Button leftSideArrowButton;
        public Button rightSideArrowButton;

    }

    [Serializable]
    class RestartConfirmationCanvasClass
    {
        public CanvasGroup restartConfirmationCanvasGroup;

        public Transform restartConfirmationButtonsParentTransform;

        [Header("Buttons")]
        public Button yesButton;
        public Button noButton;
    }

    [Serializable]
    class QuitConfirmationCanvasClass
    {
        public CanvasGroup quitConfirmationCanvasGroup;

        public Transform quitConfirmationButtonsParentTransform;

        [Header("Buttons")]
        public Button yesButton;
        public Button noButton;
    }

    [Serializable]
    class ResultCanvasClass
    {
        public CanvasGroup resultCanvasGroup;

        public Transform resultCanvasButtonsParentTransform;

        [Header("Match Result")]
        public Image matchResultImg;
        [Space]
        public Sprite matchWinSprite;
        public Sprite matchLoseSprite;

        [Header("Buttons")]
        public Button homeButton;
        public Button restartButton, optionsButton, rateButton;

        #region COIN ANIMATION
        [Space]
        [Header("Available coins : (coins to pool)")]
        [HideInInspector] public int MaxCoins = 50;
        public Queue<GameObject> coinsQueue = new Queue<GameObject>();

        [Space]
        [Header("COIN ANIMATION")]
        public Text coinRewardText;
        public Transform coinsForAnimationParent;
        public GameObject animatedCoinPrefab;

        private int resultCoinAmount = 0;
        public int ResultCoinAmount
        {
            get { return resultCoinAmount; }
            set
            {
                resultCoinAmount = value;
                if (resultCoinAmount <= 0)
                    resultCoinAmount = 0;
                coinRewardText.text = resultCoinAmount.ToString();
            }
        }


        [Space]
        [Header("Animation settings")]
        [Range(0.5f, 0.9f)] public float minAnimDuration;
        [Range(0.9f, 2f)] public float maxAnimDuration;

        public Ease easeType;
        public float spread;

        #endregion
    }

    [Serializable]
    class ShopCanvasClass
    {
        public CanvasGroup shopCanvasGroup;

        public Transform shopCanvasButtonsParentTransform;

        public Button ConfirmButton;
    }

    [Serializable]
    class LanguageCanvasClass
    {
        public LanguageManager languageManager;
        public CanvasGroup LanguageCanvasGroup;

        public Transform LanguageCanvasButtonsParentTransform;

        public Text LanguageText;
        public Button ConfirmButton, LeftArrowButton, RightArrowButton;

        public int index = 0;

        public List<string> LanguagesName = new List<string>(9);

        public void Init()
        {
            index = languageManager.SelectedLanguage;
            SelectedLanguageTextFunc();

            LeftArrowButton.onClick.AddListener(() => { DecrementButonFunc(); });
            RightArrowButton.onClick.AddListener(() => { IncrementButtonFunc(); });
            ConfirmButton.onClick.AddListener(() => { ConfirmButtonFunc(); });

        }

        private void SelectedLanguageTextFunc()
        {
            LanguageText.text = LanguagesName[index % LanguagesName.Count];
        }
        private void IncrementButtonFunc()
        {
            Debug.Log("A");
            index++;
            if (index >= LanguagesName.Count) index = 0;
            SelectedLanguageTextFunc();
        }

        private void DecrementButonFunc()
        {
            index--;
            if (index < 0) index = LanguagesName.Count - 1;
            SelectedLanguageTextFunc();
        }

        private void ConfirmButtonFunc()
        {
            languageManager.SelectedLanguage = index;

            DeActivateLanguageMenuCanvas(() =>
            {
                UIManager.Instance.ActivateStartMenuCanvas();
            });
        }

        public void ActivateLanguageMenuCanvas()
        {
            GameManager.Instance.IsPlay = false;
            LanguageCanvasGroup.alpha = 1;
            LanguageCanvasGroup.interactable = true;
            LanguageCanvasGroup.blocksRaycasts = true;
            LanguageCanvasButtonsParentTransform.DOScale(Vector3.one, 1f);


            UIManager.Instance.SetUIState(UI_State.LanguageMenu);
        }

        public void DeActivateLanguageMenuCanvas(Action action = null)
        {
            LanguageCanvasGroup.interactable = false;
            LanguageCanvasGroup.blocksRaycasts = false;
            LanguageCanvasButtonsParentTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                LanguageCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
            });
        }
    }

    [Serializable]
    class SetMatchCanvasClass
    {
        public CanvasGroup SetMatchCanvasGroup;

        public Transform SetMatchCanvasButtonsParentTransform;

        [Header("Buttons")]
        public Button ContinueButton;

        public List<Button> DifficultyButtons;
        public Image DifficultyImage;
        public void Init()
        {
            ContinueButton.onClick.AddListener(() =>
            {
                DeActivateSetMatchMenuCanvas(() =>
                {
                    GameManager.Instance.StartCountDown();
                });
            });
        }
        public void ActivateSetMatchMenuCanvas()
        {
            switch (GameManager.Instance.PlayMode)
            {
                case GamePlayMode.OFFLINE:
                    GameManager.Instance.IsPlay = false;
                    SetMatchCanvasGroup.alpha = 1;
                    SetMatchCanvasGroup.interactable = true;
                    SetMatchCanvasGroup.blocksRaycasts = true;
                    SetMatchCanvasButtonsParentTransform.DOScale(Vector3.one, 1f);
                    ActivateDifficultyButtons();
                    break;
                case GamePlayMode.LOCAL_MULTIPLAYER:
                    GameManager.Instance.IsPlay = false;
                    SetMatchCanvasGroup.alpha = 1;
                    SetMatchCanvasGroup.interactable = true;
                    SetMatchCanvasGroup.blocksRaycasts = true;
                    SetMatchCanvasButtonsParentTransform.DOScale(Vector3.one, 1f);
                    DeActivateDifficultyButtons();
                    GroundChanger.Instance.ActivateLocalMultiplayerField();
                    break;
                case GamePlayMode.GLOBAL_MULTIPLAYER:
                    /// Not Implmented Yet
                    break;
            }



            UIManager.Instance.SetUIState(UI_State.Null);
        }

        public void DeActivateSetMatchMenuCanvas(Action action = null)
        {
            SetMatchCanvasGroup.interactable = false;
            SetMatchCanvasGroup.blocksRaycasts = false;
            SetMatchCanvasButtonsParentTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                SetMatchCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
            });
        }

        public void ActivateDifficultyButtons()
        {
            foreach (var b in DifficultyButtons) b.interactable = true;
            DifficultyImage.color = new Color(DifficultyImage.color.r, DifficultyImage.color.g, DifficultyImage.color.b, 1f);
        }

        public void DeActivateDifficultyButtons()
        {
            foreach (var b in DifficultyButtons) b.interactable = false;
            DifficultyImage.color = new Color(DifficultyImage.color.r, DifficultyImage.color.g, DifficultyImage.color.b, 0.5f);
        }

    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private UI_State currentUIState, previousUIState;

        [SerializeField] private Text totalCoinText;
        [SerializeField] private Image totalCoinTextImg;

        [SerializeField] private StartMenuCanvasClass startMenuCanvasClass;
        [SerializeField] private PauseMenuCanvasClass pauseMenuCanvasClass;
        [SerializeField] private OptionsMenuCanvasClass optionsMenuCanvasClass;
        [SerializeField] private RestartConfirmationCanvasClass restartConfirmationCanvasClass;
        [SerializeField] private QuitConfirmationCanvasClass quitConfirmationCanvasClass;
        [SerializeField] private ResultCanvasClass resultCanvasClass;
        [SerializeField] private LanguageCanvasClass languageCanvasClass;

        [SerializeField] private SetMatchCanvasClass setMatchCanvasClass;

        [SerializeField] private ShopCanvasClass shopCanvasClass;

        [Header("Buttons")]
        [SerializeField] private Button pasueButton;

        [HideInInspector] public bool FirstTimeGameOn = false;

        private bool tempWinStatus = true;  //Hold Win or lose status value

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
            currentUIState = UI_State.StartMenu;
            UpdateCoinInUI();

            if (!FirstTimeGameOn)
            {
                ActivateStartMenuCanvas();
                DeActivatePauseButtonUI();
            }
            else
            {
                SelectedLanguageButtonFunc();
            }

            //Init All the button function
            InItStartMenuButtonsFunc();
            InitPauseMenuButtonsFunc();
            InitResultMenuButtonsFunc();
            InitOptionsMenuButtonsFunc();
            InitQuitMenuButtonsFunc();
            InitRestartConfimationMenuButtonsFunc();
            InitShopMenuButtonsFunc();
            languageCanvasClass.Init();
            setMatchCanvasClass.Init();
        }

        private void Update()
        {
            GameQuitRequestFunc();
        }

        public void SetUIState(UI_State state)
        {
            previousUIState = currentUIState != previousUIState ? currentUIState : previousUIState;
            currentUIState = state;
        }

        public void UpdateCoinInUI()
        {
            totalCoinText.text = CoinSystem.Instance.GetCoin().ToString();
        }

        #region Start Menu Canvas Func
        public void ActivateStartMenuCanvas()
        {
            GameManager.Instance.IsPlay = false;
            startMenuCanvasClass.startMenuCanvasGroup.alpha = 1;
            startMenuCanvasClass.startMenuCanvasGroup.interactable = true;
            startMenuCanvasClass.startMenuCanvasGroup.blocksRaycasts = true;
            startMenuCanvasClass.startMenuButtonsParentTransform.DOScale(Vector3.one, 1f);
            startMenuCanvasClass.titleImgTransform.DOMove(startMenuCanvasClass.titleImgEndPos.position, 1f);

            SetUIState(UI_State.StartMenu);
        }

        public void DeActivateStartMenuCanvas(Action action = null)
        {
            startMenuCanvasClass.startMenuCanvasGroup.interactable = false;
            startMenuCanvasClass.startMenuCanvasGroup.blocksRaycasts = false;
            startMenuCanvasClass.startMenuButtonsParentTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                startMenuCanvasClass.startMenuCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
            });
            startMenuCanvasClass.titleImgTransform.DOMove(startMenuCanvasClass.titleImgStartPos.position, 0.5f);
        }

        private void InItStartMenuButtonsFunc()
        {
            startMenuCanvasClass.playButton.onClick.AddListener(() => { PlayButtonFunc(); });

            startMenuCanvasClass.rateButton.onClick.AddListener(() => { RateUs(); });

            startMenuCanvasClass.optionsButton.onClick.AddListener(() => { OptionsButtonFunc(); });

            startMenuCanvasClass.shareButton.onClick.AddListener(() => { startMenuCanvasClass.MultiplayerButtonFunc(); });

            startMenuCanvasClass.moreGamesButton.onClick.AddListener(() => { ShopButtonFunc(); });

            startMenuCanvasClass.selectLanguageButton.onClick.AddListener(() => { SelectedLanguageButtonFunc(); });

            startMenuCanvasClass.localButton.onClick.AddListener(() => { PlayLocalButtonFunc(); });

            startMenuCanvasClass.localButton.onClick.AddListener(() => { PlayGlobalButtonFunc(); });
        }

        private void PlayButtonFunc()
        {
            DeActivateStartMenuCanvas(() =>
            {
                GameManager.Instance.PlayMode = GamePlayMode.OFFLINE;
                setMatchCanvasClass.ActivateSetMatchMenuCanvas();
                ActivatePauseButtonUI();
            });
        }

        private void PlayLocalButtonFunc()
        {
            DeActivateStartMenuCanvas(() =>
            {
                GameManager.Instance.PlayMode = GamePlayMode.LOCAL_MULTIPLAYER;
                setMatchCanvasClass.ActivateSetMatchMenuCanvas();
                ActivatePauseButtonUI();
            });
        }

        private void PlayGlobalButtonFunc()
        {
            DeActivateStartMenuCanvas(() =>
            {
                GameManager.Instance.PlayMode = GamePlayMode.GLOBAL_MULTIPLAYER;
                setMatchCanvasClass.ActivateSetMatchMenuCanvas();
                ActivatePauseButtonUI();
            });
        }

        private void OptionsButtonFunc()
        {
            DeActivateStartMenuCanvas(() =>
            {
                ActivateOptionsMenuCanvas();
            });
        }

        private void ShareButtonFunc()
        {

        }

        private void ShopButtonFunc()
        {
            DeActivateStartMenuCanvas(() =>
            {
                ActivateShopMenuCanvas();
            });
        }

        private void SelectedLanguageButtonFunc()
        {
            DeActivateStartMenuCanvas(() => { languageCanvasClass.ActivateLanguageMenuCanvas(); });
        }
        #endregion

        #region Pause Menu Canvas Func

        public void ActivatePauseButtonUI()
        {
            pasueButton.gameObject.SetActive(true);
        }

        public void DeActivatePauseButtonUI()
        {
            pasueButton.gameObject.SetActive(false);
        }

        public void ActivatePauseButtonInterectable()
        {
            pasueButton.interactable = true;
        }
        public void DeActivatePauseButtonInterectable()
        {
            pasueButton.interactable = false;
        }

        public void ActivatePauseMenuCanvas()
        {
            GameManager.Instance.IsPlay = false;
            GameManager.Instance.DeActivateBallToPause();
            DeActivatePauseButtonUI();
            pauseMenuCanvasClass.pauseMenuCanvasGroup.alpha = 1;
            pauseMenuCanvasClass.pauseMenuCanvasGroup.interactable = true;
            pauseMenuCanvasClass.pauseMenuCanvasGroup.blocksRaycasts = true;
            pauseMenuCanvasClass.pauseMenuButtonsParentTransform.transform.DOScale(Vector3.one, 0.7f);

            SetUIState(UI_State.PauseMenu);
        }

        public void DeActivatePauseMenuCanvas(bool resumeGame = true, Action action = null)
        {
            Debug.Log("AAA " + resumeGame);
            UIManager.Instance.ActivatePauseButtonInterectable();

            pauseMenuCanvasClass.pauseMenuCanvasGroup.interactable = false;
            pauseMenuCanvasClass.pauseMenuCanvasGroup.blocksRaycasts = false;
            pauseMenuCanvasClass.pauseMenuButtonsParentTransform.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                pauseMenuCanvasClass.pauseMenuCanvasGroup.alpha = 0;
                if (resumeGame)
                {
                    GameManager.Instance.ActivateBallFromPause();
                    GameManager.Instance.IsPlay = true;

                    SetUIState(UI_State.Null);
                }

                ActivatePauseButtonUI();

                if (action != null) action?.Invoke();
            });
        }

        private void InitPauseMenuButtonsFunc()
        {
            pasueButton.onClick.AddListener(() => { PauseButtonFunc(); });

            pauseMenuCanvasClass.homeButtons.onClick.AddListener(() =>
            {
                DeActivatePauseMenuCanvas(false);
                Home();
                Shop.Instance.CurrentySelectedShopItemField.ActivateItem();
            });

            pauseMenuCanvasClass.restartButton.onClick.AddListener(() => { PauseMenuRestartButtonFunc(); });

            pauseMenuCanvasClass.optionsButton.onClick.AddListener(() => { PauseMenuOptionsButtonFunc(); });

            pauseMenuCanvasClass.continueButton.onClick.AddListener(() => { ContnueButtonFromPauseMenuFunc(); });
        }

        private void PauseButtonFunc()
        {
            ActivatePauseMenuCanvas();
        }

        private void PauseMenuRestartButtonFunc()
        {
            DeActivatePauseMenuCanvas(false, () => { ActivateReStartConfimationMenuCanvas(); });
        }

        private void PauseMenuOptionsButtonFunc()
        {
            DeActivatePauseMenuCanvas(false, () =>
            {
                ActivateOptionsMenuCanvas();
            });
        }

        private void ContnueButtonFromPauseMenuFunc()
        {
            DeActivatePauseMenuCanvas();
            GameManager.Instance.IsPlay = true;
        }
        #endregion

        #region Result Menu Canvas Func

        public void ActivateResultMenuCanvas(bool isWon, bool giveReward = false)
        {
            tempWinStatus = isWon;
            GameManager.Instance.IsPlay = false;
            DeActivatePauseButtonUI();
            resultCanvasClass.matchResultImg.sprite = isWon ? resultCanvasClass.matchWinSprite : resultCanvasClass.matchLoseSprite;

            resultCanvasClass.resultCanvasGroup.alpha = 1f;
            resultCanvasClass.resultCanvasGroup.interactable = true;
            resultCanvasClass.resultCanvasGroup.blocksRaycasts = true;
            resultCanvasClass.resultCanvasButtonsParentTransform.transform.DOScale(Vector3.one, 0.7f).OnComplete(() =>
            {
                if (giveReward)
                {
                    CoinAddAnimation(isWon ? 100 : 20);
                }
            });

            SetUIState(UI_State.ResultMenu);

            resultCanvasClass.ResultCoinAmount = isWon ? 100 : 20;
        }

        public void DeActivateResultMenuCanvas(Action action = null)
        {
            resultCanvasClass.resultCanvasGroup.interactable = false;
            resultCanvasClass.resultCanvasGroup.blocksRaycasts = false;
            resultCanvasClass.resultCanvasButtonsParentTransform.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                resultCanvasClass.resultCanvasGroup.alpha = 0f;
                if (action != null) action?.Invoke();
            });
        }

        private void InitResultMenuButtonsFunc()
        {
            resultCanvasClass.homeButton.onClick.AddListener(() =>
            {
                DeActivateResultMenuCanvas();
                Home();
                Shop.Instance.CurrentySelectedShopItemField.ActivateItem();
            });
            resultCanvasClass.restartButton.onClick.AddListener(() => { ResultMenuRestartButtonFunc(); });
            resultCanvasClass.optionsButton.onClick.AddListener(() => { ResultMenuOptionsButtonFunc(); });
            //resultCanvasClass.rateButton.onClick.AddListener(() => { RateUs(); });

            //Prepare coin animation obj (object-pool)
            PrepareCoins();
        }

        private void ResultMenuRestartButtonFunc()
        {
            DeActivateResultMenuCanvas(() => { ActivateReStartConfimationMenuCanvas(); });
        }
        private void ResultMenuOptionsButtonFunc()
        {
            DeActivateResultMenuCanvas(() =>
            {
                ActivateOptionsMenuCanvas();
            });
        }

        private void PrepareCoins()
        {
            GameObject coin;
            for (int i = 0; i < resultCanvasClass.MaxCoins; i++)
            {
                coin = Instantiate(resultCanvasClass.animatedCoinPrefab);
                coin.transform.parent = resultCanvasClass.coinsForAnimationParent;
                coin.SetActive(false);
                resultCanvasClass.coinsQueue.Enqueue(coin);
            }
        }

        void CoinAddAnimation(int amount)
        {
            int coinPerFraction = amount > resultCanvasClass.MaxCoins ? amount / resultCanvasClass.MaxCoins : 1;
            AudioManager.Instance.Play(AudioName.COIN_SOUND);

            resultCanvasClass.coinRewardText.DOCounter(amount, 0, 0.5f);

            for (int i = 0; i < amount; i++)
            {
                //check if there's coins in the pool
                if (resultCanvasClass.coinsQueue.Count > 0)
                {
                    //extract a coin from the pool
                    GameObject coin = resultCanvasClass.coinsQueue.Dequeue();
                    coin.SetActive(true);
                    coin.transform.localScale = Vector3.one;

                    //move coin to the collected coin pos
                    coin.transform.position = resultCanvasClass.coinsForAnimationParent.transform.position + new Vector3(UnityEngine.Random.Range(-resultCanvasClass.spread, resultCanvasClass.spread), 0f, 0f);

                    //animate coin to target position
                    float duration = UnityEngine.Random.Range(resultCanvasClass.minAnimDuration, resultCanvasClass.maxAnimDuration);
                    coin.transform.DOMove(totalCoinTextImg.transform.position, duration)
                    .SetEase(resultCanvasClass.easeType)
                    .OnComplete(() =>
                    {
                        //executes whenever coin reach target position
                        coin.SetActive(false);
                        resultCanvasClass.coinsQueue.Enqueue(coin); ;
                        CoinSystem.Instance.AddCoin(coinPerFraction);

                    });


                }
            }
        }

        #endregion

        #region Options Menu Canvas Func


        public void ActivateOptionsMenuCanvas(bool changeUIState = true)
        {
            optionsMenuCanvasClass.optionsMenuCanvasGroup.alpha = 1;
            optionsMenuCanvasClass.optionsMenuCanvasGroup.interactable = true;
            optionsMenuCanvasClass.optionsMenuCanvasGroup.blocksRaycasts = true;
            optionsMenuCanvasClass.optionsButtonsParentTransform.DOScale(Vector3.one, 1f);

            if (changeUIState)
            {
                SetUIState(UI_State.OptionsMenu);
            }
        }

        public void DeActivateOptionsMenuCanvas(Action action = null, Action<bool, bool> actionWithBool = null)
        {
            optionsMenuCanvasClass.optionsMenuCanvasGroup.interactable = false;
            optionsMenuCanvasClass.optionsMenuCanvasGroup.blocksRaycasts = false;
            optionsMenuCanvasClass.optionsButtonsParentTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                optionsMenuCanvasClass.optionsMenuCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
                if (actionWithBool != null) actionWithBool?.Invoke(tempWinStatus, false);
            });
        }

        private void InitOptionsMenuButtonsFunc()
        {
            optionsMenuCanvasClass.confirmationButton.onClick.AddListener(() => { OnOptionExitFunc(); });
            optionsMenuCanvasClass.soundButton.onClick.AddListener(() => { SoundSettingsFunc(); });
            optionsMenuCanvasClass.vibrationButton.onClick.AddListener(() => { VibrationSettingsFunc(); });
            optionsMenuCanvasClass.leftSideArrowButton.onClick.AddListener(() => { DifficultyIncreaseDecreaseButton(false); });
            optionsMenuCanvasClass.rightSideArrowButton.onClick.AddListener(() => { DifficultyIncreaseDecreaseButton(true); });
        }

        private void SoundSettingsFunc()
        {
            bool _soundModeStatus = !GameManager.Instance.IsSoundModeOn;
            AudioManager.Instance.MuteUnmute(!_soundModeStatus);
            GameManager.Instance.IsSoundModeOn = _soundModeStatus;

            optionsMenuCanvasClass.soundButton.interactable = false;
            optionsMenuCanvasClass.SoundButtonImg.rectTransform.DOAnchorPosX(optionsMenuCanvasClass.SoundButtonImg.rectTransform.anchoredPosition.x * -1f, 0.2f).OnComplete(() =>
            {
                optionsMenuCanvasClass.soundButton.interactable = true;
                optionsMenuCanvasClass.SoundButtonImg.sprite = _soundModeStatus ? optionsMenuCanvasClass.OnButtonSprite : optionsMenuCanvasClass.OffButtonSprite;
            });
        }

        private void VibrationSettingsFunc()
        {
            bool _vibrationModeStatus = !GameManager.Instance.IsVibrateModeOn;
            GameManager.Instance.IsVibrateModeOn = _vibrationModeStatus;

            optionsMenuCanvasClass.vibrationButton.interactable = false;
            optionsMenuCanvasClass.vibrationButtonImg.rectTransform.DOAnchorPosX(optionsMenuCanvasClass.vibrationButtonImg.rectTransform.anchoredPosition.x * -1f, 0.2f).OnComplete(() =>
            {
                optionsMenuCanvasClass.vibrationButton.interactable = true;
                optionsMenuCanvasClass.vibrationButtonImg.sprite = _vibrationModeStatus == true ? optionsMenuCanvasClass.OnButtonSprite : optionsMenuCanvasClass.OffButtonSprite;
            });
        }

        private void DifficultyIncreaseDecreaseButton(bool isIncrease)
        {
            if (isIncrease)
            {
                switch (GameManager.Instance.GameDifficultyLevel)
                {
                    case DifficultyLevel.EASY:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.MEDIUM;
                        break;
                    case DifficultyLevel.MEDIUM:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.HARD;
                        break;
                    case DifficultyLevel.HARD:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.EASY;
                        break;
                }
            }
            else
            {
                switch (GameManager.Instance.GameDifficultyLevel)
                {
                    case DifficultyLevel.EASY:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.HARD;
                        break;
                    case DifficultyLevel.MEDIUM:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.EASY;
                        break;
                    case DifficultyLevel.HARD:
                        GameManager.Instance.GameDifficultyLevel = DifficultyLevel.MEDIUM;
                        break;
                }
            }

            optionsMenuCanvasClass.difficultyLevelText.text = DifultyLevelString(GameManager.Instance.GameDifficultyLevel);
        }

        private string DifultyLevelString(DifficultyLevel difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case DifficultyLevel.EASY:
                    return "Easy";
                case DifficultyLevel.MEDIUM:
                    return "Medium";
                case DifficultyLevel.HARD:
                    return "Hard";
            }
            return null;
        }

        private void OnOptionExitFunc()
        {
            Action tempAction = null;
            Action<bool, bool> tempActionWithBool = null;
            if (previousUIState == UI_State.StartMenu)
            {
                tempAction += ActivateStartMenuCanvas;
            }
            else if (previousUIState == UI_State.PauseMenu)
            {
                tempAction += ActivatePauseMenuCanvas;
            }
            else if (previousUIState == UI_State.ResultMenu)
            {
                tempActionWithBool += ActivateResultMenuCanvas;
            }

            DeActivateOptionsMenuCanvas(tempAction, tempActionWithBool);
        }

        #endregion

        #region Quit Confirmation Menu Canvas Func

        private void GameQuitRequestFunc()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (currentUIState)
                {
                    case UI_State.Null:
                        /* if (GameManager.Instance.IsPlay)    // To avoid Quit Menu Pop Up In CountDown Animation
                         {
                             GameManager.Instance.IsPlay = false;
                             GameManager.Instance.DeActivateBallToPause();
                             DeActivatePauseButtonUI();
                             ActivateQuitConfimationMenuCanvas();
                         }*/
                        break;
                    case UI_State.StartMenu:
                        DeActivateStartMenuCanvas(() => { ActivateQuitConfimationMenuCanvas(); });
                        break;
                    case UI_State.PauseMenu:
                        //DeActivatePauseMenuCanvas(false, () => { ActivateQuitConfimationMenuCanvas(); });
                        break;
                    case UI_State.OptionsMenu:
                        //DeActivateOptionsMenuCanvas(() => { ActivateQuitConfimationMenuCanvas(); });
                        break;
                    case UI_State.ResultMenu:
                        //DeActivateResultMenuCanvas(() => { ActivateQuitConfimationMenuCanvas(); });
                        break;
                    case UI_State.QuitMenu:
                        break;
                    case UI_State.RestartConfimationMenu:
                        break;
                }
            }

        }

        private void ActivateQuitConfimationMenuCanvas()
        {
            GameManager.Instance.IsPlay = false;

            quitConfirmationCanvasClass.quitConfirmationCanvasGroup.alpha = 1f;
            quitConfirmationCanvasClass.quitConfirmationCanvasGroup.interactable = true;
            quitConfirmationCanvasClass.quitConfirmationCanvasGroup.blocksRaycasts = true;
            quitConfirmationCanvasClass.quitConfirmationButtonsParentTransform.transform.DOScale(Vector3.one, 0.7f);
        }

        private void DeActivateQuitConfimationMenuCanvas(Action action = null)
        {
            quitConfirmationCanvasClass.quitConfirmationCanvasGroup.interactable = false;
            quitConfirmationCanvasClass.quitConfirmationCanvasGroup.blocksRaycasts = false;
            quitConfirmationCanvasClass.quitConfirmationButtonsParentTransform.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                quitConfirmationCanvasClass.quitConfirmationCanvasGroup.alpha = 0f;
                if (action != null) action?.Invoke();
                switch (currentUIState)
                {
                    case UI_State.Null:
                        //GameManager.Instance.ActivateBallFromPause();
                        // GameManager.Instance.IsPlay = true;
                        // ActivatePauseButtonUI();
                        break;
                    case UI_State.StartMenu:
                        ActivateStartMenuCanvas();
                        break;
                    case UI_State.PauseMenu:
                        //ActivatePauseMenuCanvas();
                        break;
                    case UI_State.OptionsMenu:
                        //ActivateOptionsMenuCanvas(false);
                        break;
                    case UI_State.ResultMenu:
                        // ActivateResultMenuCanvas(tempWinStatus);
                        break;

                }
            });
        }

        private void InitQuitMenuButtonsFunc()
        {
            quitConfirmationCanvasClass.yesButton.onClick.AddListener(() => { QuitMenuYesButtonFunc(); });
            quitConfirmationCanvasClass.noButton.onClick.AddListener(() => { QuitMenuNoButtonFunc(); });
        }

        private void QuitMenuYesButtonFunc()
        {
            Application.Quit();
        }

        private void QuitMenuNoButtonFunc()
        {
            DeActivateQuitConfimationMenuCanvas();
        }

        #endregion

        #region ReStart Confimation Menu Canvas Func

        public void ActivateReStartConfimationMenuCanvas()
        {
            restartConfirmationCanvasClass.restartConfirmationCanvasGroup.alpha = 1;
            restartConfirmationCanvasClass.restartConfirmationCanvasGroup.interactable = true;
            restartConfirmationCanvasClass.restartConfirmationCanvasGroup.blocksRaycasts = true;
            restartConfirmationCanvasClass.restartConfirmationButtonsParentTransform.DOScale(Vector3.one, 0.7f);
        }

        public void DeActivateReStartConfimationMenuCanvas(Action action = null)
        {
            restartConfirmationCanvasClass.restartConfirmationCanvasGroup.interactable = false;
            restartConfirmationCanvasClass.restartConfirmationCanvasGroup.blocksRaycasts = false;
            restartConfirmationCanvasClass.restartConfirmationButtonsParentTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                restartConfirmationCanvasClass.restartConfirmationCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
            });
        }

        private void InitRestartConfimationMenuButtonsFunc()
        {
            restartConfirmationCanvasClass.yesButton.onClick.AddListener(() => { RestartMenuYesButtonFunc(); });
            restartConfirmationCanvasClass.noButton.onClick.AddListener(() => { RestartMenuNoButtonFunc(); });
        }

        private void RestartMenuYesButtonFunc()
        {
            DeActivateReStartConfimationMenuCanvas(() => { GameManager.Instance.StartCountDown(); });
        }

        private void RestartMenuNoButtonFunc()
        {
            DeActivateReStartConfimationMenuCanvas(() =>
            {
                switch (currentUIState)
                {
                    case UI_State.PauseMenu:
                        ActivatePauseMenuCanvas();
                        break;
                    case UI_State.ResultMenu:
                        ActivateResultMenuCanvas(tempWinStatus);
                        break;
                }
            });
        }

        #endregion

        #region  Shop Menu Canvas Func

        public void ActivateShopMenuCanvas()
        {
            SetUIState(UI_State.ShopMenu);
            shopCanvasClass.shopCanvasGroup.alpha = 1;
            shopCanvasClass.shopCanvasGroup.interactable = true;
            shopCanvasClass.shopCanvasGroup.blocksRaycasts = true;
            shopCanvasClass.shopCanvasButtonsParentTransform.DOScale(Vector3.one, 0.7f);
        }

        public void DeActivateShopMenuCanvas(Action action = null)
        {
            shopCanvasClass.shopCanvasGroup.interactable = false;
            shopCanvasClass.shopCanvasGroup.blocksRaycasts = false;
            shopCanvasClass.shopCanvasButtonsParentTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                shopCanvasClass.shopCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
            });
        }

        private void InitShopMenuButtonsFunc()
        {
            shopCanvasClass.ConfirmButton.onClick.AddListener(() => { ShopConfimButtonFunc(); });
        }

        private void ShopConfimButtonFunc()
        {
            DeActivateShopMenuCanvas(() => { ActivateStartMenuCanvas(); });
        }
        #endregion

        #region Common Functions

        private void Home()
        {
            ActivateStartMenuCanvas();
        }

        private void RateUs()
        {
            Application.OpenURL("https://www.google.com");
        }

        #endregion

    }
}