using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;


public class Card
{
    public CardData CardData;
    public int ManaCost;


    public Action OnEnterHand;
    public Action<Card> OnCardPlayed;

    public Card(CardData cardData)
    {
        CardData = cardData;
        ManaCost = cardData.ManaCost;

        SetupCallbacks();
    }

    private void SetupCallbacks()
    {
        Type actionType = Type.GetType(CardData.ActionFile);
        if (actionType == null)
            return;
        // Now go through each of the callbacks and set them up appropriately
        MethodInfo turnStarted = actionType.GetMethod(CardData.TurnStartedMethod);
        if (turnStarted != null)
        {
            // FIXME: pass in parameters
            Delegate d = Delegate.CreateDelegate(typeof(Action<bool>), turnStarted);
            Action<bool> turnStartedAction = (Action<bool>)d;
            TurnManager.Instance.OnTurnStarted += turnStartedAction;
        }
        MethodInfo turnEnded = actionType.GetMethod(CardData.TurnEndedMethod);
        if (turnEnded != null)
        {
            // FIXME: pass in parameters
            Delegate d = Delegate.CreateDelegate(typeof(Action<bool>), turnEnded);
            Action<bool> turnEndedAction = (Action<bool>)d;
            TurnManager.Instance.OnTurnEnded += turnEndedAction;
        }
    }

    public Action<Card> OnCardSelected;

}

