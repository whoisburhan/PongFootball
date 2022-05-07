using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class CoinSystem : MonoBehaviour
    {
        public static CoinSystem Instance { get; private set; }

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

        public int GetCoin()
        {
            return GameData.Instance.State.TotalCoin;
        }

        public void AddCoin(int _coinAmount)
        {
            GameData.Instance.State.TotalCoin += _coinAmount;
            GameData.Instance.Save();
            UIManager.Instance.UpdateCoinInUI();
        }

        public void DeductCoin(int _coinAmount)
        {
            GameData.Instance.State.TotalCoin -= _coinAmount;

            if (GameData.Instance.State.TotalCoin < 0)
            {
                GameData.Instance.State.TotalCoin = 0;
            }

            GameData.Instance.Save();
            UIManager.Instance.UpdateCoinInUI();
        }

    }
}
