using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Canvas Group")]
        [SerializeField] private CanvasGroup startMenuCanvasGroup;
        [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
        [SerializeField] private CanvasGroup optionsMenuCanvasGroup;
        [SerializeField] private CanvasGroup restartConfirmationCanvasGroup;
        [SerializeField] private CanvasGroup quitConfirmationCanvasGroup;
        [SerializeField] private CanvasGroup resultCanvasGroup;


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
    }
}