using UnityEngine;
using System.Collections;

public class CPalabraSintesis {

    private string _trialCode, _target, _struct;
    private string[] _palabras;
	private AudioClip _audio1;
    private Sprite[] _visual;   
    private int _nroSilabas, _nroFonemas;



    public CPalabraSintesis(string _tc, string _tar, string _st, string[] _pals, Sprite[] _v, AudioClip _a1, int _sils, int _fons)
	{
        _trialCode = _tc;
        _target = _tar;
        _palabras = _pals;
        _struct = _st;
        _visual = _v;
        _audio1 = _a1;
        _nroSilabas = _sils;
        _nroFonemas = _fons;
    }
	
	public string obtenerCorrecta()
	{
        return _target;
	}
	
	public string obtenerTrialCode()
	{
		return _trialCode;
	}


    public Sprite[] obtenerEstimulos()
    {
        return _visual;
    }
    public string[] obtenerpalabras()
    {
        return _palabras;
    }


    public AudioClip obtenerAudio()
	{
        return _audio1;
    }


}
