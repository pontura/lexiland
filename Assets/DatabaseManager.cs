using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager mInstance;
    public string url;
    public string phpLoginFile;
    public TeacherData teacherData;

    [Serializable]
    public class TeacherData
    {
        public TeacherSingleData[] all;
    }

    [Serializable]
    public class TeacherSingleData
    {
        public int admin_id; // de que admin es (de qué país?)
        public int id;
        public string name;
        public string lastname;
        public string email;
    }
    
    public static DatabaseManager Instance
    {
        get
        {
            return mInstance;
        }
    }
    void Awake()
    {
        mInstance = this;
    }
    public TeacherSingleData GetData()
    {
        if (teacherData.all.Length == 0)
            return null;
        return teacherData.all[0];
    }
    System.Action OnLoginDone;
    public void Login(string email, System.Action OnLoginDone)
    {
        StopAllCoroutines();
        this.OnLoginDone = OnLoginDone;
        StartCoroutine(LoadJson(url + phpLoginFile + "?email=" + email.ToLower(), LoginDone));
    }
    void LoginDone(string data)
    {
        teacherData = JsonUtility.FromJson<TeacherData>(data);
        OnLoginDone();
    }
    IEnumerator LoadJson(string url, System.Action<string> OnDone)
    {
        print(url);

        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            if (OnDone != null)
                OnDone(www.text);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
}
