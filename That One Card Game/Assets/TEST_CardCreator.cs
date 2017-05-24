using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CardCreator : MonoBehaviour
{

    public GameObject CardHolder;
    public int CardCount;

    private void Awake()
    {
        for (int i = 0; i < CardCount; i++)
        {
            GameObject go = Instantiate(CardHolder, this.transform);
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Random.ColorHSV(0, 1, 0.8f, 1, 0.8f, 1);
        }
    }
}
