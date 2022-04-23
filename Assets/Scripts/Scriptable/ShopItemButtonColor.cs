using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    [CreateAssetMenu(fileName = "ShopItemButtonColor", menuName = "ShopItemButtonColor")]
    public class ShopItemButtonColor : ScriptableObject
    {
        public Color BuyColor;
        public Color EquipColor;
        public Color EquipedColor;
    }
}