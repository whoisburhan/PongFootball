using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class WorldCupFixtureScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> worldCupGroupsPanel;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;

        int currentIndex;
        private void Start()
        {
            currentIndex = 0;

            nextButton.onClick.AddListener(() => { ButtonAction(true); });
            prevButton.onClick.AddListener(() => { ButtonAction(false); });
        }

        private void ButtonAction(bool isPositive)
        {
            worldCupGroupsPanel[currentIndex % worldCupGroupsPanel.Count].SetActive(false);
            currentIndex = isPositive ? currentIndex + 1 : currentIndex - 1;
            currentIndex = currentIndex < 0 ? worldCupGroupsPanel.Count : currentIndex;
            worldCupGroupsPanel[currentIndex % worldCupGroupsPanel.Count].SetActive(true);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.PlayButtonClickSound();
            }
        }
    }
}