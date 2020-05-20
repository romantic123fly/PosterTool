#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             SetFestivalView
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetFestivalView : MonoBehaviour
{
    public  Dropdown dp;
    public GameObject MainView;
    public GameObject Select;
    public InputField index;
    public InputField fname;
    public InputField opcName;
    public Text tips;

    // Start is called before the first frame update
    void Start()
    {
        index.text = "0";
        dp.onValueChanged.AddListener((a)=> {
            fname.text = dp.options[a].text;
        });
    }

    public void SetType(int a)
    {
        PostersManager.GetInstance().type = a;
        Select.SetActive(false);
    }

   public  void StartMake()
    {
        if (PostersManager.GetInstance().type== 1 && opcName.text =="")
        {
            tips.text = "请输入指定门诊";
            return;
        }
        PostersManager.GetInstance().theCurrentOpcName = opcName.text;
        PostersManager.GetInstance().theCurrentFestival = fname.text;
        PostersManager.GetInstance().CreateInfoPath();
        gameObject.SetActive(false);
        MainView.SetActive(true);
        PostersManager.GetInstance().index = int.Parse(index.text);
    }
}
