using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTapping2 : MonoBehaviour {

    public GameObject _bomba;
    public Text _tessto;
    public Sprite _botonOn, _botonOff;
    public Image _imgBoton;
    // Variables
    //private CListaTestSegundaParte _data;
    private CListaTestSegundaParte _data;
    public CanvasGroup _fondoNegro;
    private CEstimuloSegundaParte _estimuloActual;
    private int _trialPos, _numPrac,  _targetPos;
    public CConfiguracion _configH;
    public bool _conf;
    public AudioClip _practica3Aplicador, _tapping_I2_vovlerAentrar, _tapping_I3_vovlerAsalir, _explosion;


	// Cosas robadas de tapping
	enum estado {test, idle, ejemplo, instrucciones, guardado};
	estado Estado;
	List<float> taps;
    public GameObject tambor;
    public GameObject mano;
    private int n, Tsamples, _tSampleActual;
    private float dt = 0;
    private bool _ladoBuho;
    private int contador = 0;
	private float tiempoInicial = 0;
    private bool _respuestaHabilitada;
    public bool _acierto;

    private bool _feedbackEN;
    float _tresp, _trespAnterior, _treal;
    float _tol = 150f / 1000f;
    public GameObject _feedback;

    public GameObject _panelCargando;

    // V2018
    Output output;

    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }

    // Corutina de entrada
    public IEnumerator CorrerTest()
    {

        // Inicio
        Debug.Log("Empezando Test Tapping");
        inicializar();
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));

        // Ejemplo
        for (int _i = 0; _i < _data.CantEjemplos; _i++)
        {
            _bomba.SetActive(true);
            yield return StartCoroutine(CorrerEstimulo(_i, null, 0));
            _bomba.SetActive(false);
        }

        // Practica
        if (_data.CantPractica != 0)
        {
            yield return StartCoroutine(CAudioManager.Instance.APracticar());
            yield return StartCoroutine(CAudioManager.Instance.Tetocati());
            CCanvasManager.Instance.EmpezarPractica();
            _numPrac = -1;
            while (CCanvasManager.Instance.PracticarDeVuelta)
            {
                _numPrac += 1;
                _trialPos = 0;
                for (int _cont = _data.CantEjemplos; _cont < _data.CantEjemplos + _data.CantPractica; _cont++)
                {
                    _trialPos += 1;
                    yield return new WaitForSeconds(0.5f);
                    _bomba.transform.localScale = Vector3.one;
                    _bomba.SetActive(true);
                    yield return StartCoroutine(CorrerEstimulo(_cont, null, 1));
                    _bomba.SetActive(false);
                    yield return null;
                }                
                if (_acierto)
                {
                    yield return StartCoroutine(ExplotarBomba());
                    yield return StartCoroutine(CAudioManager.Instance.VolverAPracticar());
                }
                else
                {                    
                    yield return StartCoroutine(CAudioManager.Instance.Reproducir(_practica3Aplicador));
                    CCanvasManager.Instance.pausar();
                    while (CCanvasManager.Instance.Pausado)
                    {
                        yield return null;
                    }
                }
                CCanvasManager.Instance.FinPractica();
                yield return StartCoroutine(esperarRespuestaPractica());
                if (CCanvasManager.Instance.PracticarDeVuelta & _acierto)
                {
                    _fondoNegro.alpha = 0f;
                }
            }
        }
        // Si le pifió pero quiere seguir pelamos el buho para que vuelva en los fillers
        if (!_acierto)
        {
            yield return StartCoroutine(ExplotarBomba());
        }


        // Test
        _trialPos = 0;        
        int[] _indicesTest;        
       _indicesTest = _data.IndicesTestMixed;
       // _buho.SetActive(false);
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_tapping_I2_vovlerAentrar));
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            if(_cont ==4)
            {
                yield return new WaitForSeconds(0.5f);
                _bomba.SetActive(true);
                _fondoNegro.alpha = 0f;
                _bomba.transform.localScale = Vector3.one;
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_tapping_I3_vovlerAsalir));                
                yield return StartCoroutine(CorrerEstimulo(_data.IndicesFiller[0], null, 3));
                yield return StartCoroutine(ExplotarBomba());
                _bomba.SetActive(false);
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_tapping_I2_vovlerAentrar));
                _trialPos += 1;
            }     
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareaTapping();
        CLogManager.Instance.GuardarLista("tapping");
        CCanvasManager.Instance.ApagarMano();
        //salirTest();
        Salir(false);
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (Estado == estado.test)
        {
           // if (n == 0) tiempoInicial = Time.time;
            _tSampleActual = CAudioManager.Instance.TimeSample();
            if (_tSampleActual > n * Tsamples)
            {
                n++;
            }
        }
        else if (Estado == estado.idle)
        {

        }
        else if(Estado == estado.ejemplo)
        {

			//if (n==0) tiempoInicial = Time.time;
            _tSampleActual = CAudioManager.Instance.TimeSample();
            if (_tSampleActual > n * Tsamples)
            {
                n++;
                mano.transform.localScale = Vector3.one * 0.8f;
                _imgBoton.sprite = _botonOn;
                //tambor.transform.localScale = 0.95f*Vector3.one;
                AgrandarBomba();
            }
            if (2 * _tSampleActual > (2 * n - 1) * Tsamples)
            {
                mano.transform.localScale = Vector3.one;
                _imgBoton.sprite = _botonOff;
                //tambor.transform.localScale = Vector3.one;
            }
        }


    }







    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Tapping);
        taps = new List<float>();
        Estado = estado.idle;
        mano.SetActive(false);
        tambor.SetActive(true);
        _fondoNegro.alpha = 0f;
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();
        _data = CInfoManager.Instance.DataTapping();
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloSegundaParte.GetHeaders());
        output.Save();


    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------

    public int _contCorrectas, _contTotal;
	void OnGUI()
	{
      
		if (Event.current.type == EventType.MouseDown) {
            if (_respuestaHabilitada)
            {
                taps.Add(Time.time - tiempoInicial);
                _tresp = Time.time - _trespAnterior;
                _trespAnterior = Time.time;
                if (Mathf.Abs(_tresp - _treal) < _tol) // && ((Vector3.Magnitude(_bomba.transform.localScale) < 2.8f * Vector3.one.magnitude)))
                {
                    _contCorrectas += 1;
                    AgrandarBomba();
                }
            }
            _imgBoton.sprite = _botonOn;
        }
		if (Event.current.type == EventType.MouseUp) {
            _imgBoton.sprite = _botonOff;
        }
	}

    private IEnumerator AchicarBomba()
    {
        while (_feedbackEN)
        {
            if (Vector3.Magnitude(_bomba.transform.localScale) > 1f * Vector3.one.magnitude)
            {
                _bomba.transform.localScale -= 0.003f * Vector3.one;
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }


    private void AgrandarBomba()
    {
        _bomba.transform.localScale *= 1.02f;
    }

    private IEnumerator VolverBuho()
    {
        _bomba.transform.localScale = Vector3.one;

        yield break;
    }

    private IEnumerator ExplotarBomba()
    {
        StartCoroutine(CAudioManager.Instance.Reproducir(_explosion));        
        float _valInicial = _fondoNegro.alpha;
        float _startTime = Time.time;
        float _overTime = 2f;
        while (Time.time < _startTime + _overTime)
        {
            _fondoNegro.alpha = Mathf.Lerp(_valInicial, 1f, (Time.time - _startTime) / _overTime);            
            yield return null;
        }
        _fondoNegro.alpha = 1f;         
        yield return new WaitForSeconds(2f);      
        yield break;
    }


    private bool EvaluarPractica()
    {
        bool _aciertoAux=_contCorrectas>(Mathf.Round(n/2f));
        return _aciertoAux;
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
        // Inicializaciones de estímulo nuevo     
        tambor.SetActive(true);
        _contCorrectas = 0;
        n = 0;
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);

        Tsamples = (int)(1 / _estimuloActual.FrecFloat * _estimuloActual.Audios[0].samples / _estimuloActual.Audios[0].length);
        _treal = 1f / _estimuloActual.FrecFloat;
        // Prendemos y habilitamos cosas según tipo de estímulo
        if (_tipoEjPrTest == 0)
        {
            Estado = estado.ejemplo;
            mano.SetActive(true);
        }
        else
        {
            Estado = estado.test;
        }
        if(_tipoEjPrTest == 1)
        {
            _feedbackEN = true;
            StartCoroutine(AchicarBomba());
        }
        _respuestaHabilitada = true;
        // Vamos a mostrar estímulo en si
        tiempoInicial = Time.time;
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_estimuloActual.Audios[0]));
        // Apagamos cosas luego de estímulo
        _respuestaHabilitada = false;
        _imgBoton.sprite = _botonOff;
        _feedbackEN = false;
        Estado = estado.idle;
        mano.SetActive(false);
        // Para práctica
        if (_tipoEjPrTest == 1)
        {
            _acierto = EvaluarPractica();
        }
        tambor.SetActive(false);

        // Loggeo  
        contador = CLogManager.Instance.GuardarEstimuloTapping(_estimuloActual, _numPrac, _trialPos, "NA", taps,  contador);

        // V2018
        // TODO: Guardado Test
        output.addItem(_estimuloActual.GetEstimulos(_numPrac, _trialPos, "NA", taps));


        taps.Clear ();
        yield return new WaitForSeconds(0.5f);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
