using UnityEngine;

namespace GS.PongFootball
{
    public class PlayerPaddle : Paddle
    {
        public Vector2 direction { get; private set; }

        private Vector2 fingerUpPosition, fingerDownPosition;
        private float minimumDistanceForSwipe = 1f;

        private void Update()
        {
            if (GameManager.Instance.IsPlay)
            {
                TouchInput();
                KeyBoardInput();
                
            }
        }

        private void FixedUpdate()
        {
            if (direction.sqrMagnitude != 0)
            {
                rigidbody.AddForce(direction * speed);
            }
        }

        #region  Touch Input
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

                        if (touch.phase == TouchPhase.Ended)
                        {
                            fingerDownPosition = touch.position;
                            DetectSwipe();
                        }
                    }
                }
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                direction = fingerDownPosition.y - fingerUpPosition.y == 0 ? Vector2.zero : fingerDownPosition.y - fingerUpPosition.y > 0 ? Vector2.up : Vector2.down;
                fingerUpPosition = Vector2.zero;
                fingerDownPosition = Vector2.zero;
            }
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > minimumDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        }

        #endregion        

        #region KeyBoard Input

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

    }
}