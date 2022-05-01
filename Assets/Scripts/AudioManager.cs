using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource audioSource;

        [SerializeField] private List<AudioClip> sounds;

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

            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioName audioName)
        {
            audioSource.PlayOneShot(sounds[(int)audioName]);
        }

        public void MuteUnmute(bool isMute)
        {
            audioSource.mute = isMute;
        }


    }

    public enum AudioName
    {
        FREE_KICK = 0, GOAL_SOUND, BALL_HIT, COIN_SOUND
    }
}