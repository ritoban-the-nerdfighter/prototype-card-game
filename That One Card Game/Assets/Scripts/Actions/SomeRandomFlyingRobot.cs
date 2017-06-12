using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  static class SomeRandomFlyingRobot
{
    public static void OnTurnStarted(bool playerTurn)
    {
        Debug.Log("SomeRandomMinionActions::OnTurnStarted");
    }

    public static void OnTurnEnded(bool playerTurn)
    {
        Debug.Log("SomeRandomMinionActions::OnTurnEnded");
    }
}
