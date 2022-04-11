using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class TextGoalScript : MonoBehaviour
    {
        public void StartRound()
        {
            GameManager.Instance.StartRound();
        }
    }
}