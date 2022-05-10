using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class Field_ShopItem : ShopItem
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GroundObject groundObject;

        [Header("Field Decoration")]
        [SerializeField] private Image groundImg;
        [SerializeField] private Image sandLayerImg;
        [SerializeField] private Image flowerLayerImg;
        [SerializeField] private Image fieldStyleImg;

        public override void Start()
        {
            base.Start();
            InItFieldGraphics();
        }

        private void InItFieldGraphics()
        {
            groundImg.color = groundObject.GroundColor;
            sandLayerImg.color = groundObject.SandColor;
            fieldStyleImg.color = groundObject.FieldPatternColor;

            fieldStyleImg.sprite = GroundChanger.Instance.FieldPatterns[(int)groundObject.m_FieldPattern];

            price = groundObject.Price;
            priceText.text = price.ToString();
        }


        protected override void EquipedItem()
        {
            base.EquipedItem();


        }

        protected override void EquipeItem()
        {
            base.EquipeItem();
            ActivateItem();
        }

        public override void ActivateItem()
        {
            GroundChanger.Instance.SetField(groundObject.m_FieldPattern, groundObject.GroundColor, groundObject.SandColor,groundObject.FieldPatternColor);
            Shop.Instance.CurrentySelectedShopItemField.UpdateShopItemState(ShopItemState.EQUIPE);
            UpdateShopItemState(ShopItemState.EQUIPED);
            Shop.Instance.CurrentySelectedShopItemField = this;

            GameData.Instance.State.Grounds[GameData.Instance.CurrentlySelectedFieldIndex] = 1;
            GameData.Instance.CurrentlySelectedFieldIndex = itemIndex;
            GameData.Instance.State.Grounds[GameData.Instance.CurrentlySelectedFieldIndex] = 2;
            GameData.Instance.Save();
        }
    }
}