using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    public static bool UseAI = true; // FIXME: We only support AI;

    private void Awake()
    {
        if(UseAI != true)
        {
            throw new Exception("Non-AI opponent not supported!");
        }
        TurnManager.Instance.OnTurnStarted += OnTurnStarted;
    }

    private void OnTurnStarted(bool playerTurn)
    {
        if (playerTurn == true)
            return; // It's the player's turn, so we don't do anything
        // If it is our turn,
        AI.StartOpponentAI();
    }
}

