using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Server
{
    enum TickType
    {
        Hour,
        Minute,
        Second
    }
    [CreateAssetMenu(fileName = "ServerTick", menuName = "ServerTicks/ServerTick", order = 0)]
    public class ServerTick : ScriptableObject
    {
        [SerializeField] TickType tickType;
        [SerializeField] int interval;

        [SerializeField] public UnityEvent OnTick { get; private set; }
        int prevTicked = -1;
       
        public void CheckTick(DateTime dateTime)
        {
            switch (tickType)
            {
                case TickType.Hour:
                    // See TickType.Second for explanation.
                    int currentHour = DateTime.Now.Hour;

                    if (interval == 0)
                    {
                        if (currentHour == 0 && prevTicked != currentHour)
                        {
                            Tick();
                            prevTicked = -1;
                        }
                        break;
                    }

                    if (currentHour % interval == 0)
                    {
                        if (prevTicked != currentHour)
                        {
                            Tick();
                            prevTicked = currentHour;
                        }
                    }
                    break;
                case TickType.Minute:
                    // See TickType.Second for explanation.
                    int currentMinute = DateTime.Now.Minute;

                    if (interval == 0)
                    {
                        if (currentMinute == 0 && prevTicked != currentMinute)
                        {
                            Tick();
                            prevTicked = -1;
                        }
                        break;
                    }

                    if (currentMinute % interval == 0)
                    {
                        if (prevTicked != currentMinute)
                        {
                            Tick();
                            prevTicked = currentMinute;
                        }
                    }
                    break;
                case TickType.Second:
                    int currentSecond = DateTime.Now.Second;

                    // If set to tick every 0 seconds, tick at 0 second instead
                    // e.g. at the start of a new minute
                    if(interval == 0)
                    {
                        if (currentSecond == 0 && prevTicked != currentSecond)
                        {
                            Tick();
                            // Set prevTicked to something else so ticking only happens once per second
                            // but also so it happens again when we're back to 0 second next time.
                            prevTicked = -1;
                        }
                        break;
                    }

                    // Interval of 5 means that tick happens every 5 seconds.
                    // At 0, 5, 10, 15..
                    if(currentSecond % interval == 0)
                    {
                        if(prevTicked != currentSecond)
                        {
                            Tick();
                            // Set prevTicked to current so tick only happens once per second.
                            prevTicked = currentSecond;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        void Tick()
        {
            OnTick.Invoke();
            //Debug.Log("Ticking: " + name + " " + DateTime.Now);
        }
    }
}

