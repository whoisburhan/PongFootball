using UnityEngine;
using UnityEngine.Events;

namespace GS.PongFootball
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScoringZone : MonoBehaviour
    {
        public UnityEvent scoreTrigger;

        [SerializeField] private GameObject goalTextObjecct;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();

            if (ball != null)
            {
                scoreTrigger.Invoke();

                goalTextObjecct.SetActive(false);
                goalTextObjecct.SetActive(true);
                AudioManager.Instance.Play(AudioName.GOAL_SOUND);
                CameraShake.Instance.Shake(0.7f);
                ball.GetComponent<TrailRenderer>().enabled = false;
                ball.GetComponent<TrailRenderer>().Clear();
                ball.gameObject.SetActive(false);

                #if UNITY_ANDROID && !UNITY_EDITOR
                Vibrator.Vibrate(700);
                #endif

            }
        }

    }
}