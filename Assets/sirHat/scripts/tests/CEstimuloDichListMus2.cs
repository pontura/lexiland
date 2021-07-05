using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimuloDichListMus2
{
    //task	trialCode	song	frec	trialType	audioL	audioR	audio1	numPrac	session	taskPos	trialPos	resp	RT

    // Levantado del input
    private string _task, _trialCode, _song, _frec, _trialType, _audioL, _audioR, _audio1;
    //Para log
    private string _numPrac, _session, _taskPos, _trialPos, _targetPos, _resp, _acierto, _RT;
    // Auxiliares internas
    private string _lineaEntera; 
    private AudioClip[] _audios;
    private int _tipoEstimulo, _canal;
    private float _frecInt;
    private string _pathAudios, _respuesta;
   



    // Constructor
    public CEstimuloDichListMus2(string _linea, string _pathA)
    {
        // Seteamos los path correspondientes
        _pathAudios = _pathA;
        // Strings        
        CargarStrings(_linea);
        // Audios
        CargarAudios(_audio1, "NA");
        // Tipo estímulo
        _tipoEstimulo = int.Parse(_trialType);
        // Frecuencia
        _frecInt = float.Parse(_frec);
        // Canal
        switch (_audioL)
        {
            case "bi":
                _canal = 0;
                break;
            case "b":
                _canal = 1;
                break;
            case "m":
                _canal = 2;
                break;
            default:
                Debug.Log("error canal");
                break;
        }
    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task = _lineaData[0];
        _trialCode = _lineaData[1];
        _song = _lineaData[2];
        _frec = _lineaData[3];
        _trialType = _lineaData[4];
        _audioL = _lineaData[5];
        _audioR = _lineaData[6];
        _audio1 = _lineaData[7];
  
    }
   
    private void CargarAudios(string _a1, string _a2)
    {
        if (_a1 == "NA")
        {
            Debug.Log("No hay audios");
            _audios = null;
        }
        else
        {
            if (_a2 == "NA")
            {
                _audios = new AudioClip[1];
                _audios[0] = Resources.Load<AudioClip>(_pathAudios + _a1);
                if (_audios[0] == null) Debug.Log("No se pudo cargar: " + _pathAudios + _a1);
            }
            else 
            {               
                _audios = new AudioClip[2];
                _audios[0] = Resources.Load<AudioClip>(_pathAudios + _a1);
                _audios[1] = Resources.Load<AudioClip>(_pathAudios + _a2);
                if (_audios[0] == null) Debug.Log("No se pudo cargar: " + _pathAudios + _a1);
                if (_audios[1] == null) Debug.Log("No se pudo cargar: " + _pathAudios + _a2);
                            
            }
        }
    }

    // Metodos
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
    public AudioClip[] Audios
    {
        get { return _audios; }
    }
    // Log
    public string Task
    {
        get { return _task; }
    }
    public string Song
    {
        get { return _song; }
    }
    public int Canal
    {
        get { return _canal; }
    }
    public string Freq
    {
        get { return _frec; }
    }
    public float FreqInt
    {
        get { return _frecInt; }
    }
    public string TrialCode
    {
        get { return _trialCode; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }
    public string Audio1
    {
        get { return _audio1; }
    }
    public string AudioR
    {
        get { return _audioR; }
    }
    public string AudioL
    {
        get { return _audioL; }
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

}
