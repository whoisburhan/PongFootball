using UnityEngine;

namespace GS.PongFootball
{
    public enum PaddleType
    {
        PLAYER, AI
    }

    public enum PaddleSide
    {
        LEFT, RIGHT
    }
    [RequireComponent(typeof(Rigidbody2D))]
    public class Paddle : MonoBehaviour
    {
        public float speed = 8f;
        public new Rigidbody2D rigidbody { get; private set; }

        public Rigidbody2D ball;

        [SerializeField] protected PaddleType paddleType;
        [SerializeField] protected PaddleSide paddleSide;

        private Vector2 fingerUpPosition, fingerDownPosition;
        private Vector2 direction;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (paddleType == PaddleType.PLAYER)
            {
                if (GameManager.Instance.IsPlay)
                {
                    TouchInput();
#if UNITY_EDITOR
                    KeyBoardInput();
#endif

                }
            }
        }

        private void FixedUpdate()
        {
            if (paddleType == PaddleType.PLAYER)
            {
                if (direction.sqrMagnitude != 0)
                {
                    rigidbody.AddForce(direction * speed);
                }
            }

            else if (paddleType == PaddleType.AI)
            {
                AIPaddle();
            }
        }

        #region Player Paddle

        private void TouchInput()
        {
            if (GameManager.Instance.IsPlay)
            {
                if (Input.touchCount > 0)
                {
                    Touch[] touches = Input.touches;
                    foreach (Touch touch in touches)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            fingerUpPosition = touch.position;
                            fingerDownPosition = touch.position;
                        }

                        // if (touch.phase == TouchPhase.Ended)
                        // {
                        //     fingerDownPosition = touch.position;
                        //     DetectSwipe();
                        // }

                        fingerUpPosition = fingerDownPosition;
                        fingerDownPosition = touch.position;

                        direction = fingerDownPosition.y - fingerUpPosition.y == 0 ? Vector2.zero : fingerDownPosition.y - fingerUpPosition.y > 0 ? Vector2.up : Vector2.down;

                    }
                }
            }
        }

        private void KeyBoardInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.zero;
            }
        }

        #endregion

        #region AI Paddle

        private void AIPaddle()
        {
            if ((paddleSide == PaddleSide.LEFT && ball.velocity.x < 0f) || (paddleSide == PaddleSide.RIGHT && ball.velocity.x > 0f))
            {
                // Move the paddle in the direction of the ball to track it
                if (ball.position.y > rigidbody.position.y)
                {
                    rigidbody.AddForce(Vector2.up * speed);
                }
                else if (ball.position.y < rigidbody.position.y)
                {
                    rigidbody.AddForce(Vector2.down * speed);
                }
            }
            else
            {
                // Move towards the center of the field and idle there until the
                // ball starts coming towards the paddle again
                if (rigidbody.position.y > 0f)
                {
                    rigidbody.AddForce(Vector2.down * speed);
                }
                else if (rigidbody.position.y < 0f)
                {
                    rigidbody.AddForce(Vector2.up * speed);
                }
            }
        }

        #endregion

        public void ResetPosition()
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.position = new Vector2(rigidbody.position.x, 0f);
        }

    }
}