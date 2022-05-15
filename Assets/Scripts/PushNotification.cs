using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GS.PongFootball
{
    public enum PushNotificationColor
    {
        YELLOW = 0 , RED , GREEN
    }
    public class PushNotification : MonoBehaviour
    {
        public static PushNotification Instance {get;private set;}

        [SerializeField] private Image notificationBar;
        [SerializeField] private Text notificationText;

        [SerializeField] private List<Color> notificationColors;
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
        
        public void ShowNotification(string notificationString)
        {
            notificationText.text = notificationString;
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(notificationBar.transform.DOScaleY(1f,0.25f));
            _sequence.AppendInterval(0.5f);
            _sequence.Append(notificationBar.transform.DOScaleY(0f,0.25f));
        }

        public void ShowNotification_Long(string notificationString)
        {
            notificationText.text = notificationString;
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(notificationBar.transform.DOScaleY(1f,0.25f));
        }

        public void HideNotification()
        {
            notificationBar.transform.DOScaleY(0f,0.25f);
        }

        public void SetNotificationColor(PushNotificationColor color)
        {
            notificationBar.color = notificationColors[(int)color];
        }
    }
}