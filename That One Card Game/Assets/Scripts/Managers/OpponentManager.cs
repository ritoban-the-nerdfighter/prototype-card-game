using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Rand = UnityEngine.Random;

public class OpponentManager : MonoBehaviour
{
    public static bool UseAI = true; // FIXME: We only support AI;

    public CardData[] CardData;

    // FIXME: The BoardManager stores both players hands, but the HandManager stores only 1 players hand
    public Hand Hand { get; protected set; }

    private void Awake()
    {
        Hand = new Hand();
        if(UseAI != true)
        {
            throw new Exception("Non-AI opponent not supported!");
        }
        TurnManager.Instance.OnTurnStarted += OnTurnStarted;
        for (int i = 0; i < 10; i++)
        {
            Hand.AddCard(new Card(CardData[Rand.Range(0, CardData.Length)]));
        }
    }

    private void OnTurnStarted(bool playerTurn)
    {
        if (playerTurn == true)
            return; // It's the player's turn, so we don't do anything
        // If it is our turn,
        AI.StartOpponentAI(Hand, BoardManager.Instance.OpponentBoard);
    }
}

