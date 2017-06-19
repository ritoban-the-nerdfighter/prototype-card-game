using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardHolder))]
public class Minion : MonoBehaviour
{
    public static string MINION_UI_PREFAB_NAME = "MinionUIPrefab";


    public Card Card
    {
        get
        {
            return GetComponent<CardHolder>().Card;
        }
    }

    private CardHolder CardHolder;

    private void Awake()
    {
        CardHolder = GetComponent<CardHolder_Hand>();
        // FIXME: SUCH AN UGLY way to do this. FIND A BETTER SOLUTION!
        SetupCardAsMinion();
    }

    private void SetupCardAsMinion()
    {
        // CREATE HEALTH AND ATTACK UI VALUES
        // FIXME: Hard coding in canvas as parent
        GameObject UIData = Instantiate(ResourceManager.Instance.GetResourcePrefab(MINION_UI_PREFAB_NAME),
            this.transform.GetComponentInChildren<Canvas>().transform, false);
        Text[] texts = UIData.GetComponentsInChildren<Text>();

        // FIXME: Card should have a "Statistics" dictionary
        texts[0].text = Card.CardData.Attack.ToString();
        texts[1].text = Card.CardData.Health.ToString();
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
}
