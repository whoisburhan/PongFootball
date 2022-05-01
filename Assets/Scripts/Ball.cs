using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [RequireComponent(typeof(Rigidbody2D),typeof(SpriteRenderer),typeof(Animator))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] BallAnimations ballAnimations;
        public float speed = 200f;
        [SerializeField] private GameObject ballHitEffect;

        public new Rigidbody2D rigidbody { get; private set; }

        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            if(sr == null)  sr = GetComponent<SpriteRenderer>();
            if(animator == null) animator = GetComponent<Animator>();

            
        }

        private void OnEnable()
        {
            SetBallAnimation(GameData.Instance.CurrentlySelectedBallIndex);
        }
        private void Update()
        {
            if(rigidbody.velocity.x < 0 && !sr.flipX)
            {
                sr.flipX = true;
            }
            else if(rigidbody.velocity.x > 0 && sr.flipX)
            {
                sr.flipX = false;
            }
        }

        public void ResetPosition()
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.position = Vector2.zero;
        }

        public void AddStartingForce()
        {
            // Flip a coin to determine if the ball starts left or right
            float x = Random.value < 0.5f ? -1f : 1f;

            // Flip a coin to determine if the ball goes up or down. Set the range
            // between 0.5 -> 1.0 to ensure it does not move completely horizontal.
            float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f)
                                          : Random.Range(0.5f, 1f);

            Vector2 direction = new Vector2(x, y);
            rigidbody.AddForce(direction * speed);
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            AudioManager.Instance.Play(AudioName.BALL_HIT);

            var pos = col.collider.ClosestPoint(transform.position);
            ballHitEffect.SetActive(false);
            ballHitEffect.transform.position = pos;
            ballHitEffect.SetActive(true);

            if (col.gameObject.name == "LeftSidePlayer")
            {
                var _animator = col.gameObject.GetComponent<Animator>();
                // _animator.Play("LeftSidePlayerHit");
                _animator.Play(GameManager.Instance.CurrentlySelectedPudTwoHitAnimation);
            }
            if (col.gameObject.name == "RightSidePlayer")
            {
                var _animator = col.gameObject.GetComponent<Animator>();
                //_animator.Play("RightSidePlayerHit");
                _animator.StopPlayback();
                _animator.Play(GameManager.Instance.CurrentlySelectedPudOneHitAnimation);
                
            }
        }

        public void SetBallAnimation(int index)
        {
            animator.Play(ballAnimations.AnimationList[index]);
        }

    }
}