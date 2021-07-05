using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


[Serializable]
[XmlRoot]
public class CJuegoDisk : ScriptableObject
{
    [SerializeField]
    public string nombre = "name";
    [SerializeField]
    public string[] codes = new string[6];

    public CJuegoDisk(){}

}


[CreateAssetMenu(fileName = "Juego", menuName = "Lexiland/Juego", order = 1)]
[Serializable]
[XmlRoot]
public class CJuego : ScriptableObject
{
    [XmlElement("Nombre")]
    public string nombre = "name";
    [XmlArray("tasks")]
    [XmlArrayItem("task")]
    public Task[] tareas = new Task[6];


    public CJuego() {    }

    /*
    public static CJuego CreateInstance(string _nombre, Task[] tareas){
        var juego = ScriptableObject.CreateInstance<CJuego>();
        juego.nombre = _nombre;
        juego.tareas = tareas;
        return juego;
    }
    */

    public void Save()
    {
        string _path = Application.persistentDataPath + "/juegos/";
        Directory.CreateDirectory(_path);

        DirectoryInfo directoryInfo = new DirectoryInfo(_path);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.xml");

        string _filepath = _path + "/" + fileInfos.Length.ToString() + ".xml";

        CJuegoDisk cJuegoDisk = new CJuegoDisk();
        cJuegoDisk.codes = new string[6];
        cJuegoDisk.nombre = nombre;
        for (int i = 0; i < tareas.Length; i++){
            cJuegoDisk.codes[i] = "";
            if (tareas[i] != null)
                cJuegoDisk.codes[i] = tareas[i]._code;
        }


        XmlSerializer serializer = new XmlSerializer(typeof(CJuegoDisk));
        StreamWriter writer = new StreamWriter(_filepath);
        serializer.Serialize(writer, cJuegoDisk);
        writer.Close();

    }

    public void Delete(){
        List<CJuego> juegos = new List<CJuego>();


        string _path = Application.persistentDataPath + "/juegos/";
        Directory.CreateDirectory(_path);
        DirectoryInfo directoryInfo = new DirectoryInfo(_path);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.xml");

        foreach (FileInfo fi in fileInfos)
            if(LoadJuego(fi.FullName).nombre == nombre)
                File.Delete(fi.FullName);

        Debug.Log("Borrando " + nombre);
    }

    public static List<CJuego> LoadJuegosFromDisk()
    {
        List<CJuego> juegos = new List<CJuego>();


        string _path = Application.persistentDataPath + "/juegos/";
        Directory.CreateDirectory(_path);
        DirectoryInfo directoryInfo = new DirectoryInfo(_path);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.xml");

        foreach (FileInfo fi in fileInfos){
            CJuego juego = LoadJuego(fi.FullName);
            juegos.Add(juego);
            //juegos.Add(juego);
        }


        return juegos;
    }

    public static CJuego LoadJuego(string _path){
        Task[] listaTasks = Resources.LoadAll<Task>("tasks");


        CJuegoDisk juegoDisk = new CJuegoDisk();

        XmlSerializer serializer = new XmlSerializer(typeof(CJuegoDisk));
        StreamReader reader = new StreamReader(_path);
        juegoDisk = serializer.Deserialize(reader) as CJuegoDisk;
        reader.Close();

        CJuego juego = new CJuego();
        juego.nombre = juegoDisk.nombre;
        juego.tareas = new Task[6];
        for (int i = 0; i < juegoDisk.codes.Length; i++)
        {
            int index = Array.FindIndex<Task>(listaTasks, t => t._code == juegoDisk.codes[i]);
            if (index != -1){
                juego.tareas[i] = listaTasks[index];
                Debug.Log(i + ": " + juego.tareas[i]._name);
            }
            else
                juego.tareas[i] = null;
        }

        return juego;

    }


}
