using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.InfoClasses;
using Assets.Scripts.Util;
using Assets.Scripts.GraphicsMonobehaviours;

namespace Assets.Scripts.Managers
{
    public class BoardManager : Singleton<BoardManager>
    {
        public float CardWidth = 2;
        public float Padding = 0.12f;

        public Board PlayerBoard { get; protected set; }
        public Board OpponentBoard { get; protected set; }

        private Transform playerBoardParent;
        private Transform opponentBoardParent;


        public BidirectionalDictionary<Card, GameObject> CardGameObjectMap { get; protected set; }


        private void Start()
        {
            PlayerBoard = new Board();
            PlayerBoard.OnMinionAdded += SetupPlayerCardGameObject;
            // ALERT: This relies on these being called in order
            PlayerBoard.OnMinionAdded += UpdateCardPositionsWrapper;
            OpponentBoard = new Board();
            OpponentBoard.OnMinionAdded += SetupPlayerCardGameObject;
            // ALERT: This relies on these being called in order
            OpponentBoard.OnMinionAdded += UpdateCardPositionsWrapper; CardGameObjectMap = new BidirectionalDictionary<Card, GameObject>();

            playerBoardParent = new GameObject("Player Board").transform;
            playerBoardParent.SetParent(this.transform);
            playerBoardParent.localPosition = new Vector3(0, -1.5f);
            playerBoardParent.localScale = Vector3.one;

            opponentBoardParent = new GameObject("Opponent Board").transform;
            opponentBoardParent.SetParent(this.transform);
            opponentBoardParent.localPosition = new Vector3(0, 1.5f);
            opponentBoardParent.localScale = Vector3.one;

        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if(hit.collider != null && CardGameObjectMap.ContainsValue(hit.collider.gameObject))
            {
                GameObject cardGo = hit.collider.gameObject;
                CardHolder_Board holder = cardGo.GetComponent<CardHolder_Board>();
                holder.OnCardHighlighted();
            }
        }

        private void SetupPlayerCardGameObject(Card c)
        {
            GameObject cardGO = CardManager.Instance.GetGameObjectForCardOnBoard(c, TurnManager.Instance.PlayerTurn ? playerBoardParent : opponentBoardParent);
            cardGO.SetLayerRecursively(10);
            CardGameObjectMap.Add(c, cardGO);
        }


        private void UpdateCardPositionsWrapper(Card c)
        {
            UpdateCardPositions(c, TurnManager.Instance.PlayerTurn);
        }

        private void UpdateCardPositions(Card c, bool player)
        {
            Transform boardParent = player ? playerBoardParent : opponentBoardParent;
            Board board = (player ? PlayerBoard : OpponentBoard);
            if (board.CardCount == 1)
            {
                boardParent.GetChild(0).position = boardParent.position;
                return;
            }
            HandManager hand = HandManager.Instance;
            float range = (CardWidth + Padding) * (board.CardCount - 1);
            float min = -range / 2;
            float max = range / 2;
            float delta = (max - min) / (board.CardCount - 1);
            // FIXME: This is terrible for garbage collection
            foreach (Transform card in boardParent)
            {
                card.localPosition = new Vector3(min + delta * card.GetSiblingIndex(), 0);
            }
        }

    }
}