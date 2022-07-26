using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class ScreenshotManager : MonoBehaviour
    {
        public void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot("SC_"+ DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") +".png");
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeScreenshot();
            }
#endif
        }
    }
}