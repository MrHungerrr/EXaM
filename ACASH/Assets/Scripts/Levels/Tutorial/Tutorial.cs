﻿using System.Collections;
using PlayerOptions;
using UnityEngine;
using Single;

public class Tutorial : Singleton<Tutorial>
{
    public TutorialPlayerWatcher Watcher;
    private TutorialScholarCheatSet Cheat;
    private TutorialKeyHint Hint;

    private KeyWord key = new KeyWord("Tutorial");
    private KeyWord key_mistake = new KeyWord("Tutorial", "Mistake");



    [Header("Begining")]
    public GameObject phone;


    [Header("First Room")]
    public TriggerAction first_room;
    public Scholar first_room_scholar;
    public Scholar[] first_room_other_scholars = new Scholar[2];
    public StressScreen[] first_room_text_screens = new StressScreen[3];

    public Door first_room_door_enter;
    public Door first_room_door_exit;


    [Header("Second Room")]
    public TriggerAction second_room;
    public Scholar[] second_room_scholars = new Scholar[3];
    public Door second_room_door_enter;
    public TeacherComputer computer;







    private void Awake()
    {
        first_room.OnEnter += StartFirstRoom;
        second_room.OnEnter += StartSecondRoom;
        phone.GetComponent<InteractAction>().OnInteraction += StartElevator;

        Watcher = new TutorialPlayerWatcher();
        Cheat = new TutorialScholarCheatSet();
        Hint = GetComponent<TutorialKeyHint>();
        Hint.Begin();

        StartCoroutine(StartLevel());


        InputManager.get.Controls.Gameplay.HUD.Disable();
    }


    private IEnumerator StartLevel()
    {
        while (LevelManager.get.IsLoad())
            yield return new WaitForEndOfFrame();



        Transform point = GameObject.FindGameObjectWithTag("PlayerPoint").transform;
        Player.get.Move.Position(point.position);
        Destroy(point.gameObject);

        GameManager.get.SetLevel();

        FadeHUDController.get.FastFade(true);
        FadeController.get.FastFade(false);

        yield return new WaitForSeconds(1f);

        key *= "Introdaction_Home";

        HUDManager.get.IntrodactionHUD(key);

        yield return new WaitForSeconds(2f);

        phone.GetComponent<ObjectSound>().Play();

        HUDManager.get.CloseIntrodactionHUD();

        yield return new WaitForSeconds(1f);



        GameManager.get.StartGame();


    }


    //===================================================================================================================================
    // Первая комната
    //===================================================================================================================================
    private void StartElevator()
    {
        phone.GetComponent<InteractAction>().Remove();
        phone.GetComponent<ObjectSound>().Stop();

        FadeHUDController.get.FastFade(true);
        InputManager.get.SwitchGameInput("cutscene");

        StartCoroutine(ElevatorRoom());
    }


    private IEnumerator ElevatorRoom()
    {
        Player.get.Move.Position(Elevator.get.position);

        key *= "Begining";

        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        yield return new WaitForSeconds(1f);

        key *= "Introdaction_Training";

        HUDManager.get.IntrodactionHUD(key);

        yield return new WaitForSeconds(3f);

        HUDManager.get.CloseIntrodactionHUD();

        yield return new WaitForSeconds(1f);

        GameManager.get.StartGame();

        key *= "Elevator";

        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();



        ElevatorController.get.Ready();
        ElevatorController.get.Open();

        while (!Elevator.get.open)
            yield return new WaitForEndOfFrame();

        Hint.Zoom();
    }





    //===================================================================================================================================
    // Первая комната
    //===================================================================================================================================

    private void StartFirstRoom()
    {

        StartCoroutine(First_Room());
        first_room.Remove();
    }

    private IEnumerator First_Room()
    {
        first_room_scholar.ResetType();
        
        key *= "First_Room";
        key_mistake *= "First_Room";
        key += 0;

        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Hint.Set(GetP.actions.Talk_Good);

        Player.get.Talk.talk_good_control = true;


        while(!Watcher.talk_good)
        {
            yield return new WaitForEndOfFrame();
        }

        Hint.Disable();

        Watcher.Reset();

        key += 1;
        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Hint.Set(GetP.actions.Talk_Bad);

        Player.get.Talk.talk_bad_control = true;

        bool option = true;

        while (option)
        {
            if(Watcher.done)
            {
                if(Watcher.talk_bad)
                {
                    option = false;
                    Watcher.Reset();
                }
                else
                {
                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    key_mistake += 0;
                    SubtitleManager.get.Say(key_mistake);
                    Watcher.Reset();
                }
            }

            yield return new WaitForEndOfFrame();
        }

        Hint.Disable();

        key += 2;
        SubtitleManager.get.Say(key);

        yield return new WaitForSeconds(1f);

        foreach (Scholar s in first_room_other_scholars)
        {
            s.ResetType();
        }

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Hint.Set(GetP.actions.Shout);

        Player.get.Talk.shout_control = true;

        option = true;

        while (option)
        {
            if (Watcher.done)
            {
                if (Watcher.shout)
                {
                    option = false;
                    Watcher.Reset();
                }
                else
                {
                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    key_mistake += 1;
                    SubtitleManager.get.Say(key_mistake);
                    Watcher.Reset();
                }
            }

            yield return new WaitForEndOfFrame();
        }

        Hint.Disable();

        key += 3;
        SubtitleManager.get.Say(key);

        first_room_door_exit.locked = false;
    }


