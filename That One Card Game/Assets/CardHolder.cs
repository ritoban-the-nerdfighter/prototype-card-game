using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    public static readonly float CARD_HIGHLIGHT_SCALE_INCREASE = 1.5f;


    public Card Card;

    public GameObject MinionUIDataPrefab;


    public Text NameText;
    public SpriteRenderer CardPortrait;
    public Text ManaCostText;
    public Text CardText;
    public Text CardTypeText;

    public GameObject[] ChildrenToDisableOnPlay;

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

    private void Start()
    {
        ManaCost = Card.ManaCost;
        // Create the card based on the CardData
        // QUESTION: Should Start be called when the card exits the deck, or at the beginning of the game?
        NameText.text = Card.CardData.Name;
        CardPortrait.sprite = Card.CardData.CardPortrait;
        CardText.text = Card.CardData.CardText;
        CardTypeText.text = Card.CardData.CardType.ToString();
        switch (Card.CardData.CardType)
        {
            case CardType.Minion:
                gameObject.AddComponent<Minion>();
                // FIXME: Should this be on the minion?
                break;
        }

        // FIXME: I would do this in awake, but we need to let TEST_CardCreator and HandManager's awake run first
        CardSelected = false;
        Card.OnCardPlayed += CardPlayed;

    }


    Vector3 previousScale = Vector3.one;

    public void HighlightCard()
    {
        previousScale = gameObject.transform.localScale;
        // FIXME: Hard Coding in scale
        gameObject.transform.localScale = previousScale * CARD_HIGHLIGHT_SCALE_INCREASE;
        // FIXME: Hard Coding
        gameObject.SetSortingLayerRecursively("HighlightedCard");
    }

    public void UnhighlightCard()
    {
        this.transform.localScale = previousScale;
        gameObject.SetSortingLayerRecursively("CardsInHand");
    }

    public bool CardSelected { get; protected set; }


    private Vector3 select_MouseOffset = Vector3.zero;

    // FIXME: We are not using c because it is the same as CArd
    private void CardPlayed(Card c)
    {
        if (c.CardData.CardType == CardType.Minion)
        {
            // Disable everything except the CardPortrait, which we should scale up
            // TODO: Animations
            for (int i = 0; i < ChildrenToDisableOnPlay.Length; i++)
            {
                ChildrenToDisableOnPlay[i].gameObject.SetActive(false);
            }
            select_MouseOffset = Vector3.zero;
            CardSelected = false;
        }
        else if (c.CardData.CardType == CardType.Spell)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SelectCard()
    {
        gameObject.SetSortingLayerRecursively("SelectedCard");
        CardSelected = true;
        select_MouseOffset = this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.SetLayerRecursively(9);
        this.transform.localScale = previousScale;
        previousScale = Vector3.one;
    }

    private void LateUpdate()
    {
        if (CardSelected)
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + select_MouseOffset;
        }
    }
}
