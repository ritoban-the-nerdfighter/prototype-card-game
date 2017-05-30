using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    public CardData CardData;

    public GameObject MinionUIDataPrefab;


    public Text NameText;
    public SpriteRenderer CardPortrait;
    public Text ManaCost;
    public Text CardText;
    public Text CardTypeText;

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
                break;
        }
    }
}
