﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vkimow.Tools.Single;
using Exam;
using AI.Scholars.Actions;

namespace AI.Scholars
{
    public class ScholarManager : Singleton<ScholarManager>
    {
        public Scholar[] Scholars => _scholars;


        private Scholar[] _scholars;



        public void SetLevel()
        {
            ExamManager.Instance.OnExamStart += StartExam;
            ExamManager.Instance.OnExamEnd += EndExam;

            _scholars = GameObject.FindObjectsOfType<Scholar>();
        }


        public void Update()
        {
            for (int i = 0; i < _scholars.Length; i++)
            {
                _scholars[i].MyUpdate();
            }
        }

        public void FixUpdate()
        {
            for(int i = 0; i < _scholars.Length; i++)
            {
                _scholars[i].FixUpdate();
            }
        }




        public Scholar GetRandomScholar()
        {
            return _scholars[GetRandomScholarIndex()];
        }

        private int GetRandomScholarIndex()
        {
            return Random.Range(0, _scholars.Length);
        }

        private void StartExam()
        {
            for (int i = 0; i < _scholars.Length; i++)
            {
                _scholars[i].Actions.Updater.Enable(true);
                var action = ScholarActionsTemplates.StartExam(_scholars[i]);
                _scholars[i].Actions.Execute(action);
            }
        }

        private void EndExam()
        {
            for (int i = 0; i < _scholars.Length; i++)
            {
                _scholars[i].Actions.Reset();
                var action = ScholarActionsTemplates.EndExam(_scholars[i]);
                _scholars[i].Actions.Execute(action);
            }
        }
    }
}