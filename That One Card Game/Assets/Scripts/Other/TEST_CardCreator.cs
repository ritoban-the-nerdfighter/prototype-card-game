using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.InfoClasses;

namespace Assets.Scripts.Test
{
    public class TEST_CardCreator : MonoBehaviour
    {
        public CardData[] Deck;
        public int CardCount;

        private void Awake()
        {
            for (int i = 0; i < CardCount; i++)
            {
                HandManager.Instance.PlayerHand.AddCard(new Card(Deck[i % Deck.Length], true));
            }
        }
    }

}