using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionarioAis {
	
	private Dictionary<int, CPalabraAis> diccInterno;

	
	public Dictionary<int, CPalabraAis> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionarioAis()
	{
		diccInterno= new Dictionary<int, CPalabraAis>();		
	}


	public void agregarPalabra (int _iid,CPalabraAis _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraAis obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraAis> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerPalabras());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}