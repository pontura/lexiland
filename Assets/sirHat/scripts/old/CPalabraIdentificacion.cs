using UnityEngine;
using System.Collections;

public class CPalabraIdentificacion {

    private string _trialCode, _target, _struct;
	private AudioClip _audio1,_audio2;
    private Sprite _visual;   
    private int _nroSilabas, _nroFonemas, _sylPos;
    private int _corrResp;



    public CPalabraIdentificacion(string _tc, string _tar, string _st, Sprite _v, AudioClip _a1, AudioClip _a2, int _sils, int _fons, int _sPos, int _resp)
	{
        _trialCode = _tc;
        _target = _tar;
        _struct = _st;
        _visual = _v;
        _audio1 = _a1;
        _audio2 = _a2;
        _nroSilabas = _sils;
        _nroFonemas = _fons;
        _sylPos = _sPos;
        _corrResp = _resp;
    }
	
	public int obtenerCorrecta()
	{
        return _corrResp;
	}
	
	public string obtenerTrialCode()
	{
		return _trialCode;
	}

    public int obtenerSilabas()
    {
        return _nroSilabas;
    }

    public string obtenerStruct()
    {
        return _struct;
    }




    public Sprite obtenerEstimulo()
    {
        return _visual;
    }

    public AudioClip obtenerAudio1()
	{
        return _audio1;
    }
    public AudioClip obtenerAudio2()
    {
        return _audio2;
    }

}
