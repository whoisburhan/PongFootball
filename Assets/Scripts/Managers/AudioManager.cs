using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource backgroundMusicSource;

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

          //  audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioName audioName)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(sounds[(int)audioName]);
        }

        public void Play(int index)
        {
            audioSource.PlayOneShot(sounds[index]);
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public void MuteUnmute(bool isMute)
        {
            audioSource.mute = isMute;
            backgroundMusicSource.mute = isMute;
        }


    }

    public enum AudioName
    {
        FREE_KICK = 0, GOAL_SOUND, COIN_SOUND, BUTTON_CLICK, WARNING, BALL_HIT,
        WIN_SOUND = 10, LOSE_SOUND = 11
    }
}