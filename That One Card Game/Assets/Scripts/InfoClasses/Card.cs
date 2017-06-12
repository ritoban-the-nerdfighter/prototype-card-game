using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Card
{
    public CardData CardData;
    public int ManaCost;

    // FIXME: Minion specific properties. Find a better way to do this
    public int Health;
    public int Attack;

    public Action OnEnterHand;
    public Action<Card> OnCardPlayed;

    public Card(CardData cardData)
    {
        CardData = cardData;
        ManaCost = cardData.ManaCost;
        Health = cardData.Health;
        Attack = cardData.Attack;
    }


    public Action<Card> OnCardSelected;

}

