using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionarioSintesis {
	
	private Dictionary<int, CPalabraSintesis> diccInterno;

	
	public Dictionary<int, CPalabraSintesis> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionarioSintesis()
	{
		diccInterno= new Dictionary<int, CPalabraSintesis>();		
	}


	public void agregarPalabra (int _iid, CPalabraSintesis _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraSintesis obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraSintesis> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerCorrecta());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}