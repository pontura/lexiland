using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiccionarioLetras {
	
	private Dictionary<int, CPalabraLetras> diccInterno;

	
	public Dictionary<int, CPalabraLetras> copiaDiccionario()
	{
		return diccInterno;
	}
	public DiccionarioLetras()
	{
		diccInterno= new Dictionary<int, CPalabraLetras>();		
	}


	public void agregarPalabra (int _iid, CPalabraLetras _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraLetras obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraLetras> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerLetraCorrecta());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}