using UnityEngine;
using System.Collections;

namespace GS.PongFootball
{
    public class CameraShake : MonoBehaviour
    {

        public static CameraShake Instance {get;private set;}
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        public Transform camTransform;

        // How long the object should shake for.
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        Vector3 originalPos;

        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }

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

        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }

        public void Shake(float duration = 0.5f)
        {
            shakeDuration = duration;
        }
        void Update()
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
            }
        }
    }
}