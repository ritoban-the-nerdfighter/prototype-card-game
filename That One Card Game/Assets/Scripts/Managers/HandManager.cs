using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GraphicsMonobehaviours;
using Assets.Scripts.Util;
using Assets.Scripts.InfoClasses;

namespace Assets.Scripts.Managers
{
    public class HandManager : Singleton<HandManager>
    {
        public GameObject CardPrefab;
        public float CardWidth = 1;
        public float Padding = 0.1f;
        public Vector2 MaxDistanceFromBoardCenter;
        public Vector2 BoardCenter;

        // TODO: Add different axes (x, y) for positioning cards
        public LayerMask CardMask;

        public Hand PlayerHand { get; protected set; }

        private Card highlightedCard = null;

        private Card selectedCard = null;


        #region Unity Methods
        private void Awake()
        {
            PlayerHand = new Hand();
            PlayerHand.OnCardAdded += OnCardAdded;
        }



        private void Update()
        {
            CheckCardUnderMouse();
            HandleMouseClicks();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                new Vector3(
                    BoardCenter.x - MaxDistanceFromBoardCenter.x,
                    BoardCenter.y - MaxDistanceFromBoardCenter.y),
                new Vector3(
                    BoardCenter.x + MaxDistanceFromBoardCenter.x,
                    BoardCenter.y - MaxDistanceFromBoardCenter.y)
                    );
            Gizmos.DrawLine(
                new Vector3(
                    BoardCenter.x - MaxDistanceFromBoardCenter.x,
                    BoardCenter.y + MaxDistanceFromBoardCenter.y),
                new Vector3(
                    BoardCenter.x + MaxDistanceFromBoardCenter.x,
                    BoardCenter.y + MaxDistanceFromBoardCenter.y)
                    );
            Gizmos.DrawLine(
                new Vector3(
                    BoardCenter.x - MaxDistanceFromBoardCenter.x,
                    BoardCenter.y - MaxDistanceFromBoardCenter.y),
                new Vector3(
                    BoardCenter.x - MaxDistanceFromBoardCenter.x,
                    BoardCenter.y + MaxDistanceFromBoardCenter.y)
                    );
            Gizmos.DrawLine(
                new Vector3(
                    BoardCenter.x + MaxDistanceFromBoardCenter.x,
                    BoardCenter.y - MaxDistanceFromBoardCenter.y),
                new Vector3(
                    BoardCenter.x + MaxDistanceFromBoardCenter.x,
                    BoardCenter.y + MaxDistanceFromBoardCenter.y)
                    );
        }


        #endregion

        #region Checks Every Update


        private void CheckCardUnderMouse()
        {
            // Calculate Card Under Mouse
            // FIXME: ADD ANIMATIONS!!
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            // FIXME: Hard Coding in layer. Layer HighlightedCards may not always be layer 8
            if (hit.collider != null && hit.collider.gameObject.layer == 8 && selectedCard == null) // if the mouse is over a card, we haven't selected a card
            {
                // and this is a different card (or we moved from no card to a card
                if (highlightedCard == null || (hit.collider.gameObject != CardGameObjectMap[highlightedCard]))
                {
                    // because this is a different card, reset our last card
                    if (highlightedCard != null)
                    {
                        ResetCurrentHighlightedCard();
                    }
                    // FIXME: Hard Coding
                    GameObject cardGO = hit.collider.transform.parent.parent.gameObject;
                    cardGO.GetComponent<CardHolder_Hand>().HighlightCard();
                    highlightedCard = CardGameObjectMap[cardGO];
                }

            }
            else if (highlightedCard != null)
            {
                ResetCurrentHighlightedCard();
            }
        }


        // FIXME: create a more centralized mouse manager
        private void HandleMouseClicks()
        {
            // if we click the mouse over a card in the hand (which, presumably, is the highlighted card)
            // FIXME: What if we try to click on a card that for some reason has not been set as the highlighted card? is this a bug?
            if (highlightedCard != null && Input.GetMouseButtonDown(0))
            {
                if (TurnManager.Instance.OpponentTurn)
                {
                    Debug.LogWarning("You cannot select cards! It's not your turn!");
                    return;
                }
                if (highlightedCard.OnCardSelected != null)
                    highlightedCard.OnCardSelected(highlightedCard);

            }
            // another reason why we might click is to place a card
            // FIXME: Make it so that it automatically figures out where to place the card on a board
            // We shouldn't need to check it it's our turn since you shouldn't even be able to select a card, but we are checking nonetheless

            else if (TurnManager.Instance.PlayerTurn == true && selectedCard != null && Input.GetMouseButtonDown(0))
            {
                GameObject selectedCardGO = CardGameObjectMap[selectedCard];
                if (Mathf.Abs(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - BoardCenter.x) < MaxDistanceFromBoardCenter.x &&
                    Mathf.Abs(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - BoardCenter.y) < MaxDistanceFromBoardCenter.y)
                {

                    GameObject cardGO = CardGameObjectMap[selectedCard];
                    CardGameObjectMap.Remove(selectedCard);
                    // FIXME: Animations!
                    Destroy(cardGO);
                    BoardManager.Instance.PlayerBoard.PlayCard(selectedCard);
                    UpdateCardPositions(); // FIXME: SHould we instead add this onto the cardPlayed callback
                    selectedCard = null;

                }
                else
                {
                    CardGameObjectMap[selectedCard].SetSortingLayerRecursively("CardsInHand");
                    CardGameObjectMap[selectedCard].GetComponent<CardHolder_Hand>().UnSelectCard();
                    selectedCardGO.SetLayerRecursively(8);
                    // FIXME: this is simply not scalable
                    selectedCard = null;
                    UpdateCardPositions();

                }

            }

        }

        #endregion

        #region Card Utility Methods

        private void ResetCurrentHighlightedCard()
        {
            GameObject cardGO = CardGameObjectMap[highlightedCard];
            cardGO.GetComponent<CardHolder_Hand>().UnhighlightCard();
            highlightedCard = null;
        }


        // FIXME: We are not using c because should be the same as selectedCard
        private void SelectHighlightedCard(Card c)
        {
            selectedCard = highlightedCard;
            highlightedCard = null;
            GameObject cardGO = CardGameObjectMap[selectedCard];
            cardGO.GetComponent<CardHolder_Hand>().SelectCard();

        }

        #endregion
        // TODO: Setup animations
        // FIXME: Base off the data layer rather than the number of children
        private void UpdateCardPositions()
        {
            int cardCount = PlayerHand.CardCount;
            if (cardCount == 1)
            {
                transform.GetChild(0).position = this.transform.position;
                return;
            }

            float range = (CardWidth + Padding) * (cardCount - 1);
            float min = -range / 2;
            float max = range / 2;
            float delta = (max - min) / (cardCount - 1);
            for (int i = 0; i < cardCount; i++)
            {
                CardGameObjectMap[PlayerHand.Cards[i]].transform.localPosition = new Vector3(min + delta * i, 0);
            }
        }

        #region Handle Number of Cards Changed

        public BidirectionalDictionary<Card, GameObject> CardGameObjectMap = new BidirectionalDictionary<Card, GameObject>();

        private void OnCardAdded(Card c)
        {
            // Create a Card GameObject
            // FIXME: Should the card be handling card selecting???
            c.OnCardSelected += SelectHighlightedCard;
            GameObject cardGO = CardManager.Instance.GetGameObjectForCardInHand(c, this.transform);
            if (c.OnEnterHand != null)
            {
                c.OnEnterHand();
            }
            CardGameObjectMap.Add(c, cardGO);
            UpdateCardPositions();
        }

        #endregion
    }
}