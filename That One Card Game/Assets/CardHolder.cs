using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    public static readonly float CARD_PLACE_SCALE = 1.7f;

    public CardData CardData;

    public GameObject MinionUIDataPrefab;


    public Text NameText;
    public SpriteRenderer CardPortrait;
    public Text ManaCost;
    public Text CardText;
    public Text CardTypeText;

    public Action OnCardPlaced; // TODO: Trigger Battlecries here (or maybe later, after processing secrets?)
    public Action OnCardEnterHand; 
    



    private void Start()
    {
        // Create the card based on the CardData
        // QUESTION: Should Start be called when the card exits the deck, or at the beginning of the game?
        NameText.text = CardData.Name;
        CardPortrait.sprite = CardData.CardPortrait;
        ManaCost.text = CardData.ManaCost.ToString();
        CardText.text = CardData.CardText;
        CardTypeText.text = CardData.CardType.ToString();
        switch (CardData.CardType)
        {
            case CardType.Minion:
                Minion minion = gameObject.AddComponent<Minion>();
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
                child.transform.localScale *= CARD_PLACE_SCALE;
        }
    }
}
