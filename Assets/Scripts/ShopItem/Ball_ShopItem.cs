using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.PongFootball
{
    public class Ball_ShopItem : ShopItem
    {
        [SerializeField] private Animator animator;
        [SerializeField] private int ballIndex;
        
        [Space]
        [SerializeField] private BallAnimations ballImgAnimations;

        private void OnEnable()
        {
            animator.Play(ballImgAnimations.AnimationList[itemIndex]);
        }

        public override void SetItemIndex(int _index)
        {
            base.SetItemIndex(_index);
            animator.Play(ballImgAnimations.AnimationList[itemIndex]);
        }

        public int GetItemIndex()
        {
            return itemIndex;
        }


        protected override void EquipedItem()
        {
            base.EquipedItem();


        }

        protected override void EquipeItem(bool buyAndEquipe = false)
        {
            base.EquipeItem();
            ActivateItem();
        }
        
        public override void ActivateItem()
        {
            Shop.Instance.CurrentlySelectedShopItemBall.UpdateShopItemState(ShopItemState.EQUIPE);           
            UpdateShopItemState(ShopItemState.EQUIPED);
            Shop.Instance.CurrentlySelectedShopItemBall = this;
            GameManager.Instance.ball.SetBallAnimation(itemIndex);

            GameData.Instance.State.Balls[GameData.Instance.CurrentlySelectedBallIndex] = 1;
            GameData.Instance.CurrentlySelectedBallIndex = itemIndex;
            GameData.Instance.State.Balls[GameData.Instance.CurrentlySelectedBallIndex] = 2;
            GameData.Instance.Save();
        }
    }
}