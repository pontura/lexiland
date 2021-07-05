using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


[CreateAssetMenu(fileName = "Task", menuName = "Lexiland/Task", order = 1)]
[Serializable]
public class Task : ScriptableObject
{
    [XmlElement("Nombre")]
    public string _name = "NombreTarea";
    [XmlElement("Codigo")]
    public string _code = "CodeTarea";
    [XmlElement("Descripcion")]
    public string _descripcion = "Lorem Ipsum";
    [XmlElement("Con Material")]
    public bool material = false;

    public Task() {    }

    public static Task CreateInstance(string name, string code, string description)
    {
        var data = ScriptableObject.CreateInstance<Task>();
        data._name = name;
        data._code = code;
        data._descripcion = description;
        return data;
    }

}

/*
public class Task : MonoBehaviour {

    public CTask task;


    public void AgregarTarea(){
        Juego.j.AgregarTarea(this);
    }

}
*/

