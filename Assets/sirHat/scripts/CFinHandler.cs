using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CFinHandler : MonoBehaviour {

    public GameObject panel;
    public string _password;
    public InputField _textoPassword;
    public GameObject _wrongPasswordMsg;

	public void OnEnable()
	{
        _wrongPasswordMsg.SetActive(false);
        panel.SetActive(false);
        Invoke("prenderPanel", 4.0f);

	}
    public void prenderPanel(){
        panel.SetActive(true);
    }

	public void CheckPassword()
    {
        Debug.Log(_password);
        Debug.Log(_textoPassword.text);
        if (_password == _textoPassword.text)
        {
            Debug.Log("COrrectooooo maldito");
            _wrongPasswordMsg.SetActive(false);
            // Reiniciar Juego           
            _textoPassword.text = "";
            // V2018
            //CGameManager.Instance.ReiniciarJuego();
            this.gameObject.SetActive(false);
            CCanvasManager.Instance.IrAMain();
            CAudioManager.Instance.ApagarMusica();
        }
        else
        {
            _textoPassword.text = "";
            _wrongPasswordMsg.SetActive(true);
        }
    }

}
