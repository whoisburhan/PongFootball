using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class GameDistributionAds : MonoBehaviour
    {
        public static GameDistributionAds Instance {get;private set;}

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            GameDistribution.OnPauseGame += OnPauseGame;
            GameDistribution.OnResumeGame += OnResumeGame;
            
        }

        private void OnDisable()
        {
            GameDistribution.OnPauseGame -= OnPauseGame;
            GameDistribution.OnResumeGame -= OnResumeGame;
        }

        public void OnPauseGame()
        {
            Time.timeScale = 0f;
        }

        public void OnResumeGame()
        {
            Time.timeScale = 1f;
        }

        public void ShowAds()
        {
            GameDistribution.Instance.ShowAd();
        }

        public void ShowRewardedAd()
        {
            GameDistribution.Instance.ShowRewardedAd();
        }

        
    }
}