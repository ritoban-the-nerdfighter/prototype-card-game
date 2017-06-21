using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;
using Assets.Scripts.InfoClasses;


namespace Assets.Scripts.GraphicsMonobehaviours
{
    class CardHolder_Hand : CardHolder
    {
        public static readonly float CARD_HIGHLIGHT_SCALE_INCREASE = 3.5f;


        public Text NameText;
        public SpriteRenderer CardPortrait;
        public Text ManaCostText;
        public Text CardText;
        public Text CardTypeText;

        public Transform CardCollider;


        // TODO: What about cards that change the mana cost?
        public int ManaCost
        {
            get
            {
                return manaCost;
            }
            set
            {
                manaCost = value;
                ManaCostText.text = manaCost.ToString();
            }
        }

        private int manaCost;

  

        protected new void Start()
        {
            ManaCost = Card.ManaCost;
            // Create the card based on the CardData
            // QUESTION: Should Start be called when the card exits the deck, or at the beginning of the game?
            NameText.text = Card.CardData.Name;
                CardPortrait.sprite = Card.CardData.CardPortrait;
            CardText.text = Card.CardData.CardText;
            CardTypeText.text = Card.CardData.CardType.ToString();

            Card.OnCardPlayed += OnCardPlayed;
            base.Init();
        }

        private void OnCardPlayed(Card c)
        {
            gameObject.SetSortingLayerRecursively("CardsOnBoard");
        }

        Vector3 previousScale = Vector3.one;

        public void HighlightCard()
        {
            previousScale = gameObject.transform.localScale;
            gameObject.transform.localScale = previousScale * CARD_HIGHLIGHT_SCALE_INCREASE;
            //Debug.Log("HighlightCard");
            CardCollider.localScale = CardCollider.transform.localScale / CARD_HIGHLIGHT_SCALE_INCREASE;
            // FIXME: Hard Coding
            gameObject.SetSortingLayerRecursively("HighlightedCard");
        }

        public void UnhighlightCard()
        {
            this.transform.localScale = previousScale;
            //Debug.Log("UnhighlightCard");
            CardCollider.localScale = CardCollider.transform.localScale * CARD_HIGHLIGHT_SCALE_INCREASE;
            gameObject.SetSortingLayerRecursively("CardsInHand");
        }

        public bool CardSelected { get; protected set; }


        private Vector3 select_MouseOffset = Vector3.zero;



        public void SelectCard()
        {
            // HACK: We need to unhighlight the card before we select it!
            UnhighlightCard();
            gameObject.SetSortingLayerRecursively("SelectedCard");
            CardSelected = true;
            select_MouseOffset = this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.SetLayerRecursively(9);
            this.transform.localScale = previousScale;
            previousScale = Vector3.one;
        }

        public void UnSelectCard()
        {
            gameObject.SetSortingLayerRecursively("CardsInHand");
            CardSelected = false;
            select_MouseOffset = Vector3.zero;
            gameObject.SetLayerRecursively(8);
        }

        private void LateUpdate()
        {
            if (CardSelected)
            {
                this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + select_MouseOffset;
            }
        }
    }
}