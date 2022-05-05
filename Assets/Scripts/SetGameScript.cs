using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class SetGameScript : MonoBehaviour
    {
        /// Note: Animation Transition is controlling by UI_Manager
        [SerializeField] private List<Button> SetPointButton;

        [SerializeField] private List<Button> SetDifficultyButton;

        [SerializeField] private Button continueButton;

        [Header("Colors")]
        [SerializeField] private Color selectedButtonColor;
        [SerializeField] private Color normalButtonColor;

        private void Start()
        {
            SetDeafultButtonsColors();
            InitButtons();
        }

        private void InitButtons()
        {
            SetPointButton[0].onClick.AddListener(() => { SetGamePoint(1); });
            SetPointButton[1].onClick.AddListener(() => { SetGamePoint(2); });
            SetPointButton[2].onClick.AddListener(() => { SetGamePoint(3); });
            SetPointButton[3].onClick.AddListener(() => { SetGamePoint(4); });
            SetPointButton[4].onClick.AddListener(() => { SetGamePoint(5); });
            SetPointButton[5].onClick.AddListener(() => { SetGamePoint(6); });
            SetPointButton[6].onClick.AddListener(() => { SetGamePoint(7); });
            SetPointButton[7].onClick.AddListener(() => { SetGamePoint(8); });
            SetPointButton[8].onClick.AddListener(() => { SetGamePoint(9); });

            SetDifficultyButton[0].onClick.AddListener(() => { SetGameDifficulty(0);});
            SetDifficultyButton[1].onClick.AddListener(() => { SetGameDifficulty(1);});
            SetDifficultyButton[2].onClick.AddListener(() => { SetGameDifficulty(2);});
        }
        private void SetDeafultButtonsColors()
        {
            SetPointButton[GameManager.Instance.TargetGoal - 1].GetComponent<Image>().color = selectedButtonColor;

            SetDifficultyButton[(int)GameManager.Instance.GameDifficultyLevel].GetComponent<Image>().color = selectedButtonColor;
        }

        private void SetGamePoint(int point)
        {
            if (point != GameManager.Instance.TargetGoal)
            {
                Debug.Log("P:" + GameManager.Instance.TargetGoal);
                SetPointButton[GameManager.Instance.TargetGoal - 1].GetComponent<Image>().color = normalButtonColor;
                GameManager.Instance.TargetGoal = point;
                Debug.Log("P:" + GameManager.Instance.TargetGoal);
                SetPointButton[GameManager.Instance.TargetGoal - 1].GetComponent<Image>().color = selectedButtonColor;
            }
        }

        private void SetGameDifficulty(int difficultyLevel)
        {
            if (difficultyLevel != (int)GameManager.Instance.GameDifficultyLevel)
            {
                SetDifficultyButton[(int)GameManager.Instance.GameDifficultyLevel].GetComponent<Image>().color = normalButtonColor;
                GameManager.Instance.GameDifficultyLevel = (DifficultyLevel)difficultyLevel;
                SetDifficultyButton[(int)GameManager.Instance.GameDifficultyLevel].GetComponent<Image>().color = selectedButtonColor;
            }
        }
    }
}