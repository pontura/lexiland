using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CSintesisOld : MonoBehaviour {


    // Variables publicas a setear
    public CTablero _tablero;
    public Image[] _opcionesImageEstimulo;
    public Button[] _buttonsRespuesta;  	// botones de respuesta
    public GameObject _buttonPause, _buttonUnPause, _panelPausa, _panelFinPractica;
    public GameObject[] _estimuloVisualGO;
    public CsvReadWriteLinebyLine _csvWriter;

    // Variables internas
    public AudioSource _audio;
    private AudioClip _AInstrucciones, _AEjemplo1, _AEjemplo2, _AEjemplo3, _APracticar, _AComenzar, _Abien, _Amal, _AFin, _APausa, _ABienvenido, _AVolverAPracticar, _APulsaResp;
    private AudioClip _audioLetra;
    private CDiccionarioSintesis _diccionario;
    private CPalabraSintesis _palabraActual;
    private Button _buttonCorrecto;

    public string _palabraCorrecta;
    public string[] _opcionesPalabras;
    public Sprite[] _opcionesPalabrasImagenes;

    private bool _respondio, _repetir, _respondioPractica, _practicarDeVuelta, _pausado;
    private string _respuesta;
    public int _estBien, _estMal;
    private int[] _indicesTest;
    private int _cantEjEje, _cantEjPra, _cantEjTest;
    public enum tipoTest { Silabas, Fonemas };
    public tipoTest _tipoTest;
    private Color _butApretado;
    private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tentreIeA, _tTotalPausa, _tini;



    // Datos para loggear
    private string _pTask;
    private int _trialPos, _numPrac, _targetPos;
    private bool _acierto;

    public CConfiguracion _configH;
    public bool _conf;




    void Start()
    {
        //StartCoroutine(TestSintesis());
    }


    ////----------------------METODOS -----------------------------

    //// Test
    private IEnumerator TestSintesis()
    {
            // Seteos iniciales
            inicializacion();

        ////// Instrucciones y ejemplos
        yield return StartCoroutine(reproducirAudio(_ABienvenido));
        yield return StartCoroutine(reproducirAudio(_AInstrucciones));
        yield return StartCoroutine(ejemplo(0, 2f, _AEjemplo1));
        yield return StartCoroutine(ejemplo(1, 2f, _AEjemplo2));


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
           // activarVisual();
            if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
            for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
            {
                _trialPos += 1;

                _repetir = true;
                while (_repetir)
                {

                    yield return new WaitForSeconds(_tInterEnsayo);
                    yield return StartCoroutine(practica(_cont));
                    if (_respondio & _respuesta == _palabraCorrecta)
                    {
                        _repetir = false;
                    }
                    yield return null;
                }
            }
           // desactivarVisual();
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
        //desactivarBotones();
        _audio.clip = _AFin;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        salirTestSintesis();
        yield break;
    }




    // Inicializacion
    protected void inicializacion()
    {
        
        cargarDiccionario();
        cargarAudios();
        _butApretado = _buttonsRespuesta[0].colors.pressedColor; 
        _tiempoMaxResp = 30.0f;
        _tInterEnsayo = 0.1f;
        _tentreIeA = 0.1f;
        _estBien = 0;
        _estMal = 0;
        _panelFinPractica.SetActive(false);
        _panelPausa.SetActive(false);
        deshabilitarRespuesta();
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _pTask = "blendingS";                
                break;
            default:
                _pTask = "blendingF";
                break;
        }
        desactivarVisual();
    }



    ////--------------------------------------------------------------------

    // Boton de salida
    public void salirTestSintesis()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _configH.marcarTareaSintesisS();
                break;
            default:
                _configH.marcarTareaSintesisF();
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


    // Carga del diccionario
    private void cargarDiccionario()
    {
        _diccionario = new CDiccionarioSintesis();
        // Variables para rellenar la palabra
        AudioClip _audioLetra1;
        string[] _palabras;
        Sprite[] _imagenes;
        int _nroSilabas, _nroFOnemas;
        string _trialCode, _target, _struct;
        
        

        string _path;
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _path = "inputs/sintesisS";
                break;
            default:
                _path = "inputs/sintesisF";
                break;
        }
        TextAsset _fileData = Resources.Load(_path) as TextAsset;
        string[] _lines = _fileData.text.Split("\n"[0]);
        //Texture2D _imageAux;
        for (int _i = 1; _i < _lines.Length; _i++)
        {

            string[] _lineData = (_lines[_i].Trim()).Split(","[0]);
           

            // Audios
            switch (_tipoTest)
            {
                case tipoTest.Silabas:
                    _audioLetra1 = Resources.Load<AudioClip>("audios/sintesis/" + _lineData[13]);
                    break;
                default:
                    _audioLetra1 = Resources.Load<AudioClip>("audios/sintesis/" + _lineData[13]);
                    break;
            }
            // Palabras
            if (_lineData[5] == "NA")
            {
                _palabras = new string[3];
                _imagenes = new Sprite[3];
                _palabras[0] = _lineData[3];
                _palabras[1] = _lineData[4];
                _palabras[2] = _lineData[6];
            }
            else
            {
                _palabras = new string[4];
                _imagenes = new Sprite[4];
                _palabras[0] = _lineData[3];
                _palabras[1] = _lineData[4];
                _palabras[2] = _lineData[5];
                _palabras[3] = _lineData[6];
            }
            Debug.Log(_palabras.ToString());
            // Imagenes
            for(int _j =0; _j<_palabras.Length; _j++)
            {
                _imagenes[_j] = Resources.Load<Sprite>("imagenes/" + _palabras[_j]);
            }           

            // Resto de las cosas
            _nroSilabas = int.Parse(_lineData[8]);
            _nroFOnemas = int.Parse(_lineData[9]);
            _trialCode = _lineData[1];
            _target = _lineData[2];
            _struct = _lineData[10];

            // CPalabraSintesis(string _tc, string _tar, string _st, string[] _pals, Sprite[] _v, AudioClip _a1, int _sils, int _fons)
            _diccionario.agregarPalabra(_i - 1, new CPalabraSintesis(_trialCode, _target, _struct, _palabras, _imagenes, _audioLetra1, _nroSilabas, _nroFOnemas));


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
                _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa");
                _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/sintesisS_I1");
                _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/sintesisS_E1_perro");
                _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/sintesisS_E2_tetera");
                break;
            default:
                _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa2");
                _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/sintesisF_I1");
                _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/sintesisF_E1_gorro");
                _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/sintesisF_E2_fruta");
                break;
        }
        _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
        _AFin = Resources.Load<AudioClip>("audios/instrucciones/globo");
        _APausa = Resources.Load<AudioClip>("audios/instrucciones/Pausa");
        _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
        _APulsaResp = Resources.Load<AudioClip>("audios/instrucciones/pulsarespuesta");
        _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
        _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");
        _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar");

    }

    // Deshabilitar botones respuesta
    private void deshabilitarRespuesta()
    {
        for (int _i = 0; _i < _buttonsRespuesta.Length; _i++)
        {
            _buttonsRespuesta[_i].interactable = false;
        }
    }
    // habilitar botones respuesta
    private void habilitarRespuesta()
    {
        for (int _i = 0; _i < _buttonsRespuesta.Length; _i++)
        {
            _buttonsRespuesta[_i].interactable = true;
        }
    }


    // METODOS GENERICOS

    // BOTONES
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

    // ESPERAS
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

    // Reproducir sonido respuesta
    private IEnumerator reproducirRespuesta()
    {
        if (_respondio & _respuesta == _palabraCorrecta)
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
        yield return StartCoroutine(esperarAudio(_audio));
        yield break;
    }

    // AUDIO
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
    // Esperar a que termine el audio
    private IEnumerator esperarAudio(AudioSource _a)
    {
        while (_a.isPlaying | _pausado)
        {
            // Debug.Log("Esperando a: " + _a);
            yield return null;
        }
        yield break;
    }


    // PAUSA
    // Boton pausa
    public void pausar()
    {
        _pausado = true;
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
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
        _panelPausa.SetActive(false);
        _audio.UnPause();
        //Time.timeScale = 1;
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

    // Activar visual
    private void activarVisual()
    {
        for (int _i = 0; _i < _opcionesPalabras.Length; _i++)
        {
            _estimuloVisualGO[_i].SetActive(true);
        }
    }

    // DesActivar visual
    private void desactivarVisual()
    {
        for (int _i = 0; _i < _opcionesPalabras.Length; _i++)
        {
            _estimuloVisualGO[_i].SetActive(false);
        }
    }

    // METODOS AUXILIARES PARA LOGICA

    // Método para mezclar indices
    protected int[] mezclarIndices(int[] _indMixed)
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


    private int asignarOpciones(Sprite[] _opciones, Image[] _images, string _opCorrecta) // Ojo que también mezcla los audios!
    {
        int _pos = 0;
        int[] _indOrden;
        if (_opcionesPalabras.Length == 3)
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
            Debug.Log(_opCorrecta);
            Debug.Log(_opciones[_i].name);
            if (_opciones[_i].name == _opCorrecta) _pos = _indDesorden[_i];
        }
        return _pos;
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

    // Espera respuesta del usuario
    private IEnumerator esperarRespuesta()
    {
        _respuesta = "";
        habilitarRespuesta();
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
        deshabilitarRespuesta();
        if (_respondio & _respuesta == _palabraCorrecta)
        {
            _estBien++;
            _acierto = true;
        }
        else
        {
            _estMal++;
            _acierto = false;
        }
        yield break;
    }

    // button 0
    public void Button0()
    {
        _respuesta = _opcionesImageEstimulo[0].sprite.name;
        _respondio = true;
    }
    // button 1
    public void Button1()
    {
        _respuesta = _opcionesImageEstimulo[1].sprite.name;
        _respondio = true;
    }
    // button 2
    public void Button2()
    {
        _respuesta = _opcionesImageEstimulo[2].sprite.name;
        _respondio = true;
    }
    // button 3
    public void Button3()
    {
        _respuesta = _opcionesImageEstimulo[3].sprite.name;
        _respondio = true;
    }





    //// FUNCIONES GRANDES -------------------------------------------------------------------------------------------------------------
    // Ejemplo
    private IEnumerator ejemplo(int _indPalabra, float _tResp, AudioClip _audioEj)
    {
        // Desabilitamos botones
        deshabilitarRespuesta();
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _palabraCorrecta = _palabraActual.obtenerCorrecta();
        _opcionesPalabras = _palabraActual.obtenerpalabras();
        _opcionesPalabrasImagenes = _palabraActual.obtenerEstimulos();
        _audioLetra = _palabraActual.obtenerAudio();
        _targetPos = asignarOpciones(_opcionesPalabrasImagenes, _opcionesImageEstimulo, _palabraCorrecta);
        _buttonCorrecto = _buttonsRespuesta[_targetPos];       

        // Activamos audios
        yield return StartCoroutine(reproducirAudio(_audioEj));

        // Activamos visual
        activarVisual();

        // Esperamos un momentiño
        yield return new WaitForSeconds(_tResp);

        //prendemos boton correcto
        Sprite _imNormal;
        _imNormal = apretarBoton(_buttonCorrecto);

        // Esperamos un rato
        yield return new WaitForSeconds(_tResp);

        // apagamos boton correcto
        _buttonCorrecto.GetComponent<Image>().sprite = _imNormal;
        _buttonCorrecto.gameObject.transform.localScale = 1f * Vector3.one;

        // Habilitamos botones nuevamente      
        desactivarVisual();

        // Log
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
        //    _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), "", _trialPos.ToString(), "NC", "NC", "NC");
        //_csvWriter.saveLine();
        //yield return new WaitForSeconds(1f);
        yield break;
    }

    // Ejemplo
    private IEnumerator practica(int _indPalabra)
    {
         // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _palabraCorrecta = _palabraActual.obtenerCorrecta();
        _opcionesPalabras = _palabraActual.obtenerpalabras();
        _opcionesPalabrasImagenes = _palabraActual.obtenerEstimulos();
        _audioLetra = _palabraActual.obtenerAudio();
        _targetPos = asignarOpciones(_opcionesPalabrasImagenes, _opcionesImageEstimulo, _palabraCorrecta);
        _buttonCorrecto = _buttonsRespuesta[_targetPos];

         // Activamos audios
        yield return StartCoroutine(reproducirAudio(_audioLetra));

        // Activamos visual
        activarVisual();

        // Esperar Respuesta
        yield return StartCoroutine(esperarRespuesta());

        // Respondemos segun si le pego o no
        yield return StartCoroutine(reproducirRespuesta());

        // Apagamos visual      
        desactivarVisual();

        // Log
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
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
        //    _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), "", _trialPos.ToString(), "NC", "NC", "NC");
        //_csvWriter.saveLine();
        //yield return new WaitForSeconds(1f);
        yield break;
    }




    // Test posta
    private IEnumerator testPosta(int _indPalabra)
    {
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _palabraCorrecta = _palabraActual.obtenerCorrecta();
        _opcionesPalabras = _palabraActual.obtenerpalabras();
        _opcionesPalabrasImagenes = _palabraActual.obtenerEstimulos();
        _audioLetra = _palabraActual.obtenerAudio();
        _targetPos = asignarOpciones(_opcionesPalabrasImagenes, _opcionesImageEstimulo, _palabraCorrecta);
        _buttonCorrecto = _buttonsRespuesta[_targetPos];

        // Activamos audios
        yield return StartCoroutine(reproducirAudio(_audioLetra));

        // Activamos visual
        activarVisual();

        // Esperar Respuesta
        yield return StartCoroutine(esperarRespuesta());


        // Apagamos visual      
        desactivarVisual();

        // Log
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
        //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
        //    _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), "", _trialPos.ToString(), "NC", "NC", "NC");
        //_csvWriter.saveLine();
        //yield return new WaitForSeconds(1f);
        yield break;
    }


}
