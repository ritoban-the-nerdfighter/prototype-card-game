using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InfoClasses;

namespace Assets.Scripts.Actions
{
    public static class SomeRandomFlyingRobot
    {
        public static void OnTurnStarted(bool playerTurn)
        {
            //Debug.Log("SomeRandomMinionActions::OnTurnStarted");
        }

        public static void OnTurnEnded(bool playerTurn)
        {
           //Debug.Log("SomeRandomMinionActions::OnTurnEnded");
        }

        public static void OnCardPlayed(Card c)
        {
            Debug.Log("Battlecry: " + c.CardData.Name);
        }
    }
}