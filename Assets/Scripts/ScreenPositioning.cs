using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class ScreenPositioning : MonoBehaviour
    {
        [SerializeField] Transform leftSidePaddle;
        [SerializeField] Transform repositionedLeftSidePaddleOffset;
        [SerializeField] Transform rightSidePaddle;
        [SerializeField] Transform repositionedRightSidePaddleOffset;
        [SerializeField] Transform leftSideStar;
        [SerializeField] Transform repositionLeftSideStaroffset;
        [SerializeField] Transform rightSideStar;
        [SerializeField] Transform repositionRightSideStaroffset;
        private void Start()
        {
            if((float)Screen.height/ (float)Screen.width >= 2f || (float)Screen.width / (float)Screen.height >= 2f)
            {
                Debug.Log("OKOK");
                Reposition();
            }
        }

        private void Reposition()
        {
            leftSidePaddle.position = repositionedLeftSidePaddleOffset.position;
            rightSidePaddle.position = repositionedRightSidePaddleOffset.position;
            leftSideStar.position = repositionLeftSideStaroffset.position;
            rightSideStar.position = repositionRightSideStaroffset.position;
        }
    }
}