using UnityEngine;
using System.Collections;

public class CPalabra {

	private string _palabra;
	private int _nroSilabas;
	private Sprite _imagen;
	private AudioClip _audio;
    private string _struct;


	public CPalabra(string _p, int _s, Sprite _i, AudioClip _a, string _str)
	{
		_palabra 	= _p;
		_nroSilabas = _s;
		_imagen 	= _i;
		_audio 		= _a;
        _struct     = _str;
    }
	
	public string obtenerPalabra()
	{
		return _palabra;
	}
	
	public int obtenerSilabas()
	{
		return _nroSilabas;
	}

	public Sprite obtenerImagen()
	{
		return _imagen;
	}

	public AudioClip obtenerAudio()
	{
		return _audio;
	}

    public string obtenerStruct()
    {
        return _struct;
    }
}
