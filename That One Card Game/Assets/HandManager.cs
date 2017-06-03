using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    //public float MinimumCardHolderPosition = -10;
    //public float MaximumCardHolderPosition = 10;
    public float CardWidth = 1;
    public float Padding = 0.1f;
    public float HighlightScaleIncrease = 1.5f;
    // TODO: Add different axes (x, y) for positioning cards
    public LayerMask CardMask;

    // FIXME: This will eventually become a BidirectionalDictionary<Card, GameObject>
    public List<GameObject> Cards { get; protected set; }


    public GameObject BoardParent;

    // FIXME: Eventually, this is going to take in a card
    private event Action OnCardAdded;
    private event Action OnCardRemoved;
    private int lastChildCount;

    private GameObject highlightedCard = null;
    private Vector3 previousScale = Vector3.one;

    private GameObject selectedCard = null;
    private Vector3 mouseOffset = Vector3.zero;

    #region Unity Methods
    private void Awake()
    {
        Cards = new List<GameObject>();
        OnCardAdded += UpdateCardPositions;
        OnCardRemoved += UpdateCardPositions;
    }



    private void Start()
    {
        lastChildCount = transform.childCount;
        for (int i = 0; i < lastChildCount; i++)
        {
            Cards.Add(transform.GetChild(i).gameObject);
        }
        // FIXME: The cards that we have at the start of the game are not updated by the update method
        // and it would be extra computation time to make it iterate through all of the cards several times on the first frame. 
        UpdateCardPositions();
    }

    private void Update()
    {

        HandleNumberChildrenChanged();
        CheckCardUnderMouse();
        HandleMouseClicks();
    }

    private void LateUpdate()
    {
        if (selectedCard != null)
        {
            selectedCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;

        }
    }

    #endregion

    #region Checks Every Update

    private void HandleNumberChildrenChanged()
    {
        // If the number of children we had changed
        if (transform.childCount != lastChildCount)
        {
            CheckChildAdded();
            CheckChildRemoved();
        }
    }


    private void CheckCardUnderMouse()
    {
        // Calculate Card Under Mouse
        // FIXME: ADD ANIMATIONS!!
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 12, CardMask);
        // FIXME: Hard Coding in layer. Layer HighlightedCards may not always be layer 8
        if (hit.collider != null && hit.collider.gameObject.layer == 8 && selectedCard == null) // if the mouse is over a card, we haven't selected a card
        {
            // and this is a different card (or we moved from no card to a card
            if (hit.collider.gameObject != highlightedCard)
            {
                // because this is a different card, reset our last card
                if (highlightedCard != null)
                {
                    ResetCurrentHighlightedCard();
                }
                // set the appropriate card and save the scale
                highlightedCard = hit.collider.transform.parent.gameObject;
                previousScale = highlightedCard.transform.localScale;
                // FIXME: Hard Coding in scale
                highlightedCard.transform.localScale = previousScale * HighlightScaleIncrease;
                // FIXME: Hard Coding
                highlightedCard.SetSortingLayerRecursively("HighlightedCard");
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
            if(TurnManager.Instance.OpponentTurn)
            {
                Debug.LogWarning("You cannot select cards! It's not your turn!");
                return;
            }
            SelectCardUnderMouse();
        }
        // another reason why we might click is to place a card
        // FIXME: Make it so that it automatically figures out where to place the card on a board
        // We shouldn't need to check it it's our turn since you shouldn't even be able to select a card, but we are checking nonetheless
        else if (TurnManager.Instance.PlayerTurn == true && selectedCard != null && Input.GetMouseButtonDown(0))
        {
            if (Vector3.SqrMagnitude(transform.position - selectedCard.transform.position) > Vector3.SqrMagnitude(BoardParent.transform.position - selectedCard.transform.position))
            {
                PlaceSelectedCard();
            }
            else
            {
                selectedCard.SetSortingLayerRecursively("CardsInHand");
                selectedCard.SetLayerRecursively(8);
                // FIXME: this is simply not scalable
                UpdateCardPositions();
                selectedCard = null;
            }
        }
    }

    #endregion

    #region Sub-checks every update

    private void CheckChildAdded()
    {
        // And, we actually got 1 or more new children
        if (transform.childCount > lastChildCount)
        {
            // Find the child that isn't already in the list of children
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                // FIXME: This is terrible for performance. Another reason why we need a BidirectionalDictionary<Card, GameObject>
                if (Cards.Contains(child) == false)
                {
                    // FIXME: This is a really hack-ish way to handle cards that weren't added as the last child
                    Cards.Insert(i, child);
                    lastChildCount++;

                    OnCardAdded();
                }

            }
        }
    }

    private void CheckChildRemoved()
    {
        // And, we actually lost a card
        if (transform.childCount < lastChildCount)
        {

            // FIMXE: Just completely recalculate the Cards list. WE NEED TO CALCULATE THE CARD THAT DISAPEARED
            Cards = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Cards.Add(transform.GetChild(i).gameObject);
            }
            lastChildCount = Cards.Count;
            OnCardRemoved();
        }
    }

    #endregion

    #region Card Utility Methods

    private void ResetCurrentHighlightedCard()
    {
        highlightedCard.transform.localScale = previousScale;
        // FIXME: Hard Coding
        highlightedCard.SetSortingLayerRecursively("CardsInHand");
        highlightedCard = null;
    }




    // FIXME: Work with the CardHolder class!
    private void SelectCardUnderMouse()
    {
        selectedCard = highlightedCard;
        highlightedCard = null;
        selectedCard.SetSortingLayerRecursively("SelectedCard");
        mouseOffset = selectedCard.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // FIXME: Hard Coding
        selectedCard.SetLayerRecursively(9);
        selectedCard.transform.localScale = previousScale;
        previousScale = Vector3.one;
    }

    private void PlaceSelectedCard()
    {
        
        CardHolder c = selectedCard.GetComponent<CardHolder>();
        if (c.CardData.CardType == CardType.Minion)
        {
            // Place the selected card
            // FIXME: What if the card is not a minion???
            selectedCard.SetSortingLayerRecursively("PlacedCards");
            selectedCard.transform.SetParent(BoardParent.transform);
            selectedCard.transform.SetAsLastSibling();
            if (c.OnCardPlaced != null)
                c.OnCardPlaced();
            selectedCard = null;
        }else if(c.CardData.CardType == CardType.Spell)
        {
            Destroy(c.gameObject);
        }
    }

    #endregion
    // TODO: Setup animations
    private void UpdateCardPositions()
    {
        if (lastChildCount == 1)
        {
            transform.GetChild(0).position = this.transform.position;
            return;
        }

        float range = (CardWidth + Padding) * (lastChildCount - 1);
        float min = -range / 2;
        float max = range / 2;
        float delta = (max - min) / (lastChildCount - 1);
        for (int i = 0; i < lastChildCount; i++)
        {
            Cards[i].transform.localPosition = new Vector3(min + delta * i, 0);
        }
    }
}
