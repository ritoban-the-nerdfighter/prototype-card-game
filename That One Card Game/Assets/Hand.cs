using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Hand
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

    public Hand()
    {
        Cards = new List<Card>();
    }

    public void AddCard(Card c)
    {
        Cards.Add(c);
        if (OnCardAdded != null)
            OnCardAdded(c);
        c.OnCardPlayed += CardPlayed;
    }

    // FIXME: Not used!!!
    public void DrawCard(Deck d)
    {
       AddCard(d.DrawCardFromDeck());
    }

    // FIXME: We should have a centralized Random Manager (maybe so that people could share seeds?)
    public Card GetRandomCardInHand(Random r)
    {
        return Cards[r.Next(Cards.Count)];
    }

    private void CardPlayed(Card c)
    {
        Cards.Remove(c);
    }

}
