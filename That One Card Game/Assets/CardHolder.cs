using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    public static readonly float PLACED_CARD_PORTRAIT_SCALE = 1.5f;

    public CardData CardData;

    public GameObject MinionUIDataPrefab;


    public Text NameText;
    public SpriteRenderer CardPortrait;
    public Text ManaCostText;
    public Text CardText;
    public Text CardTypeText;

    public Action OnCardPlaced; // TODO: Trigger Battlecries here (or maybe later, after processing secrets?)
    public Action OnCardEnterHand;

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
        ManaCost = CardData.ManaCost;
        // Create the card based on the CardData
        // QUESTION: Should Start be called when the card exits the deck, or at the beginning of the game?
        NameText.text = CardData.Name;
        CardPortrait.sprite = CardData.CardPortrait;
        CardText.text = CardData.CardText;
        CardTypeText.text = CardData.CardType.ToString();
        switch (CardData.CardType)
        {
            case CardType.Minion:
                gameObject.AddComponent<Minion>();
                // FIXME: Should this be on the minion?
                OnCardPlaced += OnCardPlaces_UpdateGraphics;
                break;
        }

       
    }

    private void OnCardPlaces_UpdateGraphics()
    {
        // Show only the card art. 
        // TODO: build the entire card on mouse over
        foreach(Transform child in transform)
        {
            if (child.CompareTag("CardPortrait") == false)
                child.gameObject.SetActive(false);
            else
                child.transform.localScale *= PLACED_CARD_PORTRAIT_SCALE;
        }
    }
}
