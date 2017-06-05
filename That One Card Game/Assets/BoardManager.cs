using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public Board Board { get; protected set; }

    public BidirectionalDictionary<Card, GameObject> CardGameObjectMap { get; protected set; }


    private void Start()
    {
        Board = new Board();
        Board.OnCardAdded += OnChildAdded;
        CardGameObjectMap = new BidirectionalDictionary<Card, GameObject>();
    }



    private void OnChildAdded(Card c)
    {
        if (Board.CardCount == 1)
        {
            transform.GetChild(0).position = this.transform.position;
            return;
        }
        HandManager hand = HandManager.Instance;
        float range = (hand.CardWidth + hand.Padding) * (Board.CardCount - 1);
        float min = -range / 2;
        float max = range / 2;
        float delta = (max - min) / (Board.CardCount - 1);
        // FIXME: This is terrible for garbage collection
        foreach (Transform card in transform)
        {
            card.position = new Vector3(min + delta * card.GetSiblingIndex(), transform.position.y);
        }
    }

    public void AddCard(Card cardToPlace, GameObject cardGO)
    {
        cardGO.transform.SetParent(this.transform);
        CardGameObjectMap.Add(cardToPlace, gameObject);
        Board.PlayCard(cardToPlace);
    }
}
