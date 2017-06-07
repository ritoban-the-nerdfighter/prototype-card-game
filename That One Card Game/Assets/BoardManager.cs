using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public Board PlayerBoard { get; protected set; }
    public Board OpponentBoard { get; protected set; }

    public BidirectionalDictionary<Card, GameObject> CardGameObjectMap { get; protected set; }


    private void Start()
    {
        PlayerBoard = new Board();
        PlayerBoard.OnMinionAdded += OnChildAdded;
        OpponentBoard = new Board();
        CardGameObjectMap = new BidirectionalDictionary<Card, GameObject>();
    }



    private void OnChildAdded(Card c)
    {
        if (PlayerBoard.CardCount == 1)
        {
            transform.GetChild(0).position = this.transform.position;
            return;
        }
        HandManager hand = HandManager.Instance;
        float range = (hand.CardWidth + hand.Padding) * (PlayerBoard.CardCount - 1);
        float min = -range / 2;
        float max = range / 2;
        float delta = (max - min) / (PlayerBoard.CardCount - 1);
        // FIXME: This is terrible for garbage collection
        foreach (Transform card in transform)
        {
            card.position = new Vector3(min + delta * card.GetSiblingIndex(), transform.position.y);
        }
    }

    
    public void AddCardGameObject(Card cardToPlace, GameObject cardGO)
    {
        cardGO.transform.SetParent(this.transform);
        CardGameObjectMap.Add(cardToPlace, gameObject);
        PlayerBoard.PlayCard(cardToPlace);
    }

    /*
     * Ways to transfer card gameobject:
     *  CardManager that keeps track of gameobjects for all possible cards
     *  CardManager that generates a new gameObject givern a card
     */
}
