using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTestVisualSeg : MonoBehaviour {

    //public Image _target;
    public Sprite _pelota,_noTarget;
    public Image[] _estimuloVisual;
    public Button[] _buttonsRespuesta;
    public GameObject[] _estimuloVisualGO, _buttonsRespuestaGO;
    public Text _SOAText;
    // Variables
    private CListaTestVisSS _data;
    private int _trialPos, _numPrac;
    string _targetPos;
    public CConfiguracion _configH;
    public bool _conf;
    private bool _presentarEstimulos;
    enum Estado { nada, faseUp, faseDown};
    Estado _estado = Estado.nada;
    public int _frameCount, _repCount,_offset, _offset2;
    private string _respuesta, _tResp, _respuestaCorrecta, _respuestaAnterior;
    private bool _acierto, _repetir;
    private Button _buttonCorrecto;
    private int _repEnt, _contRep, _contFrames, _pasoCambio, _offsetEjemplo1Pelota, _offsetEjemplo2Pelota, _duracionPelota, _offsetMinimo, _offsetMaximo, _offsetInicial;
    private int _contador;

    public GameObject _panelCargando;

    // V2018
    Output output;


    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        _presentarEstimulos = false;
        _Alexhabilitado = false;
        _AlexMomentaneo = false;
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.VisSeg);
        CCanvasManager.Instance.SetearBotonesGO(_buttonsRespuestaGO);
        CCanvasManager.Instance.SetearEstimulosGO(_estimuloVisualGO);
        CCanvasManager.Instance.SetearEstimulos(_estimuloVisual);
        CCanvasManager.Instance.SetearBotones(_buttonsRespuesta);
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();
        _data = CInfoManager.Instance.DataVisSeg();
        CCanvasManager.Instance.ActivarPanelComun();
        CCanvasManager.Instance.ApagarBotones();
        CCanvasManager.Instance.ApagarEstimulos();
        //StartCoroutine(TestTimeInterval());
        // En frames del fixed update!
        _repEnt = 15; 
        _offsetEjemplo1Pelota = 17;
        _duracionPelota = 2;
        _offsetEjemplo2Pelota = 2;
        _offsetMaximo = 30;
        _offsetMinimo = 0;
        _offsetInicial = 17;
        _contador = 0;

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloVisSeg.GetHeaders());
        output.Save();

    }

    // Corutina de entrada
    public IEnumerator CorrerTest()
    {
        // Inicio
        Debug.Log("Empezando Test Visual Seg");
        inicializar();
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (_data.TieneInstrucciones())
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
        }
       // CAudioManager.Instance.reproducirVisSeg();
        // Ejemplos
        _offset = _offsetEjemplo1Pelota;
        CalcularOffset2();
        yield return StartCoroutine(CorrerEstimulo(0, _data.AudiosEjemplo[0], 0));
        _offset = _offsetEjemplo2Pelota;
        CalcularOffset2();
        yield return StartCoroutine(CorrerEstimulo(1, _data.AudiosEjemplo[1], 0));

        // Practica
        yield return StartCoroutine(CAudioManager.Instance.APracticar());
        yield return StartCoroutine(CAudioManager.Instance.Tetocati());
        CCanvasManager.Instance.EmpezarPractica();
        _numPrac = -1;
        while (CCanvasManager.Instance.PracticarDeVuelta)
        {
            _numPrac += 1;
            _trialPos = 0;
            for (int _cont = 2; _cont < 6; _cont++)
            {
                _repetir = true;
                _offset = SetOffsetPractica();
                CalcularOffset2();
                while (_repetir)
                {
                    _trialPos += 1;
                    yield return new WaitForSeconds(_data.TiempoInterEnsayo);                    
                    yield return StartCoroutine(CorrerEstimulo(_cont, null, 1));
                    _repetir = !(CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
                    yield return null;
                }
            }
            CCanvasManager.Instance.FinPractica();
            yield return StartCoroutine(CAudioManager.Instance.VolverAPracticar());
            yield return StartCoroutine(esperarRespuestaPractica());
        }

        // Test
        _trialPos = 0;
        // int[] _indicesTest;        
        //_indicesTest = _data.IndicesTestMixed;
        //_indicesTest = _data.IndicesTest;
        yield return StartCoroutine(CAudioManager.Instance.Comenzar());
        _offset = _offsetInicial;
        _pasoCambio = 3;
        _respuestaAnterior = "0";
        for (int _cont = 6; _cont < 31; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_cont, null, 2));
        }
        _configH.marcarTareaVisSeg();
        CLogManager.Instance.GuardarLista("visSS");
        CCanvasManager.Instance.ApagarMano();
        //salirTest();
        Salir(false);
    }

    private int SetOffsetPractica()
    {
        int _aux;
        _aux = Random.Range(1, 3);
        if (_aux == 1)
        {
            _aux = _offsetEjemplo1Pelota;
        }
        else
        {
            _aux = _offsetEjemplo2Pelota;
        }
        return _aux; 
    }

    private int _contFramesUp, _contFramesDown, _contRepUp, _contRepDown;
    private bool _presentarEstimulosUp, _presentarEstimulosDown;
    private float _minUpdateF = 100f;
    private float _maxUpdateF = 0f; 
    public Text _frameRateDisplay,_fixedRateDisplay, _minUpdateDis, _maxUpdateDis, _SOAframesdisplay;
    float _FupateFixed, _Fupate;
    //void Update()
    //{
    //    if (_presentarEstimulos)
    //    {
    //        _Fupate = 1f / Time.deltaTime;
    //        if (_Fupate > _maxUpdateF)
    //            _maxUpdateF = _Fupate;
    //        if (_Fupate < _minUpdateF)
    //            _minUpdateF = _Fupate;
    //        _frameRateDisplay.text = "Update: " + _Fupate.ToString("000") + " Hz";
    //        _fixedRateDisplay.text = "FixedUpdate: " + _FupateFixed.ToString("000") + " Hz";
    //        _minUpdateDis.text = "Update Min: " + _minUpdateF.ToString("000") + " Hz";
    //        _maxUpdateDis.text = "Update Max: " + _maxUpdateF.ToString("000") + " Hz";
    //        _SOAText.text = "SOA t: " + (_offset * Time.fixedDeltaTime).ToString("0.000") + " s";
    //        _SOAframesdisplay.text = "SOA frames: " + _offset.ToString("00") + " frames";
    //    }
    //}

    void Update()
    {
       
        if (_presentarEstimulos)
        {
            _FupateFixed = 1f / Time.fixedDeltaTime;
            _Fupate = 1f / Time.deltaTime;
            if (_Fupate > _maxUpdateF)
                _maxUpdateF = _Fupate;
            if (_Fupate < _minUpdateF)
                _minUpdateF = _Fupate;
            _frameRateDisplay.text = "Update: " + _Fupate.ToString("000") + " Hz";
            _fixedRateDisplay.text = "FixedUpdate: " + _FupateFixed.ToString("000") + " Hz";
            _minUpdateDis.text = "Update Min: " + _minUpdateF.ToString("000") + " Hz";
            _maxUpdateDis.text = "Update Max: " + _maxUpdateF.ToString("000") + " Hz";
            _SOAText.text = "SOA t: " + (_offset * Time.deltaTime).ToString("0.000") + " s";
            _SOAframesdisplay.text = "SOA frames: " + _offset.ToString("00") + " frames";

            // Todas las pelotas de arriba
            if (_presentarEstimulosUp)
            {
                //Debug.Log("presentando estimulos up");
                
                // Prender pelota
                if (_contFramesUp == 0)
                {                    
                    _estimuloVisual[0].sprite = _pelota;
                    _contRepUp += 1;
                }
                // Apagar pelota
                if (_contFramesUp == _duracionPelota)
                {                    
                    _estimuloVisual[0].sprite = _noTarget;                   
                }
                // Habilitar la de abajo
                if (_contFramesUp == _offset && _contRepUp ==1)
                {
                    _presentarEstimulosDown = true;
                }
                // Reiniciar 
                if (_contFramesUp == _offset+_offset2)
                {                                                          
                    _contFramesUp = -1;
                    // Apagar
                    if(_contRepUp == _repEnt)
                    {
                        _presentarEstimulosUp = false;
                    }
                }
                _contFramesUp += 1;

            }

            // Todas las pelotas de abajo
            if (_presentarEstimulosDown)
            {
                // Prender pelota
                if (_contFramesDown == 0)
                {
                    _estimuloVisual[1].sprite = _pelota;
                    _contRepDown += 1;
                }
                // Apagar pelota
                if (_contFramesDown == _duracionPelota )
                {
                    _estimuloVisual[1].sprite = _noTarget;
                }
                // Reiniciar 
                if (_contFramesDown == _offset + _offset2)
                {
                    _contFramesDown = -1;
                    // Apagar
                    if (_contRepDown == _repEnt)
                    {
                        _presentarEstimulosDown = false;
                        _presentarEstimulos = false;
                        if (_Alexhabilitado)
                        {
                            _AlexMomentaneo = true;
                        }                        
                        Invoke("PrenderBotonesAux", 0.6f);
                    }
                }
                _contFramesDown += 1;
            }
           

        }
    }


    private IEnumerator EmpezarFase1()
    {
        _Alexhabilitado = false;
        _minUpdateF = 100f;
        _maxUpdateF = 0f;
        CCanvasManager.Instance.PrenderEstimulos();
        _contRep = 0;
        _contRepUp = 0;
        _contRepDown = 0;
        _contFrames = 0;
        _contFramesUp = 0;
        _contFramesDown = 0;

        _estado = Estado.faseUp;
        _presentarEstimulos = true;
        _presentarEstimulosUp = true;
        while (_presentarEstimulos)
        {
            yield return null;
        }
        CCanvasManager.Instance.ApagarEstimulos();
      //  Invoke("BotonRaton", 1);
        yield break;
    }


    private void PrenderBotonesAux()
    {
        CCanvasManager.Instance.PrenderBotones();
    }

 


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------


    // Salir
    public void salirTest()    {
        CAudioManager.Instance.finreproducirVisSeg();
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
        _presentarEstimulos = false;
        _Alexhabilitado = false;
        _AlexMomentaneo = false;
        this.gameObject.SetActive(false);
        
    }

    public void Salir(bool salidaForzada){
        CAudioManager.Instance.finreproducirVisSeg();
        StopAllCoroutines();
        CCanvasManager.Instance.DesactivarPanelComun();   
        _presentarEstimulos = false;
        _Alexhabilitado = false;
        _AlexMomentaneo = false;
        this.gameObject.SetActive(false);
        CCanvasManager.Instance.DesactivarPanelComun();
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);

    }


    // Alex
    private bool _Alexhabilitado, _AlexMomentaneo;
    public void Alex()
    {
        if (_Alexhabilitado & _AlexMomentaneo)
        {
            _AlexMomentaneo = false;
            CCanvasManager.Instance.ApagarBotones();
            StartCoroutine(EmpezarFase1());
        }
    }

    public void Boton1()
    {
        _respuesta = "1";
        CCanvasManager.Instance.Respondio = true;
    }
    public void Boton2()
    {
        _respuesta = "2";
        CCanvasManager.Instance.Respondio = true;
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

    // calculo de nuevo offset
    private void CalcularOffset()
    {
        // CALCULAR EL PRÓXIMO OFFSET
        if (_respuesta == "1")
        {
            if (_respuestaAnterior == "2")
            {
                _pasoCambio = _pasoCambio - 1;
                if (_pasoCambio == 0)
                {
                    _pasoCambio = 1;
                }
            }
            _offset = _offset - _pasoCambio;
        }
        if (_respuesta == "2")
        {
            if (_respuestaAnterior == "1")
            {
                _pasoCambio = _pasoCambio - 1;
                if (_pasoCambio == 0)
                {
                    _pasoCambio = 1;
                }
            }
            
            _offset = _offset + _pasoCambio;
        }
        if (_offset < _offsetMinimo)
            _offset = _offsetMinimo;
        if (_offset > _offsetMaximo)
        {
            _offset = _offsetMaximo;
        }
        CalcularOffset2();

        // ACTUALIZAR RESPUESTA ANTERIOR
        _respuestaAnterior = _respuesta;
    }


    private void CalcularOffset2()
    {
        _offset2 = _offset;
        if (_offset2 < (_duracionPelota + 1))
        {
            _offset2 = _duracionPelota + 1;
        }
    }


    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------

   private IEnumerator CorrerEstimulo(int _indPalabra, AudioClip _audioAux, int _tipoEjPrTest)
    {
        _contador++;
        // Desabilitamos botones
        CCanvasManager.Instance.DeshabilitarRespuesta();
        CCanvasManager.Instance.ApagarBotones();
        // Actualizamos offset si es práctica
        Debug.Log("indice: " + _indPalabra);
        Debug.Log("offset: " + _offset);
        _repEnt =(int) Mathf.Round(2f * Application.targetFrameRate / (_offset+ _offset2));
        Debug.Log("Rep: " + _repEnt.ToString());
        if (_offset == _offsetEjemplo1Pelota)
        {
            _buttonCorrecto = _buttonsRespuesta[0];
            _respuestaCorrecta = "1";
        }
        else
        {
            _buttonCorrecto = _buttonsRespuesta[1];
            _respuestaCorrecta = "2";
        }
        Debug.Log("Respuesta correcta: " + _respuestaCorrecta);
        Debug.Log("Buton correcta: " + _buttonCorrecto); 
        // Mostramos estímulo
        yield return StartCoroutine(EmpezarFase1());
        yield return new WaitForSeconds(0.6f);
        //CCanvasManager.Instance.PrenderBotones();
        Debug.Log("Prendiendo botones");
        // Msotramos respuesta si es ejemplo
        if (_tipoEjPrTest == 0)
        {
            _Alexhabilitado = false;
            // esperamos a que termine el audio y un ratito más
            yield return StartCoroutine(CAudioManager.Instance.Esperar());
            yield return new WaitForSeconds(0.8f);
            // prendemos boton correcto
            CAudioManager.Instance.ReproducirSinEspera(_audioAux);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(CCanvasManager.Instance.apretarBoton2(_buttonCorrecto));

        }
        else if (_tipoEjPrTest == 1)// Sino esperamos respuesta, etc
        {
            // Esperamos la respuesta            
            _respuesta = "NA";
            _Alexhabilitado = true;
            _AlexMomentaneo = true;
            yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(null, _data.TiempoRespMaximo));
            _Alexhabilitado = false;
            _tResp = CCanvasManager.Instance.TLog;
            // Respondemos segun si le pego o no            
            _acierto = (CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
            if (_tipoEjPrTest == 1)
            {
                yield return StartCoroutine(CAudioManager.Instance.ReproducirResultado(_acierto));
            }

        }
        else if (_tipoEjPrTest == 2)// Sino esperamos respuesta, etc
        {
            // Esperamos la respuesta       
            _Alexhabilitado = true;
            _AlexMomentaneo = true;
            _respuesta = "NA";
            yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(null, _data.TiempoRespMaximo));
            _Alexhabilitado = false;
            _tResp = CCanvasManager.Instance.TLog;
            // Respondemos segun si le pego o no            
            _acierto = false;
            // Calculamos nuevo offset y actualizamos respuesta
            CalcularOffset();
        }
        
        CCanvasManager.Instance.ApagarBotones();

        // Loggeo        
        CLogManager.Instance.GuardarEstimuloVisSeg(_tipoEjPrTest, _trialPos, _numPrac, _offset, _respuesta, _tResp);

        // V2018
        output.addItem(CEstimuloVisSeg.GetEstimulo(_tipoEjPrTest, _trialPos, _numPrac, CLogManager.Instance._taskPos, _offset, _respuesta, _tResp));

        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
