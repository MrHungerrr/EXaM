﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool open = false;
    public bool locked;
    private bool ninety;
    private bool act = false;
    private Quaternion commonRot;
    private Quaternion targetRot;
    private Transform trans;
    private Vector3 door_position;
    private const float close_distance = 1f;
    private float close_time;
    private const float close_time_cd = 3f;
    private Vector3 last_pos;
    private Transform player;
    private ScholarManager ScholarMan;
    private int scholar_nom;
    private bool scholar_open;
    private bool in_range;
    private float scholar_open_cd;
    


    void Start()
    {
        ScholarMan = GameObject.FindObjectOfType<ScholarManager>();
        player = GameObject.FindWithTag("Player").transform;
        door_position = transform.parent.parent.position;
        commonRot = transform.parent.transform.rotation;
        trans = transform.parent.transform;
        close_time = close_time_cd;
        this.tag = "Door";
        in_range = false;

        if (transform.parent.parent.rotation.eulerAngles.y == 0)
        {
            ninety = false;
        }
        else if (transform.parent.parent.rotation.eulerAngles.y == 0)
        {
            ninety = true;
        }
        else
        {
            Debug.Log("Не правильно поставлена дверь!");
        }



    }

    

    void Update()
    {
        if (act)
            DoorRot();

        ScholarOpen();
    }

    public void DoorInteract(Vector3 pos)
    {
        if (!locked)
        {
            open = !open;

            if (open)
            {
                if (ninety)
                {
                    if (trans.position.z > pos.z)
                    {
                        targetRot = Quaternion.Euler(commonRot.eulerAngles.x, commonRot.eulerAngles.y + 90, commonRot.eulerAngles.z);
                    }
                    else
                    {
                        targetRot = Quaternion.Euler(commonRot.eulerAngles.x, commonRot.eulerAngles.y - 90, commonRot.eulerAngles.z);
                    }
                }
                else
                {
                    if (trans.position.x > pos.x)
                    {
                        targetRot = Quaternion.Euler(commonRot.eulerAngles.x, commonRot.eulerAngles.y + 90, commonRot.eulerAngles.z);
                    }
                    else
                    {
                        targetRot = Quaternion.Euler(commonRot.eulerAngles.x, commonRot.eulerAngles.y - 90, commonRot.eulerAngles.z);
                    }
                }

                ScholarMan.SpecialHear(door_position);
            }
            else
            {
                targetRot = commonRot;
            }

            last_pos = pos;
            act = true;
        }
    }



    private void DoorRot()
    {
       trans.rotation = Quaternion.Lerp(trans.rotation, targetRot, 6 * Time.deltaTime);

        if (Mathf.Abs(trans.rotation.eulerAngles.y - targetRot.eulerAngles.y) < 0.1f)
        {
            if(open)
            {
                if(close_time>0)
                {
                    close_time -= Time.deltaTime;
                }
                else if(Vector3.Distance(door_position, player.position) > close_distance)
                {
                    close_time = close_time_cd;
                    DoorInteract(last_pos);
                }
            }
            else
            {
                act = false;
            }
        }
    }



    private void ScholarOpen()
    {
        int i;
        in_range = false;

        for(i = 0; i < ScholarMan.scholars.Length; i++)
        {
            if(Vector3.Distance(door_position, ScholarMan.scholars[i].transform.position) < 0.4f)
            {
                in_range = true;
                break;
            }
        }

        if (in_range && !open && !scholar_open)
        {
            DoorInteract(ScholarMan.scholars[i].Action.transform.position);
            scholar_open = true;
        }
        else if (!in_range && open && scholar_open)
        {
            DoorInteract(last_pos);
            scholar_open = false;
        }
    }
}