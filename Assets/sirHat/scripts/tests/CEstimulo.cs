using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimulo
{
    private string _task, _trialCode, _target, _visual1, _visual2, _visual3, _visual4, _corrResp, _numSyll, _numPhon, _struct, _syllPos, _trialType, _audio1, _audio2;
    private string _lineaEntera;
    private Sprite[] _visualesImagenes;
    private string[] _visuales;
    private AudioClip[] _audios;
    private int _tipoEstimulo;
    private string _pathAudios, _pathImages, _respuesta;
    //Para log
    private string _numPrac, _taskPos, _trialPos, _targetPos, _resp, _acierto, _RT;

    // V2018
    // Header y estimulo
    public static string[] GetHeaders(){
        string[] rowDataTemp = {"trialCode", "target", "visual1", "visual2", "visual3", "visual4", "corrResp", "numSyll", "numPhon", "struct",
            "syllPos", "trialType","audio1", "audio2", "numPrac", "taskPos", "trialPos", "targetPos", "resp", "acierto", "RT"};
        return rowDataTemp;
    }

    public string[] GetEstimulo(int _numPrac, int _trialPos, int _targetPos, string _tResp, string _respuesta, string _acierto){
        string[] rowDataTemp = {  TrialCode, Target, Visual1, Visual2, Visual3, Visual4, CorrResp, NumSyll, NumPhon, Struct, SyllPos, TrialType, Audio1, Audio2,
            _numPrac.ToString(), _taskPos, _trialPos.ToString(), _targetPos.ToString(), _respuesta, _acierto, _tResp};
        return rowDataTemp;
    }

    // Constructor
    public CEstimulo(string _linea, string _pathA, string _pathV)
    {
        // Seteamos los path correspondientes
        _pathAudios = _pathA;
        _pathImages = _pathV;
        // Strings        
        CargarStrings(_linea);
        // Imagenes
        CargarVisuales();
        // Audios
        CargarAudios(_audio1, _audio2);
        // Tipo estímulo
        _tipoEstimulo = int.Parse(_trialType);
        // Respuesta
        _respuesta = _corrResp;

    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        Debug.Log(_linea);
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task = _lineaData[0];
        _trialCode = _lineaData[1];
        _target = _lineaData[2];
        _visual1 = _lineaData[3];
        _visual2 = _lineaData[4];
        _visual3 = _lineaData[5];
        _visual4 = _lineaData[6];
        _corrResp = _lineaData[7];
        _numSyll = _lineaData[8];
        _numPhon = _lineaData[9];
        _struct = _lineaData[10];
        _syllPos = _lineaData[11];
        _trialType = _lineaData[12];
        _audio1 = _lineaData[13];
        _audio2 = _lineaData[14];
    }
    private void CargarVisuales()
    {
        int _largoVisual;
        _largoVisual = 4;
        if (_visual1 == "NA")
        {
            _largoVisual -= 1;
        }
        if (_visual2 == "NA")
        {
            _largoVisual -= 1;
        }
        if (_visual3 == "NA")
        {
            _largoVisual -= 1;
        }
        if (_visual4 == "NA")
        {
            _largoVisual -= 1;
        }
        _visuales = new string[_largoVisual];
        _visualesImagenes = new Sprite[_largoVisual];
        switch (_largoVisual)
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
                if(_visual3 == "NA") _visuales[2] = _visual4;
                else _visuales[2] = _visual3;
                break;
            case 4:
                _visuales[0] = _visual1;
                _visuales[1] = _visual2;
                _visuales[2] = _visual3;
                _visuales[3] = _visual4;
                break;
            default:
                Debug.Log("Largo Visuale 0 o mayor que 4");
                break;
        }
        for (int _i = 0; _i < _largoVisual; _i++)
        {           
            _visualesImagenes[_i] = Resources.Load<Sprite>(_pathImages + _visuales[_i]);
            if (_visualesImagenes[_i] == null) Debug.Log("No se pudo cargar: " + _pathImages + _visuales[_i]);
        }
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
    public string TrialCode
    {
        get { return _trialCode; }
    }
    public string Target
    {
        get { return _target; }
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
    public string CorrResp
    {
        get { return _corrResp; }
    }
    public string NumSyll
    {
        get { return _numSyll; }
    }
    public string NumPhon
    {
        get { return _numPhon; }
    }
    public string Struct
    {
        get { return _struct; }
    }
    public string SyllPos
    {
        get { return _syllPos; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }
    public string Audio1
    {
        get { return _audio1; }
    }
    public string Audio2
    {
        get { return _audio2; }
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
