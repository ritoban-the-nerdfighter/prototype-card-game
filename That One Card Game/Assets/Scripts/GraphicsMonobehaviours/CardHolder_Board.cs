using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GraphicsMonobehaviours
{
    class CardHolder_Board : CardHolder
    {
        // FIXME: Because all cards on the board are minions, we are doing this here! (Later on, weapons will work similarly)
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag();
            }
            if (Input.GetMouseButton(0))
            {
                UpdateDrag();
            }
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }

        private void EndDrag()
        {
            throw new NotImplementedException();
        }

        private void UpdateDrag()
        {
            throw new NotImplementedException();
        }

        private void StartDrag()
        {
            if (CardHighlighted == false)
                return;
            // Create an arrow
            Debug.Log("Start Drag");
        }

        protected bool CardHighlighted = false;

        public void OnCardHighlight()
        {
            CardHighlighted = true;
        }

        public void LateUpdate()
        {
            // HACK: this is a terrible way to do this. you should keep a "card highlighted last frame" thing in the board manager
            CardHighlighted = false;
        }
    }
}