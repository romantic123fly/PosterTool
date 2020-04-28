#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             PostersManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PostersManager : MonoBehaviour
{
    private static PostersManager instance;
    public static PostersManager GetInstance(){
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public string theCurrentFestival ="营销海报";
    private Color textColor = Color.black;
    private int textSize = 80;
    public List<OutpatientInfo> outpatientInfoList = new List<OutpatientInfo> ();
    public Dictionary<string, Dictionary<string, string>> cfgData;
    public PosterTools mainBg;
    public Color TextColor
    {
        get => textColor;
        set
        {
            textColor = value;
            mainBg.GetComponent<PosterTools>().address.color = textColor;
            mainBg.GetComponent<PosterTools>().phoneNum.color = textColor;
        }
    }

    public int TextSize {
        get => textSize;
        set
        {
            textSize = value;
            mainBg.GetComponent<PosterTools>().address.fontSize = textSize;
            mainBg.GetComponent<PosterTools>().phoneNum.fontSize = textSize;
        }

    }
    public int index =1;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(540, 960, false);
        Debug.Log(Screen.width + "/" + Screen.height);
        string cfgPath = Application.streamingAssetsPath + "/门诊信息.txt";
        if (File.Exists(cfgPath))
        {
            StartCoroutine(LoadCfg(cfgPath));
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (var item in outpatientInfoList)
            {
              string path =  Application.streamingAssetsPath + "/门诊信息/" + item.name + "/海报";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path,true);
                    Debug.Log("清除海报");
                }
            }
        }
    }
    public IEnumerator LoadCfg(string cfgPath)
    {
        WWW www = new WWW(cfgPath);
        yield return www;
        if (www.isDone)
        {
            cfgData = ExplainString(www.text);
        }
        //门诊列表赋值
        for (int i = 0; i < cfgData["Name"].Values.Count; i++)
        {
            OutpatientInfo opinfo = new OutpatientInfo();
            opinfo.id = i + "";
            opinfo.name = cfgData["Name"][i + ""];
  
            opinfo.phoneNum = cfgData["PhoneNum"][i + ""] == ""?"": cfgData["PhoneNum"][i + ""];
            opinfo.address = cfgData["Address"][i + ""];
            opinfo.content = cfgData["Content"][i + ""] == "" ? "" : cfgData["Content"][i + ""];

            var size = cfgData["LogoSize"][i + ""].Replace('"',' ').Split(',');
            opinfo.logoSize = new Vector2(float.Parse(size[0]),float.Parse(size[1]));
            var pos = cfgData["LogoPos"][i + ""].Replace('"', ' ').Split(',');
            opinfo.logoPos = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
            outpatientInfoList.Add(opinfo);
        }
    }

    public void CreateInfoPath()
    {
        string infoPath = Application.streamingAssetsPath + "/门诊信息";

        //生成门诊存储路径
        foreach (var item in outpatientInfoList)
        {
            string namePath = infoPath + "/" + item.name;
            if (!Directory.Exists(namePath))
            {
                Debug.Log("新加门诊文件路径："+ namePath);
                Directory.CreateDirectory(namePath);
            }
            if (!Directory.Exists(namePath + "/Logo"))
            {
                Directory.CreateDirectory(namePath + "/Logo");
            }
            if (!Directory.Exists(namePath + "/二维码"))
            {
                Directory.CreateDirectory(namePath + "/二维码");
            }
            if (!Directory.Exists(namePath + "/海报" + "/" + theCurrentFestival))
            {
                Directory.CreateDirectory(namePath + "/海报" + "/" + theCurrentFestival);
            }
        }
    }

    public string GetPosterPath(string outpatientName)
    {
        return Application.streamingAssetsPath + "/门诊信息/" + outpatientName  + "/海报" + "/" + theCurrentFestival;
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainString(string strLine)
    {
        Dictionary<string, string[]> content = new Dictionary<string, string[]>();
        string[] lineArray = strLine.Replace("\r\n", "*").Split(new char[] { '*' });
        //获取行数
        int rows = lineArray.Length - 1;
        //获取列数
        int Columns = lineArray[0].Split(new char[] { '\t' }).Length;
        //定义一个数组用于存放字段名
        string[] ColumnName = new string[Columns];
        for (int i = 0; i < rows; i++)
        {
            string[] Array = lineArray[i].Split(new char[] { '\t' });
            for (int j = 0; j < Columns; j++)
            {
                //获取Array的列的值
                string nvalue = Array[j].Trim();
                if (i == 0)
                {
                    //存储字段名
                    ColumnName[j] = nvalue;
                    content[ColumnName[j]] = new string[rows - 1];
                }
                else
                {
                    //存储对应字段的默认值//<字段名，默认值>
                    content[ColumnName[j]][i - 1] = nvalue;
                }
            }
        }
        //开始解析
        return ExplainDictionary(content);
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainDictionary(Dictionary<string, string[]> content)
    {
        Dictionary<string, Dictionary<string, string>> DicContent = new Dictionary<string, Dictionary<string, string>>();
        //获取字典中所有的键(字段名)
        Dictionary<string, string[]>.KeyCollection Allkeys = content.Keys;
        //遍历所有的字段名
        foreach (string key in Allkeys)
        {
            //实例化一个hasData的字典//<ID,字段值>
            Dictionary<string, string> hasData = new Dictionary<string, string>();
            string[] Data = content[key];
            for (int i = 0; i < Data.Length; i++)
            {
    
                //<ID><字段值>
                hasData[i+""] = Data[i];
            }
            DicContent[key] = hasData;
        }
        return DicContent;
    }

}
