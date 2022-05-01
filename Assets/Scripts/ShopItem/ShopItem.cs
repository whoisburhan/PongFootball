using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public enum ShopItemState
    {
        BUY, EQUIPE, EQUIPED
    }
    public class ShopItem : MonoBehaviour
    {
        private ShopItemState shopItemState;

        [SerializeField] protected int itemIndex;
        [SerializeField] ShopItemButtonColor shopItemButtonColor;
        [SerializeField] protected GameObject priceObj;

        [SerializeField] public int price;
        [SerializeField] public Text priceText;
        [SerializeField] protected Button shopItemButton;
        [SerializeField] protected Text shopItemButtonText;
        [SerializeField] public Image shopItemImg;

        public virtual void Start()
        {
            shopItemButton.onClick.AddListener(() =>
            {
                ShopItemButtonFunc();
            });

        }

        public virtual void SetItemIndex(int _index)
        {
            itemIndex = _index;
        }
        private void ShopItemButtonFunc()
        {
            switch (shopItemState)
            {
                case ShopItemState.BUY:
                    BuyItem();
                    break;
                case ShopItemState.EQUIPE:
                    EquipeItem();
                    break;
                case ShopItemState.EQUIPED:
                    EquipedItem();
                    break;
            }
        }

        public virtual void UpdateShopItemState(ShopItemState _shopItemState)
        {
            shopItemState = _shopItemState;

            switch (shopItemState)
            {
                case ShopItemState.BUY:
                    priceObj.SetActive(true);
                    shopItemButton.GetComponent<Image>().color = shopItemButtonColor.BuyColor;
                    shopItemButtonText.text = "BUY";
                    break;
                case ShopItemState.EQUIPE:
                    priceObj.SetActive(false);
                    shopItemButton.GetComponent<Image>().color = shopItemButtonColor.EquipColor;
                    shopItemButtonText.text = "EQUIP";
                    break;
                case ShopItemState.EQUIPED:
                    priceObj.SetActive(false);
                    shopItemButton.GetComponent<Image>().color = shopItemButtonColor.EquipedColor;
                    shopItemButtonText.text = "EQUIPED";
                    break;
            }
        }

        protected virtual void BuyItem()
        {
            if(CoinSystem.Instance.GetCoin()>= price)
            {
                // After Buying
                CoinSystem.Instance.DeductCoin(price);
                //

                EquipeItem();
               // UpdateShopItemState(ShopItemState.EQUIPE);
            }
            else
            {
                Debug.Log("FOINNI");
            }

            Debug.Log("BUY ITEM Function Executed");
        }

        protected virtual void EquipeItem() { Debug.Log("EQUIP ITEM Function Execute"); }

        protected virtual void EquipedItem() { Debug.Log("EQUIPED ITEM Function Execute"); }

        public virtual void ActivateItem()
        {

        }

    }
}