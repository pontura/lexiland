using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CTestDichListSyl : MonoBehaviour {
    
    // Variables publicas a setear    
    public Image[] _estimuloVisual;
    public Button[] _buttonsRespuesta;
    public Button _buttonPregunta;
    public GameObject _buttonPreguntaGO;
    public GameObject[] _estimuloVisualGO, _buttonsRespuestaGO;
    public Text[] _opcionesLetraText;
    public AudioClip _llamaAlAplicador;
    public Text _trialCodeText;
    // Nuevo manejo data
    private CListaTestSegundaParte _data;
    private CEstimuloSegundaParte _estimuloActual;
    //Para log
    private int _trialPos, _numPrac,  _targetPos;
    // Variables internas
    public string _respuesta, _tResp;
    public enum tipoCanvas {VoF, Opciones, OpcionesTexto, OpcionesBotones, IQ}
    public tipoCanvas _tipoCanvas;
    // Datos para loggear
    public CConfiguracion _configH;
    public bool _conf;
    int _contador;
    private bool _esperarAlex, _habilitadoAlexTrigger, _habilitadoAlexRep;
    public GameObject _panelCargando;

    // V2018
    Output output;


    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }

    public IEnumerator CorrerTest()
    {
        Debug.Log("Empezando Test DichList Syl");
        // Inicio
        inicializar();    
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_llamaAlAplicador));

        // Practica
        if (_data.CantPractica != 0)
        {
            CCanvasManager.Instance.EmpezarPractica();
            _numPrac = -1;
            while (CCanvasManager.Instance.PracticarDeVuelta)
            {
                _numPrac += 1;
                _trialPos = 0;
                for (int _cont = _data.CantEjemplos; _cont < _data.CantEjemplos + _data.CantPractica; _cont++)
                {
                    _trialPos += 1;
                        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                        yield return StartCoroutine(CorrerEstimulo(_cont, null, 1));
                        yield return null;
                }
                CCanvasManager.Instance.FinPractica();               
                yield return StartCoroutine(CAudioManager.Instance.VolverAPracticar());
                yield return StartCoroutine(esperarRespuestaPractica());
            }
        }
        // Test
        _trialPos = 0;
        int[] _indicesTest;        
       _indicesTest = _data.IndicesTestMixed;
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareaDichListSyl();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarAudios();
        CAudioManager.Instance.SetearARepetir(null);
        //salirTest();
        Salir(false);
    }




    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.DichListSyl);
        _estimuloVisualGO[0].GetComponent<animar_imagen>().enabled = false;
        _estimuloVisualGO[1].GetComponent<animar_imagen>().enabled = false;
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();       
       _data = CInfoManager.Instance.DataDichListSyl();
        CCanvasManager.Instance.SetearBotonesGO(_buttonsRespuestaGO);
        CCanvasManager.Instance.SetearEstimulosGO(_estimuloVisualGO);
        CCanvasManager.Instance.SetearEstimulos(_estimuloVisual);
        CCanvasManager.Instance.SetearBotones(_buttonsRespuesta);
        CCanvasManager.Instance.ApagarBotones();
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;
        _contador = 0;

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloSegundaParte.GetHeaders());
        output.Save();

    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------

    // button 0
    public void ButtonPa()
    {
        _respuesta = _estimuloActual.AudioL ;
        CCanvasManager.Instance.Respondio = true;
    }
    // button 1
    public void ButtonBa()
    {
        _respuesta = _estimuloActual.AudioR;
        CCanvasManager.Instance.Respondio = true;
    }
    // button 2
    public void ButtonPaBa()
    {
        _respuesta = _estimuloActual.AudioL + _estimuloActual.AudioR;
        CCanvasManager.Instance.Respondio = true;
    }
    // button 3
    public void ButtonBaPa()
    {
        _respuesta = _estimuloActual.AudioR + _estimuloActual.AudioL;
        CCanvasManager.Instance.Respondio = true;
    }
    // button 4
    public void ButtonX()
    {
        _respuesta = "NA";
        CCanvasManager.Instance.Respondio = true;
    }
    // button 5
    public void Button5()
    {
        RespuestaBoton(5);
    }
    // button 6
    public void Button6()
    {
        RespuestaBoton(6);
    }
    // respuesta boton genérica
    private void RespuestaBoton(int _i)
    {
        _respuesta = _buttonsRespuesta[_i].GetComponentInChildren<Text>().text;
        if (_respuesta == "X") _respuesta = "NA";
        CCanvasManager.Instance.Respondio = true;
    }
    // Salir
    public void salirTest()    {
        
        StopAllCoroutines();
        if (!_conf)
        {
            CCanvasManager.Instance.StartCoroutine("vueltaDeTest");
            CCanvasManager.Instance.DesactivarPanelComun();        
        }
        else
        {
            _configH.gameObject.SetActive(true);
            CCanvasManager.Instance.DesactivarPanelComun();
            _conf = false;
        }
        CCanvasManager.Instance.ApagarMano();
        this.gameObject.SetActive(false);        
    }

    public void Salir(bool salidaForzada){
        StopAllCoroutines();
        this.gameObject.SetActive(false);
        CCanvasManager.Instance.DesactivarPanelComun();
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);

    }

    // Alex
    public void Alex()
    {
        if (_habilitadoAlexTrigger) {
            _esperarAlex = true;
            _habilitadoAlexTrigger = false;
            _habilitadoAlexRep = true;
        }
        else {
            if (_habilitadoAlexRep)
            {
                CAudioManager.Instance.ReproducirSinEspera(_estimuloActual.Audio);
                _estimuloVisualGO[0].GetComponent<animar_imagen>().enabled = true;
                _estimuloVisualGO[1].GetComponent<animar_imagen>().enabled = true;
                Invoke("ApagarMovimiento", 1f);
            }
        }
    }
    // Espera Alex
    private IEnumerator EsperarAlex()
    {
        while (!_esperarAlex)
        {
            yield return null;
        }
        yield break;
    }


    // ESPERAS  --------------------------------------------------------------------------------

    // Esperar que responda si quiere practicar de vuelta
    public IEnumerator esperarRespuestaPractica()
    {
        while (!CCanvasManager.Instance.RespondioPractica)
        {
            yield return null;
        }
        yield break;
    }
    // Reproduccion de estimulos auditivos
    private IEnumerator ReproducirAudiosEstimulo()
    {
       yield return StartCoroutine(CAudioManager.Instance.Reproducir(_estimuloActual.Audio));       
        CAudioManager.Instance.SetearARepetir(_estimuloActual.Audios);
        yield break;
    }
    // boton pregunta
    public void BotonPregunta()
    {
        _buttonPreguntaGO.SetActive(false);
        _trialCodeText.gameObject.SetActive(true);
        CCanvasManager.Instance.PrenderBotones();        
    }
    // Apagar movimiento
    private void ApagarMovimiento()
    {
        _estimuloVisualGO[0].GetComponent<animar_imagen>().enabled = false;
        _estimuloVisualGO[1].GetComponent<animar_imagen>().enabled = false;
    }


    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------
   private IEnumerator CorrerEstimulo(int _indPalabra, AudioClip _audioAux, int _tipoEjPrTest)
    {
        // Desabilitamos botones
        _buttonPreguntaGO.SetActive(true);
        _buttonPregunta.interactable = false;
        // Actualizamos info actual
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);
        _trialCodeText.text = "Trial Code: " + _estimuloActual.TrialCode;
        // Actualizar estimulo visual
        _opcionesLetraText[0].text = _estimuloActual.AudioL;
        _opcionesLetraText[1].text = _estimuloActual.AudioR;
        _opcionesLetraText[2].text = _estimuloActual.AudioL + '-' + _estimuloActual.AudioR;
        _opcionesLetraText[3].text = _estimuloActual.AudioR + '-' + _estimuloActual.AudioL;
        // Esperar a que toquen a Alex para decir el audio
        _esperarAlex = false;
        _habilitadoAlexTrigger = true;
        yield return StartCoroutine(EsperarAlex());
        CAudioManager.Instance.ReproducirSinEspera(_estimuloActual.Audio);
        
        // Mover personajes
        _estimuloVisualGO[0].GetComponent<animar_imagen>().enabled = true;
        _estimuloVisualGO[1].GetComponent<animar_imagen>().enabled = true;
        Invoke("ApagarMovimiento", 1f);

        _buttonPregunta.interactable = true;
        // Esperamos la respuesta
        _respuesta = "NA";
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
        _tResp = CCanvasManager.Instance.TLog;
        _habilitadoAlexRep = false;
        // Habilitamos botones nuevamente 
        CCanvasManager.Instance.ApagarBotones();
        _trialCodeText.gameObject.SetActive(false);
        _buttonPreguntaGO.SetActive(false);
        // Loggeo  
        CLogManager.Instance.GuardarEstimuloDichListSyl(_estimuloActual, _numPrac, _trialPos, _tResp, _respuesta, _contador++);

        // V2018
        output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _tResp, _respuesta, _contador));

       // yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
