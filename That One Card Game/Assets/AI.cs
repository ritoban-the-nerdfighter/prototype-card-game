using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AI
{
    public static void StartOpponentAI()
    { 
        CoroutineManager.Instance.SetupTimer(3f, null, () => { TurnManager.Instance.EndTurn(); });
    }
}
