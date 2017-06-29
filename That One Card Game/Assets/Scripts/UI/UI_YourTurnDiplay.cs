using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI
{
    class UI_YourTurnDiplay : MonoBehaviour
    {
        private void Awake()
        {
            TurnManager.Instance.OnTurnStarted += OnTurnStarted;
            gameObject.SetActive(false);
        }

        private void OnTurnStarted(bool playerTurn)
        {
            if (playerTurn == false)
                return;
            gameObject.SetActive(true);
            CoroutineManager.Instance.SetupTimer(1f, null, () => { gameObject.SetActive(false); });
        }
    }
}
