using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardadoJuego : MonoBehaviour {

    public InputField nombre;
    public GameObject panelConfirmacion;
    public GameObject panelGuardado;

	public void OnEnable()
	{
        panelGuardado.SetActive(false);
	}

	public void GuardarJuego(){


    }

}
