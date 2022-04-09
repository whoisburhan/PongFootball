using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GroundChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;

        [Header("Field Sprites")]
        [SerializeField] private List<Sprite> fields;

        private void Awake()
        {
            if(sr == null)  sr = GetComponent<SpriteRenderer>();
        }
        public void SetGround(int fieldIndex = 0)
        {
            if (fields.Count > 0)
            {
                if (fieldIndex >= 0 && fieldIndex < fields.Count)
                {
                    sr.sprite = fields[fieldIndex];
                }
                else
                {
                    Debug.LogError("Index Out of bounds");
                }
            }
            else
            {
                Debug.LogError("Field is empty");
            }
        }

        public void SetGroundRandomly()
        {
            if (fields.Count > 0)
            {
                sr.sprite = fields[UnityEngine.Random.Range(0,fields.Count)];
            }
        }
    }
}