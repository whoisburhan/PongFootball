using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public enum FieldPattern
    {
        Pattern_1 = 0, Pattern_2, Pattern_3
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class GroundChanger : MonoBehaviour
    {
        public static GroundChanger Instance {get;private set;}

        [SerializeField] private SpriteRenderer groundSr;
        [SerializeField] private SpriteRenderer sandLayerSr;
        [SerializeField] private SpriteRenderer fieldPattenSr;

        [Header("Field Pattern Sprites")]
        [SerializeField] public List<Sprite> FieldPatterns;

        [Header("Local Multiplayer Ground Objs")]
        [SerializeField] private GameObject purpleSideGround;
        [SerializeField] private GameObject blueSideGround;
        [SerializeField] private GroundObject localMultiplayerGroundObj;

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
        }

        public void SetField(FieldPattern _fieldPattern, Color groundColor, Color sandColor, Color fieldPatternColor)
        {
            fieldPattenSr.sprite = FieldPatterns[(int)_fieldPattern];
            groundSr.color = groundColor;
            sandLayerSr.color = sandColor;
            fieldPattenSr.color = fieldPatternColor;
            purpleSideGround.SetActive(false);
            blueSideGround.SetActive(false);
        }

        public void ActivateLocalMultiplayerField()
        {
            fieldPattenSr.sprite = FieldPatterns[(int)localMultiplayerGroundObj.m_FieldPattern];
            groundSr.color = localMultiplayerGroundObj.GroundColor;
            sandLayerSr.color = localMultiplayerGroundObj.SandColor;
            fieldPattenSr.color = localMultiplayerGroundObj.FieldPatternColor;
            purpleSideGround.SetActive(true);
            blueSideGround.SetActive(true);
        }
       
    }
}