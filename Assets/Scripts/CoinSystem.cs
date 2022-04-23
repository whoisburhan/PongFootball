using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class CoinSystem : MonoBehaviour
    {
        public static CoinSystem Instance { get; private set; }

        private int coinAmount = 5000;

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
            return coinAmount;
        }

        public void AddCoin(int _coinAmount)
        {
            coinAmount += _coinAmount;
        }

        public void DeductCoin(int _coinAmount)
        {
            coinAmount -= _coinAmount;

            if (coinAmount < 0)
            {
                coinAmount = 0;
            }
        }

    }
}
