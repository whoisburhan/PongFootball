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
        [Space]
        [SerializeField] protected Text shopItemButtonText_Buy;
        [SerializeField] protected Text shopItemButtonText_Equip;
        [SerializeField] protected Text shopItemButtonText_Equipped;
        [Space]
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
                    shopItemButtonText_Buy.gameObject.SetActive(true);
                    shopItemButtonText_Equip.gameObject.SetActive(false);
                    shopItemButtonText_Equipped.gameObject.SetActive(false);
                    break;
                case ShopItemState.EQUIPE:
                    priceObj.SetActive(false);
                    shopItemButton.GetComponent<Image>().color = shopItemButtonColor.EquipColor;
                    shopItemButtonText_Buy.gameObject.SetActive(false);
                    shopItemButtonText_Equip.gameObject.SetActive(true);
                    shopItemButtonText_Equipped.gameObject.SetActive(false);
                    break;
                case ShopItemState.EQUIPED:
                    priceObj.SetActive(false);
                    shopItemButton.GetComponent<Image>().color = shopItemButtonColor.EquipedColor;
                    shopItemButtonText_Buy.gameObject.SetActive(false);
                    shopItemButtonText_Equip.gameObject.SetActive(false);
                    shopItemButtonText_Equipped.gameObject.SetActive(true);
                    break;
            }
        }

        protected virtual void BuyItem()
        {
            if (CoinSystem.Instance.GetCoin() >= price)
            {
                // After Buying
                CoinSystem.Instance.DeductCoin(price);
                //
                PushNotification.Instance.SetNotificationColor(PushNotificationColor.GREEN);
                PushNotification.Instance.ShowNotification("Purchased & Used");
                EquipeItem(true);
                // UpdateShopItemState(ShopItemState.EQUIPE);
            }
            else
            {
                Debug.Log("FOINNI");
                PushNotification.Instance.SetNotificationColor(PushNotificationColor.RED);
                PushNotification.Instance.ShowNotification("Not Enough Coin..");
                AudioManager.Instance.Play(AudioName.WARNING);
            }

            Debug.Log("BUY ITEM Function Executed");
        }

        protected virtual void EquipeItem(bool buyAndEquipe = false)
        {
            Debug.Log(buyAndEquipe);
            /*EQUIP ITEM Function Execute */
            if (!buyAndEquipe)
            {
                PushNotification.Instance.SetNotificationColor(PushNotificationColor.YELLOW);
                PushNotification.Instance.ShowNotification("Used");
                AudioManager.Instance.Play(AudioName.BUTTON_CLICK);
            }
        }

        protected virtual void EquipedItem()
        {
            /*EQUIPED ITEM Function Execute */

        }

        public virtual void ActivateItem()
        {

        }

    }
}