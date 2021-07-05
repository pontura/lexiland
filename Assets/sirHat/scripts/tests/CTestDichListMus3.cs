using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTestDichListMus3 : MonoBehaviour {

    public GameObject _buho;
    public Image _buhoImage;
    public Sprite _tamborOn, _tamborOff;
    public Image _tambor;
    public Sprite[] _buhoStates;
    public Sprite _buhoOn, _buhoOff;
    private int _indBuhos;
    public Text _tessto;
    // Variables
    private CListaTestSegundaParte _data;
    private CEstimuloSegundaParte _estimuloActual;
    private int _trialPos, _numPrac,  _targetPos;
    public CConfiguracion _configH;
    public bool _conf;
    public AudioClip _practica3Aplicador, _buhoVuela, _volverAdespertar;


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
        Debug.Log("Empezando Test DichList Mus");
        inicializar();
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        _buho.SetActive(true);
        if (_data.TieneInstrucciones())
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
        }

        // Ejemplo
        for (int _i = 0; _i < _data.CantEjemplos; _i++)
        {
           
            yield return StartCoroutine(CorrerEstimulo(_i, null, 0));
            BuhoOff();
           
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
                    yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                   
                    yield return StartCoroutine(CorrerEstimulo(_cont, null, 1));
                    yield return null;
                }
                
                if (_acierto)
                {
                    yield return StartCoroutine(VolarBuho());
                    CCanvasManager.Instance.FinPractica();
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
                    CCanvasManager.Instance.FinPractica();

                }

                yield return StartCoroutine(esperarRespuestaPractica());
                if (CCanvasManager.Instance.PracticarDeVuelta & _acierto)
                {
                    yield return StartCoroutine(VolverBuho());
                }
            }
        }
        // Si le pifió pero quiere seguir pelamos el buho para que vuelva en los fillers
        if (!_acierto)
        {
            yield return StartCoroutine(VolarBuho());
        }


        // Test
        _trialPos = 0;        
        int[] _indicesTest;        
       _indicesTest = _data.IndicesTest;
       // _buho.SetActive(false);
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_buhoVuela));
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            if(_cont ==7)
            {
                yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                yield return StartCoroutine(VolverBuho());
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_volverAdespertar));                
                yield return StartCoroutine(CorrerEstimulo(_data.IndicesFiller[0], null, 3));
                yield return StartCoroutine(VolarBuho());
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_buhoVuela));
                _trialPos += 1;
            }
            else if(_cont == 13)
            {
                yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                yield return StartCoroutine(VolverBuho());
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_volverAdespertar));
                yield return StartCoroutine(CorrerEstimulo(_data.IndicesFiller[1], null, 3));
                yield return StartCoroutine(VolarBuho());
                yield return StartCoroutine(CAudioManager.Instance.Reproducir(_buhoVuela));
                _trialPos += 1;
            }
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareaDichListMus();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CCanvasManager.Instance.ApagarMano();
        //salirTest();
        Salir(false);
    }


    private void AnimarBuho()
    {
        _buhoImage.sprite = _buhoStates[_indBuhos];
        _indBuhos += 1;
        if(_indBuhos == (_buhoStates.Length))
        {
            _indBuhos = 0;
        }
    }

    private void BuhoOn()
    {
        _buhoPrendido = true;
        _buhoImage.sprite = _buhoOn;
        _buho.transform.localPosition += new Vector3(0f, 15f);
    }


    private void BuhoOff()
    {
        if (_buhoPrendido)
        {
            _buhoImage.sprite = _buhoOff;
            _buho.transform.localPosition -= new Vector3(0f, 15f);
            _buhoPrendido = false;
        }
       
    }



    private IEnumerator VolverBuho()
    {
       // _buho.SetActive(true);
        _buho.GetComponent<Animator>().enabled = true;
        _buho.GetComponent<Animator>().SetTrigger("Volver");
        yield return new WaitForSeconds(4f);

        yield break;
    }

    private IEnumerator VolarBuho()
    {
        //_buho.SetActive(true);
        _buho.GetComponent<Animator>().enabled = true;
        _buho.GetComponent<Animator>().SetTrigger("Volar");
        yield return new WaitForSeconds(4f);
        //_buho.SetActive(false);
        yield break;
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
                _tambor.sprite = _tamborOn;
                //tambor.transform.localScale = 0.95f*Vector3.one;
                //AnimarBuho();
                BuhoOn();
            }
            if (2 * _tSampleActual > (2 * n - 1) * Tsamples)
            {
                mano.transform.localScale = Vector3.one;
                _tambor.sprite = _tamborOff;
				//tambor.transform.localScale = Vector3.one;
                BuhoOff();
            }
        }


    }


    private bool _feedbackEN;
    float _tresp, _trespAnterior, _treal;    
    float _tol = 150f / 1000f;
    public GameObject _feedback;




    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.DichListMus);
        taps = new List<float>();
        Estado = estado.idle;
        mano.SetActive(false);
        tambor.SetActive(true);
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();
        _data = CInfoManager.Instance.DataDLM();
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;
        _respuestaHabilitada = false;
        //_data.debugTestData();
        //_feedback.SetActive(false);

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloSegundaParte.GetHeaders());
        output.Save();

    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------

    private bool _buhoPrendido;

    public int _contCorrectas, _contTotal;
	void OnGUI()
	{
        if (_respuestaHabilitada)
        {
            if (Event.current.type == EventType.MouseDown)
            {

                taps.Add(Time.time - tiempoInicial);
                _tresp = Time.time - _trespAnterior;
                _trespAnterior = Time.time;
                if (Mathf.Abs(_tresp - _treal) < _tol)
                {
                    //AnimarBuho();
                    BuhoOn();
                    _contCorrectas += 1;
                    // _feedback.transform.localScale += 0.075f * Vector3.one;
                }

                _tambor.sprite = _tamborOn;
                // tambor.transform.localScale = 0.95f*Vector3.one;
            }
            if (Event.current.type == EventType.MouseUp)
            {
                _tambor.sprite = _tamborOff;
                //tambor.transform.localScale = Vector3.one;
                BuhoOff();
            }
        }
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
        _buho.GetComponent<Animator>().enabled = false;
        tambor.SetActive(true);
        _contCorrectas = 0;
        n = 0;
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);        
        Tsamples = (int)(1 / _estimuloActual.FrecFloat * _estimuloActual.Audios[0].samples / _estimuloActual.Audios[0].length);
        _treal = 1f / _estimuloActual.FrecFloat;
        // Prendemos y habilitamos cosas según tipo de estímulo
        if (_tipoEjPrTest == 0)
        {
            _respuestaHabilitada = false;
            Estado = estado.ejemplo;
            mano.SetActive(true);
        }
        else
        {
            _respuestaHabilitada = true;
            Estado = estado.test;
        }
        if(_tipoEjPrTest == 1)
        {
            _feedbackEN = true;
        }
        
        // Vamos a mostrar estímulo en si
        tiempoInicial = Time.time;
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_estimuloActual.Audios[0]));
        // Apagamos cosas luego de estímulo
        _respuestaHabilitada = false;
        _feedbackEN = false;
        _tambor.sprite = _tamborOff;
        Estado = estado.idle;
        mano.SetActive(false);
        // Para práctica
        if (_tipoEjPrTest == 1)
        {
            _acierto = EvaluarPractica();
        }
        tambor.SetActive(false);

        // Loggeo  
        contador = CLogManager.Instance.GuardarEstimuloDichListMus(_estimuloActual, _numPrac, _trialPos, "NA", taps,  contador);
        // V2018
        output.addItem(_estimuloActual.GetEstimulos(_numPrac, _trialPos, "NA", taps));

        taps.Clear ();


        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
