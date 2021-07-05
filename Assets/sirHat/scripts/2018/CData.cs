using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public enum gender { Masculino, Femenino, Otro };

[Serializable]
[XmlRoot("Usuario")]
public class Usuario{

    [XmlElement("nombre")]
    public string nombre;
    [XmlElement("ID")]
    public string ID;
    [XmlElement("mail")]
    public string mail;

    public Usuario(string _nombre, string _ID, string _mail){
        nombre = _nombre;
        ID = _ID;
        mail = _mail;
    }
    public Usuario(string _nombre, string _mail)
    {
        nombre = _nombre;
        mail = _mail;
    }

    public Usuario(){}

    public void Save(bool login)
    {
        string _path = Application.persistentDataPath + "/Users/";
        if (!login){
            _path += ID;
            Directory.CreateDirectory(_path + "/borrados/");
        }

        Directory.CreateDirectory(_path);

        string _filepath = _path + "/user.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(Usuario));
        StreamWriter writer = new StreamWriter(_filepath);
        serializer.Serialize(writer, this);
        writer.Close();

    }

    public static Usuario Load(string _path)
    {
        Usuario user = new Usuario();
        XmlSerializer serializer = new XmlSerializer(typeof(Usuario));
        StreamReader reader = new StreamReader(_path);
        user = (Usuario)serializer.Deserialize(reader);
        reader.Close();

        return user;
    }

}


[Serializable]
[XmlRoot("Sujeto")]
public class Sujeto{

    public string nombre;
    public string apellido;
    public string escuela;
    [XmlElement("FechaNacimiento")]
    public DateTime bornDate;
    [XmlElement("genero")]
    public gender genero;
    [XmlElement("escolaridad")]
    public string escolaridad;
    [XmlElement("ultimoTablero")]
    public string ultimoTablero;
    [XmlElement("ID")]
    public string ID;
    [XmlElement("usuarioID")]
    public string usuarioID;

    public Sujeto (string _nombre, string _apellido, string _escuela, DateTime _bornDate, gender _genero, string _escolaridad, string _ultimotablero, string _id){
        nombre = _nombre;
        apellido = _apellido;
        escuela = _escuela;
        bornDate = _bornDate;
        genero = _genero;
        escolaridad = _escolaridad;
        ultimoTablero = _ultimotablero;
        ID = _id;
    }
    public Sujeto (){
        ID = "";
    }

    // TODO: agregar que el guardado se corresponda con el user
    public void Save()
    {
        string _filepath;
        StreamWriter writer;
        XmlSerializer serializer = new XmlSerializer(typeof(Sujeto));

        if (ID == ""){

            //int i = new DirectoryInfo(Application.persistentDataPath + "/Users/" + usuarioID).GetDirectories().Length - 1;
            //i += new DirectoryInfo(Application.persistentDataPath + "/Users/" + usuarioID + "/borrados/").GetDirectories().Length;

            string devID = SystemInfo.deviceUniqueIdentifier;
            devID = devID.Substring(devID.Length - 3);

            ID = PlayerPrefs.GetString("UserID") + devID + GetNextID().ToString("00");

        string _path = Application.persistentDataPath + "/Users/" + usuarioID + "/" + ID;
        Directory.CreateDirectory(_path);
        _filepath = _path + "/" + "sujeto.xml";

        writer = new StreamWriter(_filepath);
        serializer.Serialize(writer, this);
        writer.Close();

        }

        // Guarda datos sin nombre y apellido para mandar
        nombre = null;
        apellido = null;
        ultimoTablero = null;
        _filepath = Application.persistentDataPath + "/Users/" + usuarioID + "/" + ID + "/" + ID + ".xml";
        writer = new StreamWriter(_filepath);
        serializer.Serialize(writer, this);
        writer.Close();

    }

    int GetNextID()
    {
        int i = -1;

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Users/" + usuarioID);
        foreach (DirectoryInfo fi in di.GetDirectories())
        {
            Debug.Log(fi.Name);
            if (int.TryParse(fi.Name.Substring(7), out int splitName))
                i = Mathf.Max(splitName, i);
        }

        di = new DirectoryInfo(Application.persistentDataPath + "/Users/" + usuarioID + "/borrados/");
        foreach (DirectoryInfo fi in di.GetDirectories())
        {
            Debug.Log(fi.Name);
            if (int.TryParse(fi.Name.Substring(7), out int splitName))
                i = Mathf.Max(splitName, i);
        }

        return (i+1);
    }

    public static Sujeto Load(string _path)
    {
        Sujeto subject = new Sujeto();
        XmlSerializer serializer = new XmlSerializer(typeof(Sujeto));
        StreamReader reader = new StreamReader(_path);
        subject = (Sujeto)serializer.Deserialize(reader);
        reader.Close();

        return subject;
    }

