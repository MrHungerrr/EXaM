﻿using System;
using GameTime.Action;
using System.Collections.Generic;
using UnityEngine;
using Single;



namespace GameTime
{
    public class TimeManager : Singleton<TimeManager>
    {
        public System.Action OnSecondDone;
        public System.Action OnTimeDone;
        public int TimeInSec => _timeInSec;
        public int TimeLeftInSec => _timeGeneral - _timeInSec;


        private bool _active = false;

        private int _timeGeneral;
        private float _timePassed;
        private float _timeLeft;
        private int _timeInSec;

        public void SetLevel()
        {
            OnSecondDone = null;
            OnTimeDone = null;
            _active = false;
            _timeInSec = 0;

            ActionPerTimeManager.Instance.SetLevel();
            ActionSchedule.Instance.SetLevel();
            TimeShower.Instance.SetLevel();
        }

        public void Reset()
        {
            SetTime(_timeGeneral);
        }

        public void Update()
        {
            if (_active)
            {
                Time();
            }
        }

        public void Disable()
        {
            _active = false;
        }

        public void SetTime(int time)
        {
            _timeGeneral = time;
            _timeLeft = _timeGeneral;
            _timePassed = 0;
            _timeInSec = 0;

            SecondDone();

            _active = true;
        }

        private void Time()
        {
            if (_timeLeft > 0)
            {
                float deltaTime = UnityEngine.Time.deltaTime;
                _timePassed += deltaTime;
                _timeLeft = _timeGeneral - _timePassed;

                ActionPerTimeManager.Instance.Update(deltaTime);

                if (_timeInSec != (int)_timePassed)
                {
                    SecondDone();
                }
            }
            else
            {
                TimeDone();
            }
        }


        private void SecondDone()
        {
            Debug.Log("Second Done");
            _timeInSec = (int)_timePassed;
            OnSecondDone?.Invoke();
        }


        private void TimeDone()
        {
            _active = false;

            _timeLeft = 0;
            _timePassed = _timeGeneral;
            _timeInSec = (int)_timePassed;

            OnTimeDone?.Invoke();
        }
    }

}





