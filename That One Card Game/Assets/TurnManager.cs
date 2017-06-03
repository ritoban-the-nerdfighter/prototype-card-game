using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool PlayerTurn { get; protected set; }
    public bool OpponentTurn
    {
        get
        {
            return PlayerTurn == false;
        }
    }

    private void Start()
    {
        // FIXME: This may not always apply!
        PlayerTurn = true;
        if (OnTurnStarted != null)
            OnTurnStarted(PlayerTurn);
    }

    private void Update()
    {
    }

    /// <summary>
    /// Called when the turn has ended. Called before PlayerTurn is set. Passes last value of PlayerTurn.
    /// </summary>
    public Action<bool> OnTurnEnded;
    /// <summary>
    /// Called when the turn just started. Called after PlayerTurn is set. Passes new value of PlayerTurn
    /// </summary>
    public Action<bool> OnTurnStarted;

    public void EndTurn()
    {
        if (OnTurnEnded != null)
            OnTurnEnded(PlayerTurn);

        PlayerTurn = PlayerTurn == false;

        if (OnTurnStarted != null)
            OnTurnStarted(PlayerTurn);
    }
}
