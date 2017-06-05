using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CardCreator : MonoBehaviour
{
    public CardData[] Deck;
    public int CardCount;

    private void Awake()
    {
        for (int i = 0; i < CardCount; i++)
        {
            HandManager.Instance.Hand.AddCard(new Card(Deck[i % Deck.Length]));
        }
    }
}
