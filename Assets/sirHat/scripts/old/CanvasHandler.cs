using UnityEngine;
using System.Collections;

public class CanvasHandler : MonoBehaviour {

    public GameObject _panelTablero, _panelLogIn, _panelInicio, _panelIntro,_panleConf,_panelLoading,_panelContrasena;
    public GameObject _botonInicio, _jugadorInicio;
    public GameObject[] _panelesTests;
    private CTablero _tableroHanlder;
    public AudioSource _MusicaFondo;
    //public CsvReadWrite _csvWriter;
    //public CsvReadWriteSTM _csvWriterSTM;

    // Use this for initialization
    void Start () {
        _botonInicio.SetActive(true);
        _jugadorInicio.SetActive(false);
        _panelIntro.SetActive(false);
        _tableroHanlder = this.GetComponent<CTablero>();
       // StartCoroutine(iniciarJuego());
    }
	


    public void irAConfiguracion()
    {
        _panelContrasena.SetActive(true);
        _MusicaFondo.Pause();
    }

    public void volverDeConfiguracion()
    {
        _MusicaFondo.UnPause();
    }


 

}