    public void SetUltimoJuego(string ultimoJuego){
        ultimoTablero = ultimoJuego;
        this.Save();
    }

}

[Serializable]
[XmlRoot("Output")]
public class Output{

    [XmlElement("sujetoID")]
    public string sujetoID;
    [XmlElement("ID")]
    public string ID_tarea;
    [XmlElement("runDate")]
    public DateTime runDate;
    [XmlArray("parametros")]
    [XmlArrayItem("parametro")]
    public List<string> parametros;
    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<Item> items;

    [XmlIgnore]
    public string filepath;

    public Output(string idTarea, DateTime _runDate) {
        sujetoID = PlayerPrefs.GetString("SubjectID");
        ID_tarea = idTarea;
        runDate = _runDate;
        parametros = new List<string>();
        items = new List<Item>();
        // TODO: Agregar info para el nombre del archivo
        string path = Application.persistentDataPath + "/Users/" + PlayerPrefs.GetString("UserID") + "/" +
                                 PlayerPrefs.GetString("SubjectID") + "/outputs/";
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(Application.persistentDataPath + "/xmls/");
        Directory.CreateDirectory(Application.persistentDataPath + "/csvs/");
        //filepath = Application.persistentDataPath + "/xmls/" + idTarea + "_" + _runDate.ToString("ddMMhhmm") + ".xml";
        filepath = path + PlayerPrefs.GetString("SubjectID") + "_" + idTarea + "_" + _runDate.ToString("ddMMhhmm") + ".xml";
    }

    public void setParametros(Dictionary<string, List<string>> _estimulos, List<Dictionary<string, string>> _respuestas){
        foreach (var key in _estimulos.Keys)
            if (key != "task" && key != "trialCode" && key != "trialPos" && key != "taskPos")
                parametros.Add(key);
        foreach (var param in _respuestas[0].Keys)
            if (param != "index")
                parametros.Add(param);
    }

    public void setParametros(string[] listaParametros){
        foreach (string param in listaParametros)
            parametros.Add(param);
    }

    public void setItems(Dictionary<string, List<string>> _estimulos, List<Dictionary<string, string>> _respuestas){
        foreach (var respuesta in _respuestas)
        {
            Item item = new Item();
            item.values = new List<string>();
            foreach (var key in _estimulos.Keys)
                if (key != "task" && key != "trialCode" && key != "trialPos" && key != "taskPos")
                    {
//                        Debug.Log(respuesta["index"]);
//                        Debug.Log(_estimulos[key][int.Parse(respuesta["index"])]);
                        item.values.Add(_estimulos[key][int.Parse(respuesta["index"])]);
                    }

            foreach (var param in respuesta.Keys)
            {
                if (param != "index")
                    item.values.Add(respuesta[param]);
            }
            items.Add(item);
        }
    }

    public void addItem(string[] estimulos){
        Item item = new Item();
        item.values = new List<string>();
        foreach (string estimulo in estimulos){
            item.values.Add(estimulo);
        }
        items.Add(item);
        // Guarda automaticamente
        Save();
    }

    public void addItem(List<string[]> estimulos)
    {
        foreach (string[] estimulo in estimulos){
            Item item = new Item();
            item.values = new List<string>();
            foreach (string e in estimulo)
            {
                item.values.Add(e);
            }
            items.Add(item);
        }
        // Guarda automaticamente
        Save();
    }


    public Output(){}

    public void Save()
    {
        if (filepath != "")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Output));
            StreamWriter writer = new StreamWriter(filepath);
            serializer.Serialize(writer, this);
            writer.Close();
        }
        else
            Debug.Log("filepath no seteado");

        // Para comparar los csvs
        SaveCSV(Application.persistentDataPath + "/csvs/" + PlayerPrefs.GetString("UserID") +
                PlayerPrefs.GetString("SubjectID") + "_" + ID_tarea + "_" + runDate.ToString("ddMMhhmm") + ".csv");
    }

    public void SaveCSV(string _path){

        StreamWriter file = new StreamWriter(_path, false);
        string aux = "";

        foreach (string parametro in parametros)
            aux += parametro + ",";
        aux = aux.Substring(0, aux.Length - 1);
        file.WriteLine(aux);

        foreach(Item item in items)
        {
            aux = "";
            foreach (string value in item.values)
                aux += value + ",";
            aux = aux.Substring(0, aux.Length - 1);
            file.WriteLine(aux);
        }

        file.Close();


    }

}

[Serializable]
[XmlRoot("Item")]
public class Item{
    [XmlArray("values")]
    [XmlArrayItem("value")]
    public List<string> values;
}


