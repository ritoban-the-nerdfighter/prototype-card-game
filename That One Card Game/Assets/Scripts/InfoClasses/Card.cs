using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.InfoClasses
{
    public class Card
    {
        public CardData CardData;
        public int ManaCost;


        public Action OnEnterHand;
        public Action<Card> OnCardPlayed;

        public bool Player;

        public Card(CardData cardData, bool player)
        {
            CardData = cardData;
            ManaCost = cardData.ManaCost;
            this.Player = player;


            SetupBaseStats();

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
            MethodInfo cardPlayed = actionType.GetMethod(CardData.CardPlayedMethod);
            if (cardPlayed != null)
            {
                // FIXME: pass in parameters
                Delegate d = Delegate.CreateDelegate(typeof(Action<Card>), cardPlayed);
                Action<Card> cardPlayedAction = (Action<Card>)d;
                OnCardPlayed += cardPlayedAction;
            }
        }

        private void SetupBaseStats()
        {
            Stats = new Dictionary<string, int>();
            Listeners = new Dictionary<string, Action>();
            switch (CardData.CardType)
            {
                case CardType.Minion:
                    SetStat("Attack", CardData.Attack);
                    SetStat("Health", CardData.Health);
                    SetStat("BaseAttack", CardData.Attack);
                    SetStat("BaseHealth", CardData.Health);
                    break;
                default:
                    break;
            }
        }

        public Action<Card> OnCardSelected;

        /// Storing Health and Attack Data
        /// 2 ways:
        ///     Dictionary string-int/float
        ///     Nullable<int> Health, Nullable<int> Attack, etc
        /// I think the dictioanry is more flexible
        /// 


        #region Stats Dictionaries
        protected Dictionary<string, int> Stats { get; set; }
        protected Dictionary<string, Action> Listeners { get; set; }

        public void AddListener(string s, Action a)
        {
            
            if (Listeners.ContainsKey(s))
                Listeners[s] += a;
            else
                Listeners[s] = a;
        }

        public void RemoveListener(string s, Action a)
        {
            Listeners[s] -= a;
        }

        public void SetStat(string s, int value)
        {
            Stats[s] = value;
            if (Listeners.ContainsKey(s) && Listeners[s] != null)
                Listeners[s].Invoke();
        }

        public void ChangeStat(string s, int offset)
        {
            Stats[s] += offset;
            if (Listeners.ContainsKey(s) && Listeners[s] != null)
                Listeners[s].Invoke();

        }

        public int GetStat(string s)
        {
            return Stats[s];
        }

        #endregion
    }

}