using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CsegmentacionOld : MonoBehaviour {

    // Variables publicas a setear
    public CTablero _tablero;
    public Image _estimuloVisual;       // donde se va a poner el estimulo visual	
    public AudioSource _audio;          // donde se va a reproducir el estimulo auditivo etc
    public Button[] _buttonsSilabas;  	// notones de respuesta
    public enum tipoTest { Silabas, Fonemas };
    public tipoTest _tipoTest;
    public CsvReadWriteLinebyLine _csvWriter;
    public GameObject _panelFinPractica, _panelPausa, _estimuloVisualGO, _buttonPause,_buttonUnPause;

    // Variables internas
    private AudioClip _ABienvenido, _AInstrucciones, _AEjemplo1, _AEjemplo2, _APracticar, _APulsaResp, _AVolverAPracticar, _AComenzar, _Abien, _Amal, _AFin, _APausa;
    private CDiccionario _diccionario;
    private CPalabra _palabraActual;
    private Sprite _auxSprite;
    private int _nroSilabas;
    private Button _buttonCorrecto;
    private bool _respondio, _repetir, _respondioPractica, _practicarDeVuelta, _pausado;
    private int _respuesta, _estBien, _estMal, _cantEjEje, _cantEjPra, _cantEjTest;
    private int[] _indicesTest;    
    private GameObject[] _buttonsGO;
    private Color _butApretado;
    private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tentreIeA, _tini, _tTotalPausa;

    // Datos para loggear
    private string _pTask;
    private int _trialPos,_numPrac;
    private bool _acierto;

    public CConfiguracion _configH;
    public bool _conf;

   
    private IEnumerator TestSegmentacion()
    {
        // Seteos iniciales
        inicializacion();

        ////// Instrucciones y ejemplos
        yield return StartCoroutine(reproducirAudio(_ABienvenido));        
        yield return StartCoroutine(reproducirAudio(_AInstrucciones));
        yield return StartCoroutine(ejemplo(0, _AEjemplo1, 2f));
        yield return StartCoroutine(ejemplo(1, _AEjemplo2, 2f));


        // Practica
        // Queres practicar?
        _audio.clip = _APracticar;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        // Hay un boton de pausa
        yield return StartCoroutine( mostrarPausa());
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
            activarVisual();
            activarBotones();
            if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
            for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
            {
                _trialPos += 1;
                _repetir = true;
                
                while (_repetir)
                {                    
                    yield return new WaitForSeconds(_tInterEnsayo);
                    yield return StartCoroutine(practica(_cont));
                    if (_respondio & _respuesta == _nroSilabas)
                    {
                        _repetir = false;
                    }
                    yield return null;
                }
            }
            desactivarVisual();
            desactivarBotones();
            _panelFinPractica.SetActive(true);
            yield return StartCoroutine(reproducirAudio(_AVolverAPracticar));
            yield return StartCoroutine(esperarRespuestaPractica());
        }
        _panelFinPractica.SetActive(false);

        // Test
        _trialPos = 0;
        desactivarVisual();
        activarBotones();
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
        desactivarBotones();
        //_audio.clip = _AFin;
        //_audio.Play();
        //yield return StartCoroutine(esperarAudio(_audio));
        salirTestSegmentacion();

    }



    // INICIALIZACION -------------------------------------------------

    // Cosas generales
    private void inicializacion()
    {
        _buttonsGO = new GameObject[_buttonsSilabas.Length];
        for (int _i = 0; _i < _buttonsGO.Length; _i++)
        {
            _buttonsGO[_i] = _buttonsSilabas[_i].gameObject;
        }
        _butApretado = _buttonsSilabas[0].colors.pressedColor;
        desactivarVisual();
        cargarDiccionario();
        cargarAudios();
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
                _pTask = "segmentationS";                
                break;
            default:
                _pTask = "segmentationF";
                break;
        }
    }


    // Carga del diccionario
    private void cargarDiccionario()
    {
        _diccionario = new CDiccionario();
        Sprite _imagenPalabra;
        AudioClip _audioPalabra;
        string _path;
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _path = "textos/segmentacionS";
                break;
            default:
                _path = "textos/segmentacionF";
                break;
        }
        TextAsset _fileData = Resources.Load(_path) as TextAsset;
        string[] _lines = _fileData.text.Split("\n"[0]);
        //Texture2D _imageAux;
        for (int _i = 1; _i < _lines.Length; _i++)
        {

            string[] _lineData = (_lines[_i].Trim()).Split(";"[0]);
            Debug.Log(_lineData[1]);
            _imagenPalabra = Resources.Load<Sprite>("imagenes/" + _lineData[1]);

            //_imageAux = Resources.Load<Texture2D>("imagenes/" + _lineData[1]); //Loading your texture
            //_imagenPalabra = Sprite.Create(_imageAux, new Rect(0, 0, 0, 0), Vector2.zero);

            _audioPalabra = Resources.Load<AudioClip>("audios/palabras/" + _lineData[1]);
            _diccionario.agregarPalabra(_i - 1, new CPalabra(_lineData[1], int.Parse(_lineData[2]), _imagenPalabra, _audioPalabra, _lineData[4]));
            if (int.Parse(_lineData[5]) == 1)
            {
                _cantEjPra += 1;
            }
            else if (int.Parse(_lineData[5]) == 0)
            {
                _cantEjEje += 1;
            }
            else if (int.Parse(_lineData[5]) == 2)
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
                _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/segmentacionS_I1");
                _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/segmentacionS_E1_mesa");
                _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/segmentacionS_E2_platillo");
                _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar");                
                _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
                _AFin = Resources.Load<AudioClip>("audios/instrucciones/globo");               
                break;
            default:
                _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa2");
                _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/segmentacionF_I1");
                _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/segmentacionF_E1_luna");
                _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/segmentacionF_E2_traje");
                _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar2");
                _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
                _AFin = Resources.Load<AudioClip>("audios/instrucciones/globo");
                break;
        }
        _APausa = Resources.Load<AudioClip>("audios/instrucciones/Pausa");
        _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
        _APulsaResp = Resources.Load<AudioClip>("audios/instrucciones/pulsarespuesta");
        _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
        _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");
       

    }




    // INTERACCION BOTONES -------------------------------------------------

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

    // Boton respuesta boton 1
    public void boton1silabas()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _respuesta = 2;
                break;
            default:
                _respuesta = 3;
                break;
        }

        _respondio = true;
    }

    // Boton respuesta boton 2
    public void boton2silabas()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _respuesta = 3;
                break;
            default:
                _respuesta = 4;
                break;
        }
        _respondio = true;
    }

    // Boton respuesta boton 3
    public void boton3silabas()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _respuesta = 4;
                break;
            default:
                _respuesta = 5;
                break;
        }
        _respondio = true;
    }

    // Boton respuesta boton 4
    public void boton4silabas()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _respuesta = 5;
                break;
            default:
                _respuesta = 6;
                break;
        }
        _respondio = true;
    }

    // Boton de salida
    public void salirTestSegmentacion()
    {
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _configH.marcarTareaSegS();
                break;
            default:
                _configH.marcarTareaSegF();
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
        //Debug.Log("despausandooooo");
        _panelPausa.SetActive(false);
        _audio.UnPause();
        //Time.timeScale = 1;
    }



    // ESPERAS --------------------------------------------------------

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

    // Espera respuesta del usuario
    private IEnumerator esperarRespuesta()
    {
        _respuesta = -1;
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


    // FUNCIONES AUXILIARES -------------------------------------------------------------

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

    // Deshabilitar botones respuesta
    private void deshabilitarRespuesta()
    {
        for (int _i = 0; _i < _buttonsSilabas.Length; _i++)
        {
            _buttonsSilabas[_i].interactable = false;
        }
    }

    // Habilitar Respuesta
    private void habilitarRespuesta()
    {
        for (int _i = 0; _i < _buttonsSilabas.Length; _i++)
        {
            _buttonsSilabas[_i].interactable = true;
        }
    }


    // desactivar botones
    private void desactivarBotones()
    {
        for (int _i = 0; _i < _buttonsGO.Length; _i++)
        {
            Debug.Log(_i);
            _buttonsGO[_i].SetActive(false);
        }
    }

    // activar botones
    private void activarBotones()
    {
        for (int _i = 0; _i < _buttonsGO.Length; _i++)
        {
            Debug.Log(_i);
            _buttonsGO[_i].SetActive(true);
        }
    }


    // Desactivar elementos de estimulo visual
    private void desactivarVisual()
    {
        _estimuloVisualGO.SetActive(false);
        //for (int _i = 0; _i < _buttonsGO.Length; _i++)
        //{
        //    Debug.Log(_i);
        //    _buttonsGO[_i].SetActive(false);
        //}
    }

    // Activar elementos de estimulo visual
    private void activarVisual()
    {
        _estimuloVisualGO.SetActive(true);
        //for (int _i = 0; _i < _buttonsGO.Length; _i++)
        //{
        //    _buttonsGO[_i].SetActive(true);
        //}
    }


    //private void mezclarBotonesOld()
    //{


    //    int[] _indMixed = { 0, 1, 2 };
    //    for (var i = _indMixed.Length - 1; i > 0; i--)
    //    {
    //        var r = Random.Range(0, i);
    //        var tmp = _indMixed[i];
    //        _indMixed[i] = _indMixed[r];
    //        _indMixed[r] = tmp;
    //    }
    //    for (int _i = 0; _i <= 2; _i++)
    //    {
    //        _buttonsSilabas[_i].GetComponent<RectTransform>().anchoredPosition = new Vector2(_posOpciones[_indMixed[_i]], _posOpcionesV);
    //    }
    //}

    // Mezcla de botones pro aunque no se usa
    private void mezclarBotones()
    {

        //_buttonCorrecto = _buttonsSilabas[_nroSilabas - 3];
        // Creamos 
        int[] _indMixed = new int[_buttonsSilabas.Length];
        for (int _i = 0; _i < _buttonsSilabas.Length; _i++)
        {
            _indMixed[_i] = _i;
        }
        // Mezclamos
        for (var i = _indMixed.Length - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = _indMixed[i];
            _indMixed[i] = _indMixed[r];
            _indMixed[r] = tmp;
        }
        // Asignamos mezcla
        for (int _i = 0; _i < _buttonsSilabas.Length; _i++)
        {
            _buttonsSilabas[_i].transform.SetSiblingIndex(_indMixed[_i]);
            _buttonsSilabas[_i].gameObject.SetActive(true);
        }
        // Sacamos 1 si son más de tres
        if (_tipoTest == tipoTest.Fonemas)
        {
            for (int _i = 0; _i < _buttonsSilabas.Length; _i++)
            {
                if (_indMixed[_i] != _nroSilabas - 3)
                {
                    _buttonsSilabas[_indMixed[_i]].gameObject.SetActive(false);
                    break;
                }

            }
        }

    }

    // Mezcla de indices para el test
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



    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------

    // Ejemplo
    private IEnumerator ejemplo(int _indPalabra, AudioClip _audioAux, float _tResp)
    {
        // Desabilitamos botones
        deshabilitarRespuesta();       
        // Actualizamos info actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _estimuloVisual.sprite = _palabraActual.obtenerImagen();
        activarVisual();
        _nroSilabas = _palabraActual.obtenerSilabas();
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 2];
                break;
            default:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 3];
                break;
        }
        _audio.clip = _audioAux;
        // Empezamos audio
        _audio.enabled = true;
        _audio.Play();
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
        _estimuloVisual.sprite = null;
        desactivarVisual();
        _csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
            _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(),"", _trialPos.ToString(), "NC", "NC", "NC");
        _csvWriter.saveLine();
        yield return new WaitForSeconds(1f);
        yield break;
    }

    // Practica
    private IEnumerator practica(int _indPalabra)
    {
        // Actualizamos estimulo
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);        
        _nroSilabas = _palabraActual.obtenerSilabas();
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 2];
                break;
            default:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 3];
                break;
        }
        _audio.clip = _palabraActual.obtenerAudio();
        // Play al audio
        _audio.Play();
        // Mostrar imagen        
        _estimuloVisual.sprite = _palabraActual.obtenerImagen();
        yield return new WaitForSeconds(_tentreIeA);
        activarVisual();
        // Esperamos audio
        yield return StartCoroutine(esperarAudio(_audio));
        // Esperamos la respuesta        
        yield return StartCoroutine(esperarRespuesta());
        // Respondemos segun si le pego o no
        if (_respondio & _respuesta == _nroSilabas)
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
        _estimuloVisual.sprite = null;
        desactivarVisual();
        // loggeo
        string _tlog;
        if (_tResp < _tiempoMaxResp)
            _tlog = (_tResp+_tTotalPausa).ToString();
        else
            _tlog = "NC";
        string __resplog;
        if (_respuesta == -1)
            __resplog = "NC";
        else
            __resplog = _respuesta.ToString();

        _csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "1",
            _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(),_numPrac.ToString(),_trialPos.ToString(), __resplog, _acierto.ToString(), _tlog);
        _csvWriter.saveLine();
        yield return new WaitForSeconds(1f);
        yield break;
    }

    // Test
    private IEnumerator testPosta(int _indPalabra)
    {
        
        // Actualizamos estimulo actual
        _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
        _estimuloVisual.sprite = _palabraActual.obtenerImagen();       
        _nroSilabas = _palabraActual.obtenerSilabas();
        switch (_tipoTest)
        {
            case tipoTest.Silabas:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 2];
                break;
            default:
                _buttonCorrecto = _buttonsSilabas[_nroSilabas - 3];
                break;
        }
        _audio.clip = _palabraActual.obtenerAudio();  
        // Play al audio
        _audio.Play();
        // Mostrar imagen
        yield return new WaitForSeconds(_tentreIeA);        
        activarVisual();
        // Esperamos audio
        yield return StartCoroutine(esperarAudio(_audio));
        // Esperamos la respuesta
        yield return StartCoroutine(esperarRespuesta());
        // Actualizamos estadisticas 
        if (_respondio & _respuesta == _nroSilabas)
        {
            _estBien++;
            _acierto = true;
        }
        else
        {
            _estMal++;
            _acierto = false;
        }
        desactivarVisual();
        string _tlog;
        if (_tResp < _tiempoMaxResp)
            _tlog = (_tResp + _tTotalPausa).ToString();
        else
            _tlog = "NC";
        string __resplog;
        if (_respuesta == -1)
            __resplog = "NC";
        else
            __resplog = _respuesta.ToString();
        _csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(),"2",
            _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), _numPrac.ToString(), _trialPos.ToString(), __resplog, _acierto.ToString(),
            _tlog);
        _csvWriter.saveLine();
    }











 

}
