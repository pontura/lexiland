using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimuloVisEnt
{
   
    private string _task, _trialCode, _target, _offset, _dur1, _onsetMask, _catch, _inPhase, _corrResp, _trialType;
    private string _numPrac, _taskPos, _trialPos, _targetPos, _resp, _acierto, _RT;
    private string _lineaEntera;
    private Sprite[] _visualesImagenes;
    private string[] _visuales;
    private int _tipoEstimulo;
    private string _pathImages, _respuesta;
    //Para log   
    private bool _catchBool;
    private int _inPhaseBool, _offsetInt, _dur1Int, _onsetMaskInt;

    // V2018
    // Header y estimulo
    public static string[] GetHeaders()
    {
        string[] rowDataTemp = {"trialCode", "target", "offset", "dur1", "onsetMask", "catch", "inPhase",
            "corrResp", "trialType", "numPrac", "taskPos", "trialPos", "targetPos", "resp","acierto","RT"};

        return rowDataTemp;
    }

    public string[] GetEstimulo(int _numPrac, int _trialPos, string _tResp, string _respuesta, string _acierto)
    {
        string[] rowDataTemp = { TrialCode, Target, Offset, Dur1, OnsetMask, Catch, InPhase, CorrResp, TrialType,
            _numPrac.ToString(), _taskPos, _trialPos.ToString(), TargetPosition,_respuesta, _acierto, _tResp};
        return rowDataTemp;
    }



    // Constructor
    public CEstimuloVisEnt(string _linea, string _pathV)
    {
        // Seteamos los path correspondientes
        _pathImages = _pathV;
        // Strings        
        CargarStrings(_linea);
        // Imagenes
        CargarVisuales();
        // Tipo estímulo
        _tipoEstimulo = int.Parse(_trialType);
        _inPhaseBool = int.Parse(_inPhase);
        Debug.Log(_catch);
        _catchBool = bool.Parse(_catch);
        Debug.Log(_catchBool);
        _offsetInt = int.Parse(_offset);
        _dur1Int = int.Parse(_dur1);
        _onsetMaskInt = int.Parse(_onsetMask);
        // Respuesta
        _respuesta = _corrResp;

    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        //task 	trialCode	target	offset	dur1	onsetMask	catch	inPhase	corrResp	trialType	numPrac	taskPos	trialPos	targetPos	resp	acierto	RT	session
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);        
        _task       = _lineaData[0];
        _trialCode  = _lineaData[1];
        _target     = _lineaData[2];
        _offset     = _lineaData[3];
        _dur1       = _lineaData[4];
        _onsetMask  = _lineaData[5];
        _catch      = _lineaData[6];
        _inPhase    = _lineaData[7];        
        _corrResp   = _lineaData[8];
        _trialType  = _lineaData[9];
        

    }
    private void CargarVisuales()
    {
        _visuales = new string[1];
        _visualesImagenes = new Sprite[1];
        _visuales[0] = _target;        
        _visualesImagenes[0] = Resources.Load<Sprite>(_pathImages + _visuales[0]);
        if (_visualesImagenes[0] == null) Debug.Log("No se pudo cargar: " + _pathImages + _visuales[0]);
        
    }

    // Metodos
    public int InPhaseBool
    {
        get { return _inPhaseBool; }
    }
    public bool CatchBool
    {
        get { return _catchBool; }
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
    public int OffsetInt
    {
        get { return _offsetInt; }
    }
    public int OnsetMaskInt
    {
        get { return _onsetMaskInt; }
    }
    public int Dur1Int
    {
        get { return _dur1Int; }
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
    public string Offset
    {
        get { return _offset; }
    }
    public string Dur1
    {
        get { return _dur1; }
    }
    public string OnsetMask
    {
        get { return _onsetMask; }
    }
    public string Catch
    {
        get { return _catch; }
    }
    public string InPhase
    {
        get { return _inPhase; }
    }
    public string Target
    {
        get { return _target; }
    }
    public string CorrResp
    {
        get { return _corrResp; }
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

}


public class CEstimuloVisSeg{

    public static string[] GetHeaders()
    {
        string[] rowDataTemp = {"trialCode", "trialType", "trialPos", "numPrac", "taskPos", "SOA", "resp", "RT"};

        return rowDataTemp;
    }

    public static string[] GetEstimulo(int _tipoEjPrTest, int _trialPos, int _numPrac, int _taskPos, int _SOA, string _respuesta, string _tResp)
    {
        string[] rowDataTemp = { "NA", _tipoEjPrTest.ToString(), _trialPos.ToString(), _numPrac.ToString(),
            _taskPos.ToString(), _SOA.ToString(), _respuesta, _tResp};

        return rowDataTemp;
    }


}