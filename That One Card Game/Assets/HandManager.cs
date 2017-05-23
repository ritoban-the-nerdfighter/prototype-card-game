using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // FIXME: Make these fluid based on number of cards
    public float MinimumCardHolderRotation = -60;
    public float MaximumCardHolderRotation = 60;
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
        OnCardAdded += UpdateCardRotations;
        OnCardRemoved += UpdateCardRotations;
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
        UpdateCardRotations();
    }

    private GameObject highlightedCard = null;
    private Vector3 previousScale = Vector3.one;

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
                // FIXME: FOR NOW! We will completely rework this soon
                lastChildCount--;
                // FIMXE: Just completely recalculate the Cards list. WE NEED TO CALCULATE THE CARD THAT DISAPEARED
                Cards = new List<GameObject>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Cards.Add(transform.GetChild(i).gameObject);
                }
                OnCardRemoved();
            }
        }

        // Calculate Card Under Mouse
        // FIXME: ADD ANIMATIONS!!
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 12, CardMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != highlightedCard)
            {
                if (highlightedCard != null)
                {
                    highlightedCard.transform.localScale = previousScale;
                }
                highlightedCard = hit.collider.gameObject;
                previousScale = highlightedCard.transform.localScale;
                highlightedCard.transform.localScale = previousScale * 2;
            }
        }
        else if(highlightedCard != null)
        {
            highlightedCard.transform.localScale = previousScale;
            highlightedCard = null;
        }

        

    }

    // TODO: Setup animations
    private void UpdateCardRotations()
    {
        if (lastChildCount == 1)
        {
            // If there is only 1 card, rotate it to the average of min and max
            Cards[0].transform.rotation = Quaternion.Euler(0, 0, (MinimumCardHolderRotation + MaximumCardHolderRotation) / 2);
        }
        else
        {
            // Otherwise, 
            float angleDiff = MaximumCardHolderRotation - MinimumCardHolderRotation;
            float delta = angleDiff / (lastChildCount - 1);
            float currentRotation = MinimumCardHolderRotation;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localEulerAngles = new Vector3(0, 0, currentRotation);

                currentRotation += delta;
            }
        }
    }
}
