using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public Action OnUpdate;

    private void Update()
    {
        if (OnUpdate != null)
            OnUpdate();
    }


    public void SetupTimer(float time, Action updateAction, Action expireAction)
    {
        new Timer(time, updateAction, expireAction);
    }


    internal class Timer
    {
        internal float TotalTimeElapsed { get; private set; }

        protected float ExpireTime;
        protected Action updateWrapper;
        protected Action expireWrapper;

        // NOTE: To create repeating timers, simply Reset the timer when it expires

        internal Timer(float time, Action updateAction, Action expireAction)
        {
            this.ExpireTime = time;
            this.updateWrapper =
                () =>
                {
                    TotalTimeElapsed += Time.deltaTime;

                    if (TotalTimeElapsed >= ExpireTime)
                    {
                        if (this.expireWrapper != null)
                        {
                            expireWrapper.Invoke();
                            return;
                        }

                    }
                    if (updateAction != null)
                    {
                        updateAction.Invoke();
                    }

                };
            CoroutineManager.Instance.OnUpdate += this.updateWrapper;
            this.expireWrapper = () =>
            {
                expireAction.Invoke();
                CoroutineManager.Instance.OnUpdate -= this.updateWrapper;

            };
        }

        public void Reset()
        {
            // The timer already went off, so we need to add it back to OnUpdate
            if (TotalTimeElapsed >= ExpireTime)
            {
                CoroutineManager.Instance.OnUpdate += this.updateWrapper;
            }

            TotalTimeElapsed = 0f;
        }
    }


}
