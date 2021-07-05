using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimuloTapping
{
    private string _task, _trialCode, _trialType, _audioFile;
    private string _lineaEntera;
    private AudioClip _audioEstimulo;
    private float  _freq;
    private string _pathAudios;
    private bool _estimuloVisual;
	public int _tipoEstimulo;
    
    // Constructor
    public CEstimuloTapping(string _linea, string _pathAudio)
    {
        // Strings        
        CargarStrings(_linea);
        // Audios
        CargarAudio(_audioFile, _pathAudio);
    }

    // Funciones auxiliares para emprolijar
    private void CargarStrings(string _linea)
    {
        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task = _lineaData[0];
        _trialCode = _lineaData[1];
        _trialType = _lineaData[2];
        if (int.Parse(_trialType) == 2) _estimuloVisual = false; else _estimuloVisual = true;
        _freq = float.Parse(_lineaData[3]);
        _audioFile = _lineaData[4];
    }
    private void CargarAudio(string _audioFile, string _pathAudio)
    {
         _audioEstimulo = Resources.Load<AudioClip>(_pathAudio + _audioFile);
         if (_audioEstimulo == null) Debug.Log("No se pudo cargar: " + _pathAudios + _audioFile);
    }

    // Metodos
    public string LineaEntera
    {
        get { return _lineaEntera; }
    }
    public AudioClip getAudioEstimulo
    {
        get { return _audioEstimulo; }
    }
    public string TrialCode
    {
        get { return _trialCode; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }
    public bool isVisual
    {
        get { return _estimuloVisual; }
    }
    public float freq
    {
        get { return _freq; }
    }

}
