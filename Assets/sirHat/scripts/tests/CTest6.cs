using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CTest6 : MonoBehaviour
{

    // Nuevas post orden
    private CListaTestSTMsimon _data;
    private CEstimuloSTMsimon _estimuloActual;

    // Variables publicas a setear
    public GameObject _panelShow,_panelResp, _manoEjemplo;  
    public Color _colorOff, _colorOn;
    public Sprite _spriteOn, _spriteOff;

    // Variables internas    
    public int[] _flashCorrecto,_flashRespuesta;
    public int _nroFlashes, _nroRespuesta;
    private bool _respondio, _correcta;
    public Image[] _imageShow;
    private float _timeOn, _timeOff;
    public int _contadorCorrectas,_nroFlashesanterior,_contadorIgualCant;
    private bool _alexhabilitado;
    // Datos para loggear
    private int _trialPos, _numPrac;
    private bool _seguirCheckeando;
    public CConfiguracion _configH;
    public bool _conf;
    public GameObject _panelCargando;

    // V2018
    Output output;

    // Para repetida de alex
    private int _indiceActualAlex;

    void OnEnable()
    {
        _panelCargando.SetActive(true);
    }


    public IEnumerator CorrerTest()
    {
        // Seteos iniciales
        inicializar();
        // Instrucciones y ejemplos
        //yield return StartCoroutine(CAudioManager.Instance.Bienvenida());
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
            yield return StartCoroutine(ejemplo(_i));
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        }


        // Practica
        if (_data.CantPractica != 0)
        {
            yield return StartCoroutine(CAudioManager.Instance.APracticar());
            yield return StartCoroutine(CAudioManager.Instance.Tetocati());
            CCanvasManager.Instance.EmpezarPractica();

            while (CCanvasManager.Instance.PracticarDeVuelta)
            {
                _trialPos = 0;
                for (int _cont = _data.CantEjemplos; _cont < _data.CantEjemplos + _data.CantPractica; _cont++)
                {
                    _trialPos += 1;
                    _correcta = false;
                    _numPrac = -1;
                    while (!_correcta)
                    {
                        if (_numPrac < 4)
                        {
                            _numPrac += 1;
                            yield return StartCoroutine(practica(_cont));
                            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                            //_repetir = !(CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);                           
                        }
                        else
                        {
                            yield return StartCoroutine(CAudioManager.Instance.AlexAyuda());
                            _numPrac = -1;
                        }
                    }
                   
                }
                CCanvasManager.Instance.FinPractica();
                yield return StartCoroutine(CAudioManager.Instance.VolverAPracticar());
                yield return StartCoroutine(esperarRespuestaPractica());
            }                  
        }

        // Test
        _trialPos = 0;
        _contadorCorrectas = 0;
        yield return StartCoroutine(CAudioManager.Instance.Comenzar());
        int[] _indicesTest = _data.IndicesTest;
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;            
            if (_contadorCorrectas == 0 & _contadorIgualCant == 4)
            {
                break;
            }
            yield return StartCoroutine(testPosta(_indicesTest[_cont]));
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            if (_contadorCorrectas == 2)
            {
                _cont += 4 - _contadorIgualCant;
            }
        }
        _configH.marcarTareaSTMsimon();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarAudios();
        CAudioManager.Instance.SetearARepetir(null);
        //salirTest();
        Salir(false);

    }




    // INICIALIZACION ---------------------------------------------------------------------------


    // Inicializar
    private void inicializar()
    {
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.STMs);
        CLogManager.Instance.IncrementarTaskPos();
        _timeOn = 1f;
        _timeOff = 0.25f;
        _data = CInfoManager.Instance.DataSTMsimon();
        // Canvas   
        CCanvasManager.Instance.ActivarPanelComun();
        for (int _i = 0; _i < _imageShow.Length; _i++)
        {
           // _imageShow[_i].color = _colorOff;
            _imageShow[_i].sprite = _spriteOff;
        }
        _panelShow.SetActive(false);
        _panelResp.SetActive(false);
        CCanvasManager.Instance.ApagarRepeticionAlex();
        _alexhabilitado = false;

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloSTMsimon.GetHeaders());
        output.Save();


    }

    
    // Esperar que responda si quiere practicar de vuelta
    public IEnumerator esperarRespuestaPractica()
    {
        while (!CCanvasManager.Instance.RespondioPractica)
        {
            yield return null;
        }
        yield break;
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

    private IEnumerator moverMano(RectTransform _panelRT, float overTime, Vector2 _fin)
    {
        Vector2 _valInicialPos = _panelRT.anchoredPosition;
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            _panelRT.anchoredPosition = Vector2.Lerp(_valInicialPos, _fin, (Time.time - startTime) / overTime);
            yield return null;
        }
        _panelRT.anchoredPosition = _fin;
        yield break;
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

    // BOTONES  ---------------------------------------------------------------------------------


    public void respButton0()
    {
        _flashRespuesta[_nroRespuesta] = 0;
        _nroRespuesta += 1;
        respCorrecta();
    }

    public void respButton1()
    {
        _flashRespuesta[_nroRespuesta] = 1;
        _nroRespuesta += 1;
        respCorrecta();
    }

    public void respButton2()
    {
        _flashRespuesta[_nroRespuesta] = 2;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton3()
    {
        _flashRespuesta[_nroRespuesta] = 3;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton4()
    {
        _flashRespuesta[_nroRespuesta] = 4;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton5()
    {
        _flashRespuesta[_nroRespuesta] = 5;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton6()
    {
        _flashRespuesta[_nroRespuesta] = 6;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton7()
    {
        _flashRespuesta[_nroRespuesta] = 7;
        _nroRespuesta += 1;
        respCorrecta();
    }
    public void respButton8()
    {
        _flashRespuesta[_nroRespuesta] = 8;
        _nroRespuesta += 1;
        respCorrecta();
    }

    public void salirTest()
    {
        CCanvasManager.Instance.PrenderRepeticionAlex();
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

    public void Salir(bool salidaForzada){
        StopAllCoroutines();
        this.gameObject.SetActive(false);
        CCanvasManager.Instance.DesactivarPanelComun();
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);


    }


    // FUNCIONES AUXILIARES ------------------------------------------------------------------------

    private int[] crearEstimulos(int _long)
    {
        _nroFlashes = _long;
        _flashRespuesta = new int[_long];
        _nroRespuesta = 0;
        int[] _est = new int[_long];
        for (int _i = 0; _i < _long; _i++)
        {
            _est[_i] = (int)Mathf.Round(Random.Range(-0.5f, 8.5f));
        }
        return _est;
    }

    private IEnumerator mostrarSecuencia(int[] _sec)
    {
        yield return new WaitForSeconds(0.5f);
        for (int _i = 0; _i < _sec.Length; _i++)
        {
            CAudioManager.Instance.ReproducirSinEspera(_data.PressSound);
          //  _imageShow[_sec[_i]].color = _colorOn;
            _imageShow[_sec[_i]].sprite = _spriteOn;
            yield return new WaitForSeconds(_timeOn);
            //_imageShow[_sec[_i]].color = _colorOff;
            _imageShow[_sec[_i]].sprite = _spriteOff;
            yield return new WaitForSeconds(_timeOff);
        }

        yield break;
    }

    private IEnumerator mostrarSecuenciaConMano(int[] _sec)
    {
        yield return new WaitForSeconds(0.5f);
        CanvasGroup _manoCG = _manoEjemplo.GetComponent<CanvasGroup>();
       // RectTransform _manoRT = (RectTransform)_manoEjemplo.transform;
        _manoCG.alpha = 0f;
        _manoEjemplo.SetActive(true);
        yield return StartCoroutine(prenderMano(_manoCG, 0.6f));
        for (int _i = 0; _i < _sec.Length; _i++)
        {
            // _manoEjemplo.transform.SetParent(_imageShow[_sec[_i]].transform); 
            RectTransform _destRT = (RectTransform)_imageShow[_sec[_i]].transform;
            //yield return StartCoroutine(moverMano(_manoRT, 0.6f,_destRT.anchoredPosition ));
            yield return StartCoroutine(MoveObject(_manoEjemplo, _manoEjemplo.transform.position, _imageShow[_sec[_i]].transform.position, 0.6f));
            CAudioManager.Instance.ReproducirSinEspera(_data.PressSound);
            //_imageShow[_sec[_i]].color = _colorOn;
            _imageShow[_sec[_i]].sprite = _spriteOn;
            yield return new WaitForSeconds(_timeOn);
            _imageShow[_sec[_i]].sprite = _spriteOff;
           // _imageShow[_sec[_i]].color = _colorOff;
            yield return new WaitForSeconds(_timeOff);
        }
        _manoEjemplo.SetActive(false);
        yield break;
    }


    private void respCorrecta()
    {
        CAudioManager.Instance.ReproducirSinEspera(_data.PressSound);
        if (_nroRespuesta == _nroFlashes)
        {
            _correcta = true;
            for (int _i = 0; _i< _nroFlashes; _i++)
            {
                if (_flashRespuesta[_i] != _flashCorrecto[_i])
                {
                    _correcta = false;
                }
            }
            CCanvasManager.Instance.Respondio = true;
        }
    }


    // Repetir secuencia
    public IEnumerator RepetirSecuencia()
    {
        _panelShow.SetActive(true);
        _panelResp.SetActive(false);       
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));
        _panelResp.SetActive(true);
        _panelShow.SetActive(false);
        yield return StartCoroutine(CAudioManager.Instance.Tetocati());
        _alexhabilitado = true;
        yield break;
    }
    public void AlexRepetidor()
    {
        if (_alexhabilitado)
        {
            _alexhabilitado = false;
            StartCoroutine(CorutinaAlexRepetidor());
            //StartCoroutine(RepetirSecuencia());

           
        }

    }

    private IEnumerator CorutinaAlexRepetidor()
    {
        // Reiniciando todo ----------------------
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _estimuloActual = _data.ObtenerEstimulo(_indiceActualAlex);
        _flashCorrecto = _estimuloActual.SecuenciaCorrecta;
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _estimuloActual.NumEstimulos;
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;
        // reseteamos contador de correctas
        if (_nroFlashesanterior != _nroFlashes)
        {
            _contadorCorrectas = 0;
            _contadorIgualCant = 0;
        }
        _contadorIgualCant += 1;
        // Mostramos secuencia
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));
        // Esperamos la respuesta       
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.DingSound));
        _panelResp.SetActive(true);
        _panelShow.SetActive(false);
        CAudioManager.Instance.RepetirSimon = true;
        _alexhabilitado = true;
        // ---------------------------------------------------------------
    }



    // FUNCIONES GRANDES ------------------------------------------------------------------------

    private IEnumerator ejemplo(int _indice)
    {
        // Ajustes iniciales: panel y secuencia
        _indiceActualAlex = _indice;
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _estimuloActual = _data.ObtenerEstimulo(_indice);
        _flashCorrecto = _estimuloActual.SecuenciaCorrecta;
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _estimuloActual.NumEstimulos;
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;
        // Mostramos secuencia
        yield return StartCoroutine( mostrarSecuencia(_flashCorrecto));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.DingSound));   
        // Mostramos secuencia respuesta
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(mostrarSecuenciaConMano(_flashCorrecto));
        // Apagamos el panel antes de irnos
        _panelShow.SetActive(false);
        CLogManager.Instance.GuardarEjemploSTMsimon(_estimuloActual,_numPrac, _trialPos);

        // V2018
        // TODO: Guardado Ejemplo
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, "NA", _flashRespuesta, 0, "NA"));


        yield break;
    }

 


    private IEnumerator practica(int _indice) {

        // Ajustes iniciales: panel y secuencia
        _indiceActualAlex = _indice;
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _estimuloActual = _data.ObtenerEstimulo(_indice);
        _flashCorrecto = _estimuloActual.SecuenciaCorrecta;
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _estimuloActual.NumEstimulos;
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;
        // Mostramos secuencia        
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));
        // Esperamos la respuesta       
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.DingSound));
        _panelResp.SetActive(true);
        _panelShow.SetActive(false);
        CAudioManager.Instance.RepetirSimon = true;
        _alexhabilitado = true;
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(null, _data.TiempoRespMaximo));
        _alexhabilitado = false;
        CAudioManager.Instance.RepetirSimon = false;
        // Respondemos segun si le pego o no
        yield return new WaitForSeconds(0.5f);
        _panelResp.SetActive(false);
        yield return StartCoroutine(CAudioManager.Instance.ReproducirResultado(_correcta));
        // Log
        string _tResp = CCanvasManager.Instance.TLog;
        CLogManager.Instance.GuardarEstimuloSTMsimon(_estimuloActual, _trialPos, _tResp, _flashRespuesta, _nroRespuesta, _correcta);

        // V2018
        // TODO: Guardado Practica
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, _tResp, _flashRespuesta, _nroRespuesta, _correcta.ToString()));


        yield break;
    }




    private IEnumerator testPosta(int _indice)
    {
        // Ajustes iniciales: panel y secuencia
        _indiceActualAlex = _indice;
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _estimuloActual = _data.ObtenerEstimulo(_indice);
        _flashCorrecto = _estimuloActual.SecuenciaCorrecta;
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _estimuloActual.NumEstimulos;
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;

        // reseteamos contador de correctas
        if (_nroFlashesanterior != _nroFlashes)
        {
            _contadorCorrectas = 0;
            _contadorIgualCant = 0;
        }
        _contadorIgualCant += 1;

        // Mostramos secuencia
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));
        // Esperamos la respuesta       
        yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.DingSound));
        _panelResp.SetActive(true);
        _panelShow.SetActive(false);
        CAudioManager.Instance.RepetirSimon = true;
        _alexhabilitado = true;
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(null, _data.TiempoRespMaximo));
        _alexhabilitado = false;
        CAudioManager.Instance.RepetirSimon = false; 
        // Respondemos segun si le pego o no       
        yield return new WaitForSeconds(0.5f);
        _panelResp.SetActive(false);
        if ( _correcta) _contadorCorrectas += 1;
        // Log
        string _tResp = CCanvasManager.Instance.TLog;
        CLogManager.Instance.GuardarEstimuloSTMsimon(_estimuloActual, _trialPos, _tResp, _flashRespuesta, _nroRespuesta, _correcta);

        // V2018
        // TODO: Guardado Test Posta
        output.addItem(_estimuloActual.GetEstimulo(_trialPos, _tResp, _flashRespuesta, _nroRespuesta, _correcta.ToString()));


        yield break;
    }



}
