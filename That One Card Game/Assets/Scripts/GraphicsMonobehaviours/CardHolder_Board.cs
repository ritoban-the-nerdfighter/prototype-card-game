﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GraphicsMonobehaviours
{
    class CardHolder_Board : CardHolder
    {
        public GameObject ArrowPrefab;


        protected bool cardHighlighted = false;
        protected GameObject arrowGO;

        public void OnCardHighlight()
        {
            cardHighlighted = true;
        }

        public void LateUpdate()
        {
            if (cardHighlighted)
            {
                if (Input.GetMouseButtonDown(0) && arrowGO == null)
                {
                    // Create Arrow
                    arrowGO = Instantiate(ArrowPrefab, this.transform, false);
                }
            }

            if(arrowGO != null)
            {
                // Follow the mouse!
                // Step 1: Convert Mouse Position to world coordinates!!
                Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldSpaceMouse.z = 0;
                Vector3 diff = this.transform.position - worldSpaceMouse;
                diff.z = 0;
                arrowGO.transform.position = this.transform.position;
                arrowGO.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x));
                arrowGO.transform.localScale = Vector3.one * diff.magnitude;
            }


            if (Input.GetMouseButtonUp(0))
            {
                Destroy(arrowGO);
                // Check what's under the mouse!
             
            }



            // HACK: this is a terrible way to do this. you should keep a "card highlighted last frame" thing in the board manager
            cardHighlighted = false;
        }
    }
}