using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int childCount;


    private HandManager hand;

    private void Start()
    {
        childCount = transform.childCount;
        hand = FindObjectOfType<HandManager>();
    }

    private void Update()
    {
        if (transform.childCount != childCount)
        {
            childCount = transform.childCount;
            SendMessage("OnChildAdded");
        }
    }

    private void OnChildAdded()
    {
        if(childCount == 1)
        {
            transform.GetChild(0).position = this.transform.position;
            return;
        }
        float range = (hand.CardWidth + hand.Padding) * (childCount - 1) * CardHolder.CARD_PLACE_SCALE;
        float min = -range / 2;
        float max = range / 2;
        float delta = (max - min) / (childCount - 1);
        // FIXME: This is terrible for garbage collection
        foreach(Transform card in transform)
        {
            card.position = new Vector3(min + delta * card.GetSiblingIndex(), transform.position.y);
        }
    }
}
