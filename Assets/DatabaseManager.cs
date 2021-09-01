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
    string email;
    public Login login;

    [Serializable]
    public class TeacherData
    {
        public List<TeacherSingleData> all;
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
        if(PlayerPrefs.GetString("teacher_email", "") != "")
        {
            teacherData.all[0].admin_id = PlayerPrefs.GetInt("teacher_admin_id");
            teacherData.all[0].id = PlayerPrefs.GetInt("teacher_id");
            teacherData.all[0].name = PlayerPrefs.GetString("teacher_name");
            teacherData.all[0].lastname = PlayerPrefs.GetString("teacher_lastname");
            teacherData.all[0].email = PlayerPrefs.GetString("teacher_email");
        }
    }
    public TeacherSingleData GetData()
    {
        if (teacherData.all.Count == 0)
            return null;
        return teacherData.all[0];
    }
    System.Action OnLoginDone;
    public void Login(string email, System.Action OnLoginDone)
    {
        this.email = email;
        StopAllCoroutines();
        this.OnLoginDone = OnLoginDone;
        StartCoroutine(LoadJson(url + phpLoginFile + "?email=" + email.ToLower(), LoginDone));
    }
    void LoginDone(string data)
    {
        teacherData = JsonUtility.FromJson<TeacherData>(data); 
        if(teacherData == null || teacherData.all.Count == 0)
        {
            login.SetError();
            return;
        }
        if (teacherData != null)
        {
            PlayerPrefs.SetString("teacher_name", teacherData.all[0].name);
            PlayerPrefs.SetInt("teacher_id", teacherData.all[0].id);
            PlayerPrefs.SetInt("teacher_admin_id", teacherData.all[0].admin_id);
            PlayerPrefs.SetString("teacher_lastname", teacherData.all[0].lastname);
            PlayerPrefs.SetString("teacher_email", teacherData.all[0].email);
        }
        OnLoginDone();
    }
    public void ResetUser()
    {
        teacherData.all[0].name = "";
        teacherData.all[0].id = 0;
        teacherData.all[0].admin_id = 0;
        teacherData.all[0].lastname = "";
        teacherData.all[0].email = "";
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
