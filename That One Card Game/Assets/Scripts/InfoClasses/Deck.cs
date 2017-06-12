using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Deck
{
    public Stack<Card> CardDeck;

    public Deck()
    {

    }

    public Card DrawCardFromDeck()
    {
        return CardDeck.Pop();
    }
}

