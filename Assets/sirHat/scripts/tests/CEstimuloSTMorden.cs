using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimuloSTMorden
{

    private string _task, _visual1, _visual2, _visual3, _visual4, _visual5, _visual6, _numEl,_trialType;
    private string _lineaEntera;
    private Sprite[] _visualesImagenes;
    private string[] _visuales;
    private AudioClip[] _audios;
    private int _tipoEstimulo, _numEstimulos;
    private string _pathAudios, _pathImages,_respuesta;
    //Para log
    private string _numPrac, _taskPos, _trialPos, _targetPos, _resp, _acierto, _RT;
    string[] _palabrasLog; 
  
    // V2018
    // Header y estimulo
    public static string[] GetHeaders()
    {
        string[] rowDataTemp = {"word1", "word2", "word3", "word4", "word5", "word6", "word7", "word8", "word9", "numEl", "trialType", "taskPos", "trialPos",
            "resp1", "resp2", "resp3", "resp4", "resp5", "resp6", "resp7", "resp8", "resp9", "acierto", "RT"};
        return rowDataTemp;
    }

    public string[] GetEstimulo(int _trialPos, int _numPrac, string _tResp, string[] _palabrasRespuesta, string _acierto)
    {
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < this.NumEstimulos)
            {
                _respLog[_i] = _palabrasRespuesta[_i];
            }
            else
            {
                _respLog[_i] = "";
            }

            if (TrialType.Equals("0"))
                _respLog[_i] = "NA";
        }
        string[] rowDataTemp = {Visual1, Visual2, Visual3, Visual4, Visual5, Visual6 , "", "", "",
            NumEl, TrialType, _taskPos, _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2], _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8],
            _acierto, _tResp};
        
        return rowDataTemp;
    }

    // Constructor
    public CEstimuloSTMorden(string _linea, string _pathA, string _pathV)
    {
        // Seteamos los path correspondientes
        _pathAudios = _pathA;
        _pathImages = _pathV;
        // Strings        
        CargarStrings(_linea);
        // Imagenes
        CargarVisuales();
        // Audios
        CargarAudios();
        // Tipo estímulo
        _tipoEstimulo = int.Parse(_trialType);
        // Respuesta
        //_respuesta = _corrResp;

        _palabrasLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _numEstimulos)
            {
                _palabrasLog[_i] = Visuales[_i];
            }
            else
            {
                _palabrasLog[_i] = "";
            }
        }

    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task = _lineaData[0];  
        _visual1 = _lineaData[1];
        _visual2 = _lineaData[2];
        _visual3 = _lineaData[3];
        _visual4 = _lineaData[4];
        _visual5 = _lineaData[5];
        _visual6 = _lineaData[6];
        _numEl = _lineaData[7];
        _trialType = _lineaData[8];
        //Debug.Log("numEl: " + _numEl);
        _numEstimulos = int.Parse(_numEl);
    }

    private void CargarVisuales()
    {        
        _visuales = new string[_numEstimulos];
        _visualesImagenes = new Sprite[_numEstimulos];
        switch (_numEstimulos)
        {
            case 1:
                _visuales[0] = _visual1;
                break;
            case 2:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                break;
            case 3:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                _visuales[2] = _visual3;
                break;
            case 4:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                _visuales[2] = _visual3;
                _visuales[3] = _visual4;
                break;
            case 5:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                _visuales[2] = _visual3;
                _visuales[3] = _visual4;
                _visuales[4] = _visual5;
                break;
            case 6:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                _visuales[2] = _visual3;
                _visuales[3] = _visual4;
                _visuales[4] = _visual5;
                _visuales[5] = _visual6;
                break;
            default:
                Debug.Log("Largo Visuale 0 o mayor que 4");
                break;
        }

        for (int _i = 0; _i < _numEstimulos; _i++)
        {
            _visualesImagenes[_i] = Resources.Load<Sprite>(_pathImages + _visuales[_i]);
            if (_visualesImagenes[_i] == null) Debug.Log("No se pudo cargar: " + _pathImages + _visuales[_i]);
        }
    }


    private void CargarAudios()
    {
        _audios = new AudioClip[_numEstimulos];
        for (int _i = 0; _i < _numEstimulos; _i++)
        {
            _audios[_i] = Resources.Load<AudioClip>(_pathAudios + _visuales[_i]);
            if (_audios[_i] == null) Debug.Log("No se pudo cargar: " + _pathAudios + _visuales[_i]);
        }

    }


    
    










    // Metodos
    public string[] PalabrasLog
    {
        get { return _palabrasLog; }
    }
    public int NumEstimulos
    {
        get { return _numEstimulos; }
    }
    public int TipoEstimulo
    {
        get { return _tipoEstimulo; }
    }
    public string Respuesta
    {
        get { return _respuesta; }
    }
    public string LineaEntera
    {
        get { return _lineaEntera; }
    }

    public Sprite[] VisualesImagenes
    {
        get { return _visualesImagenes; }
    }
    public string[] Visuales
    {
        get { return _visuales; }
    }
    public AudioClip[] Audios
    {
        get { return _audios; }
    }

    // Log
    public string Task
    {
        get { return _task; }
    }


    public string Visual1
    {
        get { return _visual1; }
    }
    public string Visual2
    {
        get { return _visual2; }
    }
    public string Visual3
    {
        get { return _visual3; }
    }
    public string Visual4
    {
        get { return _visual4; }
    }
    public string Visual5
    {
        get { return _visual5; }
    }
    public string Visual6
    {
        get { return _visual6; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }

    public string NumPractica
    {
        get { return _numPrac; }
        set { _numPrac = value; }
    }
    public string TaskPosition
    {
        get { return _taskPos; }
        set { _taskPos = value; }
    }
    public string TrialPosition
    {
        get { return _trialPos; }
        set { _trialPos = value; }
    }
    public string TargetPosition
    {
        get { return _targetPos; }
        set { _targetPos = value; }
    }
    public string Resp
    {
        get { return _resp; }
        set { _resp = value; }
    }
    public string Acierto
    {
        get { return _acierto; }
        set { _acierto = value; }
    }
    public string RT
    {
        get { return _RT; }
        set { _RT = value; }
    }
    public string NumEl
    {
        get { return _numEl; }
    }

}
