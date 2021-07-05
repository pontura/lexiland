using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CCanvasManager : MonoBehaviour {

    // Singleton
    public static CCanvasManager Instance;
    // Pausa
    public GameObject _buttonPause, _buttonUnPause, _panelPausa, _panelFinPractica, _panelLoading, _panelIntro, _panelTablero, _panelGlobos, _alexRepetidor, _manoEjemplo;
    private bool _pausado;
    // Botones y estimulos para apagar y prender
    private GameObject[] _estimulosVisualesGO, _botonesGO;
    private Button[] _botones;
    private Image[] _estimulosVisuales;
    private Text[] _opcionesLetraText;
    // Paneles para prender y apagar
    public GameObject _panelComun, _panelConfiguracion,_panelLogIn, _panelInicio, _panelAdminLogIn, _panelMain, _panelLog, _panelTareas, _panelJuego;
    // Variables a setar
    private float _tiempoRespuesta;
    // Fin práctica
    private bool _respondioPractica, _practicarDeVuelta;
    // Globos
    public CmanejoDeGlobos _globos; 


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _panelLog.SetActive(false);
        _panelMain.SetActive(false);
        _panelLoading.SetActive(true);
        //Invoke("ApagarLoading", 4f);
        if (PlayerPrefs.GetString("UserID")=="")
            Invoke("IrALogin", 4f);
        else
            Invoke("IrAMain", 4f);
        _tiempoRespuesta = 2f;
        _panelFinPractica.SetActive(false);
        //_botonesGO = new GameObject[];
    }

    // V2018 ----------------------------------------------------------------------------------------------

    public void IrALogin(){
        _panelLog.SetActive(true);
        _panelMain.SetActive(false);
        _panelLoading.SetActive(false);
        _panelTareas.SetActive(false);
        _panelJuego.SetActive(false);
    }

    public void IrAMain(){
        _panelLog.SetActive(false);
        _panelLoading.SetActive(false);
        _panelJuego.SetActive(false);
        _panelMain.SetActive(true);
        _panelTareas.SetActive(false);
        Main.m.Initiate();
    }

    public void IrATareas(bool juego){
        _panelTareas.SetActive(true);

        /*
        if (!juego)
            _panelTareas.GetComponent<PantallaTareas>().ActivateAll();
        _panelTareas.GetComponent<PantallaTareas>().SetMode(juego);
        */
    }

    public void IrAJuego(){
        _panelLog.SetActive(false);
        _panelLoading.SetActive(false);
        _panelMain.SetActive(false);
        _panelTareas.SetActive(false);

        _panelJuego.SetActive(true);
//        _panelJuego.GetComponent<Juego>().ClearAll();
    }

    // Navegacion paneles ----------------------------------------------------------------------------------------------
    // Apagar Loading
    private void ApagarLoading()
    {
        _panelLogIn.SetActive(true);
        _panelLoading.SetActive(false);
    }
    // Ir a Configuración
    public void IrAConfiguracion()
    {
        CAudioManager.Instance.ApagarMusica();
        _panelAdminLogIn.SetActive(true);
        //_panelConfiguracion.GetComponent<CConfiguracion>().ModoConf = true;
        //_panelConfiguracion.SetActive(true);
    }
    // Ir a Configuración
    public void IrAConfiguracionDirecto()
    {
        _panelConfiguracion.GetComponent<CConfiguracion>().ModoConf = true;
        _panelConfiguracion.GetComponent<CConfiguracion>().DesactivarVuelta();
        _panelConfiguracion.SetActive(true);
    }
    // Ir a Log In
    public void IrALogIn()
    {
        _panelLogIn.SetActive(true);
    }
    // Ir al inicio (pantalla de espera a que empiece el niño)
    public void IrAInicio()
    {
        _panelInicio.SetActive(true);
        CAudioManager.Instance.PrenderMusica();
    }
    // Ir a itro (desde inicio)
    public void IrAIntro()
    {
        //if(CGameManager.Instance._session != "1")
        //{
        //    CAudioManager.Instance.ReproducirWelcomeBack();
        //    CGameManager.Instance.llamarTablero();
           
        //}
        //else
        //{
            _panelIntro.SetActive(true);
        //}
        
        _panelInicio.SetActive(false);
    }


    // Llamada desde los tests cuando terminan.
    public IEnumerator vueltaDeTest()
    {
        CCanvasManager.Instance.DesactivarPanelComun();
        CCanvasManager.Instance.ApagarMano();
        CCanvasManager.Instance.PrenderRepeticionAlex();
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarAudios();
        CAudioManager.Instance.SetearARepetir(null);
        // Globos
        _panelGlobos.SetActive(true);
        CGameManager.Instance._tableroActual.ApagarBotonCasa();
        yield return _globos.StartCoroutine("aumentarGlobo");
        CGameManager.Instance._tableroActual.vueltaDeTest();
        // Musica de fondo
        // CAudioManager.Instance.PrenderMusica();
        // Avanzamos Alex

        yield break;
    }


    //// Ir a tablero
    //public void IrATablero()
    //{
    //    _panelTablero.SetActive(true);
    //}


    // Metodos intra Test ---------------------------------------------------------------------------------------------
    // Empezar Práctica
    public void EmpezarPractica()
    {
        _practicarDeVuelta = true;
    }
    // Fin de práctica
    public void FinPractica()
    {
        _respondioPractica = false;
        _panelFinPractica.SetActive(true);
    }
    // Boton de no repetir la practica
    public void botonContinuarTest()
    {
        _practicarDeVuelta = false;
        _respondioPractica = true;
        _panelFinPractica.SetActive(false);
    }

    public void apagarPanelPractica()
    {
        _panelFinPractica.SetActive(false);
    }

    // Boton de repetir practica
    public void botonRepetirPractica()
    {
        _practicarDeVuelta = true;
        _respondioPractica = true;
        _panelFinPractica.SetActive(false);
    }
    public bool PracticarDeVuelta
    {
        get { return _practicarDeVuelta; }
    }
    public bool RespondioPractica
    {
        get { return _respondioPractica; }
    }
    // Activar Panel Comun
    public void ActivarPanelComun()
    {
        _panelComun.SetActive(true);
    }
    // Activar Panel Comun
    public void DesactivarPanelComun()
    {
        _panelComun.SetActive(false);
    }
    // Pausa 
    // Boton pausa
    public void pausar()
    {
        _pausado = true;
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        _panelPausa.SetActive(true);
        CAudioManager.Instance.Pausar();
    }
    // Boton unpausar
    public void unPausar()
    {
        _pausado = false;
        _tTotalPausa = Time.time - _tini;
        _tini = Time.time;
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        _panelPausa.SetActive(false);
        CAudioManager.Instance.UnPausar();
    }


    // Obtener si estamos en pausa
    public bool Pausado
    {
        get { return _pausado; }
    }
    // Mostrar pausa.
    private IEnumerator mostrarPausa()
    {
        StartCoroutine(CAudioManager.Instance.MostrarPausa());
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(true);
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        yield return StartCoroutine(CAudioManager.Instance.Esperar());
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(false);
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        yield break;
    }
    // Repetir estimulo 
    public void RepetirEstimulos()
    {       
        StartCoroutine(CAudioManager.Instance.RepetirAudio());               
    }
    public void ApagarRepeticionAlex()
    {
        _alexRepetidor.SetActive(false);
    }
    public void PrenderRepeticionAlex()
    {
        _alexRepetidor.SetActive(true);
    }

    // Mezclar Opciones 
    public int asignarOpciones(Sprite[] _opciones, Image[] _images, string _opCorrecta) // Ojo que también mezcla los audios!
    {
        int _pos = 0;
        int[] _indOrden;
        if (_opciones.Length == 3)
        {
            _indOrden = new int[3] { 0, 1, 2 };
        }
        else
        {
            _indOrden = new int[4] { 0, 1, 2, 3 };
        }

        int[] _indDesorden;
        _indDesorden = mezclarIndices(_indOrden);
        for (int _i = 0; _i < _indOrden.Length; _i++)
        {
            _images[_indDesorden[_i]].sprite = _opciones[_i];
            if (_opciones[_i].name == _opCorrecta) _pos = _indDesorden[_i];
        }
        return _pos;
    }
    // Para test IQ
    public int asignarOpcionesIQ(GameObject[] _opciones, string _opCorrecta, Image[] _soportesVisuales, Sprite[] _visuales) // Para segmentacion
    {
        // Asignamos visuales (para segmentacion es 1 solo)
        for (int _i = 0; _i < _visuales.Length; _i++)
        {
            _soportesVisuales[_i].sprite = _visuales[_i];
        }
        // No asigna imagenes, solo marca cual es el boton correcto;
        int _pos = -1;
        for (int _i = 0; _i < _opciones.Length; _i++)
        {
            Debug.Log("verificando opción correcta: " + _i);
            if(_opciones[_i].GetComponent<Image>().sprite != null)
            {
                if (_opciones[_i].GetComponent<Image>().sprite.name == _opCorrecta) _pos = _i;
            }
           
        }
        return _pos;
    }


    public int asignarOpcionesBotones(GameObject[] _opciones, string _opCorrecta, Image[] _soportesVisuales, Sprite[] _visuales) // Para segmentacion
    {
        // Asignamos visuales (para segmentacion es 1 solo)
        for (int _i = 0; _i < _visuales.Length; _i++)
        {
            _soportesVisuales[_i].sprite = _visuales[_i];
        }
        // No asigna imagenes, solo marca cual es el boton correcto;
        int _pos = -1;
        for (int _i = 0; _i < _opciones.Length; _i++)
        {                      
            if (_opciones[_i].GetComponent<Image>().sprite.name == _opCorrecta) _pos = _i;
        }
        return _pos;
    }
    public int asignarOpcionesTexto(string[] _opciones, Text[] _textos, string _opCorrecta) // Ojo que también mezcla los audios!
    {
        int _pos = 0;
        int[] _indOrden = new int[3] { 0, 1, 2 };
        int[] _indDesorden;
        _indDesorden = mezclarIndices(_indOrden);
        for (int _i = 0; _i < _indOrden.Length; _i++)
        {
            _textos[_indDesorden[_i]].text = _opciones[_i];
            if (_opciones[_i] == _opCorrecta) _pos = _indDesorden[_i];
        }
        return _pos;
    }
    // Método para mezclar indices
    protected int[] mezclarIndices(int[] _indMixed)
    {
        for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
        {
            var r = Random.Range(0, i+1);
            var tmp = _indMixed[i];
            _indMixed[i] = _indMixed[r];
            _indMixed[r] = tmp;
        }
        return _indMixed;
    }
    // Botones y estimulos
    public void SetearBotones(Button[] _buts)
    {
        _botones = _buts;
    }
    public void SetearBotonesGO(GameObject[] _butsGO)
    {
        _botonesGO = _butsGO;
    }
    public void SetearEstimulos(Image[] _est)
    {
        _estimulosVisuales = _est;
    }
    public void SetearEstimulosGO(GameObject[] _estGO)
    {
        _estimulosVisualesGO = _estGO;
    }
    // Apagar Visual
    public void ApagarVisual()
    {
        ApagarEstimulos();
        ApagarBotones();
    }
    // Activar visual
    public void PrenderVisual()
    {
        PrenderBotones();
        PrenderEstimulos();
    }
    // Activar visual
    public void PrenderVisualLargoFijo( int _largoEst)
    {
        PrenderBotonesLargoFijo(_largoEst);
        PrenderEstimulosLargoFijo(_largoEst);
    }
    // Apagar estimulo
    public void ApagarEstimulos()
    {
        for (int _i = 0; _i < _estimulosVisualesGO.Length; _i++)
        {
            _estimulosVisualesGO[_i].SetActive(false);           
        }
    }
    // Prender estímulo
    public void PrenderEstimulos()
    {
        for (int _i = 0; _i < _estimulosVisualesGO.Length; _i++)
        {
            _estimulosVisualesGO[_i].SetActive(true);
        }
    }
    // Prender estímulo
    public void PrenderEstimulosLargoFijo(int _largoEst)
    {
        for (int _i = 0; _i < _largoEst; _i++)
        {
            _estimulosVisualesGO[_i].SetActive(true);
        }
    }
    // Apagar botones
    public void ApagarBotones()
    {
        for (int _i = 0; _i < _botonesGO.Length; _i++)
        {
            _botonesGO[_i].SetActive(false);
        }
    }
    // Prender botones
    public void PrenderBotonesLargoFijo(int _largoEst)
    {
        for (int _i = 0; _i < _largoEst; _i++)
        {
            _botonesGO[_i].SetActive(true);
        }
    }
    // Prender botones
    public void PrenderBotones()
    {
        for (int _i = 0; _i < _botonesGO.Length; _i++)
        {
            _botonesGO[_i].SetActive(true);
        }
    }
    // deshabilitar respuesta
    public void DeshabilitarRespuesta()
    {
        if (_botonesGO != null)
        {
            for (int _i = 0; _i < _botonesGO.Length; _i++)
            {
                _botones[_i].interactable = false;
            }
        }
    }
    // habilitar respuesta
    public void HabilitarRespuesta()
    {
        if(_botonesGO != null)
        {
            for (int _i = 0; _i < _botonesGO.Length; _i++)
            {
                _botones[_i].interactable = true;
            }
        }        
    }

    // Apagar mano forzada
    public void ApagarMano()
    {
        _manoEjemplo.transform.localScale = 1f * Vector3.one;
        _manoEjemplo.SetActive(false);
    }

    // Marcar botón correcto
    public IEnumerator apretarBoton(Button _button)
    {
        yield return StartCoroutine(MoverMano(_manoEjemplo, 0.6f, _button.transform.position));
        yield return new WaitForSeconds(0.2f); 
        Sprite _imNormal, _imApretado;
        _imNormal = _button.GetComponent<Image>().sprite;
        _imApretado = _button.spriteState.pressedSprite;
        _button.GetComponent<Image>().sprite = _imApretado;
       // _button.gameObject.transform.localScale = 1.3f * Vector3.one;
        _manoEjemplo.transform.localScale =(1f/ 1.3f) * Vector3.one;
        yield return new WaitForSeconds(_tiempoRespuesta);
        _button.GetComponent<Image>().sprite = _imNormal;
       // _button.gameObject.transform.localScale = 1f * Vector3.one;
        _manoEjemplo.transform.localScale = 1f * Vector3.one;
        _manoEjemplo.SetActive(false);
        yield break;
    }
    // Marcar botón correcto
    public IEnumerator apretarBoton2(Button _button)
    {
       // Debug.Log(_manoEjemplo);
        yield return StartCoroutine(MoverMano(_manoEjemplo, 0.6f, _button.transform.position));
        yield return new WaitForSeconds(0.2f);
        _button.gameObject.transform.localScale = (1f / 1.1f) * Vector3.one;
        _manoEjemplo.transform.localScale = (1f / 1.1f) * Vector3.one;
        yield return new WaitForSeconds(_tiempoRespuesta);
        _button.gameObject.transform.localScale = 1f * Vector3.one;
        _manoEjemplo.transform.localScale = 1f * Vector3.one;
        _manoEjemplo.SetActive(false);
        yield break;
    }
    // Esperar respuesta
    private float _tini, _tResp, _tTotalPausa;
    private bool _respondio;
    private string _tlog;
    public IEnumerator esperarRespuesta(AudioClip[] _estimuloActualAudios, float _tiempoMaximoResp)
    {
        _tlog = "";
        CAudioManager.Instance.SetearARepetir(_estimuloActualAudios);
        CAudioManager.Instance.RepeticionAlex = true;     
        CCanvasManager.Instance.HabilitarRespuesta();

        _respondio = false;
        _tini = Time.time;
        _tResp = 0f;
        _tTotalPausa = 0f;
        while (!_respondio & _tResp < _tiempoMaximoResp)
        {
            if (!CCanvasManager.Instance.Pausado)
            {
                _tResp = Time.time - _tini;
            }
            yield return null;
        }
        if (!_respondio)
        {
            _tResp = _tiempoMaximoResp;
        }
        // Audios a repetir
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarRepeticion();
        CAudioManager.Instance.SetearARepetir(null);
        CCanvasManager.Instance.DeshabilitarRespuesta();
       
        // Tresp logeo       
        if (_tResp < _tiempoMaximoResp)
            _tlog = (_tResp + _tTotalPausa).ToString();
        else
            _tlog = "NA";
       // yield return new WaitForSeconds(0.5f);
       // CAudioManager.Instance.FrenarAudios();
        yield break;
    }
    // log
    public float TRespuesta
    {
        get { return _tResp; }
    }
    public bool Respondio
    {
        get { return _respondio; }
        set { _respondio = value; }
    }
    public string TLog
    {
        get { return _tlog; }
    }



    // Cambiar alpha corrutina
    private IEnumerator CambiarAlpha(CanvasGroup _panelCG,RectTransform _panelRT,float overTime)
    {
        float _valInicial = _panelCG.alpha;
        Vector2 _valInicialPos = _panelRT.anchoredPosition;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _panelCG.alpha = Mathf.Lerp(_valInicial, 1f, (Time.time - startTime) / overTime);
            _panelRT.anchoredPosition = Vector2.Lerp(_valInicialPos, Vector2.zero, (Time.time - startTime) / overTime);           
            yield return null;
        }
        _panelCG.alpha = 1f;
        _panelRT.anchoredPosition = Vector2.zero;
    }

    // Cambiar alpha corrutina
    private IEnumerator CambiarAlpha2(CanvasGroup _panelCG, RectTransform _panelRT, float overTime, Vector2 _posFin)
    {
        float _valInicial = _panelCG.alpha;
        Vector2 _valInicialPos = _panelRT.anchoredPosition;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _panelCG.alpha = Mathf.Lerp(_valInicial, 1f, (Time.time - startTime) / overTime);
            _panelRT.anchoredPosition = Vector2.Lerp(_valInicialPos, _posFin, (Time.time - startTime) / overTime);
            yield return null;
        }
        _panelCG.alpha = 1f;
        _panelRT.anchoredPosition = _posFin;
    }

    // Cambiar alpha corrutina
    private IEnumerator MoverMano(GameObject _mano, float overTime, Vector3 _posFin)
    {
        CanvasGroup _manoCG = _manoEjemplo.GetComponent<CanvasGroup>();
        _manoCG.alpha = 0f;
        _manoEjemplo.SetActive(true);
        float _valInicial = _manoCG.alpha;
        Vector3 _valInicialPos = _manoEjemplo.transform.position;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _manoCG.alpha = Mathf.Lerp(_valInicial, 1f, (Time.time - startTime) / overTime);
            _manoEjemplo.transform.position = Vector3.Lerp(_valInicialPos, _posFin, (Time.time - startTime) / overTime);
            yield return null;
        }
        _manoCG.alpha = 1f;
        _manoEjemplo.transform.position = _posFin;
    }



}
