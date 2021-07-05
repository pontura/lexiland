using UnityEngine;
using System.Collections;

public class CPalabraSTMsimon {

	private int[] _secuencia;
	private int _largoSecuencia;


	public CPalabraSTMsimon(int[] _sec)
	{
        _secuencia = _sec;
        _largoSecuencia = _sec.Length;
    }
	
	public int[] obtenerSecuencia()
	{        
        return _secuencia;
	}
	
	public int obtenerLargo()
	{
		return _largoSecuencia;
	}


}
