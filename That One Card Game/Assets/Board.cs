using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Board
{
    public List<Card> Cards { get; protected set; }
    public event Action<Card> OnCardAdded;

    public int CardCount
    {
        get
        {
            return Cards.Count;
        }
    }

    public Board()
    {
        Cards = new List<Card>();
    }

    public void PlayCard(Card c)
    {
        Cards.Add(c);
        if (OnCardAdded != null)
            OnCardAdded(c);
    }

    // FIXME: We should have a centralized Random Manager (maybe so that people could share seeds?)
    public Card GetRandomCardInHand(Random r)
    {
        return Cards[r.Next(Cards.Count)];
    }
}

