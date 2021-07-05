using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class CTarea : MonoBehaviour
{
    public static CTarea Inst;
    public string _name;
    public string _code;
    public string _timeStamp;
    string _timeStampDate;
    string _timeStampHour;
    DateTime timestampDatetime;
    public CModo _mode;
    public Dictionary<string, List<string>> _estimulos;
    public List<Dictionary<string, string>> _respuestas;
    public List<string> _subtareas;
    
    public StreamWriter _file;

    public enum acierto { correcto, incorrecto, duda, lexicalization, unMedio, noContesta};

    public CConfiguracion config;

    public Output output;

    void Awake()
    {
        Inst = this;
    }

    public void LoadTarea(CTareaData _tareaData)
    {
        _name = _tareaData._name;
        _code = _tareaData._code;
        _mode = _tareaData._mode;

        timestampDatetime = DateTime.Now;
        _timeStampDate = DateTime.Now.ToString("MMddyy");
        _timeStampHour = System.DateTime.Now.ToString("HHmmss");
        _timeStamp = _timeStampDate + "_" + _timeStampHour;
        
        string[,] inputMatrix = CSVReader.SplitCsvGrid(_tareaData._input.text);
        
        _estimulos = new Dictionary<string, List<string>>();

        
        //int cantEstimulos = inputMatrix.GetLength(1)-1;

        for (int _i=0; _i< inputMatrix.GetLength(0); _i++)
        {
            List<string> _listaEstimulos = new List<string>();
            for (int _j=1; _j< inputMatrix.GetLength(1); _j++)
            {
                _listaEstimulos.Add(inputMatrix[_i, _j]);
            }
            _estimulos.Add(inputMatrix[_i, 0], _listaEstimulos);
        }

        // Armado de la lista de subtareas
        if (_estimulos.ContainsKey("subTask"))
        {
            _subtareas = new List<string>();
            foreach (string s in _estimulos["subTask"])
                if (!_subtareas.Contains(s))
                    _subtareas.Add(s);
        }
        else
            _subtareas = null;

        // Armado de vector de string de respuestas
        _respuestas = new List<Dictionary<string, string>>();

/*
        _respuestas.Add("taskPos", CLogManager.Instance._taskPos.ToString());
        _respuestas.Add("trialPos", "");
        _respuestas.Add("acierto", "NA");
        if (_mode._ansType == ANS_type.CIDLuM)
        {
            _respuestas.Add("lexicalization/unMedio", "NA");
        }
        _respuestas.Add("session", CLogManager.Instance._textoSesion);
        if (_mode._obs)
            _respuestas.Add("observations", "");
        if (_mode._rt != RT_type.no)
            _respuestas.Add("RT", "");
        if (_mode._rt2)
            _respuestas.Add("RT_2", "");
        if (_mode._recAudio)
            _respuestas.Add("audioPath", "");
*/

    }

    public string printHeader()
    {
        string aux = null;

        // Armado de Encabezado
        string[] rowDataTemp = new string[10] { "ID", "Clase", "Escuela", "Turno", "Sesion", "Fecha", "Hora", "Aplicador", "Tablet", "Versión App" };

        foreach (string s in rowDataTemp)
        {
            aux += s + ",";
        }
        aux = aux.Substring(0, aux.Length - 1);
        aux += "\n";

        CLogManager.Instance._textoFecha = _timeStampDate;
        CLogManager.Instance._textoHora = _timeStampHour;

        foreach (string s in CLogManager.Instance.DataLogin())
        {
            aux += s + ",";
        }

        aux = aux.Substring(0, aux.Length - 1);
        aux += "\n";

        aux += "ID,";
        foreach (var key in _estimulos.Keys)
        {
            aux += key + ",";
        }

        foreach (var param in _respuestas[0].Keys)
        {
            if (param != "index")
                aux += param + ",";
        }
        

        return aux;

    }

    /*    public void printOutput(int _indexInput)
        {
            string aux = null;

            foreach (var key in _estimulos.Keys)
            {
                aux += _estimulos[key][_indexInput] + ",";
            }

            foreach (var param in _respuestas.Keys)
            {
                aux += _respuestas[param] + ",";
            }

            _file.WriteLine(aux.Substring(0, aux.Length - 1));

        }

        public void printOutput()
        {
            printOutput(0);
        }
    */

    public void agregarRespuesta()
    {
        _respuestas.Add(new Dictionary<string, string>());
        _respuestas[_respuestas.Count - 1].Add("taskPos", CLogManager.Instance._taskPos.ToString());
        _respuestas[_respuestas.Count - 1].Add("trialPos", "");
        _respuestas[_respuestas.Count - 1].Add("acierto", "NA");
        if (_mode._ansType == ANS_type.CIDLuM)
        {
            _respuestas[_respuestas.Count - 1].Add("lexicalization/unMedio", "NA");
        }
        _respuestas[Inst._respuestas.Count - 1].Add("session", CLogManager.Instance._textoSesion);
        if (_mode._obs)
            _respuestas[Inst._respuestas.Count - 1].Add("observations", "");
        if (_mode._rt != RT_type.no)
            _respuestas[_respuestas.Count - 1].Add("RT", "");
        if (_mode._rt2)
            _respuestas[_respuestas.Count - 1].Add("RT_2", "");
        if (_mode._recAudio)
            _respuestas[_respuestas.Count - 1].Add("audioPath", "");
    }

    public void guardarOutput()
    {
        string _path = Application.persistentDataPath;// + "/CSV";
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }        
        string _filename = CTarea.Inst._timeStamp + "_" + CLogManager.Instance._textoID + "_" + CLogManager.Instance._textoSesion + "_" + _code + ".csv";


        // Escritura del header
        string aux = printHeader();
        _file = new StreamWriter(_path + "/" + _filename, false);
        _file.WriteLine(aux.Substring(0, aux.Length - 1));

        // Armado de los items

        foreach (var respuesta in _respuestas)
        {
            aux = CLogManager.Instance._textoID + ",";

            foreach (var key in _estimulos.Keys)
            {
                //Debug.Log(respuesta["index"]);
                aux += _estimulos[key][int.Parse(respuesta["index"])] + ",";
            }
            
            foreach (var param in respuesta.Keys)
            {
                if (param != "index")
                aux += respuesta[param] + ",";
            }
            
            _file.WriteLine(aux.Substring(0, aux.Length - 1));

        }
        _file.Close();

        saveXML();

    }


    public void saveXML(){

        output = new Output(_code, timestampDatetime);
        output.setParametros(_estimulos, _respuestas);
        output.setItems(_estimulos, _respuestas);

        // TODO: cambiar el path de guardado
        output.Save();

    }

    /*    public void saveOutput(string timeStamp, List<int> indexes)
        {
            string _path = Application.persistentDataPath + "/Data/outputs/" + CManager.Inst.ID;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            string _filename = timeStamp + "_" + _code + ".csv";

            string aux = null;

            // Encabezado
            if (!File.Exists(_path + "/" + _filename))
            {
                foreach (var key in _estimulos.Keys)
                {
                    aux += key + ",";
                }

                foreach (var param in _respuestas[0].Keys)
                {
                    if (param != "indexEstimulo")
                        aux += param + ",";
                }
            }

            StreamWriter _file = new StreamWriter(_path + "/" + _filename, true);

            if (aux!=null)
            _file.WriteLine(aux.Substring(0, aux.Length - 1));

            for (int i = 0; i < indexes.Count && i<_respuestas.Count; i++)
            {
                aux = null;
                foreach (var key in _estimulos.Keys)
                {
                    aux += _estimulos[key][indexes[i]] + ",";
                }

                foreach (var param in _respuestas[i].Keys)
                {
                    if (param != "indexEstimulo")
                        aux += _respuestas[i][param] + ",";
                }

                _file.WriteLine(aux.Substring(0, aux.Length - 1));
            }
            _file.Close();
            Debug.Log("Guardado en: " + _path + "/" + _filename);
        // TODAVÍA NO SE DEFINIÓ DÓNDE SE SUBE
    }
    */

    public void saveAudios()
    {
        //string _path = Application.persistentDataPath + "/Audios";// + CLogManager.Instance._textoID;

        //V2018

        string _path = Application.persistentDataPath + "/Users/" + PlayerPrefs.GetString("UserID") + "/" +
                                 PlayerPrefs.GetString("SubjectID") + "/outputs";

        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }

        foreach(var audio in CAudioManager._aclips.Keys)
        {
            //Debug.Log(audio);
            AudioClip ac = SavWav.TrimSilence(CAudioManager._aclips[audio], 0.0f);
            string _filename = audio + ".wav";
            if (ac != null){
                bool saved = SavWav.Save(_path + "/" + _filename, ac);
            }
        }
        CAudioManager._aclips.Clear();

    }


    public void marcarTarea(string code)
    {
        Debug.Log("marcando: " + code);
        switch (code)
        {
            case "ReadBCBL-ap":
                config.marcarTareaLecturaBCBL();
                break;
            case "ReadPRO":
                config.marcarTareaLecturaPRO();
                break;
            case "ReadSEM":
                config.marcarTareaLecturaSEM();
                break;
            case "ReadSINT":
                config.marcarTareaLecturaSINT();
                break;
            case "ReadSS":
                config.marcarTareaLecturaSS();
                break;
            case "RAN":
                config.marcarTareaRAN();
                break;
        }
    }

}
