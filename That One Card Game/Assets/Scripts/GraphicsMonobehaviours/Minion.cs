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

        public event Action OnCardDeath;

        private void Awake()
        {
            CardHolder = GetComponent<CardHolder>();
            SetupCardAsMinion();
        }

        private GameObject UIGo;

        private void SetupCardAsMinion()
        {

            // FIXME: These checks scattered throughout the code are UGLY!!!
            if (CardHolder is CardHolder_Hand || (CardHolder is CardHolder_Board && Card.CardData.CardType == CardType.Minion))
            {
                // CREATE HEALTH AND ATTACK UI VALUES WHILE CARD IS IN HAND (OR SOMETHING ELSE)
                UIGo = Instantiate(ResourceManager.Instance.GetResourcePrefab(MINION_UI_PREFAB_NAME),
                    this.transform.GetComponentInChildren<Canvas>().transform, false);
                // FIXME: These listeners won't kick in the first time because Cards are created in an awake method before this
                Card.AddListener("Attack", UpdateStatDisplay);
                Card.AddListener("Health", UpdateStatDisplay);
                UpdateStatDisplay();
            }

        }


        public void TakeDamage(int amount)
        {
            Card.ChangeStat("Health", -amount);
            if (Card.GetStat("Health") <= 0)
                Die();
        }

        public void RestoreHealth(int amount)
        {

        }

        public void ChangeStats(string cause, int attack, int health)
        {

        }

        private void Die()
        {
            OnCardDeath();
        }

        private void UpdateStatDisplay()
        {
            // FIXME: Find a better way to do this
            Text[] texts = UIGo.GetComponentsInChildren<Text>();

            texts[0].text = Card.GetStat("Attack").ToString();
            texts[1].text = Card.GetStat("Health").ToString();
        }
    }
}