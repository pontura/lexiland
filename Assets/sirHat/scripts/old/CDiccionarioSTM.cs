using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionarioSTM {
	
	private Dictionary<int, CPalabraSTM> diccInterno;

	
	public Dictionary<int, CPalabraSTM> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionarioSTM()
	{
		diccInterno= new Dictionary<int, CPalabraSTM>();		
	}


	public void agregarPalabra (int _iid, CPalabraSTM _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraSTM obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraSTM> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerPalabras());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}