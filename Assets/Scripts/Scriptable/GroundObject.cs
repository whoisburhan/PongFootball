using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [CreateAssetMenu(fileName = "Ground Object - 0", menuName = "Grounds")]
    public class GroundObject : ScriptableObject
    {
        public string GroundName;
        public int Price;
        public FieldPattern m_FieldPattern;
        public Color GroundColor, SandColor, FieldPatternColor;
    }
}