using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class StartCountDownScript : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.NewGame();
        }
    }
}