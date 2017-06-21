using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.InfoClasses
{
    public class Deck
    {
        public Stack<Card> CardDeck;

        public Deck()
        {

        }


        // UNUSED!!
        public Card DrawCardFromDeck()
        {
            return CardDeck.Pop();
        }
    }

}