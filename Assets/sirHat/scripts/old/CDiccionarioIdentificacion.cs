using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDiccionarioIdentificacion {
	
	private Dictionary<int, CPalabraIdentificacion> diccInterno;

	
	public Dictionary<int, CPalabraIdentificacion> copiaDiccionario()
	{
		return diccInterno;
	}
	public CDiccionarioIdentificacion()
	{
		diccInterno= new Dictionary<int, CPalabraIdentificacion>();		
	}


	public void agregarPalabra (int _iid, CPalabraIdentificacion _pal){
		diccInterno.Add (_iid,_pal);
	}


	public CPalabraIdentificacion obtenerPalabra(int _i)
	{
        Debug.Log(_i);
		return diccInterno[_i];
	}



	public void ImprimirValores()
	{
		foreach(KeyValuePair<int, CPalabraIdentificacion> p in diccInterno)
		{
			Debug.Log ("Palabra: "+p.Value.obtenerCorrecta());
			//Debug.Log ("Silabas: "+p.Value.empiezanIgual());

		}
	}
	
}