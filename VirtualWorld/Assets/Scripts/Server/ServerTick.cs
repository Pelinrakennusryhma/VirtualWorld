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
                    int currentHour = DateTime.Now.Hour;
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
                    int currentMinute = DateTime.Now.Minute;
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
                    if(currentSecond % interval == 0)
                    {
                        if(prevTicked != currentSecond)
                        {
                            Tick();
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
            Debug.Log("Ticking: " + name + " " + DateTime.Now);
        }
    }
}

