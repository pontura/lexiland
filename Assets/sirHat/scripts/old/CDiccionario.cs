using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionario {
	
	private Dictionary<int, CPalabra> diccInterno;

	
	public Dictionary<int, CPalabra> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionario()
	{
		diccInterno= new Dictionary<int, CPalabra>();		
	}


	public void agregarPalabra (int _iid,CPalabra _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabra obtenerPalabra(int _i)
	{
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabra> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerPalabra());
			Debug.Log ("Silabas: "+p.Value.obtenerSilabas());

		}
	}
	
}