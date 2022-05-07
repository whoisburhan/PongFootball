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

        [SerializeField] public PaddleType paddleType;
        [SerializeField] protected PaddleSide paddleSide;

        private Vector2 fingerUpPosition, fingerDownPosition, fingerUpPositionLeft,
        fingerDownPositionLeft, fingerUpPositionRight, fingerDownPositionRight;
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
                        if (GameManager.Instance.PlayMode != GamePlayMode.LOCAL_MULTIPLAYER)
                        {
                            if (touch.phase == TouchPhase.Began)
                            {
                                fingerUpPosition = touch.position;
                                fingerDownPosition = touch.position;
                            }


                            fingerUpPosition = fingerDownPosition;
                            fingerDownPosition = touch.position;

                            direction = fingerDownPosition.y - fingerUpPosition.y == 0 ? Vector2.zero : fingerDownPosition.y - fingerUpPosition.y > 0 ? Vector2.up : Vector2.down;

                        }

                        else
                        {
                            var _temp = Camera.main.ScreenToWorldPoint(touch.position);
                            if (paddleSide == PaddleSide.LEFT && _temp.x < 0)
                            {
                                if (touch.phase == TouchPhase.Began)
                                {
                                    fingerUpPositionLeft = touch.position;
                                    fingerDownPositionLeft = touch.position;
                                }


                                fingerUpPositionLeft = fingerDownPositionLeft;
                                fingerDownPositionLeft = touch.position;

                                direction = fingerDownPositionLeft.y - fingerUpPositionLeft.y == 0 ? Vector2.zero : fingerDownPositionLeft.y - fingerUpPositionLeft.y > 0 ? Vector2.up : Vector2.down;
                            }
                            else if (paddleSide == PaddleSide.RIGHT && _temp.x > 0)
                            {
                                if (touch.phase == TouchPhase.Began)
                                {
                                    fingerUpPositionRight = touch.position;
                                    fingerDownPositionRight = touch.position;
                                }


                                fingerUpPositionRight = fingerDownPositionRight;
                                fingerDownPositionRight = touch.position;

                                direction = fingerDownPositionRight.y - fingerUpPositionRight.y == 0 ? Vector2.zero : fingerDownPositionRight.y - fingerUpPositionRight.y > 0 ? Vector2.up : Vector2.down;
                            }
                        }
                    }
                }
            }
        }

        private void KeyBoardInput()
        {
            if (GameManager.Instance.PlayMode != GamePlayMode.LOCAL_MULTIPLAYER)
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
            else
            {
                if (paddleSide == PaddleSide.LEFT)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        direction = Vector2.up;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        direction = Vector2.down;
                    }
                    else
                    {
                        direction = Vector2.zero;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        direction = Vector2.up;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        direction = Vector2.down;
                    }
                    else
                    {
                        direction = Vector2.zero;
                    }
                }
            }
        }

        #endregion

        #region AI Paddle

        private void AIPaddle()
        {
            if ( ((GameManager.Instance.SideView == PudSideView.Right) && (paddleSide == PaddleSide.LEFT && ball.velocity.x < 0f) || (paddleSide == PaddleSide.RIGHT && ball.velocity.x > 0f))
             || ((GameManager.Instance.SideView == PudSideView.Left) && (paddleSide == PaddleSide.LEFT && ball.velocity.x > 0f) || (paddleSide == PaddleSide.RIGHT && ball.velocity.x < 0f)))
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