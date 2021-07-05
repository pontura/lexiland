using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[CreateAssetMenu(fileName = "tarea", menuName = "Lexiland/Tarea", order = 1)]
[System.Serializable]
public class CTareaData : ScriptableObject
{
//    [XmlElement("Nombre")]
    public string _name = "NombreTarea";
//    [XmlElement("Codigo")]
    public string _code = "CodeTarea";
//    [XmlElement("Modo")]
    public CModo _mode;
    //    [XmlElement("input")]
    public TextAsset _input;

    //    public CTareaData() { }
}

public enum RT_type {no, item, subTask};
public enum ANS_type {no, CIDLuM, CInc, MO};

[System.Serializable]
public class CModo
{
    //[XmlElement("Tiempo de Respuesta")]
    public RT_type _rt;
    public bool _rt2;
    //[XmlElement("Grabar Audio")]
    public bool _recAudio;
    //[XmlElement("Sacar Foto")]
    public bool _takePicture;
    //[XmlElement("Estímulos visuales")]
    public bool _visualInput;
    //[XmlElement("Formulario")]
    public bool _form;
    //[XmlElement("Observaciones")]
    public bool _obs;
    //[XmlElement("Observaciones")]
    public bool _oneEntry;
    //[XmlElement("Lexicalization")]
    public ANS_type _ansType;

    
    public CModo(CModo cm) {
        this._rt = cm._rt;
        this._recAudio = cm._recAudio;
        this._takePicture = cm._takePicture;
        this._visualInput = cm._visualInput;
        this._form = cm._form;
        this._obs = cm._obs;
        this._oneEntry = cm._oneEntry;
        this._ansType = cm._ansType;
    }
}
