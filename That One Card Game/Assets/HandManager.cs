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
    // TODO: Add different axes (x, y) for positioning cards
    public LayerMask CardMask;

    // FIXME: This will eventually become a BidirectionalDictionary<Card, GameObject>
    public List<GameObject> Cards { get; protected set; }

    // FIXME: Eventually, this is going to take in a card
    private event Action OnCardAdded;
    private event Action OnCardRemoved;
    private int lastChildCount;

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

    private GameObject highlightedCard = null;
    private Vector3 previousScale = Vector3.one;

    private GameObject selectedCard = null;
    private Vector3 mouseOffset = Vector3.zero;

    private void Update()
    {
        // If the number of children we had changed
        if (transform.childCount != lastChildCount)
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

        // Calculate Card Under Mouse
        // FIXME: ADD ANIMATIONS!!
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 12, CardMask);
        // FIXME: Hard Coding in layer. Layer HighlightedCards may not always be layer 8
        if (hit.collider != null && hit.collider.gameObject.layer == 8)
        {
            if (hit.collider.gameObject != highlightedCard)
            {
                if (highlightedCard != null)
                {
                    highlightedCard.transform.localScale = previousScale;
                }
                highlightedCard = hit.collider.transform.parent.gameObject;
                previousScale = highlightedCard.transform.localScale;
                // FIXME: Hard Coding in scale
                highlightedCard.transform.localScale = previousScale * 2;
                // FIXME: Hard Coding

                highlightedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "HighlightedCard";
            }
        }
        else if(highlightedCard != null)
        {
            highlightedCard.transform.localScale = previousScale;
            // FIXME: Hard Coding
            highlightedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "CardsInHand";
            highlightedCard = null; 


        }

        if(highlightedCard != null && Input.GetMouseButtonDown(0))
        {
            selectedCard = highlightedCard;
            highlightedCard = null;
            selectedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCard";
            mouseOffset = selectedCard.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mouseOffset.z = 0;
            // FIXME: Hard Coding

            selectedCard.SetLayerRecursively(9);
            selectedCard.transform.localScale = previousScale;
            previousScale = Vector3.zero;
        }
        // FIXME: Make it so that it automatically figures out where to place the card on a board
        else if(selectedCard != null && Input.GetMouseButtonDown(0))
        {
            // Place the selected card
            selectedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "PlacedCards";
            selectedCard = null;
        }


    }

    private void LateUpdate()
    {
        if (selectedCard != null)
        {
            selectedCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;

        }
    }

    // TODO: Setup animations
    private void UpdateCardPositions()
    {
        float range = (CardWidth + Padding) * (lastChildCount-1);
        float min = -range / 2;
        float max = range / 2;
        float delta = (max-min) / (lastChildCount-1);
        for (int i = 0; i < lastChildCount; i++)
        {
            Cards[i].transform.position = new Vector3(min + delta * i, Cards[i].transform.position.y);
        }
    }
}
 