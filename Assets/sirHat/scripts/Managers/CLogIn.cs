using UnityEngine;
using UnityEngine.UI;

public class CLogIn : MonoBehaviour {

	public GameObject _panelArchivos;
	public Text _textoID, _textoIDverif, _textoClase, _textoEscuela, _textoAplicador, _sesion, _lista, _textoTablet;	
	public GameObject _textoCompletarCampos;
    public GameObject _textoIDsIncorrectos, _textoErrorDigitosEscuela, _textoErrorDigitosID;

	
    
    public void IrAConfiguracion()
    {
        if(_textoID.text.Length != 6)
        {
            _textoErrorDigitosID.SetActive(true);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
        }
        else if(_textoID.text != _textoIDverif.text)
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(true);
        }
        else if (_textoEscuela.text.Length != 3)
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(true);
            _textoIDsIncorrectos.SetActive(false);
        }
        else if(_textoID.text == "" || _textoClase.text == "" || _textoEscuela.text == "" || _textoAplicador.text == "" || _sesion.text == "" || _lista.text == "" || _textoTablet.text == "")
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(true);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
        }
        else
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
            // Seteamos sesión en Game Manager
            CGameManager.Instance._session = _sesion.text;
            CGameManager.Instance._lista = _lista.text;
            CLogManager.Instance._textoID = _textoID.text;
            CLogManager.Instance._textoTablet = _textoTablet.text;
            CLogManager.Instance._textoClase = _textoClase.text;
            CLogManager.Instance._textoEscuela = _textoEscuela.text;
            CLogManager.Instance._textoTurno = "NA";
            CLogManager.Instance._textoSesion = _sesion.text;
            CLogManager.Instance._textoFecha = System.DateTime.Now.ToString("MMddyy");
            CLogManager.Instance._textoHora = System.DateTime.Now.ToString("HHmmss");
            CLogManager.Instance._textoAplicador = _textoAplicador.text;
            CLogManager.Instance._taskPos = 0;
            CCanvasManager.Instance.IrAConfiguracionDirecto();
        }

    }

	// Send log in data to CSVWriter
	public void SaveLogInData()
	{
        if (_textoID.text.Length != 6)
        {
            _textoErrorDigitosID.SetActive(true);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
        }
        else if (_textoID.text != _textoIDverif.text)
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(true);
        }
        else if (_textoEscuela.text.Length != 3)
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(true);
            _textoIDsIncorrectos.SetActive(false);
        }
        else if (_textoID.text == "" || _textoClase.text == "" || _textoEscuela.text == "" || _textoAplicador.text == "" || _sesion.text == "" || _lista.text == "" || _textoTablet.text == "")
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(true);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
        }
        else
        {
            _textoErrorDigitosID.SetActive(false);
            _textoCompletarCampos.SetActive(false);
            _textoErrorDigitosEscuela.SetActive(false);
            _textoIDsIncorrectos.SetActive(false);
            // Seteamos sesión en Game Manager
            CGameManager.Instance._session = _sesion.text;
            CGameManager.Instance._lista = _lista.text;
            CLogManager.Instance._textoTablet = _textoTablet.text;
            CLogManager.Instance._textoID = _textoID.text;
            CLogManager.Instance._textoClase = _textoClase.text;
            CLogManager.Instance._textoEscuela = _textoEscuela.text;
            CLogManager.Instance._textoTurno = "NA";
            CLogManager.Instance._textoSesion = _sesion.text;
            CLogManager.Instance._textoFecha = System.DateTime.Now.ToString("MMddyy");
            CLogManager.Instance._textoHora = System.DateTime.Now.ToString("HHmmss");
            CLogManager.Instance._textoAplicador = _textoAplicador.text;
            CLogManager.Instance._taskPos = 0;
            // Vamos a la pantalla de inicio
            CCanvasManager.Instance.IrAInicio();
            // NOs apagamos
            this.gameObject.SetActive(false);
        }
      
	}

	public void activarPanelSubirArchivos(){		
		_panelArchivos.SetActive (true);
		this.gameObject.SetActive(false);
	}

}
