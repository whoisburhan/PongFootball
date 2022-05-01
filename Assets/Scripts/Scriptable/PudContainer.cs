using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [System.Serializable]
    public class PudObject
    {
        public Sprite Pud;
        public string PudAnimation;
        public int Price;
    }

    [CreateAssetMenu(fileName = "Pud Container", menuName = "Pud Container")]
    public class PudContainer : ScriptableObject
    {
        public List<PudObject> container;
    }
}