using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ALERT: Weapons are not supported yet!
/// </summary>
public enum CardType { Minion, Spell, Weapon}

[CreateAssetMenu(fileName ="New Card Data", menuName = "Cards/Card Data", order = -1)]
public class CardData : ScriptableObject
{
    public string Name;
    public Sprite CardPortrait; 
    public string CardText;
    public int ManaCost;

    public CardType CardType;

    [Header("Minion Specific Data")]
    public int Health;
    public int Attack;


    [Header("Actions")]
    public string ActionFile;
    public string TurnStartedMethod;
    public string TurnEndedMethod;
   
}
