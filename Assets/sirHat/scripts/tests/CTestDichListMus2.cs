using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTestDichListMus2 : MonoBehaviour {

    public GameObject _buhos, _buho1,_buho2,_buho3, _buho4;
    public float _rot;
    public Button _tapBUtton;
    public Text _tessto;
    // Variables
    private CListaTestDichListMus _data;
    private CEstimuloDichListMus _estimuloActual;
    private int _trialPos, _numPrac,  _targetPos;
    public CConfiguracion _configH;
    public bool _conf;

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

    // Corutina de entrada
    public IEnumerator CorrerTest()
    {

        // Inicio
        Debug.Log("Empezando Test DichList Mus");
        inicializar();    
  //      yield return new WaitForSeconds(1f);     
		//if (_data.TieneInstrucciones())
		//{
		//	yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
		//}
		
  //      // Ejemplo
		//for (int _i = 0; _i < _data.CantEjemplos; _i++)
		//{
		//	yield return StartCoroutine(CorrerEstimulo(_i, null, 0));
		//}

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
                CCanvasManager.Instance.FinPractica();               
                yield return StartCoroutine(CAudioManager.Instance.VolverAPracticar());
                yield return StartCoroutine(esperarRespuestaPractica());
            }
        }

        // Test
        _trialPos = 0;
        int[] _indicesTest;        
       _indicesTest = _data.IndicesTest;
        yield return StartCoroutine(CAudioManager.Instance.Comenzar());
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareaDichListMus();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CCanvasManager.Instance.ApagarMano();
        salirTest();
    }


    private void RotarBuhos(float _rotar)
    {
        _buho1.transform.rotation = Quaternion.Euler( 0f, 0f, _rotar);
        _buho2.transform.rotation = Quaternion.Euler( 0f, 0f, -_rotar);
        _buho3.transform.rotation = Quaternion.Euler( 0f, 0f, _rotar);
        _buho4.transform.rotation = Quaternion.Euler( 0f, 0f, -_rotar);
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
                if (_ladoBuho)
                {
                    RotarBuhos(_rot);
                }
                else
                {
                    RotarBuhos(-_rot);
                }
                _ladoBuho = !_ladoBuho;
            }


            if (2 * _tSampleActual > (2 * n - 1) * Tsamples)
            {               
                RotarBuhos(0f);                
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
				tambor.transform.localScale = 0.95f*Vector3.one;
                if (_ladoBuho)
                {
                    RotarBuhos(_rot);
                }
                else
                {
                    RotarBuhos(-_rot);
                }
                _ladoBuho = !_ladoBuho;
            }
            if (2 * _tSampleActual > (2 * n - 1) * Tsamples)
            {
                mano.transform.localScale = Vector3.one;
				tambor.transform.localScale = Vector3.one;
                RotarBuhos(0f);
            }
        }


    }


    private bool _feedbackEN;
    float _tresp, _trespAnterior, _treal;    
    float _tol = 150f / 1000f;
    public GameObject _feedback;
    private IEnumerator FeedbackVisual()
    {
        Debug.Log("corriendo corutina");        
        _feedback.SetActive(true);
        StartCoroutine(AchicarGlobo());
        _trespAnterior = Time.time;
        while (_feedbackEN)
        {
           
            yield return new WaitForSeconds(_treal);
        }
        _feedback.SetActive(false);
        Debug.Log("Terminando corutina");
        yield break;
    }

    private IEnumerator AchicarGlobo()
    {
        while (_feedbackEN)
        {
            if (Vector3.Magnitude(_feedback.transform.localScale) > 0.5f * Vector3.one.magnitude)
            {
                //_feedback.transform.localScale = 1.1f * _feedback.transform.localScale;
                _feedback.transform.localScale -= 0.005f * Vector3.one;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }


    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        taps = new List<float>();
        Estado = estado.idle;
        _buhos.SetActive(true);
        mano.SetActive(false);
        tambor.SetActive(true);
        _tapBUtton.interactable = false;
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CLogManager.Instance.IncrementarTaskPos();
        //_data = CInfoManager.Instance.DataDichListMus();
      //  _data = CInfoManager.Instance._dataDichListMus;
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;
        RotarBuhos(0f);
        //_data.debugTestData();
        _feedback.SetActive(false);

    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------
//
//    public void tap()
//    {
//        float _tiempoAux = CAudioManager.Instance.TimeAudioSource();
//        taps.Add(_tiempoAux);
//        _tessto.text = (_tiempoAux - dt).ToString();
//        dt = _tiempoAux;
//    }

	void OnGUI()
	{
		if (Event.current.type == EventType.MouseDown) {
            if (_respuestaHabilitada)
            {
                taps.Add(Time.time - tiempoInicial);
                _tresp = Time.time - _trespAnterior;
                _trespAnterior = Time.time;
                if (Mathf.Abs(_tresp - _treal) < _tol && ((Vector3.Magnitude(_feedback.transform.localScale) < 2.8f * Vector3.one.magnitude)))
                {
                    _feedback.transform.localScale += 0.075f * Vector3.one;
                }
            }
			tambor.transform.localScale = 0.95f*Vector3.one;
		}
		if (Event.current.type == EventType.MouseUp) {
			tambor.transform.localScale = Vector3.one;
		}
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
        // Desabilitamos botones
        _buhos.SetActive(true);       
        tambor.SetActive(true);

        // Actualizamos info actual
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);
        
        Tsamples = (int)(1 / _estimuloActual.FreqInt * _estimuloActual.Audios[0].samples / _estimuloActual.Audios[0].length);
        _treal = 1f / _estimuloActual.FreqInt;


        if (_tipoEjPrTest == 0)
        {
            Estado = estado.ejemplo;
            mano.SetActive(true);
        }
        else
        {
            _tapBUtton.interactable = true;
            Estado = estado.test;
        }
        if(_tipoEjPrTest == 1)
        {
            _feedbackEN = true;
            StartCoroutine(FeedbackVisual());
        }

        _respuestaHabilitada = true;
        n = 0;
        tiempoInicial = Time.time;
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_estimuloActual.Audios[0]));
        _respuestaHabilitada = false;
        _feedbackEN = false;
        // CAudioManager.Instance.ReproducirSinEspera(_estimuloActual.Audios[0]);
        Estado = estado.idle;
        mano.SetActive(false);
        _buhos.SetActive(false);
        tambor.SetActive(false);
        _tapBUtton.interactable = false;

        // Loggeo  
       // contador = CLogManager.Instance.GuardarEstimuloDichListMus(_estimuloActual, taps,  contador);
		taps.Clear ();
        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