    private IEnumerator Second_Corridor()
    {
        yield return new WaitForEndOfFrame();
    }






    //===================================================================================================================================
    // Вторая комната
    //===================================================================================================================================

    private void StartSecondRoom()
    {
        StartCoroutine(Second_Room_Part_1());
        second_room.Remove();
        second_room_door_enter.Close();
        second_room_door_enter.locked = true;
    }


    private IEnumerator Second_Room_Part_1()
    {
        key *= "Second_Room";
        key_mistake *= "Second_Room";
        key += 0;

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        SubtitleManager.get.Say(key);

        second_room_scholars[1].ResetType();

        second_room_scholars[1].Action.Reset("Login");

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Hint.Set(GetP.actions.Execute);
        Player.get.Talk.execute_control = true;



        bool option = true;

        while (option)
        {
            if(second_room_scholars[1].Execute.executed)
            {
                Hint.Disable();
            }

            if (Watcher.done)
            {
                Debug.Log(Watcher.execute);

                if (Watcher.execute)
                {
                    option = false;
                }
                else
                {
                    while (SubtitleManager.get.act)
                       yield return new WaitForEndOfFrame();

                    key_mistake += 0;
                    SubtitleManager.get.Say(key_mistake);
                }
                Watcher.Reset();
            }

            yield return new WaitForEndOfFrame();
        }



        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 1;
        SubtitleManager.get.Say(key);


        while (InputManager.get.gameType != "computer")
        {
            yield return new WaitForEndOfFrame();
        }


        if (computer.Windows.current_window == "Rules")
        {
            key += 4;
            SubtitleManager.get.Say(key);

            Debug.LogWarning("Rules!");
        }
        else
        {
            key += 2;
            SubtitleManager.get.Say(key);

            while (computer.Windows.current_window != "Desktop")
            {
                yield return new WaitForEndOfFrame();
            }


            key += 3;
            SubtitleManager.get.Say(key);

            while (computer.Windows.current_window != "Rules")
            {
                yield return new WaitForEndOfFrame();
            }

            key += 4;
            SubtitleManager.get.Say(key);

            Debug.LogWarning("Rules!");
        }


        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        Cheat.RandomScholarsCheatSet(second_room_scholars, TutorialScholarCheatSet.calculate);
        Watcher.Reset();

        option = true;
        int option_num = 0;

        while (option)
        {
            if (Watcher.done)
            {
                if (Watcher.execute)
                {
                    if (Player.get.Talk.scholar.Cheat.cheating)
                    {
                        option = false;
                    }
                    else
                    {
                        while (SubtitleManager.get.act)
                            yield return new WaitForEndOfFrame();

                        key_mistake += 2;
                        SubtitleManager.get.Say(key_mistake);
                        option_num++;

                        if(option_num == 3)
                        {
                            foreach (Scholar s in second_room_scholars)
                            {
                                s.Execute.EndExamForScholar();
                            }

                            while (SubtitleManager.get.act)
                                yield return new WaitForEndOfFrame();

                            key_mistake += 3;
                            SubtitleManager.get.Say(key_mistake);

                            while (SubtitleManager.get.act)
                                yield return new WaitForEndOfFrame();

                            Cheat.RandomScholarsCheatSet(second_room_scholars, TutorialScholarCheatSet.calculate);
                            option_num = 0;
                        }
                    }
                }
                else
                {
                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    key_mistake += 1;
                    SubtitleManager.get.Say(key_mistake);
                }
                Watcher.Reset();
            }
            yield return new WaitForEndOfFrame();
        }

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 5;
        SubtitleManager.get.Say(key);


        foreach (Scholar s in second_room_scholars)
        {
            s.Execute.EndExamForScholar();
        }

        yield return new WaitForSeconds(5f);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        Cheat.RandomScholarsCheatSet(second_room_scholars, TutorialScholarCheatSet.note);
        Watcher.Reset();

        option = true;

        while (option)
        {
            if (Watcher.done)
            {
                if (Watcher.execute)
                {
                    if (Player.get.Talk.scholar.Cheat.cheating)
                    {
                        option = false;
                    }
                    else
                    {
                        while (SubtitleManager.get.act)
                            yield return new WaitForEndOfFrame();

                        key_mistake += 4;
                        SubtitleManager.get.Say(key_mistake);
                        option_num++;

                        if (option_num == 3)
                        {
                            foreach (Scholar s in second_room_scholars)
                            {
                                s.Execute.EndExamForScholar();
                            }

                            while (SubtitleManager.get.act)
                                yield return new WaitForEndOfFrame();

                            key_mistake += 5;
                            SubtitleManager.get.Say(key_mistake);

                            while (SubtitleManager.get.act)
                                yield return new WaitForEndOfFrame();

                            Cheat.RandomScholarsCheatSet(second_room_scholars, TutorialScholarCheatSet.calculate);
                            option_num = 0;
                        }
                    }
                }
                else
                {
                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    key_mistake += 1;
                    SubtitleManager.get.Say(key_mistake);
                }
                Watcher.Reset();
            }
            yield return new WaitForEndOfFrame();
        }



        StartCoroutine(Second_Room_Part_2());
    }








