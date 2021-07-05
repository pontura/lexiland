using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTestVisualEnt : MonoBehaviour {

    public Image _target;
    public Sprite _pelota, _planeta,_noTarget;
    public Image[] _estimuloVisual;
    public Button[] _buttonsRespuesta;
    public GameObject[] _estimuloVisualGO, _buttonsRespuestaGO;

    // Variables
    private CListaTestVisEnt _data;
    private CEstimuloVisEnt _estimuloActual;
    private int _trialPos, _numPrac;
    string _targetPos;
    public CConfiguracion _configH;
    public bool _conf;
    private bool _presentarEstimulos;
    enum Estado { fase1, fase1On, fase1Off, fase2 , fase2Off, fase2Target,fase2Mascara};
    Estado _estado;
    private int _frameCount, _repCount,_offset, _durTarget, _onsetMask;
    public Sprite _targetSprite;
    private string _respuesta, _tResp, _respuestaCorrecta;
    private bool _acierto, _repetir;
    private Button _buttonCorrecto;
    public Text _frameRateDisplay, _fixedDisplay, _minUpdateDis, _maxUpdateDis;
    public AudioClip _vamosApracticar;
    public GameObject _panelCargando;

    // V2018
    Output output;

    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }



    void Awake()
    {
        Application.targetFrameRate = 30;
    }





    // Corutina de entrada
    public IEnumerator CorrerTest()
    {

        // Inicio
        Debug.Log("Empezando Test Visual Ent");
        inicializar();    
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (_data.TieneInstrucciones())
		{
			yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
		}

        // Ejemplo
        for (int _i = 0; _i < _data.CantEjemplos; _i++)
        {
            yield return StartCoroutine(CorrerEstimulo(_i, _data.AudiosEjemplo[_i], 0));
        }

        // Practica
        if (_data.CantPractica != 0)
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_vamosApracticar));
            CCanvasManager.Instance.EmpezarPractica();
            _numPrac = -1;
            while (CCanvasManager.Instance.PracticarDeVuelta)
            {
                _numPrac += 1;
                _trialPos = 0;      
                for (int _cont = _data.CantEjemplos; _cont < _data.CantEjemplos + _data.CantPractica; _cont++)
                {
                    _repetir = true;
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
        }

        // Test
        _trialPos = 0;
        int[] _indicesTest;        
        _indicesTest = _data.IndicesTestMixed;
        //_indicesTest = _data.IndicesTest;
        yield return StartCoroutine(CAudioManager.Instance.Comenzar());
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareVisEnt();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CCanvasManager.Instance.ApagarMano();
        //salirTest();
        Salir(false);
    }

    private float _tiempoInicial, _Fupate, _Ffixedupate;
    private int _repEnt = 10;
    private int _contRep;
    private int _contFrames;
    private bool _apagar;
    private float _minUpdateF = 100f;
    private float _maxUpdateF = 0f;

    void Update()
    {        
        if (_presentarEstimulos)
        {
            _Fupate = 1f / Time.deltaTime;
            if (_Fupate > _maxUpdateF)
                _maxUpdateF = _Fupate;
            if (_Fupate < _minUpdateF)
                _minUpdateF = _Fupate;
            _frameRateDisplay.text = "Update: " + _Fupate.ToString();
            _minUpdateDis.text = "Update Min: " + _minUpdateF.ToString();
            _maxUpdateDis.text = "Update Max: " + _maxUpdateF.ToString();
            _Ffixedupate = 1f / Time.fixedDeltaTime;
            _fixedDisplay.text = "Fixed: " + _Fupate.ToString();
            _contFrames += 1;
            // Fase 1
            if (_estado == Estado.fase1On)
            {
                if (_contFrames % 3 == 1)
                {
                   // Debug.Log("contFramesOn: " + _contFrames);
                    // Prender pelota
                    _target.sprite = _pelota;                   
                }
                if (_contFrames % 3 == 2)
                {
                   // Debug.Log("contFramesOff: " + _contFrames);
                    // Apagar pelota
                    _target.sprite = _noTarget;
                    _contRep += 1;
                }
                if (_contRep == _repEnt)
                {
                    // Pasar a fase 2
                    _estado = Estado.fase2Off;
                    _contFrames = 1;
                }

            }

            // Fase 2 Off
            if (_estado == Estado.fase2Off)
            {
                if (_contFrames % (_offset+1) == 0)
                {
                    // Prendo Target
                    _target.sprite = _targetSprite;
                }
                if (_contFrames % (_offset + _durTarget) == 0)
                {
                    // Apago Target
                    _target.sprite = _noTarget;
                }
                if (_contFrames % (_onsetMask) == 0)
                {
                    // Prendo Máscara
                    _target.sprite = _pelota;
                }
                if (_contFrames % (_onsetMask + 1) == 0)
                {
                    // Apago todo
                    _target.sprite = _noTarget;
                    _presentarEstimulos = false;
                    if (_Alexhabilitado)
                    {
                        _AlexMomentaneo = true;
                    }
                   
                    Invoke("PrenderBotonesAux", 0.6f);
                }
            }
           
        }
    }

   

    //void Update()
    //{
    //    if (_presentarEstimulos)
    //    {
           
    //    }
    //}


    private IEnumerator EmpezarFase1()
    {
        _Alexhabilitado = false;
        _minUpdateF = 100f;
        _maxUpdateF = 0f;
        CCanvasManager.Instance.PrenderEstimulos();
        _contRep = 0;
        _contFrames = 0;
        _tiempoInicial = Time.time;
        //_target.sprite = _pelota;
        _estado = Estado.fase1On;
        _presentarEstimulos = true;
        while (_presentarEstimulos)
        {
            yield return null;
        }
        CCanvasManager.Instance.ApagarEstimulos();
      //  Invoke("BotonRaton", 1);
        yield break;
    }




    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        _presentarEstimulos = false;
        _Alexhabilitado = false;
        _AlexMomentaneo = false;
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.VisEn);
        CCanvasManager.Instance.SetearBotonesGO(_buttonsRespuestaGO);
        CCanvasManager.Instance.SetearEstimulosGO(_estimuloVisualGO);
        CCanvasManager.Instance.SetearEstimulos(_estimuloVisual);
        CCanvasManager.Instance.SetearBotones(_buttonsRespuesta);
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();       
		_data = CInfoManager.Instance.DataVisEnt();
        CCanvasManager.Instance.ActivarPanelComun();
        CCanvasManager.Instance.ApagarBotones();
        CCanvasManager.Instance.ApagarEstimulos();
        //StartCoroutine(TestTimeInterval());


        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloVisEnt.GetHeaders());
        output.Save();

    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------


    // Salir
    public void salirTest()    {
        
        StopAllCoroutines();
        if (!_conf)
        {
            CCanvasManager.Instance.StartCoroutine("vueltaDeTest");    
        }
        else
        {
            _configH.gameObject.SetActive(true);            
            _conf = false;
        }
        _presentarEstimulos = false;
        _Alexhabilitado = false;
        _AlexMomentaneo = false;
        CCanvasManager.Instance.DesactivarPanelComun();
        this.gameObject.SetActive(false);
        
    }

    public void Salir(bool salidaForzada){
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
    private void PrenderBotonesAux()
    {
        CCanvasManager.Instance.PrenderBotones();
    }



    public void BotonViAlgo()
    {
        _respuesta = "1";
        CCanvasManager.Instance.Respondio = true;
    }
    public void BotonNoViNada()
    {
        _respuesta = "0";
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
    


    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------

   private IEnumerator CorrerEstimulo(int _indPalabra, AudioClip _audioAux, int _tipoEjPrTest)
    {
        // Desabilitamos botones
        CCanvasManager.Instance.DeshabilitarRespuesta();
        CCanvasManager.Instance.ApagarBotones();       
        // Actualizamos info actual
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);
        if(_estimuloActual.CorrResp == "1")
        {
            Debug.Log("planeta");
            _targetSprite = _planeta;
            _buttonCorrecto = _buttonsRespuesta[0];
        }
        else if(_estimuloActual.CorrResp == "0")
        {
            Debug.Log("nada");
            _targetSprite = _noTarget;
            _buttonCorrecto = _buttonsRespuesta[1];
        }
       
        _targetPos = _estimuloActual.CorrResp;
        _respuestaCorrecta = _estimuloActual.CorrResp;
        _offset = _estimuloActual.OffsetInt;
        _durTarget = _estimuloActual.OnsetMaskInt;
        //_durTarget = 1; 
        _onsetMask = _estimuloActual.OnsetMaskInt;

            
        // Mostramos estímulo
        yield return StartCoroutine(EmpezarFase1());
        yield return new WaitForSeconds(0.6f);
        CCanvasManager.Instance.PrenderBotones();
        // Msotramos respuesta si es ejemplo
        _respuesta = "NA";
        _tResp = "NA";
        if (_tipoEjPrTest == 0)
        {
            _Alexhabilitado = false;
            // esperamos a que termine el audio y un ratito más
            yield return StartCoroutine(CAudioManager.Instance.Esperar());
            yield return new WaitForSeconds(0.8f);
            // prendemos boton correcto
            // Audio ejemplo            
            CAudioManager.Instance.ReproducirSinEspera(_audioAux);            
            yield return StartCoroutine(CCanvasManager.Instance.apretarBoton2(_buttonCorrecto));

        }
        else // Sino esperamos respuesta, etc
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
        CCanvasManager.Instance.ApagarBotones();
        // Loggeo        
        CLogManager.Instance.GuardarEstimuloVisEnt(_estimuloActual, _numPrac, _trialPos, _tResp, _respuesta, _acierto.ToString());

        // V2018
        output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _tResp, _respuesta, _acierto.ToString()));

        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
