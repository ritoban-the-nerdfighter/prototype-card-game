using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InfoClasses;

namespace Assets.Scripts.GraphicsMonobehaviours
{
    abstract class CardHolder : MonoBehaviour
    {
        // FIXME: This is set by the cardManager
        public Card Card { get; set; }

        protected void Start()
        {
            Debug.Log("Parentclass Start");
            Init();
        }

        protected void Init()
        {
            Debug.Log("ParentClass Init");
            switch (Card.CardData.CardType)
            {
                case CardType.Minion:
                    gameObject.AddComponent<Minion>();
                    // FIXME: Should this be on the minion?
                    break;
            }
        }
    }
}