    private IEnumerator Second_Room_Part_2()
    {
        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 6;
        SubtitleManager.get.Say(key);

        foreach (Scholar s in second_room_scholars)
        {
            s.Execute.EndExamForScholar();
        }

        Player.get.Talk.execute_control = false;

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        Cheat.RandomScholarsCheatSet(second_room_scholars);
        Watcher.Reset();

        int remarks = 0;

        while (remarks < 3)
        {
            if (Watcher.talk)
            {
                while (SubtitleManager.get.act)
                    yield return new WaitForEndOfFrame();

                if (Player.get.Talk.scholar.Cheat.cheating)
                {
                    //Сказать реплику, что правильный ученик
                    
                    key_mistake += (7 + remarks);
                    SubtitleManager.get.Say(key_mistake);
                    remarks++;
                    Watcher.Reset();
                }
                else
                {
                    key_mistake += 6;
                    SubtitleManager.get.Say(key_mistake);
                    Watcher.Reset();
                }
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Second_Room_Part_3());
    }






    private IEnumerator Second_Room_Part_3()
    {
        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 7;
        SubtitleManager.get.Say(key);

        foreach (Scholar s in second_room_scholars)
        {
            s.Execute.EndExamForScholar();
        }



        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Player.get.Talk.answer_no_control = true;

        Hint.Set(GetP.actions.Answer_No);


        Watcher.Reset();
        second_room_scholars[1].ResetType();
        second_room_scholars[1].Action.DoAction("Cheating_Toilet_3");



        bool option = true;

        while (option)
        {
            if (Watcher.answer_no)
            {
                option = false;
            }

            if (!second_room_scholars[1].Question.question && !second_room_scholars[1].Question.question_answered && second_room_scholars[1].Action.Operations.done)
            {
                key_mistake += 10;
                SubtitleManager.get.Say(key_mistake);

                while (SubtitleManager.get.act)
                    yield return new WaitForEndOfFrame();

                second_room_scholars[1].Action.DoAction("Cheating_Toilet_3");
            }

            yield return new WaitForEndOfFrame();
        }

        Hint.Disable();

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 8;
        SubtitleManager.get.Say(key);


        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Player.get.Talk.answer_yes_control = true;

        Watcher.Reset();
        second_room_scholars[1].Action.DoAction("Cheating_Toilet_3");


        option = true;

        while (option)
        {
            if (Watcher.answer_yes)
            {
                option = false;
            }

            if (!second_room_scholars[1].Question.question && second_room_scholars[1].Action.Operations.done)
            {
                if (!second_room_scholars[1].Question.question_answered)
                {
                    key_mistake += 10;
                    SubtitleManager.get.Say(key_mistake);

                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    second_room_scholars[1].Action.DoAction("Cheating_Toilet_3");
                }
                else if (!second_room_scholars[1].Question.answer)
                {
                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    key_mistake += 11;
                    SubtitleManager.get.Say(key_mistake);

                    while (SubtitleManager.get.act)
                        yield return new WaitForEndOfFrame();

                    second_room_scholars[1].Action.DoAction("Cheating_Toilet_3");
                }
            }

            yield return new WaitForEndOfFrame();
        }

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 9;
        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Player.get.Talk.execute_control = true;


        while (!Watcher.execute)
        {
            yield return new WaitForEndOfFrame();
        }

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        key += 11;
        SubtitleManager.get.Say(key);

        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();


        GameManager.get.MainMenu();

        Debug.Log("END OF LEVEL");

        yield return new WaitForEndOfFrame();
    }



    private IEnumerator EndLevel()
    {
        while (SubtitleManager.get.act)
            yield return new WaitForEndOfFrame();

        Elevator.get.Open();
    }
}