using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CAdminLogIn : MonoBehaviour {

    public GameObject _adminPanel;

	public string _password;	
	
	public InputField _textoPassword;
	
	public GameObject _wrongPasswordMsg;
	
	public void CheckPassword()
	{
		Debug.Log(_password);
		Debug.Log(_textoPassword.text);
		if(_password == _textoPassword.text)
		{
			Debug.Log("COrrectooooo maldito");
			_wrongPasswordMsg.SetActive(false);
            // llamar panel de admin
            _adminPanel.SetActive(true);
            _adminPanel.GetComponent<CConfiguracion>().ModoConf = true;
            _textoPassword.text = "";
            this.gameObject.SetActive(false);
        }
		else
		{
            _textoPassword.text = "";
            _wrongPasswordMsg.SetActive(true);
		}
	}
	
	public void Return()
	{
        // volver a pantalla anterior
        _textoPassword.text = "";
        this.gameObject.SetActive(false);
        CAudioManager.Instance.PrenderMusica();
	}
}
