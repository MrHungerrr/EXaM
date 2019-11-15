﻿using UnityEngine;
using System.Collections;
using N_BH;


public class Menu : Singleton<Menu>
{
    private bool state;

    private const float dof_coef = 0.25f;

    private MenuSection[] menu_sections;
    private int count_row;
    //private int count_column;
    private int select_row;
    //private int select_column;
    private string select_name;
    [HideInInspector]
    public string current_menu;
    [HideInInspector]
    public bool settings;

    [HideInInspector]
    public Vector2 moveInput;
    private Vector2 move;
    [HideInInspector]
    public int move_cd;
    private const int move_const_keyboard_cd = 12;
    private const int move_const_gamepad_cd = 12;


    void Awake()
    {
        MenuEnable(false);
    }

    private void Update()
    {
        MoveVertical();
        if (settings)
            MoveHorizontal();
    }




    public void MenuEnable(bool u)
    {
        if (u)
        {
            StartCoroutine(MenuOn());
        }
        else
        {
            StartCoroutine(MenuOff());
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 0;

        state = true;
        MenuAgent.get.Set("Main Menu");

        InputType();
    }




    public void Select(int row)
    {
        if (select_name != null)
            menu_sections[select_row].Select(false);

        select_name = menu_sections[row].name;
        menu_sections[row].Select(true);
        select_row = row;

        settings = menu_sections[row].settings;

        //Debug.Log("Выбранное меню: " + select_name);
    }




    public void Enter()
    {
        MenuAgent.get.Enter(select_name);
    }



    public void Escape()
    {
        MenuAgent.get.Escape(current_menu);
    }



    public void Set(MenuSection[] sections, string name)
    {
        current_menu = name;
        count_row = sections.Length;
        menu_sections = new MenuSection[count_row];

        //Debug.Log("Название меню: " + name + ". Количество элементов = " + count_row);

        for (int i = 0; i < count_row; i++)
        {
            menu_sections[i] = sections[i];
            menu_sections[i].menu_number = i;
            menu_sections[i].Select(false);
        }

        select_name = null;
        Select(0);
    }




    private void MoveVertical()
    {
        if(Mathf.Abs(moveInput.y)>0.75)
        {
            if (move_cd <= 0)
            {

                if (moveInput.y > 0)
                {
                    int buf = select_row - 1;

                    if (buf < 0)
                        buf = count_row - 1;


                    Select(buf);
                }
                else
                {
                    int buf = select_row + 1;

                    if (buf >= count_row)
                        buf = 0;

                    Select(buf);
                }

                MoveCD();

            }
        }
        else if (moveInput == Vector2.zero)
        {
            move_cd = 0;
        }

        if(move_cd > 0)
        {
            move_cd--;
        }
    }




    private void MoveHorizontal()
    {
        if (Mathf.Abs(moveInput.x) > 0.75)
        {
            if (move_cd <= 0)
            {

                if (moveInput.x > 0)
                {
                    SettingsManager.get.SwtichSettings(true);
                }
                else
                {
                    SettingsManager.get.SwtichSettings(false);
                }


                MoveCD();
            }
        }
    }

    public void MoveCD()
    {
        switch (InputManager.get.inputType)
        {
            case "keyboard":
                {
                    move_cd = move_const_keyboard_cd;
                    break;
                }
            default:
                {
                    move_cd = move_const_gamepad_cd;
                    break;
                }
        }
    }



    private IEnumerator MenuOn()
    {
        Time.timeScale = 0;

        state = true;
        float dof = PostProcessManager.get.GetDOF();
        MenuAgent.get.Set("Pause");

        InputType();

        while (dof > 0.1)
        {
            dof -= (dof / 2) * dof_coef;
            PostProcessManager.get.DOF(dof);
            yield return new WaitForEndOfFrame();
        }

    }




    private IEnumerator MenuOff()
    {
        Time.timeScale = 1;

        state = false;
        float dof = PostProcessManager.get.GetDOF();
        Cursor.lockState = CursorLockMode.Locked;

        if (current_menu != null)
        {
            MenuAgent.get.Disable(current_menu);
            Debug.Log("Disabled - " + current_menu);
        }

        while (dof < 32)
        {
            dof += ((33 - dof) / 2) * dof_coef;
            PostProcessManager.get.DOF(dof);
            yield return new WaitForEndOfFrame();
        }

    }




    public void InputType()
    {
        if (state)
        {
            switch (InputManager.get.inputType)
            {
                case "playstation":
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                    }
                case "xbox":
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                    }
                default:
                    {
                        Cursor.lockState = CursorLockMode.None;
                        break;
                    }

            }
        }
    }

}