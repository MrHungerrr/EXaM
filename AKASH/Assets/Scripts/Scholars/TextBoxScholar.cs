﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxScholar : MonoBehaviour
{

    private TextMeshPro[] textBox;
    private ScriptManager ScriptMan;
    private bool saying = false;
    private bool question = false;
    private bool act = false;
    private bool filled = false;
    private float timeClear_N;
    private float timeClear = 0;
    private float timeUnselectable_N;
    private float timeUnselectable = 0;


    private void Awake()
    {
        ScriptMan = GameObject.FindObjectOfType<ScriptManager>();
        textBox = transform.GetComponentsInChildren<TextMeshPro>();

    }

    private void Update()
    {
        if(filled && !question)
        {
            if(timeClear>=timeClear_N)
            {
                Clear();
            }
            else if (timeUnselectable >= timeUnselectable_N)
            {
                timeClear += Time.deltaTime;
            }
            else
            {
                timeUnselectable += Time.deltaTime;
            }
        }

        if (act)
        {
            if (saying)
            {
                //Звук говорения
            }
            else
            {
                //Минус звук говорения
            }
        }
    }

    public void Say(string key)
    {
        Clear();
        StartCoroutine(PlaySub(key));
        timeClear_N = 1f;
        question = false;
    }

    public void Say(string key, float t)
    {
        Clear();
        StartCoroutine(PlaySub(key));
        question = false;
        timeClear_N = t;
    }

    public void Question(string key)
    {
        Clear();
        StartCoroutine(PlaySub(key));
        question = true;
    }

    public void Clear()
    {
        if (act)
            StopAllCoroutines();
        Text("");
        act = false;
        filled = false;
        saying = false;
        timeClear = 0;
    }



    private IEnumerator PlaySub(string key)
    {
        act = true;
        var script = ScriptMan.GetText(key);
        if (script != null)
        {
            foreach (var line in script)
            {

                saying = true;
                int quant = line.Length;
                for (int i = 0; i < quant; i++)
                {

                    TextPlus(line[i]);
                    yield return new WaitForSeconds(0.02f);
                }
                saying = false;
                TextPlus(' ');
                yield return new WaitForSeconds(1f);
            }
        }
        filled = true;
        act = false;
        yield break;
    }

    public bool IsTalking()
    {
        if(question)
            return (act);
        else
            return (act || filled);
    }

    private void Text(string text)
    {
            textBox[0].text = text;
    }

    private void TextPlus(char symbol)
    {
            textBox[0].text += symbol;
    }

    public void Number(int num)
    {
        textBox[1].text = (num+1).ToString();
    }

}
