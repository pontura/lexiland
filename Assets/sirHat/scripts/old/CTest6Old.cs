using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CTest6Old : MonoBehaviour
{

    // Variables publicas a setear
    public CTablero _tablero;
    public GameObject _panelShow,_panelResp, _buttonPause, _buttonUnPause, _panelPausa, _panelFinPractica;
    public AudioSource _audio;
    public Color _colorOff, _colorOn;
    public CsvReadWriteSTMLinebyLine _csvWriterSTM;
    public AudioClip _ding,_press;

    // Variables internas
    private CDiccionarioSTMsimon _diccionario;
    private CPalabraSTMsimon _palabraActual;
    private AudioClip _AInstrucciones, _AEjemplo2, _APracticar, _AComenzar, _Abien, _Amal, _AFin, _ABienvenido, _AVolverAPracticar;
    public int[] _flashCorrecto,_flashRespuesta;
    public int _nroFlashes, _nroRespuesta;
    private bool _respondio, _correcta, _pausado, _practicarDeVuelta, _respondioPractica, _repetir;
    private int _estBien, _estMal, _cantEjEje, _cantEjPra, _cantEjTest;
    private int[] _indicesTest;
    public Image[] _imageShow;
    private float _timeOn, _timeOff;
    private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tini, _tTotalPausa;
    public int _contadorCorrectas,_nroFlashesanterior,_contadorIgualCant;

    // Datos para loggear
    private string _pTask;
    private int _trialPos, _numPrac;
    public bool _acierto;

    private bool _seguirCheckeando;

    public CConfiguracion _configH;
    public bool _conf;


    public IEnumerator Test6()
    {
        inicializacion();
        _panelResp.SetActive(false);
        _panelShow.SetActive(false);
        yield return StartCoroutine(reproducirAudio(_ABienvenido));
        yield return StartCoroutine(reproducirAudio(_AInstrucciones));


        //// Instrucciones y ejemplos
        yield return StartCoroutine(ejemplo(0, 1f, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ejemplo(1, 1f, 1f));
        yield return new WaitForSeconds(1f);

        // Practica
        _audio.clip = _APracticar;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        _practicarDeVuelta = true;
        _numPrac = -1;
        while (_practicarDeVuelta)
        {
            _numPrac += 1;
            if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
            _trialPos = 0;
            for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
            {
                _trialPos += 1;
               
                _repetir = true;
                while (_repetir & _numPrac < 3)
                {
                    Debug.Log("cont: " + _cont);
                    
                    yield return new WaitForSeconds(_tInterEnsayo);
                    Debug.Log("indice practica: " + _cont);
                    yield return StartCoroutine(practica(_cont, 1f));
                    yield return null;
                }
            }

            //desactivarVisual();
            _panelFinPractica.SetActive(true);
            yield return StartCoroutine(reproducirAudio(_AVolverAPracticar));
            yield return StartCoroutine(esperarRespuestaPractica());
        }
        _panelFinPractica.SetActive(false);



        // Test
        _trialPos = 0;
        _contadorCorrectas = 0;
        _audio.clip = _AComenzar;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;            
            yield return new WaitForSeconds(_tInterEnsayo);
            if (_contadorCorrectas == 0 & _contadorIgualCant == 4)
            {
                break;
            }
            yield return StartCoroutine(testPosta(_indicesTest[_cont]));
            if(_contadorCorrectas == 2)
            {
                _cont += 4 - _contadorIgualCant;
            }
            

        }
        salirTest6();


        }




    // INICIALIZACION ---------------------------------------------------------------------------

    // Inicializacion
    private void inicializacion()
    {
        _timeOn = 1f;
        _timeOff = 0.25f;
        _estBien = 0;
        _estMal = 0;
        _tiempoMaxResp = 60f;
        _tInterEnsayo = 1.5f;
        cargarAudios();
        cargarDiccionario();
        // _imageShow = _panelShow.GetComponentsInChildren<Image>();
        for (int _i = 0; _i < _imageShow.Length; _i++)
        {
            _imageShow[_i].color = _colorOff;
        }
        _panelShow.SetActive(true);
        _panelResp.SetActive(false);
        _pTask = "STMSpatial";
    }

    // Cargar audios
    private void cargarAudios()
    {
        _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa");
        _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/STMspatial+_I1");
        _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/listos");
        _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
        _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");
        _APracticar = Resources.Load<AudioClip>("audios/instrucciones/practicar");
        _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
    }


    // Carga de dicionarios
    private void cargarDiccionario()
    {
        // Incializaciones
        _diccionario = new CDiccionarioSTMsimon();
        TextAsset _fileData = Resources.Load("textos/STMSimon") as TextAsset;
        // Cortamos por linea
        string[] _lines = _fileData.text.Split("\n"[0]);
        // Vamos linea a linea
        for (int _i = 1; _i < _lines.Length; _i++)
        {
            Debug.Log(_i);
            Debug.Log(_lines[_i]);
            // Cortamos toda la linea            
            string[] _lineData = (_lines[_i].Trim()).Split(";"[0]);
            // Sacamos cada campo para rellenar
            _nroFlashes = int.Parse(_lineData[10]);
            _flashCorrecto = new int[_nroFlashes];
            // Sacamos cada una de las palabras
            for (int _j = 0; _j < _nroFlashes; _j++)
            {
                _flashCorrecto[_j] = int.Parse(_lineData[_j + 1])-1;
            }
            // Guardamos en el diccionario
            _diccionario.agregarPalabra(_i-1, new CPalabraSTMsimon(_flashCorrecto));
            // Guardamos cantidad de ejemplo, practica y test
            string _tipoEst;
            _tipoEst = _lineData[11];
            switch (_tipoEst)
            {
                case "0":
                    _cantEjEje += 1;
                    break;
                case "1":
                    _cantEjPra += 1;
                    break;
                case "2":
                    _cantEjTest += 1;
                    break;
            }

        }
        _indicesTest = new int[_cantEjTest];
        for (int _i = _cantEjPra + _cantEjEje; _i < _cantEjPra + _cantEjEje + _cantEjTest; _i++)
        {
            _indicesTest[_i - (_cantEjPra + _cantEjEje)] = _i;
        }
    }



    // BOTONES  ---------------------------------------------------------------------------------


    // Boton pausa
    public void pausar()
    {
        _pausado = true;
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        _panelPausa.SetActive(true);
        if (_audio.isPlaying) _audio.Pause();
       // Time.timeScale = 0;
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
       // Time.timeScale = 1;
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

    public void salirTest6()
    {
        _configH.marcarTareaSTMorden();
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


    // ESPERAS  ---------------------------------------------------------------------------------
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


    private IEnumerator esperarAudio(AudioSource _a)
    {
        while (_a.isPlaying)
        {
            yield return null;
        }
        yield break;
    }

    private IEnumerator esperarRespuesta()
    {
        //float _tini;
        _respondio = false;
        _correcta = false;
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

        yield return new WaitForSeconds(0.5f);
        yield break;
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
        for (int _i = 0; _i < _sec.Length; _i++)
        {
            _audio.clip = _press;
            _audio.Play();
            _imageShow[_sec[_i]].color = _colorOn;
            yield return new WaitForSeconds(_timeOn);
            _imageShow[_sec[_i]].color = _colorOff;
            yield return new WaitForSeconds(_timeOff);
        }
        yield break;
    }



    private void respCorrecta()
    {
        _audio.clip = _press;
        _audio.Play(); 

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
            _respondio = true;
        }

    }

    // Reproducir audio
    private IEnumerator reproducirAudio(AudioClip _audioAux)
    {
        _audio.clip = _audioAux;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(0.2f);
        yield break;
    }




    // FUNCIONES GRANDES ------------------------------------------------------------------------



    private IEnumerator ejemplo(int _indice, float _tShow, float _tResp)
    {
        // Ajustes iniciales: panel y secuencia
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _flashCorrecto = _diccionario.obtenerPalabra(_indice).obtenerSecuencia();
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _diccionario.obtenerPalabra(_indice).obtenerLargo();
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;

        // Mostramos secuencia
        yield return new WaitForSeconds(_tShow);
        yield return StartCoroutine( mostrarSecuencia(_flashCorrecto));
        //_panelShow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _audio.clip = _ding;
        _audio.Play();

        // Mostramos secuencia respuesta
        //_panelShow.SetActive(true);
        yield return new WaitForSeconds(_tResp);
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));

        // Log
        string[] _palabrasLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _nroFlashes)
                _palabrasLog[_i] = _flashCorrecto[_i].ToString();
            else
                _palabrasLog[_i] = "";
        }


        //_csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
        //     _palabrasLog[7], _palabrasLog[8], _nroFlashes.ToString(), "0", _tablero._TaskCount.ToString(), _trialPos.ToString(), "", "", "",
        //    "", "", "", "", "", "", "NC", "NC");

        _csvWriterSTM.saveLine();

        // Apagamos el panel antes de irnos
        _panelShow.SetActive(false);
        yield break;
    }



    private IEnumerator practica(int _indice, float _tShow) {

        // Ajustes iniciales: panel y secuencia
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _flashCorrecto = _diccionario.obtenerPalabra(_indice).obtenerSecuencia();
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _diccionario.obtenerPalabra(_indice).obtenerLargo();
        _flashRespuesta = new int[_nroFlashes];
        _nroRespuesta = 0;

        // Mostramos secuencia
        yield return new WaitForSeconds(_tShow);
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));



        // Esperamos la respuesta
        _panelShow.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        _audio.clip = _ding;
        _audio.Play();
        _panelResp.SetActive(true);
        yield return StartCoroutine(esperarRespuesta());
        _panelResp.SetActive(false);

        // Respondemos segun si le pego o no
        if (_correcta)
        {
            _audio.clip = _Abien;
            _repetir = false;
        }
        else
        {
            _audio.clip = _Amal;
            _repetir = true;
        }
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));

        // Log
        string[] _palabrasLog = new string[9];
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _nroFlashes)
                _palabrasLog[_i] = _flashCorrecto[_i].ToString();
            else
                _palabrasLog[_i] = "";
            if (_i < _nroRespuesta)
                _respLog[_i] = _flashRespuesta[_i].ToString();
            else
                _respLog[_i] = "";
        }
        string _tlog;
        if (_tResp < _tiempoMaxResp)
            _tlog = (_tResp + _tTotalPausa).ToString();
        else
            _tlog = "NC";
        //_csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
        //     _palabrasLog[7], _palabrasLog[8], _nroFlashes.ToString(), "1", _tablero._TaskCount.ToString(), _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2],
        //     _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto.ToString(), _tlog);
        //_csvWriterSTM.saveLine();
        yield break;
    }




    private IEnumerator testPosta(int _indice)
    {
        // Ajustes iniciales: panel y secuencia
        _panelResp.SetActive(false);
        _panelShow.SetActive(true);
        _flashCorrecto = _diccionario.obtenerPalabra(_indice).obtenerSecuencia();
        _nroFlashesanterior = _nroFlashes;
        _nroFlashes = _diccionario.obtenerPalabra(_indice).obtenerLargo();
        _flashRespuesta = new int[_nroFlashes];        
        _nroRespuesta = 0;

        // reseteamos contador de correctas
        if(_nroFlashesanterior != _nroFlashes)
        {
            _contadorCorrectas = 0;
            _contadorIgualCant = 0;
        }
        _contadorIgualCant += 1;

        // Mostramos secuencia
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(mostrarSecuencia(_flashCorrecto));

        // Esperamos la respuesta
        _panelShow.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        _audio.clip = _ding;
        _audio.Play();
        _panelResp.SetActive(true);
        yield return StartCoroutine(esperarRespuesta());
        _panelResp.SetActive(false);

        // Respondemos segun si le pego o no
        if ( _correcta)
        {
            _estBien += 1;
            _acierto = true;
            _contadorCorrectas += 1;
        }
        else
        {
            _estMal += 1;
            _acierto = false;
        }

        // Log
        string[] _palabrasLog = new string[9];
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _nroFlashes)
                _palabrasLog[_i] = _flashCorrecto[_i].ToString();
            else
                _palabrasLog[_i] = "";
            if (_i < _nroRespuesta)
                _respLog[_i] = _flashRespuesta[_i].ToString();
            else
                _respLog[_i] = "";
        }
        string _tlog;
        if (_tResp < _tiempoMaxResp)
            _tlog = (_tResp + _tTotalPausa).ToString();
        else
            _tlog = "NC";
        
        //_csvWriterSTM.AddTestRow(_pTask, _palabrasLog[0], _palabrasLog[1], _palabrasLog[2], _palabrasLog[3], _palabrasLog[4], _palabrasLog[5], _palabrasLog[6],
        //     _palabrasLog[7], _palabrasLog[8], _nroFlashes.ToString(), "2", _tablero._TaskCount.ToString(), _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2],
        //     _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto.ToString(), _tlog);

        //_csvWriterSTM.saveLine();

        yield break;
    }



}
