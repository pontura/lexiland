using UnityEngine;
using System.Collections;

public class CPalabraLetras {

	private string _trialCode, _target, _visual1, _visual2,_visual3;
	private AudioClip _audio1;


	public CPalabraLetras(string _tc, string _tar, string _v1, string _v2, string _v3, AudioClip _a1)
	{
        _trialCode = _tc;
        _target = _tar;
        _visual1 = _v1;
        _visual2 = _v2;
        _visual3 = _v3;
        _audio1 = _a1;
    }
	
	public string obtenerLetraCorrecta()
	{
        return _target;
	}
	
	public string obtenerTrialCode()
	{
		return _trialCode;
	}


    public string[] obtenerEstimulos()
    {
        string[] _vs = new string[3];
        _vs[0] = _visual1;
        _vs[1] = _visual2;
        _vs[2] = _visual3;
        return _vs;
    }



	public AudioClip obtenerAudio()
	{
        return _audio1;
    }


}
