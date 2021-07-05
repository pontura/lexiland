using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;



public class CEstimuloSegundaParte
{

    // Levantado del input
    private string _task, _trialCode, _frec, _audioL, _audioR, _cond, _trialType, _audio1;
    private string _numPrac, _taskPos, _trialPos, _resp, _RT, _contador, _session;
    private string _lineaEntera; 
    private AudioClip _audio;
    private AudioClip[] _audios;
    private int _tipoEstimulo;
    private float _frecFloat;
    private string _pathAudios;
   
    public static string[] GetHeaders()
    {
        string[] rowDataTemp = {"trial_Code", "freq", "audioL", "audioR", "cond", "trialType", "audio1", "numPrac", "taskPos",
            "trialPos", "resp", "RT", "contador"};
        return rowDataTemp;
    }

    public string[] GetEstimulo(int _numPrac, int _trialPos, string _tResp, string _respuesta, int _contador)
    {
        string _numPracString;
        if (_numPrac == -1)
        {
            _numPracString = "NA";
        }
        else
        {
            _numPracString = _numPrac.ToString();
        }
        string[] estimulos = {TrialCode, Frec, AudioL, AudioR, Cond, TrialType, Audio1, NumPractica, TaskPosition,
            _trialPos.ToString(),  _respuesta, _tResp, _contador.ToString()};
        return estimulos;
    }

    public List<string[]> GetEstimulos(int _numPrac, int _trialPos, string _respuesta, List<float> taps)
    {
        string _numPracString;
        if (_numPrac == -1)
        {
            _numPracString = "NA";
        }
        else
        {
            _numPracString = _numPrac.ToString();
        }
        List<string[]> estimulos = new List<string[]>();

        for (int i = 0; i < taps.Count; i++)
        {
            estimulos.Add(new string[]{TrialCode, Frec, AudioL, AudioR, Cond, TrialType, Audio1, _numPracString,
                _taskPos, _trialPos.ToString(), _respuesta,  taps[i].ToString(), i.ToString()});
        }
        return estimulos;
    }


    // Constructor
    public CEstimuloSegundaParte(string _linea, string _pathA)
    {
        // Seteamos los path correspondientes
        _pathAudios = _pathA;
        // Strings        
        CargarStrings(_linea);
        // Audios
        _audio = Resources.Load<AudioClip>(_pathAudios + _audio1);
        _audios = new AudioClip[1];
        _audios[0] = _audio;
        // Tipo estímulo
        _tipoEstimulo = int.Parse(_trialType);
        if((_frec != null) && (_frec != "NA")) _frecFloat = float.Parse(_frec);

    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task       = _lineaData[0];
        _trialCode  = _lineaData[1];
        _frec       = _lineaData[2];
        _audioL     = _lineaData[3];
        _audioR     = _lineaData[4];
        _cond       = _lineaData[5];
        _trialType  = _lineaData[6];
        _audio1     = _lineaData[7];
  
    }
   

    // Metodos
    public int TipoEstimulo
    {
        get { return _tipoEstimulo; }
    }
    public string LineaEntera
    {
        get { return _lineaEntera; }
    }
    public AudioClip Audio
    {
        get { return _audio; }
    }
    public AudioClip[] Audios
    {
        get { return _audios; }
    }
    // Input
    public string Task
    {
        get { return _task; }
    }
    public string TrialCode
    {
        get { return _trialCode; }
    }
    public string Frec
    {
        get { return _frec; }
    }
    public float FrecFloat
    {
        get { return _frecFloat; }
    }
    public string AudioL
    {
        get { return _audioL; }
    }
    public string AudioR
    {
        get { return _audioR; }
    }
    public string Cond
    {
        get { return _cond; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }
    public string Audio1
    {
        get { return _audio1; }
    }
    // Output
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
    public string Resp
    {
        get { return _resp; }
        set { _resp = value; }
    }
    public string RT
    {
        get { return _RT; }
        set { _RT = value; }
    }
    public string Contador
    {
        get { return _contador; }
        set { _contador = value; }
    }
    
}
