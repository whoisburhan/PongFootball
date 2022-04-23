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
        Null = 0, StartMenu, PauseMenu, OptionsMenu, ResultMenu, QuitMenu, RestartConfimationMenu, ShopMenu
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
        public Button rateButton, optionsButton, shareButton, moreGamesButton;


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
    }

    [Serializable]
    class ShopCanvasClass
    {
        public CanvasGroup shopCanvasGroup;

        public Transform shopCanvasButtonsParentTransform;

        public Button ConfirmButton;
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private UI_State currentUIState, previousUIState;

        [SerializeField] private StartMenuCanvasClass startMenuCanvasClass;
        [SerializeField] private PauseMenuCanvasClass pauseMenuCanvasClass;
        [SerializeField] private OptionsMenuCanvasClass optionsMenuCanvasClass;
        [SerializeField] private RestartConfirmationCanvasClass restartConfirmationCanvasClass;
        [SerializeField] private QuitConfirmationCanvasClass quitConfirmationCanvasClass;
        [SerializeField] private ResultCanvasClass resultCanvasClass;

        [SerializeField] private ShopCanvasClass shopCanvasClass;

        [Header("Buttons")]
        [SerializeField] private Button pasueButton;

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

            ActivateStartMenuCanvas();
            DeActivatePauseButtonUI();

            //Init All the button function
            InItStartMenuButtonsFunc();
            InitPauseMenuButtonsFunc();
            InitResultMenuButtonsFunc();
            InitOptionsMenuButtonsFunc();
            InitQuitMenuButtonsFunc();
            InitRestartConfimationMenuButtonsFunc();
            InitShopMenuButtonsFunc();
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

            startMenuCanvasClass.shareButton.onClick.AddListener(() => { ShareButtonFunc(); });

            startMenuCanvasClass.moreGamesButton.onClick.AddListener(() => { ShopButtonFunc(); });
        }

        private void PlayButtonFunc()
        {
            DeActivateStartMenuCanvas();
            GameManager.Instance.StartCountDown();
            ActivatePauseButtonUI();
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
            DeActivateStartMenuCanvas(()=>
            {
                ActivateShopMenuCanvas();
            });
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

        public void ActivateResultMenuCanvas(bool isWon)
        {
            tempWinStatus = isWon;
            GameManager.Instance.IsPlay = false;
            DeActivatePauseButtonUI();
            resultCanvasClass.matchResultImg.sprite = isWon ? resultCanvasClass.matchWinSprite : resultCanvasClass.matchLoseSprite;

            resultCanvasClass.resultCanvasGroup.alpha = 1f;
            resultCanvasClass.resultCanvasGroup.interactable = true;
            resultCanvasClass.resultCanvasGroup.blocksRaycasts = true;
            resultCanvasClass.resultCanvasButtonsParentTransform.transform.DOScale(Vector3.one, 0.7f);

            SetUIState(UI_State.ResultMenu);
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
            });
            resultCanvasClass.restartButton.onClick.AddListener(() => { ResultMenuRestartButtonFunc(); });
            resultCanvasClass.optionsButton.onClick.AddListener(() => { ResultMenuOptionsButtonFunc(); });
            resultCanvasClass.rateButton.onClick.AddListener(() => { RateUs(); });
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

        public void DeActivateOptionsMenuCanvas(Action action = null, Action<bool> actionWithBool = null)
        {
            optionsMenuCanvasClass.optionsMenuCanvasGroup.interactable = false;
            optionsMenuCanvasClass.optionsMenuCanvasGroup.blocksRaycasts = false;
            optionsMenuCanvasClass.optionsButtonsParentTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                optionsMenuCanvasClass.optionsMenuCanvasGroup.alpha = 0;
                if (action != null) action?.Invoke();
                if (actionWithBool != null) actionWithBool?.Invoke(tempWinStatus);
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
            Action<bool> tempActionWithBool = null;
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