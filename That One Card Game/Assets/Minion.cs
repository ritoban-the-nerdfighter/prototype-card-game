using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    public CardData CardData;
    public GameObject MinionUIDataPrefab;

    private CardHolder CardHolder;

    private void Awake()
    {
        CardHolder = GetComponent<CardHolder>();
        CardData = CardHolder.CardData;
        // FIXME: SUCH AN UGLY way to do this. FIND A BETTER SOLUTION!
        MinionUIDataPrefab = CardHolder.MinionUIDataPrefab;
        SetupCardAsMinion();
    }

    private void SetupCardAsMinion()
    {
        // CREATE HEALTH AND ATTACK UI VALUES
        // FIXME: Hard coding in canvas as parent
        GameObject UIData = Instantiate(MinionUIDataPrefab, this.transform.GetComponentInChildren<Canvas>().transform, false);
        Text[] texts = UIData.GetComponentsInChildren<Text>();
        // FIXME: what if there is more data? this hard coding is stupid! eventually, we should create a minion data ui script on the prefab that handles this
        texts[0].text = CardData.Attack.ToString();
        texts[1].text = CardData.Health.ToString();
    }
}
