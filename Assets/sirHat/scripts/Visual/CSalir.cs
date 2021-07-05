using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSalir : MonoBehaviour {

    public InputField password;
    public GameObject panelPost;
	
	public void checkPassword()
    {
        if (password.text == "lexiland")
        {
            password.text = "";
            this.gameObject.SetActive(false);
            panelPost.SetActive(false);
            if (Juego.j != null)
                Juego.j.SiguienteEstacion(true);
        }

    }

}
