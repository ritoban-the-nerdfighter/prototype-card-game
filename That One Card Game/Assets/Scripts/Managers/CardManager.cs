using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GraphicsMonobehaviours;
using Assets.Scripts.Util;
using Assets.Scripts.InfoClasses;

namespace Assets.Scripts.Managers
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField] private GameObject cardHolderPrefab;
        [SerializeField] private GameObject cardPortraitPrefab; // FIXME: What about different types of card portraits?

        // We do not need this just yet, but we will when we start reading card data in from JSON/XML files
        public CardData[] AllCardData;

        public GameObject GetGameObjectForCardInHand(Card c, Transform parent)
        {
            GameObject cardGO = Instantiate(cardHolderPrefab, parent);
            CardHolder_Hand holder = cardGO.GetComponent<CardHolder_Hand>();
            holder.Card = c;
            return cardGO;
        }

        public GameObject GetGameObjectForCardOnBoard(Card c, Transform parent)
        {
            GameObject cardGO = Instantiate(cardPortraitPrefab, parent);
            SpriteRenderer sr = cardGO.GetComponent<SpriteRenderer>();
            sr.sprite = c.CardData.CardPortrait;
            return cardGO;
        }
    }

}