using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AI
{
    public static void StartOpponentAI(Hand currentHand, Board myBoard)
    { 
        CoroutineManager.Instance.SetupTimer(3f, null, () => { TurnManager.Instance.EndTurn(); });
        Card c = currentHand.GetRandomCardInHand();
        // FIXME: Shouldn't the board somehow be the one doing this?
        myBoard.PlayCard(c);
    }
}
