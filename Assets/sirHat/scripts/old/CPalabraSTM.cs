using UnityEngine;
using System.Collections;

public class CPalabraSTM {

	private string[] _palabras;
	private int _nroPalabras;
	private Sprite[] _imagens;
	private AudioClip[] _audios;


	public CPalabraSTM(string[] _p, int _n, Sprite[] _i, AudioClip[] _a)
	{
        _palabras = _p;
        _nroPalabras = _n;
        _imagens = _i;
        _audios = _a;
    }
	
	public string[] obtenerPalabras()
	{        
        return _palabras;
	}
	
	public int obtenerCantPalabras()
	{
		return _nroPalabras;
	}


    public Sprite[] obtenerImagenes()
	{
        return _imagens;
    }

	public AudioClip[] obtenerAudios()
	{
        return _audios;
    }


}
