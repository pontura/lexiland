using UnityEngine;
using System.Collections;

public class CPalabraAis {

	private string _palabra1,_palabra2;
	private int _nroSilabas;
    private int _igualSilaba;
    private string _struct;
    private Sprite _imagen1,_imagen2;
	private AudioClip _audio1,_audio2;


	public CPalabraAis(string _p1, string _p2, int _s, int _igual, string _st, Sprite _i1, Sprite _i2, AudioClip _a1, AudioClip _a2)
	{
        _palabra1 = _p1;
        _palabra2 = _p2;
        _nroSilabas = _s;
        _igualSilaba = _igual;
        _struct = _st;
        _imagen1 = _i1;
        _imagen2 = _i2;
        _audio1 = _a1;
        _audio2 = _a2;
    }
	
	public string[] obtenerPalabras()
	{
        string[] _p = new string[] { _palabra1, _palabra2 };
        return _p;
	}
	
	public int obtenerSilabas()
	{
		return _nroSilabas;
	}


    public int empiezanIgual()
    {
        return _igualSilaba;
    }

    public string obtenerStruct()
    {
        return _struct;
    }

    public Sprite[] obtenerImagenes()
	{
        Sprite[] _i = new Sprite[] { _imagen1, _imagen2 };
        return _i;
    }

	public AudioClip[] obtenerAudios()
	{
        AudioClip[] _a = new AudioClip[] { _audio1, _audio2 };
        return _a;
    }


}
