using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


public class DBServices : MonoBehaviour
{

    public static DBServices db;

    public void Awake()
    {
        db = this;
    }



    public IEnumerator RegisterUser(Usuario user)
    {
        byte[] file = File.ReadAllBytes(Application.persistentDataPath + "/Users/user.xml");
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", file);
        WWW w = new WWW(DatabaseManager.Instance.url + "RegisterUser.php", form);
        Debug.Log("Registrando " + user.nombre);
        yield return w;

        if (w.error == null)
        {
            Debug.Log(w.text);
            //string ID = int.Parse(w.text).ToString("0000");
            string ID = DatabaseManager.Instance.GetData().id.ToString();
            Debug.Log(ID);
            PlayerPrefs.SetString("UserID", ID);
            user.ID = ID;
            user.Save(false);
            File.Delete(Application.persistentDataPath + "/Users/user.xml");
            CCanvasManager.Instance.IrAMain();
        }
        else
        {
            Login.login.ErrorConexion();
        }


    }

    public IEnumerator UploadFiles(List<string> listaArchivos){

        byte[] file;
        WWWForm form;
        WWW w;
        float size = 0;
        int index = 0;

        foreach(string path in listaArchivos){
            FileInfo fi = new FileInfo(path);
            index++;
            size += fi.Length;
            SubidaDatos.sd.Subiendo(index, size);

            file = File.ReadAllBytes(path);
            form = new WWWForm();
            form.AddBinaryData("file", file);
            string realURL = "";
            if (path.Contains("output"))
                realURL = "AddOutput.php";
            else
                realURL = "AddSubject.php";

            string finalURL = DatabaseManager.Instance.url + realURL;
            w = new WWW(finalURL, form);
            Debug.Log(finalURL);

            yield return w;

            Debug.Log(path);
            Debug.Log(w.text);

            if (w.error == null && w.text.Contains("OK"))
            {
                Debug.Log(path + " Subido");
                SubidaDatos.sd.cantCorrectos++;
                File.Delete(path);
            }
        
        }


    }


    public IEnumerator CheckInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            
            action(true);
        }

    }
}