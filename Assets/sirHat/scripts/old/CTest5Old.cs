using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CTest5Old : MonoBehaviour
{

    //// Variables publicas a setear
    //public CTablero _tablero;
    //public GameObject _slotPadreOrigen, _slotPadreDestino, _slotPrefab, _slotPrefabOrigen, _contenidoSlotPrefab, _panelFinPractica, _buttonPause, _buttonUnPause, _panelPausa;
    //public AudioSource _audio;
    //public CsvReadWriteSTMLinebyLine _csvWriterSTM;    

    //// Variables internas
    //private AudioClip _AInstrucciones, _APracticar, _AComenzar, _Abien, _Amal, _ABienvenido, _AVolverAPracticar, _APausa, _AEjemplo1, _AEjemplo2;
    //private CDiccionarioSTM _diccionario;
    //private CPalabraSTM _palabraActual;
    //public string[] _palabrasActuales;
    //public string _palabrasActualesJuntas;
    //private string[] _palabrasRespuesta;
    //public string _palabrasRespuestaJuntas;
    //public int _nroPalabras;
    //private bool _respondio, _repetir, _respondioPractica, _practicarDeVuelta, _pausado;
    //private int _estBien, _estMal;
    //public int[] _indicesTest;
    //public int _cantEjEje,_cantEjPra, _cantEjTest;
    //public GameObject[] _contenidos,_slotsOrigen,_slotsDestino;
    //private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tini, _tTotalPausa;

    //public Sprite[] _imgAux;
    //private AudioClip[] _audiosAux;
    //private string[] _opciones;
    //public Sprite[] _imgOpciones;

    //// Datos para loggear
    //private string _pTask;
    //private int _trialPos, _numPrac;
    //private bool _acierto;


    //public CConfiguracion _configH;
    //public bool _conf;


    //public IEnumerator Test5()
    //{
    //    Debug.Log("Test5");
    //    // inicializar
    //    inicializacion();

    //    //// Instrucciones y ejemplos
    //    yield return StartCoroutine(reproducirAudio(_ABienvenido));
    //    yield return StartCoroutine(reproducirAudio(_AInstrucciones));
    //    yield return StartCoroutine(ejemplo(0, _AEjemplo1, 1f, 1f));
    //    yield return new WaitForSeconds(_tInterEnsayo);
    //    yield return StartCoroutine(ejemplo(1, _AEjemplo2, 1f, 1f));

    //    // Practica
    //    _audio.clip = _APracticar;
    //    _audio.Play();
    //    yield return StartCoroutine(esperarAudio(_audio));
    //    _practicarDeVuelta = true;
    //    _numPrac = -1;
    //    while (_practicarDeVuelta )
    //    {
    //        _numPrac += 1;
    //        if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
    //        _trialPos = 0;
    //        for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
    //        {
    //            _trialPos += 1;
                
    //            _repetir = true;
    //            while (_repetir & _numPrac < 3)
    //            {
                    
    //                yield return new WaitForSeconds(_tInterEnsayo);
    //                Debug.Log("indice practica: " + _cont);
    //                yield return StartCoroutine(practica(_cont));                    
    //                yield return null;
    //            }
    //        }

    //        //desactivarVisual();
    //        _panelFinPractica.SetActive(true);
    //        yield return StartCoroutine(reproducirAudio(_AVolverAPracticar));
    //        yield return StartCoroutine(esperarRespuestaPractica());
    //    }
    //    _panelFinPractica.SetActive(false);


    //    // Test
    //    _trialPos = 0;
    //    _audio.clip = _AComenzar;
    //    _audio.Play();
    //    yield return StartCoroutine(esperarAudio(_audio));
    //    for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
    //    {
    //        _trialPos += 1;
    //        Debug.Log("indice: " + _indicesTest[_cont]);
    //        yield return new WaitForSeconds(_tInterEnsayo);
    //        yield return StartCoroutine(testPosta(_indicesTest[_cont]));
    //    }
    //    salirTest5();
    //}





    //// INICIALIZACIONES -----------------------------------------------------------------------------------

    //// Inicializar
    //private void inicializacion()
    //{
    //    _estBien = 0;
    //    _estMal = 0;
    //    _tiempoMaxResp = 60.0f;
    //    _tInterEnsayo = 0.5f;
    //    cargarDiccionario();
    //    cargarAudios();
    //    _pTask = "STMOrder";
    //    _palabrasRespuesta = new string[9];
    //    cargarOpciones();

    //}

    //// cargar opciones
    //private void cargarOpciones()
    //{

    //    // sol, tren, pan, rey, flor, pez, pie
    //    _opciones = new string [7]{ "sol", "tren", "pan", "rey", "flor", "pez", "pie" };
    //    _imgOpciones = new Sprite[7];
    //    for (int _i=0; _i<7; _i++)
    //    {
    //        _imgOpciones[_i] = Resources.Load<Sprite>("imagenes/" + _opciones[_i]);

    //    }

    //}


    //// Carga de dicionarios
    //private void cargarDiccionario()
    //{
    //    // Incializaciones
    //    _diccionario = new CDiccionarioSTM();
    //    Sprite[] _imagenesPalabras;
    //    AudioClip[] _audiosPalabras;
    //    string[] _textosPalabras;
    //    TextAsset _fileData = Resources.Load("textos/STMOrder") as TextAsset;
    //    // Cortamos por linea
    //    string[] _lines = _fileData.text.Split("\n"[0]);       
    //    // Vamos linea a linea
    //    for (int _i = 1; _i < _lines.Length; _i++)
    //    {
    //        // Cortamos toda la linea            
    //       string[] _lineData = (_lines[_i].Trim()).Split(";"[0]);
    //        // Sacamos cada campo para rellenar
    //        _nroPalabras        = int.Parse(_lineData[7]);
    //        Debug.Log("Dicc num pal:" + _nroPalabras);
    //        _imagenesPalabras   = new Sprite[_nroPalabras];
    //        _audiosPalabras     = new AudioClip[_nroPalabras];
    //        _textosPalabras     = new string[_nroPalabras];
    //        // Sacamos cada una de las palabras
    //        for (int _j = 0; _j < _nroPalabras; _j++)
    //        {
    //            _textosPalabras[_j]     = _lineData[_j+1];
    //            _imagenesPalabras[_j]   = Resources.Load<Sprite>("imagenes/" + _textosPalabras[_j]);
    //            _audiosPalabras[_j]     = Resources.Load<AudioClip>("audios/palabrasSTM/" + _textosPalabras[_j]);
    //        }
    //        // Guardamos en el diccionario
    //        _diccionario.agregarPalabra(_i-1, new CPalabraSTM(_textosPalabras, _nroPalabras, _imagenesPalabras, _audiosPalabras));
    //        // Guardamos cantidad de ejemplo, practica y test
    //        string _tipoEst;
    //        _tipoEst = _lineData[8];
    //        switch (_tipoEst)
    //        {
    //            case "0":
    //                _cantEjEje += 1;
    //                break;
    //            case "1":
    //                _cantEjPra += 1;
    //                break;
    //            case "2":
    //                _cantEjTest += 1;
    //                break;
    //        }

    //    }
    //    _indicesTest = new int[_cantEjTest];
    //    for (int _i = _cantEjPra + _cantEjEje; _i < _cantEjPra + _cantEjEje + _cantEjTest; _i++)
    //    {
    //        _indicesTest[_i - (_cantEjPra + _cantEjEje)] = _i;
    //    }
    //}

    //// Carga de audios
    //private void cargarAudios()
    //{
    //    _AEjemplo1 = Resources.Load<AudioClip>("audios/instrucciones/STMOrden_E1_solpantren");
    //    _AEjemplo2 = Resources.Load<AudioClip>("audios/instrucciones/STMOrden_E2_soltrenpanrey");
    //    _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa");
    //    _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/STMorden_I1");
    //    _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar");
    //    _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
    //    _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
    //    _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");
    //    _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
    //    _APausa = Resources.Load<AudioClip>("audios/instrucciones/Pausa");
    //}



    //// ESPERAS -----------------------------------------------------------------------

    //// Esperar que responda si quiere practicar de vuelta
    //public IEnumerator esperarRespuestaPractica()
    //{
    //    while (!_respondioPractica)
    //    {
    //        yield return null;
    //    }
    //    _respondioPractica = false;
    //    yield break;
    //}



    //// Esperar audio
    //private IEnumerator esperarAudio(AudioSource _a)
    //{
    //    while (_a.isPlaying)
    //    {
    //        yield return null;
    //    }
    //    yield break;
    //}

    //// Esperar respuesta
    //private IEnumerator esperarRespuesta()
    //{
    //    activarRespuesta();       
    //    _respondio = false;
    //    _tini = Time.time;
    //    _tResp = 0f;
    //    _tTotalPausa = 0f;
    //    while (!_respondio & _tResp < _tiempoMaxResp)
    //    {
    //        if (!_pausado)
    //        {
    //            _tResp = Time.time - _tini;
    //        }
    //        yield return null;
    //    }
    //    if (!_respondio)
    //    {
    //        _repetir = true;
    //        _tResp = _tiempoMaxResp;
    //    }       
    //    desactivarRespuesta();
    //    yield break;
    //}

    //// BOTONES -------------------------------------------------------------------------------------------

    //// Boton pausa
    //public void pausar()
    //{
    //    _pausado = true;
    //    _buttonPause.SetActive(false);
    //    _buttonUnPause.SetActive(true);
    //    _panelPausa.SetActive(true);
    //    if (_audio.isPlaying) _audio.Pause();
    //   // Time.timeScale = 0;
    //}

    //// Boton unpausar
    //public void unPausar()
    //{        
    //    _tTotalPausa = Time.time - _tini;
    //    _tini = Time.time;
    //    _pausado = false;
    //    _buttonPause.SetActive(true);
    //    _buttonUnPause.SetActive(false);
    //    _panelPausa.SetActive(false);
    //    _audio.UnPause();
    //   // Time.timeScale = 1;
    //}

    //// Boton de no repetir la practica
    //public void botonContinuarTest()
    //{
    //    _practicarDeVuelta = false;
    //    _respondioPractica = true;
    //}

    //// Boton de repetir practica
    //public void botonRepetirPractica()
    //{
    //    _practicarDeVuelta = true;
    //    _respondioPractica = true;
    //}

    //// FUNCIONES AUXILIARES -------------------------------------------------------------------------------

        
    //// Reproducir audio
    //private IEnumerator reproducirAudio(AudioClip _audioAux)
    //{
    //    _audio.clip = _audioAux;
    //    _audio.Play();
    //    yield return StartCoroutine(esperarAudio(_audio));
    //    yield return new WaitForSeconds(0.2f);
    //    yield break;
    //}



    //// Comparar respuesta
    //public void compararRespuesta(string _resp, int _nroResp, string[] _respCadena)
    //{
    //    _palabrasRespuesta = _respCadena;
    //    _palabrasRespuestaJuntas = _resp;
    //    if (_nroResp == _nroPalabras)
    //    {
    //        if (_resp.Equals(_palabrasActualesJuntas))
    //        {
    //            _repetir = false;
    //        }
    //        else
    //        {
    //            _repetir = true;
    //        }
    //        _respondio = true;
    //    }

    //}


    //// Mover objeto
    //private IEnumerator MoveObject(GameObject Amover, Vector3 source, Vector3 target, float overTime)
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + overTime)
    //    {
    //        Amover.transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
    //        yield return null;
    //    }
    //    Amover.transform.position = target;
    //}

    //// Mezclar índices
    //private int[] mezclarIndices(int[] _indMixed)
    //{
    //    for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
    //    {
    //        var r = Random.Range(0, i);
    //        var tmp = _indMixed[i];
    //        _indMixed[i] = _indMixed[r];
    //        _indMixed[r] = tmp;
    //    }
    //    return _indMixed;
    //}

    //// Crear indices mezclados
    //private int[] crearMezclado(int _tam)
    //{
    //    int[] _mezclado = new int[_tam];
    //    for (int _i = 0; _i < _tam; _i++)
    //    {
    //        _mezclado[_i] = _i;
    //    }
    //    _mezclado = mezclarIndices(_mezclado);
    //    return _mezclado;
    //}

    //// Crear cadena
    //private string crearCadena(string[] _s)
    //{
    //    string _ret;
    //    System.Text.StringBuilder _builder = new System.Text.StringBuilder();
    //    for (int _i = 0; _i < _s.Length; _i++)
    //    {
    //        _builder.Append(_s[_i]);
    //    }
    //    _ret = _builder.ToString();
    //    return _ret;
    //}

    //// Salir del test
    //public void salirTest5()
    //{
    //    _configH.marcarTareaSTMorden();
    //    StopAllCoroutines();
        
    //    if (!_conf)
    //    {
    //        _tablero.StartCoroutine("vueltaDeTest");
    //    }
    //    else
    //    {
    //        this.gameObject.SetActive(false);
    //    }
    //    _configH.SalirDeConfiguracion();
    //    _conf = false;
    //}

    //// habilitar respuesta
    //private void activarRespuesta()
    //{
    //    CDragHandler._enabled = true;

    //}

    //// deshabilitar respuesta
    //private void desactivarRespuesta()
    //{
    //    CDragHandler._enabled = false;
    //}



    //// Actualizar info
    //private void actualizarInfo(int _indPalabra)
    //{
    //    _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
    //    _nroPalabras = _palabraActual.obtenerCantPalabras();
    //    _palabrasActuales = _palabraActual.obtenerPalabras();
    //    _imgAux = _palabraActual.obtenerImagenes();
    //    _audiosAux = _palabraActual.obtenerAudios();
    //    _contenidos = new GameObject[_nroPalabras];
    //    _slotsOrigen = new GameObject[_nroPalabras];
    //    _slotsDestino = new GameObject[_nroPalabras];
    //    _palabrasActualesJuntas = crearCadena(_palabrasActuales);
    //}


    //// Ajustar tamaño
    //private void ajustarTamanos()
    //{
    //    if (_nroPalabras < 5)
    //    {
    //        _slotPadreOrigen.GetComponent<GridLayoutGroup>().cellSize = 150f * Vector2.one;
    //        _slotPadreDestino.GetComponent<GridLayoutGroup>().cellSize = 150f * Vector2.one;
    //        _slotPrefab.GetComponent<GridLayoutGroup>().cellSize = 140f * Vector2.one;
    //        _slotPrefabOrigen.GetComponent<GridLayoutGroup>().cellSize = 140f * Vector2.one;
    //    }
    //    else
    //    {
    //        _slotPadreOrigen.GetComponent<GridLayoutGroup>().cellSize = 100f * Vector2.one;
    //        _slotPadreDestino.GetComponent<GridLayoutGroup>().cellSize = 100f * Vector2.one;
    //        _slotPrefab.GetComponent<GridLayoutGroup>().cellSize = 90f * Vector2.one;
    //        _slotPrefabOrigen.GetComponent<GridLayoutGroup>().cellSize = 90f * Vector2.one;
    //    }
    //}

    //// slots destinos
    //private void crearSlotsDestino()
    //{
    //    Debug.Log("Creando slots destino");
    //    Debug.Log("numero de palabras: " + _nroPalabras);
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        _slotsDestino[_i] = Instantiate(_slotPrefab);
    //        _slotsDestino[_i].transform.SetParent(_slotPadreDestino.transform);
    //        _slotsDestino[_i].transform.localScale = Vector3.one;
    //    }
    //}


    //// mezclar para que estén en orden
    //private int[] mezclarEnOrden()
    //{
    //    int[] _mezclaOrden = new int[_nroPalabras];
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        for (int _j = 0; _j < _nroPalabras; _j++)
    //        {
    //            if(_palabrasActuales[_j].Equals(_opciones[_i]))
    //            {
    //                _mezclaOrden[_i] = _j;
    //            }
    //        }
    //    }


    //    return _mezclaOrden;
    //}






    //// slots origen
    //private void crearSlotsOrigen()
    //{
    //    // Slots origen ( mezclados)
    //    //int[] _indAux = crearMezclado(_nroPalabras);
    //    int[] _indAux = mezclarEnOrden();

    //    Debug.Log(_indAux.ToString());
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        _slotsOrigen[_indAux[_i]] = Instantiate(_slotPrefabOrigen);
    //        _slotsOrigen[_indAux[_i]].transform.SetParent(_slotPadreOrigen.transform);
    //        _slotsOrigen[_indAux[_i]].transform.localScale = Vector3.one;
    //    }

    //    // Contenido en orden en slots desordenados
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        _contenidos[_i] = Instantiate(_contenidoSlotPrefab);
    //        _contenidos[_i].transform.SetParent(_slotsOrigen[_i].transform);
    //        _contenidos[_i].transform.localScale = Vector3.one;
    //        _contenidos[_i].name = _palabrasActuales[_i];
    //        _contenidos[_i].GetComponent<Image>().sprite = _imgAux[_i];
    //    }


    //    // sol, tren, pan, rey, flor, pez, pie



    //}


    //// FUNCIONES GRANDES  -----------------------------------------------------------------------------------

    //// Ejemplo
    //private IEnumerator ejemplo(int _indPalabra, AudioClip _audioEj, float _timgs, float _tResp)
    //{

    //    Debug.Log("numero de palabras 0: " + _nroPalabras);
    //    // Actualizamos info actual
    //    actualizarInfo(_indPalabra);
    //    Debug.Log("numero de palabras 1: " + _nroPalabras);
    //    // Priemro decimos el audio de ejemplo
    //    yield return StartCoroutine(reproducirAudio(_audioEj));
    //    yield return new WaitForSeconds(_timgs);
    //    Debug.Log("numero de palabras 2: " + _nroPalabras);
    //    // Si son pocos elementos agrandamos tamaño de slots
    //    ajustarTamanos();
    //    Debug.Log("numero de palabras 3: " + _nroPalabras);
    //    // Slots destino
    //    crearSlotsDestino();

    //    // Slots origen ( mezclados)
    //    crearSlotsOrigen();

    //    // Mostramos la respuesta correcta
    //    yield return new WaitForSeconds(_tResp);
    //    Vector3 _PosDestino,_PosOrigen;
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        // Seteamos origen y destino
    //        _PosDestino = _slotsDestino[_i].transform.position;
    //        _PosOrigen  = _contenidos[_i].transform.position;

    //        // Para cada palabra muevo el contenido correspondiente
    //        yield return StartCoroutine(MoveObject(_contenidos[_i], _PosOrigen, _PosDestino,  1.0f));
    //        _contenidos[_i].transform.SetParent(_slotsDestino[_i].transform);
    //    }
        
    //    // Log
    //    string[] _palabrasLog = new string[9];
    //    for (int _i = 0; _i < 9; _i++)
    //    {
    //        if (_i < _nroPalabras)
    //        {
    //            _palabrasLog[_i] = _palabraActual.obtenerPalabras()[_i];
    //        }
    //        else
    //        {
    //            _palabrasLog[_i] = "";
    //        }
    //    }
    //    _csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
    //         _palabrasLog[7], _palabrasLog[8], _nroPalabras.ToString(), "0", _tablero._TaskCount.ToString(), _trialPos.ToString(), "","","",
    //        "", "","", "", "", "", _acierto.ToString(), _tResp.ToString());
    //    _csvWriterSTM.saveLine();
    //    yield return new WaitForSeconds(1f);

    //    // Eliminamos los slots
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        Destroy(_slotsDestino[_i]);
    //        Destroy(_slotsOrigen[_i]);
    //    }        
    //    yield break;
    //}


    //// Práctica
    //private IEnumerator practica(int _indPalabra) {

    //    // Actualizamos info actual
    //    actualizarInfo(_indPalabra);


    //    // Primero decimos los audios en orden
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        _audio.clip = _audiosAux[_i];
    //        _audio.Play();
    //        yield return StartCoroutine(esperarAudio(_audio));
    //    }

    //    // Si son pocos elementos agrandamos tamaño de slots
    //    ajustarTamanos();

    //    // Slots destino
    //    crearSlotsDestino();

    //    // Slots origen ( mezclados)
    //    crearSlotsOrigen();


    //    // Esperamos la respuesta
    //    yield return StartCoroutine(esperarRespuesta());


    //    // Respondemos segun si le pego o no
    //    if (!_repetir)
    //    {
    //        _audio.clip = _Abien;
    //        _acierto = true;
    //    }
    //    else
    //    {
    //        _audio.clip = _Amal;
    //        _acierto = false;
    //    }
    //    _audio.Play();
    //    yield return StartCoroutine(esperarAudio(_audio));
       
    //    // Log
    //    string[] _palabrasLog = new string[9];
    //    string[] _respLog = new string[9];
    //    for (int _i = 0; _i < 9; _i++)
    //    {
    //        if (_i < _nroPalabras)
    //        {
    //            _palabrasLog[_i] = _palabraActual.obtenerPalabras()[_i];
    //            _respLog[_i] = _palabrasRespuesta[_i];
    //        }
    //        else {
    //            _palabrasLog[_i] = "";
    //            _respLog[_i] = "";
    //        }            
    //    }
    //    string _tlog;
    //    if (_tResp < _tiempoMaxResp)
    //        _tlog = (_tResp + _tTotalPausa).ToString();
    //    else
    //        _tlog = "NC";

    //    _csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
    //         _palabrasLog[7], _palabrasLog[8],_nroPalabras.ToString(), "1", _tablero._TaskCount.ToString(), _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2],
    //         _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto.ToString(), _tlog);
    //    _csvWriterSTM.saveLine();

    //    yield return new WaitForSeconds(1f);
    //    // Eliminamos los slots
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        Destroy(_slotsDestino[_i]);
    //        Destroy(_slotsOrigen[_i]);
    //    }
        

    //    yield break;
    //}



    //// Test
    //private IEnumerator testPosta(int _indPalabra)
    //{
    //    // Actualizamos info actual
    //    actualizarInfo(_indPalabra);

    //    // Primero decimos los audios en orden
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        _audio.clip = _audiosAux[_i];
    //        _audio.Play();
    //        yield return StartCoroutine(esperarAudio(_audio));
    //    }

    //    // Si son pocos elementos agrandamos tamaño de slots
    //    ajustarTamanos();

    //    // Slots destino
    //    crearSlotsDestino();

    //    // Slots origen ( mezclados)
    //    crearSlotsOrigen();


    //    // Esperamos la respuesta
    //    yield return StartCoroutine(esperarRespuesta());

    //    // Actualizamos estadistica
    //    if (!_repetir)
    //    {
    //        _estBien += 1;
    //        _acierto = true;
    //    }
    //    else
    //    {
    //        _estMal += 1;
    //        _acierto = false;
    //    }



    //    // Log
    //    string[] _palabrasLog = new string[9];
    //    string[] _respLog = new string[9];
    //    for (int _i = 0; _i < 9; _i++)
    //    {
    //        if (_i < _nroPalabras)
    //        {
    //            _palabrasLog[_i] = _palabraActual.obtenerPalabras()[_i];
    //            _respLog[_i] = _palabrasRespuesta[_i];
    //        }
    //        else
    //        {
    //            _palabrasLog[_i] = "";
    //            _respLog[_i] = "";
    //        }
    //    }
    //    string _tlog;
    //    if (_tResp < _tiempoMaxResp)
    //        _tlog = (_tResp + _tTotalPausa).ToString();
    //    else
    //        _tlog = "NC";

    //    _csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
    //         _palabrasLog[7], _palabrasLog[8], _nroPalabras.ToString(), "2", _tablero._TaskCount.ToString(), _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2],
    //         _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto.ToString(), _tlog);
    //    _csvWriterSTM.saveLine();


    //    yield return new WaitForSeconds(1f);
    //    // Eliminamos los slots
    //    for (int _i = 0; _i < _nroPalabras; _i++)
    //    {
    //        Destroy(_slotsDestino[_i]);
    //        Destroy(_slotsOrigen[_i]);
    //    }
        


    //    yield break;        
    //}









}
