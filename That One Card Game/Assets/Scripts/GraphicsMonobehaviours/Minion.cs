using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.InfoClasses;
using Assets.Scripts.Managers;

namespace Assets.Scripts.GraphicsMonobehaviours
{
    [RequireComponent(typeof(CardHolder))]
    public class Minion : MonoBehaviour
    {
        public static string MINION_UI_PREFAB_NAME = "MinionUIPrefab";


        public Card Card
        {
            get
            {
                return CardHolder.Card;
            }
        }

        private CardHolder CardHolder;

        private void Awake()
        {
            CardHolder = GetComponent<CardHolder>();
            // FIXME: SUCH AN UGLY way to do this. FIND A BETTER SOLUTION!
            SetupCardAsMinion();
        }

        private GameObject UIGo;

        private void SetupCardAsMinion()
        {
            // CREATE HEALTH AND ATTACK UI VALUES
            // FIXME: Hard coding in canvas as parent
            UIGo = Instantiate(ResourceManager.Instance.GetResourcePrefab(MINION_UI_PREFAB_NAME),
                this.transform.GetComponentInChildren<Canvas>().transform, false);

            Card.AddListener("Attack", UpdateStatDisplay);
            Card.AddListener("Health", UpdateStatDisplay);
        }




        public void TakeDamage(int amount)
        {

        }

        public void RestoreHealth(int amount)
        {

        }

        public void ChangeStats(string cause, int attack, int health)
        {

        }

        private void UpdateStatDisplay()
        {
            Text[] texts = UIGo.GetComponentsInChildren<Text>();

            // FIXME: Card should have a "Statistics" dictionary
            texts[0].text = Card.GetStat("Attack").ToString();
            texts[1].text = Card.GetStat("Health").ToString();
        }
    }
}