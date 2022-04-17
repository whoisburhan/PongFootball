using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeadSort {
    public class Draggable : MonoBehaviour {

        public float dragMultiplayer;
        public float movementSmoothing;
        public float maxMovementPerFrame = 30;
        public event System.Action onDragEnd;

        Vector3 touchStartPos;

        private void Update () {
            if (Input.GetMouseButtonDown (0)) {
                touchStartPos = Input.mousePosition;
               // MyGameManager.Instance.SuckerActivated = true;
            }
            if (Input.GetMouseButton (0)) {
                Vector3 diff = Input.mousePosition - touchStartPos;
                diff.x = Mathf.Clamp (diff.x, -maxMovementPerFrame, maxMovementPerFrame);
                diff.y = Mathf.Clamp (diff.y, -maxMovementPerFrame, maxMovementPerFrame);
                if (diff != Vector3.zero) {
                    Vector3 newPos = new Vector3 ((transform.position.x + diff.x) * dragMultiplayer, transform.position.y,
                        (transform.position.z + diff.y * dragMultiplayer));
                    touchStartPos = Input.mousePosition;
                    transform.position = Vector3.Lerp (transform.position, newPos, Time.deltaTime * movementSmoothing);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Clamp (pos.x, -4f, 4f);
                    pos.z = Mathf.Clamp (pos.z, -6.5f, 5.5f);
                    transform.position = pos;
                }
            }
        }

        private void OnMouseUp () {
           // OnDragEnd ();
          //  MyGameManager.Instance.SuckerActivated = false;
        }

        private void OnDragEnd () {
            if (onDragEnd != null)
                onDragEnd ();
        }

    }
}