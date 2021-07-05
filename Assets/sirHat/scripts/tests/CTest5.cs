using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CTest5 : MonoBehaviour
{

    // Nuevas
    private CListaTestSTMorden _data;
    private CEstimuloSTMorden _estimuloActual;
    private string _tResp;

    // Variables publicas a setear
    public GameObject _slotPadreOrigen, _slotPadreDestino, _slotPrefab, _slotPrefabOrigen, _contenidoSlotPrefab, _manoEjemplo;
    //public CsvReadWriteSTMLinebyLine _csvWriterSTM;    

    // Variables internas
    public string[] _palabrasActuales;
    public string _palabrasActualesJuntas;
    private string[] _palabrasRespuesta;
    public string _palabrasRespuestaJuntas;
    public int _nroPalabras;
    private bool _repetir;
    private int[] _indicesTest;
    public GameObject[] _contenidos,_slotsOrigen,_slotsDestino;
    private float _tini, _tTotalPausa;

    public Sprite[] _imgAux;
    private AudioClip[] _audiosAux;

    // Datos para loggear
    private string _pTask;
    private int _trialPos, _numPrac;
    private bool _acierto;


    public CConfiguracion _configH;
    public bool _conf;
    private bool _AlexEnabled;

    public GameObject _panelCargando;

    // V2018
    Output output;

    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }

    public IEnumerator CorrerTest()
    {
        // Seteos iniciales
        inicializar();
        // Instrucciones y ejemplos
        // yield return StartCoroutine(CAudioManager.Instance.Bienvenida());
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (_data.TieneInstrucciones())
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
        }

        for (int _i = 0; _i < _data.CantEjemplos; _i++)
        {
            _trialPos = _i;
            yield return StartCoroutine(ejemplo(_i, _data.AudiosEjemplo[_i]));
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
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
                    _repetir = true;
                    while (_repetir & _numPrac < 3)
                    {
                        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                        yield return StartCoroutine(practica(_cont));

                        //_repetir = !(CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
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
        yield return StartCoroutine(CAudioManager.Instance.Comenzar());
        int[] _indicesTest = _data.IndicesTest;
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(testPosta(_indicesTest[_cont]));
        }
        _configH.marcarTareaSTMorden();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        //salirTest();
        Salir(false);

    }





    // INICIALIZACIONES -----------------------------------------------------------------------------------

    // Inicializar
    private void inicializar()
    {
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.STMo);
        CLogManager.Instance.IncrementarTaskPos();
        _data = CInfoManager.Instance.DataSTMorden();
        CCanvasManager.Instance.ActivarPanelComun(); 
        _palabrasRespuesta = new string[9];
        CCanvasManager.Instance.ApagarRepeticionAlex();

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloSTMorden.GetHeaders());
        output.Save();

    }

    // Manejo manito para ejemplo

    private IEnumerator prenderMano(CanvasGroup _panelCG, float overTime)
    {
        float _valInicial = _panelCG.alpha;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _panelCG.alpha = Mathf.Lerp(_valInicial, 1f, (Time.time - startTime) / overTime);
            yield return null;
        }
        _panelCG.alpha = 1f;
        yield break;
    }

    private IEnumerator moverMano(RectTransform _panelRT, float overTime)
    {       
        Vector2 _valInicialPos = _panelRT.anchoredPosition;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _panelRT.anchoredPosition = Vector2.Lerp(_valInicialPos, Vector2.zero, (Time.time - startTime) / overTime);
            yield return null;
        }
        _panelRT.anchoredPosition = Vector2.zero;
        yield break;
    }
    
 


    // ESPERAS -----------------------------------------------------------------------

    // Esperar que responda si quiere practicar de vuelta
    public IEnumerator esperarRespuestaPractica()
    {
        while (!CCanvasManager.Instance.RespondioPractica)
        {
            yield return null;
        }
        yield break;
    }



 

    // FUNCIONES AUXILIARES -------------------------------------------------------------------------------



    // Comparar respuesta
    public void compararRespuesta(string _resp, int _nroResp, string[] _respCadena)
    {
        _palabrasRespuesta = _respCadena;
        _palabrasRespuestaJuntas = _resp;
        if (_nroResp == _nroPalabras)
        {
            if (_resp.Equals(_palabrasActualesJuntas))
            {
                _repetir = false;
            }
            else
            {
                _repetir = true;
            }
            CCanvasManager.Instance.Respondio = true;
        }

    }


    // Mover objeto
    private IEnumerator MoveObject(GameObject Amover, Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            Amover.transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        Amover.transform.position = target;
    }
    // Mover objeto
    private IEnumerator MoveObjects(GameObject Amover, GameObject Amover2, Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            Amover.transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            Amover2.transform.position = Amover.transform.position;
            yield return null;
        }
        Amover.transform.position = target;
        Amover2.transform.position = target;
    }




    // Mezclar índices
    private int[] mezclarIndices(int[] _indMixed)
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

    // Crear indices mezclados
    private int[] crearMezclado(int _tam)
    {
        int[] _mezclado = new int[_tam];
        for (int _i = 0; _i < _tam; _i++)
        {
            _mezclado[_i] = _i;
        }
        _mezclado = mezclarIndices(_mezclado);
        return _mezclado;
    }

    // Crear cadena
    private string crearCadena(string[] _s)
    {
        string _ret;
        System.Text.StringBuilder _builder = new System.Text.StringBuilder();
        for (int _i = 0; _i < _s.Length; _i++)
        {
            _builder.Append(_s[_i]);
        }
        _ret = _builder.ToString();
        return _ret;
    }

    public void Salir(bool salidaForzada){
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            if (_slotsDestino[_i] != null) Destroy(_slotsDestino[_i]);
            if (_slotsOrigen[_i] != null) Destroy(_slotsOrigen[_i]);
        }

        StopAllCoroutines();
        this.gameObject.SetActive(false);
        CCanvasManager.Instance.DesactivarPanelComun();
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);

    }

    // Salir del test
    public void salirTest()
    {
        // Eliminamos los slots
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
           if (_slotsDestino[_i] != null) Destroy(_slotsDestino[_i]);
           if (_slotsOrigen[_i] != null) Destroy(_slotsOrigen[_i]);
        }
        
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
        this.gameObject.SetActive(false);
    }


    // habilitar respuesta
    private void activarRespuesta()
    {
        _AlexEnabled = true;
        CDragHandler._enabled = true;

    }

    // deshabilitar respuesta
    private void desactivarRespuesta()
    {
        _AlexEnabled = false;
        CDragHandler._enabled = false;
    }



    // Actualizar info
    private void actualizarInfo(int _indPalabra)
    {
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);
        _nroPalabras = _estimuloActual.NumEstimulos;
        _palabrasActuales = _estimuloActual.Visuales;
        _imgAux = _estimuloActual.VisualesImagenes;
        _audiosAux = _estimuloActual.Audios; 
        _contenidos = new GameObject[_nroPalabras];
        _slotsOrigen = new GameObject[_nroPalabras];
        _slotsDestino = new GameObject[_nroPalabras];
        _palabrasActualesJuntas = crearCadena(_palabrasActuales);
    }


    // Ajustar tamaño
    private void ajustarTamanos()
    {
        if (_nroPalabras < 5)
        {
            _slotPadreOrigen.GetComponent<GridLayoutGroup>().cellSize = 150f * Vector2.one;
            _slotPadreDestino.GetComponent<GridLayoutGroup>().cellSize = 150f * Vector2.one;
            _slotPrefab.GetComponent<GridLayoutGroup>().cellSize = 140f * Vector2.one;
            _slotPrefabOrigen.GetComponent<GridLayoutGroup>().cellSize = 140f * Vector2.one;
        }
        else
        {
            _slotPadreOrigen.GetComponent<GridLayoutGroup>().cellSize = 100f * Vector2.one;
            _slotPadreDestino.GetComponent<GridLayoutGroup>().cellSize = 100f * Vector2.one;
            _slotPrefab.GetComponent<GridLayoutGroup>().cellSize = 90f * Vector2.one;
            _slotPrefabOrigen.GetComponent<GridLayoutGroup>().cellSize = 90f * Vector2.one;
        }
    }

    // slots destinos
    private void crearSlotsDestino()
    {
        Debug.Log("Creando slots destino");
        Debug.Log("numero de palabras: " + _nroPalabras);
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            _slotsDestino[_i] = Instantiate(_slotPrefab);
            _slotsDestino[_i].transform.SetParent(_slotPadreDestino.transform);
            _slotsDestino[_i].transform.localScale = Vector3.one;
        }
    }


    // mezclar para que estén en orden
    private int[] mezclarEnOrden()
    {
        int[] _mezclaOrden = new int[_nroPalabras];
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            for (int _j = 0; _j < _nroPalabras; _j++)
            {
                if(_palabrasActuales[_j].Equals(_data.Opciones[_i]))
                {
                    _mezclaOrden[_i] = _j;
                }
            }
        }
        return _mezclaOrden;
    }






    // slots origen
    private void crearSlotsOrigen()
    {
        // Slots origen ( mezclados)
        //int[] _indAux = crearMezclado(_nroPalabras);
        int[] _indAux = mezclarEnOrden();

        Debug.Log(_indAux.ToString());
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            _slotsOrigen[_indAux[_i]] = Instantiate(_slotPrefabOrigen);
            _slotsOrigen[_indAux[_i]].transform.SetParent(_slotPadreOrigen.transform);
            _slotsOrigen[_indAux[_i]].transform.localScale = Vector3.one;
        }

        // Contenido en orden en slots desordenados
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            _contenidos[_i] = Instantiate(_contenidoSlotPrefab);
            _contenidos[_i].transform.SetParent(_slotsOrigen[_i].transform);
            _contenidos[_i].transform.localScale = Vector3.one;
            _contenidos[_i].name = _palabrasActuales[_i];
            _contenidos[_i].GetComponent<Image>().sprite = _imgAux[_i];
        }


        // sol, tren, pan, rey, flor, pez, pie
        
    }

    public void reiniciarEstimulo()
    {
        if (_AlexEnabled)
        {
            StartCoroutine(reiniciarEstimuloCorr());
        }       
    }


    public IEnumerator reiniciarEstimuloCorr()
    {
        // Terminamos el donde estábamos
        desactivarRespuesta();
        // Eliminamos los slots
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            Destroy(_slotsDestino[_i]);
            Destroy(_slotsOrigen[_i]);
        }
        // Reproducimos los audios
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_audiosAux[_i]));
        }
        // Si son pocos elementos agrandamos tamaño de slots
        ajustarTamanos();
        // Slots destino
        crearSlotsDestino();
        // Slots origen ( mezclados)
        crearSlotsOrigen();
        // Esperamos la respuesta
        activarRespuesta();
    }



    // FUNCIONES GRANDES  -----------------------------------------------------------------------------------

    // Ejemplo
    private IEnumerator ejemplo(int _indPalabra, AudioClip _audioEj)
    {        
        // Actualizamos info actual
        actualizarInfo(_indPalabra);
        // Priemro decimos el audio de ejemplo
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_audioEj));
        // Si son pocos elementos agrandamos tamaño de slots
        ajustarTamanos();
        // Slots destino
        crearSlotsDestino();
        // Slots origen ( mezclados)
        crearSlotsOrigen();
        // Mostramos la respuesta correcta

        yield return new WaitForSeconds(1f);
        CanvasGroup _manoCG = _manoEjemplo.GetComponent<CanvasGroup>();
        _manoCG.alpha = 0f;
        _manoEjemplo.SetActive(true);
        yield return StartCoroutine(prenderMano(_manoCG, 0.6f));   
        Vector3 _PosDestino,_PosOrigen;
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            // Seteamos origen y destino
            _PosDestino = _slotsDestino[_i].transform.position;
            _PosOrigen  = _contenidos[_i].transform.position;
            // Para cada palabra muevo el contenido correspondiente
            yield return StartCoroutine(MoveObject(_manoEjemplo, _manoEjemplo.transform.position, _PosOrigen, 0.5f));
            yield return StartCoroutine(MoveObjects(_contenidos[_i], _manoEjemplo, _PosOrigen, _PosDestino,  1.0f));
            _contenidos[_i].transform.SetParent(_slotsDestino[_i].transform);
        }       
        _manoEjemplo.SetActive(false);

        // Eliminamos los slots
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            Destroy(_slotsDestino[_i]);
            Destroy(_slotsOrigen[_i]);
        }
        // Loggeo               
        CLogManager.Instance.GuardarEjemploSTMorden(_estimuloActual, _numPrac, _trialPos);

        // V2018
        // TODO: Guardado Ejemplo
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, _numPrac, "NA", _palabrasRespuesta, "NA"));


         yield break;
    }


    // Práctica
    private IEnumerator practica(int _indPalabra) {
        // Actualizamos info actual
        actualizarInfo(_indPalabra);
        // Primero decimos los audios en orden
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            yield return StartCoroutine( CAudioManager.Instance.Reproducir(_audiosAux[_i]));
        }
        // Si son pocos elementos agrandamos tamaño de slots
        ajustarTamanos();
        // Slots destino
        crearSlotsDestino();
        // Slots origen ( mezclados)
        crearSlotsOrigen();
        // Esperamos la respuesta
        activarRespuesta();
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
        desactivarRespuesta();
        // Respondemos segun si le pego o no
        _acierto = !_repetir;       
        yield return StartCoroutine(CAudioManager.Instance.ReproducirResultado(_acierto));
        // Eliminamos los slots
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            Destroy(_slotsDestino[_i]);
            Destroy(_slotsOrigen[_i]);
        }        
        // Log
        _tResp = CCanvasManager.Instance.TLog;
        CLogManager.Instance.GuardarEstimuloSTM(_estimuloActual, _numPrac, _trialPos, _tResp, _palabrasRespuesta, _acierto);

        // V2018
        // TODO: Guardado Practica
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, _numPrac, _tResp, _palabrasRespuesta, _acierto.ToString()));


        yield break;
    }



    // Test
    private IEnumerator testPosta(int _indPalabra)
    {
        // Actualizamos info actual
        actualizarInfo(_indPalabra);
        // Primero decimos los audios en orden
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_audiosAux[_i]));
        }
        // Si son pocos elementos agrandamos tamaño de slots
        ajustarTamanos();
        // Slots destino
        crearSlotsDestino();
        // Slots origen ( mezclados)
        crearSlotsOrigen();
        // Esperamos la respuesta
        activarRespuesta();
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
        desactivarRespuesta();
        // Actualizamos estadistica
        _acierto = !_repetir;
        // Eliminamos los slots
        for (int _i = 0; _i < _nroPalabras; _i++)
        {
            Destroy(_slotsDestino[_i]);
            Destroy(_slotsOrigen[_i]);
        }
        // Log
        _tResp = CCanvasManager.Instance.TLog;
        CLogManager.Instance.GuardarEstimuloSTM(_estimuloActual, _numPrac, _trialPos, _tResp, _palabrasRespuesta, _acierto);

        // V2018
        // TODO: Guardado Test Posta
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, _numPrac, _tResp, _palabrasRespuesta, _acierto.ToString()));


        yield break;        
    }









}
