using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI
{
    public class UI_EndTurnButton : MonoBehaviour
    {
        private void Start()
        {
            TurnManager.Instance.OnTurnStarted += UpdateButtonStatus;
        }

        private void UpdateButtonStatus(bool playerTurn)
        {
            if (playerTurn == true)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }

        public void ButtonPressed()
        {
            TurnManager.Instance.EndTurn();
        }
    }
}