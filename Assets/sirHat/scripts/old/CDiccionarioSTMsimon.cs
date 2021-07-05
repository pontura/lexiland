using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionarioSTMsimon {
	
	private Dictionary<int, CPalabraSTMsimon> diccInterno;

	
	public Dictionary<int, CPalabraSTMsimon> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionarioSTMsimon()
	{
		diccInterno= new Dictionary<int, CPalabraSTMsimon>();		
	}


	public void agregarPalabra (int _iid, CPalabraSTMsimon _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraSTMsimon obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraSTMsimon> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerSecuencia());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}