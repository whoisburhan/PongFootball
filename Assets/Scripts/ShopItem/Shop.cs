using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance { get; private set; }

        [Header("Ball Shop")]
        [SerializeField] private GameObject ballShopPanel;
        [SerializeField] private Button selectBallShopButton;
        [SerializeField] private List<Ball_ShopItem> shopItemsBalls;
        [HideInInspector] public Ball_ShopItem CurrentlySelectedShopItemBall;

        [Header("Field Shop")]
        [SerializeField] private GameObject fieldShopPanel;
        [SerializeField] private Button selectFieldShopButton;
        [SerializeField] private List<Field_ShopItem> shopItemsFields;
        [HideInInspector] public Field_ShopItem CurrentySelectedShopItemField;

        [Header("Pud Shop")]
        [SerializeField] private GameObject pudShopPanel;
        [SerializeField] private Button selectPudShopButton;

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

            CurrentlySelectedShopItemBall = shopItemsBalls[0];
            CurrentySelectedShopItemField = shopItemsFields[0];
        }
        private void Start()
        {
            Reset();
            ShopItemsInit();
            selectBallShopButton.onClick.AddListener(() => { ExecuteButtonFunc(0); });
            selectFieldShopButton.onClick.AddListener(() => { ExecuteButtonFunc(1); });
            selectPudShopButton.onClick.AddListener(() => { ExecuteButtonFunc(2); });

            UpdateDataOnLoad();
        }

        private void ExecuteButtonFunc(int buttonInt)
        {
            switch (buttonInt)
            {
                case 0:
                    selectBallShopButton.interactable = false;
                    selectFieldShopButton.interactable = true;
                    selectPudShopButton.interactable = true;

                    ballShopPanel.SetActive(true);
                    fieldShopPanel.SetActive(false);
                    pudShopPanel.SetActive(false);

                    ballShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
                    break;
                case 1:
                    selectBallShopButton.interactable = true;
                    selectFieldShopButton.interactable = false;
                    selectPudShopButton.interactable = true;

                    ballShopPanel.SetActive(false);
                    fieldShopPanel.SetActive(true);
                    pudShopPanel.SetActive(false);

                    fieldShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
                    break;
                case 2:
                    selectBallShopButton.interactable = true;
                    selectFieldShopButton.interactable = true;
                    selectPudShopButton.interactable = false;

                    ballShopPanel.SetActive(false);
                    fieldShopPanel.SetActive(false);
                    pudShopPanel.SetActive(true);

                    pudShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
                    break;
            }
        }

        public void ShopItemsInit()
        {
            for (int i = 0; i < shopItemsBalls.Count; i++)
            {
                shopItemsBalls[i].SetItemIndex(i);
            }

            for (int i = 0; i < shopItemsFields.Count; i++)
            {
                shopItemsFields[i].SetItemIndex(i);
            }
        }
        public void Reset()
        {
            ballShopPanel.SetActive(true);
            fieldShopPanel.SetActive(false);
            pudShopPanel.SetActive(false);

            selectBallShopButton.interactable = false;
            selectPudShopButton.interactable = true;
            selectFieldShopButton.interactable = true;

            ballShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
            fieldShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
            pudShopPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        }

        private void UpdateDataOnLoad()
        {
            UpdateBallDataOnLoadExecute(GameData.Instance.State.Balls);
            UpdateFieldDataOnLoadExecute(GameData.Instance.State.Grounds);
        }

        private void UpdateBallDataOnLoadExecute(int[] _data)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                switch (_data[i])
                {
                    case 0:
                        shopItemsBalls[i].UpdateShopItemState(ShopItemState.BUY);
                        break;
                    case 1:
                        shopItemsBalls[i].UpdateShopItemState(ShopItemState.EQUIPE);
                        break;
                    case 2:
                        shopItemsBalls[i].UpdateShopItemState(ShopItemState.EQUIPED);
                        shopItemsBalls[i].ActivateItem();
                        break;
                }
            }
        }

        private void UpdateFieldDataOnLoadExecute(int[] _data)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                switch (_data[i])
                {
                    case 0:
                        shopItemsFields[i].UpdateShopItemState(ShopItemState.BUY);
                        break;
                    case 1:
                        shopItemsFields[i].UpdateShopItemState(ShopItemState.EQUIPE);
                        break;
                    case 2:
                        shopItemsFields[i].UpdateShopItemState(ShopItemState.EQUIPED);
                        shopItemsFields[i].ActivateItem();
                        break;
                }
            }
        }
    }
}