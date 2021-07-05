using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CIdentificacion : MonoBehaviour {

    // Variables publicas a setear
    public CTablero _tablero;
    public Image[] _estimuloVisual;
    public Button _buttonSi, _buttonNo;
    public GameObject _buttonPause, _buttonUnPause, _panelPausa, _panelFinPractica;
    public GameObject[] _estimuloVisualGO;
    public CsvReadWriteLinebyLine _csvWriter;

    // Variables internas
    private AudioSource _audio;
    private AudioClip _AInstrucciones, _AEjemplo1, _AEjemplo2, _AEjemplo3, _APracticar, _AComenzar, _Abien, _Amal, _AFin, _APausa, _ABienvenido, _AVolverAPracticar, _APulsaResp;
    private CDiccionarioIdentificacion _diccionario;
    private CPalabraIdentificacion _palabraActual;
    private Button _buttonCorrecto;
    private int _nroSilabas;
    private int _igualSilaba;
    private string _struct;
    private bool _respondio, _repetir, _respondioPractica, _practicarDeVuelta, _pausado;
    private int _respuesta;
    private int _estBien, _estMal;
    private int[] _indicesTest;
    private int _cantEjEje, _cantEjPra, _cantEjTest;
    public enum tipoTest { Silabas, Fonemas };
    public tipoTest _tipoTest;
    private GameObject[] _buttonsGO;
    private Color _butApretado;
    private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tentreIeA, _tTotalPausa, _tini;

    // Datos para loggear
    private string _pTask;
    private int _trialPos, _numPrac;
    private bool _acierto;

    public CConfiguracion _configH;
    public bool _conf;




    void Start()
    {
      //  StartCoroutine(TestIdentificacion());
    }


    ////----------------------METODOS -----------------------------

    //// Test
    private IEnumerator TestIdentificacion()
    {
        // Seteos iniciales
        inicializar();

        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                ////// Instrucciones y ejemplos
                yield return StartCoroutine(reproducirAudio(_ABienvenido));
                yield return StartCoroutine(reproducirAudio(_AInstrucciones));
                yield return StartCoroutine(ejemplo(0, 2f, _AEjemplo1));
                yield return StartCoroutine(ejemplo(1, 2f, _AEjemplo2));
                yield return StartCoroutine(ejemplo(2, 2f, _AEjemplo3));
                break;
            default:
                // No hay instrucciones ni ejemplos xq viene a continuación del otro
                break;
        }



        // Práctica
        // Practica
        // Queres practicar?
        _audio.clip = _APracticar;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        // Hay un boton de pausa
        // yield return StartCoroutine(mostrarPausa());
        // Pulsa la respuesta correcta
        _audio.clip = _APulsaResp;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));



        _practicarDeVuelta = true;
        _numPrac = -1;
        while (_practicarDeVuelta)
        {
            _numPrac += 1;
            _trialPos = 0;
            if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
            for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
            {
                _trialPos += 1;
                _repetir = true;
                while (_repetir)
                {

                    yield return new WaitForSeconds(_tInterEnsayo);
                    yield return StartCoroutine(practica(_cont));
                    if (_respondio & _respuesta == _igualSilaba)
                    {
                        _repetir = false;
                    }
                    yield return null;
                }
            }
            desactivarVisual();
            _panelFinPractica.SetActive(true);
            yield return StartCoroutine(reproducirAudio(_AVolverAPracticar));
            yield return StartCoroutine(esperarRespuestaPractica());

        }
        _panelFinPractica.SetActive(false);


        // Test
        _trialPos = 0;
        desactivarVisual();
        //activarBotones();
        _audio.clip = _AComenzar;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        _indicesTest = mezclarIndices(_indicesTest);
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_tInterEnsayo);
            yield return StartCoroutine(testPosta(_indicesTest[_cont]));
        }
        desactivarVisual();
        salirTestIdentificacion();
        yield break;
    }




    // Inicializacion
    private void inicializar()
    {
        _estBien = 0;
        _estMal = 0;
        _cantEjEje = 0;
        _cantEjPra = 0;
        _cantEjTest = 0;
        _tiempoMaxResp = 30.0f;
        _tInterEnsayo = 0.1f;
        _audio = this.GetComponent<AudioSource>();
        _butApretado = _buttonSi.colors.pressedColor;
        _buttonsGO = new GameObject[2];
        _buttonsGO[0] = _buttonSi.gameObject;
        _buttonsGO[1] = _buttonNo.gameObject;
        cargarDiccionario();
        cargarAudios();
        desactivarVisual();
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _pTask = "identificationS";
                break;
            default:
                _pTask = "identificationP";
                break;
        }
    }



    ////--------------------------------------------------------------------

    // Boton de salida
    public void salirTestIdentificacion()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _configH.marcarTareaIdentificacionS();
                break;
            default:
                _configH.marcarTareaIdentificacionF();
                break;
        }
        StopAllCoroutines();
        if (!_conf)
        {
            _tablero.StartCoroutine("vueltaDeTest");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        _configH.SalirDeConfiguracion();
        _conf = false;
    }

    public void SetearTipoTestSilabas()
    {
        _tipoTest = tipoTest.Silabas;
    }
    public void SetearTipoTestFonemas()
    {
        _tipoTest = tipoTest.Fonemas;
    }
    // Carga del diccionario
    private void cargarDiccionario()
    {
        _diccionario = new CDiccionarioIdentificacion();
        // Variables para rellenar la palabra
        AudioClip _audioLetra, _audioPalabra;
        Sprite _imagen;
        int _nroSilabas, _nroFOnemas, _sylPosition;
        string _trialCode, _target, _struct;
        int _corrRespuesta;

        string _path;
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _path = "inputs/identificacionS";
                break;
            default:
                _path = "inputs/identificacionF";
                break;
        }
        TextAsset _fileData = Resources.Load(_path) as TextAsset;
        string[] _lines = _fileData.text.Split("\n"[0]);
        //Texture2D _imageAux;
        for (int _i = 1; _i < _lines.Length; _i++)
        {

            string[] _lineData = (_lines[_i].Trim()).Split(","[0]);
           

            // Audios
             _audioLetra = Resources.Load<AudioClip>("audios/identificacion/" + _lineData[13]);
             _audioPalabra = Resources.Load<AudioClip>("audios/identificacion/" + _lineData[14]);

            // Imagenes
            _imagen = Resources.Load<Sprite>("imagenes/" + _lineData[3]);

            // Resto de las cosas
            _trialCode = _lineData[1];
            _target = _lineData[2];
            _corrRespuesta = int.Parse(_lineData[7]);
            _nroSilabas = int.Parse(_lineData[8]);
            _nroFOnemas = int.Parse(_lineData[9]);
            _struct = _lineData[10];
            _sylPosition = int.Parse(_lineData[11]);

              //public CPalabraIdentificacion(string _tc, string _tar, string _st, Sprite _v, AudioClip _a1, AudioClip _a2, int _sils, int _fons, int _sPos, bool _resp)
            _diccionario.agregarPalabra(_i - 1, new CPalabraIdentificacion(_trialCode, _target, _struct, _imagen, _audioLetra, _audioPalabra, _nroSilabas, _nroFOnemas, _sylPosition, _corrRespuesta));


            if (int.Parse(_lineData[12]) == 1)
            {
                _cantEjPra += 1;
            }
            else if (int.Parse(_lineData[12]) == 0)
            {
                _cantEjEje += 1;
            }
            else if (int.Parse(_lineData[12]) == 2)
            {
                _cantEjTest += 1;
            }
        }
        _indicesTest = new int[_cantEjTest];
        for (int _i = _cantEjPra + _cantEjEje; _i < _cantEjPra + _cantEjEje + _cantEjTest; _i++)
        {
            _indicesTest[_i - (_cantEjPra + _cantEjEje)] = _i;
        }
    }


    // Carga de los audios
    private void cargarAudios()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:                
                _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/identificacionS_I1");
                _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/identificacionS_E1_platillo");
                _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/identificacionS_E2_rayuela");
                _AEjemplo3 = Resources.Load<AudioClip>("audios/instrucciones/identificacionS_E3_tenedor");
                
                break;
            default:

                break;
        }
        _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa");
        _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
        _AFin = Resources.Load<AudioClip>("audios/instrucciones/globo");
        _APausa = Resources.Load<AudioClip>("audios/instrucciones/Pausa");
        _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
        _APulsaResp = Resources.Load<AudioClip>("audios/instrucciones/pulsarespuesta");
        _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
        _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");
        _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar");

    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------

    // Boton tick
    public void botonTick()
    {
        _respuesta = 1;
        _respondio = true;
    }

    // Boton de no repetir la practica
    public void botonContinuarTest()
    {
        _practicarDeVuelta = false;
        _respondioPractica = true;
    }

    // Boton de repetir practica
    public void botonRepetirPractica()
    {
        _practicarDeVuelta = true;
        _respondioPractica = true;
    }

    // Boton cruz
    public void botonCross()
    {
        _respuesta = 0;
        _respondio = true;
    }

    // Salir
    public void salirTestAislamiento()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _configH.marcarTareaAisS();
                break;
            default:
                _configH.marcarTareaAisF();
                break;
        }
        StopAllCoroutines();
        if (!_conf)
        {
            _tablero.StartCoroutine("vueltaDeTest");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        _configH.SalirDeConfiguracion();
        _conf = false;

    }





    // Boton pausa
    public void pausar()
    {
        _pausado = true;
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        // Debug.Log("pausandooooo");
        _panelPausa.SetActive(true);
        if (_audio.isPlaying) _audio.Pause();
        //Time.timeScale = 0;
    }

    // Boton unpausar
    public void unPausar()
    {
        _pausado = false;
        _tTotalPausa = Time.time - _tini;
        _tini = Time.time;
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        // Debug.Log("despausandooooo");
        _panelPausa.SetActive(false);
        _audio.UnPause();
        // Time.timeScale = 1;
    }





    // ESPERAS  --------------------------------------------------------------------------------

    // Esperar que responda si quiere practicar de vuelta
    public IEnumerator esperarRespuestaPractica()
    {
        while (!_respondioPractica)
        {
            yield return null;
        }
        _respondioPractica = false;
        yield break;
    }


    // Esperar respuesta
    private IEnumerator esperarRespuesta()
    {
        _buttonSi.interactable = true;
        _buttonNo.interactable = true;
        _respondio = false;
        _tini = Time.time;
        _tResp = 0f;
        _tTotalPausa = 0f;
        while (!_respondio & _tResp < _tiempoMaxResp)
        {
            if (!_pausado)
            {
                _tResp = Time.time - _tini;
            }
            yield return null;
        }
        if (!_respondio)
        {
            _tResp = _tiempoMaxResp;
        }

        _buttonSi.interactable = false;
        _buttonNo.interactable = false;

        yield break;
    }

    // Esperar audio
    private IEnumerator esperarAudio(AudioSource _a)
    {
        while (_a.isPlaying)
        {
            yield return null;
        }
        yield break;
    }

    // FUNCIONES AUXILIARES --------------------------------------------------------------------------------

    // Reproducir audio
    private IEnumerator reproducirAudio(AudioClip _audioAux)
    {
        Debug.Log("reproduciendo: " + _audioAux);
        _audio.clip = _audioAux;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(0.2f);
        yield break;
    }


    // Apagar Visual
    private void desactivarVisual()
    {
        apagarEstimulo();
        apagarBotones();
    }

    // Activar visual
    private void activarVisual()
    {
        prenderBotones();
        prenderEstimulo();
    }

    // Apagar estimulo
    private void apagarEstimulo()
    {
        for (int _i = 0; _i < _estimuloVisualGO.Length; _i++)
        {
            _estimuloVisualGO[_i].SetActive(false);
          
        }
    }

    // Prender estímulo
    private void prenderEstimulo()
    {
        for (int _i = 0; _i < _estimuloVisualGO.Length; _i++)
        {
            _estimuloVisualGO[_i].SetActive(true);

        }
    }

    // Apagar botones
    private void apagarBotones()
    {
        _buttonsGO[0].SetActive(false);
        _buttonsGO[1].SetActive(false);
    }

    // Prender botones
    private void prenderBotones()
    {
        _buttonsGO[0].SetActive(true);
        _buttonsGO[1].SetActive(true);
    }



    // Mezclar indices
    private int[] mezclarIndices(int[] _indMixed)
    {
        for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = _indMixed[i];
            _indMixed[i] = _indMixed[r];
            _indMixed[r] = tmp;
        }
        return _indMixed;
    }


    // Mostrar pausa.
    private IEnumerator mostrarPausa()
    {
        _audio.clip = _APausa;
        _audio.Play();
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(true);
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(false);
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        yield break;
    }


    // deshabilitar respuesta
    private void deshabilitarRespuesta()
    {
        _buttonSi.interactable = false;
        _buttonNo.interactable = false;
    }

    // habilitar respuesta
    private void habilitarRespuesta()
    {
        _buttonSi.interactable = true;
        _buttonNo.interactable = true;
    }

    // Marcar botón correcto
    private Sprite apretarBoton(Button _button)
    {
        Sprite _imNormal, _imApretado;
        _imNormal = _button.GetComponent<Image>().sprite;
        _imApretado = _button.spriteState.pressedSprite;
        _button.GetComponent<Image>().sprite = _imApretado;
        _button.gameObject.transform.localScale = 1.3f * Vector3.one;
        return _imNormal;
    }



    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------

    // Ejemplos
    private IEnumerator ejemplo(int _indPalabra, float _tResp, AudioClip _audioAux)
    {
        // Desabilitamos botones
        deshabilitarRespuesta();
        //prenderBotones();
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _estimuloVisual[0].sprite = _palabraActual.obtenerEstimulo();
        _nroSilabas = _palabraActual.obtenerSilabas();
        _igualSilaba = _palabraActual.obtenerCorrecta();
        _struct = _palabraActual.obtenerStruct();
        if (_igualSilaba == 1)
        {
            _buttonCorrecto = _buttonSi;
        }
        else
        {
            _buttonCorrecto = _buttonNo;
        }
        _audio.clip = _audioAux;
        // Empezamos audio
        _audio.Play();
        // Activamos visual
        activarVisual();
        // esperamos a que termine el audio
        yield return StartCoroutine(esperarAudio(_audio));
        // prendemos boton correcto
        Sprite _imNormal;
        _imNormal = apretarBoton(_buttonCorrecto);
        // Esperamos un rato
        yield return new WaitForSeconds(_tResp);
        // apagamos boton correcto
        _buttonCorrecto.GetComponent<Image>().sprite = _imNormal;
        _buttonCorrecto.gameObject.transform.localScale = 1f * Vector3.one;
        // Habilitamos botones nuevamente 
        desactivarVisual();
        _estimuloVisual[0].sprite = null;
        _estimuloVisual[0].sprite = null;
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _igualSilaba.ToString(), "", _nroSilabas.ToString(),
        //    _palabraActual.obtenerStruct(), "0", _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _tablero._TaskCount.ToString(), "",
        //    _trialPos.ToString(), "NC", "NC", "NC");
        //_csvWriter.saveLine();
        yield return new WaitForSeconds(1f);
        yield break;
    }


    // Practica
    private IEnumerator practica(int _indPalabra)
    {
        // Desabilitamos botones
        deshabilitarRespuesta();
        //prenderBotones();
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _estimuloVisual[0].sprite = _palabraActual.obtenerEstimulo();
        _nroSilabas = _palabraActual.obtenerSilabas();
        _igualSilaba = _palabraActual.obtenerCorrecta();
        _struct = _palabraActual.obtenerStruct();
        if (_igualSilaba == 1)
        {
            _buttonCorrecto = _buttonSi;
        }
        else
        {
            _buttonCorrecto = _buttonNo;
        }
        // Mostramos dos imágenes
       
        activarVisual();



        // Audio primer palabra
        _audio.clip = _palabraActual.obtenerAudio1();
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(0.5f);

        // Audio segunda palabra
        _audio.clip = _palabraActual.obtenerAudio2();
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));


        //// Esperamos la respuesta
        yield return StartCoroutine(esperarRespuesta());


        // Respondemos segun si le pego o no
        if (_respondio & _respuesta == _igualSilaba)
        {
            _audio.clip = _Abien;
            _acierto = true;
        }
        else
        {
            _audio.clip = _Amal;
            _acierto = false;
        }
        _audio.Play();
        desactivarVisual();
        // loggeo
        //string _tlog;
        //if (_tResp < _tiempoMaxResp)
        //    _tlog = (_tResp + _tTotalPausa).ToString();
        //else
        //    _tlog = "NC";
        //string __resplog;
        //if (_respuesta == -1)
        //    __resplog = "NC";
        //else
        //    __resplog = _respuesta.ToString();
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _igualSilaba.ToString(), "", _nroSilabas.ToString(),
        //    _palabraActual.obtenerStruct(), "1", _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _tablero._TaskCount.ToString(),
        //    _numPrac.ToString(), _trialPos.ToString(), __resplog, _acierto.ToString(), _tlog);
        //_csvWriter.saveLine();
        yield return StartCoroutine(esperarAudio(_audio));
        yield break;
    }



    // Test
    private IEnumerator testPosta(int _indPalabra)
    {
        // Desabilitamos botones
        deshabilitarRespuesta();
        //prenderBotones();
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _estimuloVisual[0].sprite = _palabraActual.obtenerEstimulo();
        _nroSilabas = _palabraActual.obtenerSilabas();
        _igualSilaba = _palabraActual.obtenerCorrecta();
        _struct = _palabraActual.obtenerStruct();
        if (_igualSilaba == 1)
        {
            _buttonCorrecto = _buttonSi;
        }
        else
        {
            _buttonCorrecto = _buttonNo;
        }
        // Mostramos dos imágenes

        activarVisual();
        // Audio primer palabra
        _audio.clip = _palabraActual.obtenerAudio1();
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(0.5f);
        // Audio segunda palabra
        _audio.clip = _palabraActual.obtenerAudio2();
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        // Esperamos la respuesta
        yield return StartCoroutine(esperarRespuesta());
        //Actualizamos estadisticas
        if (_respondio & _respuesta == _igualSilaba)
        {
            _estBien++;
            _acierto = true;
        }
        else
        {
            _estMal++;
            _acierto = false;
        }
        //// loggeo
        //string _tlog;
        //if (_tResp < _tiempoMaxResp)
        //    _tlog = (_tResp + _tTotalPausa).ToString();
        //else
        //    _tlog = "NC";
        //string __resplog;
        //if (_respuesta == -1)
        //    __resplog = "NC";
        //else
        //    __resplog = _respuesta.ToString();
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _igualSilaba.ToString(), "", _nroSilabas.ToString(),
        //    _palabraActual.obtenerStruct(), "2", _palabraActual.obtenerPalabras()[0], _palabraActual.obtenerPalabras()[1], _tablero._TaskCount.ToString(),
        //    _numPrac.ToString(), _trialPos.ToString(), __resplog, _acierto.ToString(), _tlog);
        //_csvWriter.saveLine();
        yield break;

    }


}
