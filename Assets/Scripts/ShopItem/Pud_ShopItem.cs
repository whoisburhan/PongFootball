using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class Pud_ShopItem : ShopItem
    {
        [SerializeField] private Animator animator;
        

        public override void Start()
        {
            base.Start();
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
            GameManager.Instance.UpdatePlayerPud(itemIndex);
            Shop.Instance.CurrentySelectedShopItemPud.UpdateShopItemState(ShopItemState.EQUIPE);
            UpdateShopItemState(ShopItemState.EQUIPED);
            Shop.Instance.CurrentySelectedShopItemPud = this;

            GameData.Instance.State.Puds[GameData.Instance.CurrentlySelectedPudIndex] = 1;
            GameData.Instance.CurrentlySelectedPudIndex = itemIndex;
            GameData.Instance.State.Puds[GameData.Instance.CurrentlySelectedPudIndex] = 2;
            GameData.Instance.Save();
        }
    }
}