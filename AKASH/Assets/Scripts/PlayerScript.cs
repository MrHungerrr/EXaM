﻿using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Управление
    private string horizontalInputName;
    private string verticalInputName;
    [SerializeField]
    private float movementSpeed = 6;
    private double rnd;
    private CharacterController CharController;
    private LayerMask actLayerMask;
    private InputManager InputMan;
    private CameraController CamControl;
    private bool disPlayer;

    //Действия
    [HideInInspector]
    public bool doing;
    private bool think;
    [HideInInspector]
    public bool asked;
    [HideInInspector]
    public bool act;
    [HideInInspector]
    public bool actReady;
    private float actRange = 4f;
    [HideInInspector]
    public string actTag;
    private GameObject actObject;
    private string actText;
    [HideInInspector]
    public GameObject playerCam;
    private SubtitleManager SubMan;
    private ScriptManager ScriptMan;
    private ScholarManager ScholarMan;
    private string keyWord = "Teacher_";
    private string key;



    private void Awake()
    {
        playerCam = GameObject.FindWithTag("PlayerCamera");
        horizontalInputName = "Horizontal";
        verticalInputName = "Vertical";
        InputMan = GetComponent<InputManager>();
        CharController = GetComponent<CharacterController>();
        CamControl = playerCam.GetComponent<CameraController>();
        SubMan = GameObject.FindObjectOfType<SubtitleManager>();
        ScriptMan = GameObject.FindObjectOfType<ScriptManager>();
        ScholarMan = GameObject.FindObjectOfType<ScholarManager>();
    }

    private void Start()
    {
        actLayerMask = LayerMask.GetMask("Selectable");
    }

    private void Update()
    {
        PlayerMovement();
        if(!act)
            Watching();
        Action();
    }



    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        CharController.SimpleMove(forwardMovement + rightMovement);
    }



    private void Watching()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, actRange, actLayerMask))
        {
            if (actObject != hit.collider.gameObject)
            {
                if (actObject != null)
                {
                    actObject.GetComponent<ObjectSelect>().Deselect();
                }
                actObject = hit.collider.gameObject;
                actTag = hit.collider.tag;
                actReady = true;
                actObject.GetComponent<ObjectSelect>().Select();
                
                WhatISee();
            }
        }
        else if (actReady && actObject != null)
        {
            actObject.GetComponent<ObjectSelect>().Deselect();
            actObject = null;
            actReady = false;
        }
    }



    private void WhatISee()
    {
        switch (actTag)
        {
            case "Scholar":
                {
                    if (actObject.GetComponent<Scholar>().question)
                    {
                        asked = true;
                    }
                    else
                    {
                        asked = false;
                    }
                    break;
                }
        }
    }




    private void Action()
    {
        if (doing && actReady && !act)
        {
            Debug.Log("Мы хотим что-то сделать");

            switch (actTag)
            {
                case "Computer":
                    {
                        //actObject.GetComponent<Computer>().Enable();
                        break;
                    }
                case "HandCamera":
                    {

                        break;
                    }
            }
        }
    }




    public void Shout()
    {
        StartCoroutine(Shouting());
    }

    private IEnumerator Shouting()
    {
        StopThinking();
        ScholarMan.Stress(10);
        act = true;
        key = "Shout_";
        int nomber = Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        key += nomber;
        SubMan.Say(keyWord + key);
        yield return new WaitForSeconds(1f);
        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        act = false;
    }



    public void Answer(bool answer)
    {
        StopThinking();
        act = true;

        key = "Answer_";

        var scholar = actObject.GetComponent<Scholar>();

        if (scholar.asking)
            StartCoroutine(Permission(scholar, answer));
        else
            StartCoroutine(Answering(scholar, answer));
    }

    private IEnumerator Permission(Scholar scholar, bool answer)
    {
        key += "Permission_";

        if (answer)
            key += "Yes_";
        else
            key += "No_";

        int nomber = Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        key += nomber;
        SubMan.Say(keyWord + key);

        yield return new WaitForSeconds(0.5f);

        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        scholar.TeacherPermission(answer);
        act = false;
    }

    private IEnumerator Answering(Scholar scholar, bool answer)
    {
        key += scholar.quest;

        int nomber = Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        key += nomber;
        SubMan.Say(keyWord + key);

        yield return new WaitForSeconds(1f);

        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        scholar.TeacherAnswer(answer);
        act = false;
    }

    public void Bull(bool strong)
    {
        StopThinking();
        act = true;

        if (strong)
            key = "Bull_";
        else
            key = "Joke_";

        var scholar = actObject.GetComponent<Scholar>();

        if (!scholar.executed)
            StartCoroutine(BullingForAction(scholar, strong));

    }



    //--------------------------------------------------------------------------------------------------------
    //Осторожно, разновидности одного и того же кода. Отличаются лишь скриптами, к которым обращаются.
    //--------------------------------------------------------------------------------------------------------



    //Наезд на мудака



    //Наезд на тупицу

    private IEnumerator BullingForAction(Scholar scholar, bool strong)
    {
        key += scholar.view;

        if (scholar.remarks[scholar.view])
        {
            if (Probability(0.5))
                key += "Sec_";
        }
        else
            scholar.remarks[scholar.view] = true;

        int nomber = Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        key += nomber;
        SubMan.Say(keyWord + key);

        yield return new WaitForSeconds(1f);

        scholar.HearBulling(strong);

        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        scholar.Bulling(key, strong);
        act = false;

        while (scholar.TextBox.IsTalking() && !act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Добавить вероятность + взгляд
        if (!act && Probability(0.1))
            SubMan.Say(keyWord + "Thinking_" + scholar.tag + "_" + Random.Range(0, ScriptMan.linesQuantity[keyWord + "Thinking_"]));
    }



    //--------------------------------------------------------------------------------------------------------
    //Конец разновидностей одного и того же кода.
    //--------------------------------------------------------------------------------------------------------


    public void Execute()
    {
        StopThinking();
        act = true;
        string goalTag = actTag;
        GameObject goalObject = actObject;

        key = "Execute_";


        switch (goalTag)
        {
            case "Scholar":
                {
                    var scholar = goalObject.GetComponent<Scholar>();
                    if (!scholar.executed)
                        StartCoroutine(Execute(scholar));
                    break;
                }
            case "ScholarsSubject":
                {
                    var subject = goalObject.GetComponent<ScholarSubject>();
                    Debug.Log("Fuck");
                    StartCoroutine(Execute(subject));
                    break;
                }
            case "Subject":
                {
                    var subject = goalObject.GetComponent<Subject>();
                    StartCoroutine(Execute(subject));
                    break;
                }
            default:
                {
                    act = false;
                    break;
                }
        }
    }




    private IEnumerator Execute(Scholar scholar)
    {

        int nomber = Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        key += nomber;

        SubMan.Say(keyWord + key);

        yield return new WaitForSeconds(1f);

        scholar.HearBulling(true);

        yield return new WaitForSeconds(1f);

        scholar.Execute(key);

        while (!scholar.executed && !act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        act = false;

        SubMan.Say(keyWord + "Thinking_" + key);
    }


    private IEnumerator Execute(ScholarSubject subject)
    {
        key += subject.name + "_";
        key += Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        SubMan.Say(keyWord + key);
        subject.Execute(key);

        yield return new WaitForSeconds(1f);

        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        act = false;
    }

    private IEnumerator Execute(Subject subject)
    {
        key += subject.name + "_";
        key += Random.Range(0, ScriptMan.linesQuantity[keyWord + key]);
        SubMan.Say(keyWord + key);
        subject.Execute();

        yield return new WaitForSeconds(1f);

        while (SubMan.act)
        {
            yield return new WaitForSeconds(0.1f);
        }

        act = false;
    }


    private void StopThinking()
    {
        if(SubMan.act)
            SubMan.StopSubtitile();
    }

    public void DisableControl(bool status)
    {
        InputMan.disPlayer = status;
        playerCam.SetActive(!status);
    }



    //Вероятность

    public bool Probability(double a)
    {
        rnd = Random.value;

        if (a >= rnd)
            return true;
        else
            return false;
    }
}
