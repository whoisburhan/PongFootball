using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [CreateAssetMenu(fileName = "Ball Animation List", menuName = "Ball Animation List")]
    public class BallAnimations : ScriptableObject
    {
        public List<string> AnimationList;
    }
}