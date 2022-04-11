using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GS.PongFootball
{
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

        [Header("Buttons")]
        public Button homeButtons;
        public Button restartButton, optionsButton, continueButton;
    }

    [Serializable]
    class OptionsMenuCanvasClass
    {
        public CanvasGroup optionsMenuCanvasGroup;
        public Button confirmationButton;

        [Header("Common Sprites for Sound & Vibration Settings")]
        
        public Sprite OnButtonSprite;
        public Sprite OffButtonSprite;

        [Header("Sound Settings")]
        public Button soundButton;
        public Image SoundButtonImg;
        public Transform SoundButtonOffOfset;
        public Transform SoundButtonOnOffset;

        [Header("Vibration Settings")]
        public Button vibrationButton;
        public Image vibrationButtonImg;
        public Transform vibrationButtonOffOfset;
        public Transform vibrationButtonOnOffset;

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

        [Header("Buttons")]
        public Button yesButton;
        public Button noButton;
    }

    [Serializable]
    class QuitConfirmationCanvasClass
    {
        public CanvasGroup quitConfirmationCanvasGroup;

        [Header("Buttons")]
        public Button yesButton;
        public Button noButton;
    }

    [Serializable]
    class ResultCanvasClass
    {
        public CanvasGroup resultCanvasGroup;

        [Header("Match Result")]
        public Image matchResultImg;
        [Space]
        public Sprite matchWinSprite;
        public Sprite matchLoseSprite;

        [Header("Buttons")]
        public Button homeButton;
        public Button restartButton,optionsButton, rateButton;
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private StartMenuCanvasClass startMenuCanvasClass;
        [SerializeField] private PauseMenuCanvasClass pauseMenuCanvasClass;
        [SerializeField] private OptionsMenuCanvasClass optionsMenuCanvasClass;
        [SerializeField] private RestartConfirmationCanvasClass restartConfirmationCanvasClass;
        [SerializeField] private QuitConfirmationCanvasClass quitConfirmationCanvasClass;
        [SerializeField] private ResultCanvasClass resultCanvasClass;

        [Header("Buttons")]
        [SerializeField] private Button pasueButton;


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
            ActivateStartMenuCanvas();
            DeActivatePauseButtonUI();
        }

        public void ActivatePauseButtonUI()
        {
            pasueButton.gameObject.SetActive(true);
        }

        public void DeActivatePauseButtonUI()
        {
            pasueButton.gameObject.SetActive(false);
        }

        #region Canvas Animations

        public void ActivateStartMenuCanvas()
        {
            startMenuCanvasClass.startMenuCanvasGroup.alpha = 1;
            startMenuCanvasClass.startMenuCanvasGroup.interactable = true;
            startMenuCanvasClass.startMenuCanvasGroup.blocksRaycasts = true;
            startMenuCanvasClass.startMenuButtonsParentTransform.DOScale(Vector3.one, 0.5f);
            startMenuCanvasClass.titleImgTransform.DOMove(startMenuCanvasClass.titleImgEndPos.position, 0.5f);

        }

        public void DeActivateStartMenuCanvas()
        {
            startMenuCanvasClass.startMenuCanvasGroup.interactable = false;
            startMenuCanvasClass.startMenuCanvasGroup.blocksRaycasts = false;
            startMenuCanvasClass.startMenuButtonsParentTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { startMenuCanvasClass.startMenuCanvasGroup.alpha = 0; });
            startMenuCanvasClass.titleImgTransform.DOMove(startMenuCanvasClass.titleImgStartPos.position, 0.5f);
        }

        #endregion
    }
}