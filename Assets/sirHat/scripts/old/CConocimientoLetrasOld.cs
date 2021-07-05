using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CConocimientoLetrasOld : MonoBehaviour {



   // // Variables publicas a setear
   // public CTablero _tablero;
   // public Text[] _opcionesLetraText;
   // public Button[] _buttonsRespuesta;  	// botones de respuesta
   // public GameObject _buttonPause, _buttonUnPause, _panelPausa, _panelFinPractica;
   // public GameObject[] _estimuloVisualGO;
   // public CsvReadWriteLinebyLine _csvWriter;

   // // Variables internas
   // public AudioSource _audio;
   // private AudioClip _AInstrucciones, _AEjemplo1, _AEjemplo2, _AEjemplo3, _APracticar, _AComenzar, _Abien, _Amal, _AFin, _APausa, _ABienvenido, _AVolverAPracticar, _APulsaResp;
   // public AudioClip _audioLetra;
   // private DiccionarioLetras _diccionario;
   // private CPalabraLetras _palabraActual;
   // public Button _buttonCorrecto;
   // public string _letraCorrecta;
   // public string[] _opcionesLetra;

   // private bool _respondio, _repetir, _respondioPractica, _practicarDeVuelta, _pausado;
   // public string _respuesta;
   // private int _estBien, _estMal;
   // private int[] _indicesTest;
   // private int _cantEjEje, _cantEjPra, _cantEjTest;
   // public enum tipoTest { Nombre, Sonido };
   // public tipoTest _tipoTest;
   // private Color _butApretado;
   // private float _tiempoMaxResp, _tResp, _tInterEnsayo, _tentreIeA, _tTotalPausa, _tini;




   // // Datos para loggear
   // private string _pTask;
   // private int _trialPos, _numPrac;
   // public int _targetPos;
   // private bool _acierto;

   // public CConfiguracion _configH;
   // public bool _conf;




   // void Start()
   // {
   //     //StartCoroutine(TestLN());
   // }


   // //----------------------METODOS -----------------------------

   // // Test
   // private IEnumerator TestLN()
   // {
   //     // Seteos iniciales
   //     inicializacion();

   //     ////// Instrucciones y ejemplos
   //     yield return StartCoroutine(reproducirAudio(_ABienvenido));
   //     yield return StartCoroutine(reproducirAudio(_AInstrucciones));
   //     yield return StartCoroutine(ejemplo(0, 2f));

   //     // Test
   //     _trialPos = 0;
   //     desactivarVisual();
   //     //activarBotones();
   //     _audio.clip = _AComenzar;
   //     _audio.Play();
   //     yield return StartCoroutine(esperarAudio(_audio));
   //     _indicesTest = mezclarIndices(_indicesTest);
   //     for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
   //     {
   //         _trialPos += 1;
   //         yield return new WaitForSeconds(_tInterEnsayo);
   //         yield return StartCoroutine(testPosta(_indicesTest[_cont]));
   //     }
   //     desactivarVisual();
   //     //desactivarBotones();
   //     _audio.clip = _AFin;
   //     _audio.Play();
   //     yield return StartCoroutine(esperarAudio(_audio));
   //     salirLN();

   // }



   // // A implementar ----------------------------------------------------
   // // Inicializacion
   // protected void inicializacion()
   // {    
   //     _butApretado = _buttonsRespuesta[0].colors.pressedColor;
   //     cargarDiccionario();
   //     cargarAudios();
   //     _tiempoMaxResp = 30.0f;
   //     _tInterEnsayo = 0.1f;
   //     _tentreIeA = 0.1f;
   //     _estBien = 0;
   //     _estMal = 0;
   //     _panelFinPractica.SetActive(false);
   //     _panelPausa.SetActive(false);
   //     deshabilitarRespuesta();
   //     switch (_tipoTest)
   //     {
   //         case tipoTest.Nombre:
   //             _pTask = "lettersN";
   //             break;
   //         default:
   //             _pTask = "lettersS";
   //             break;
   //     }
   //     desactivarVisual();
   // }



   // //--------------------------------------------------------------------

   //public void SetearTipoTestSonido()
   // {
   //     _tipoTest = tipoTest.Sonido;
   // }
   // public void SetearTipoTestNombre()
   // {
   //     _tipoTest = tipoTest.Nombre;
   // }
   // // Boton de salida
   // public void salirLN()
   // {
   //     switch (_tipoTest)
   //     {
   //         case tipoTest.Sonido:
   //             _configH.marcarTareaLNS();
   //             break;
   //         default:
   //             _configH.marcarTareaLNN();
   //             break;
   //     }
   //     StopAllCoroutines();
   //     if (!_conf)
   //     {
   //         _tablero.StartCoroutine("vueltaDeTest");
   //     }
   //     else
   //     {
   //         this.gameObject.SetActive(false);
   //     }
   //     _configH.SalirDeConfiguracion();
   //     _conf = false;
   // }


   // // Carga del diccionario
   // private void cargarDiccionario()
   // {
   //     _diccionario = new DiccionarioLetras();
   //     AudioClip _audioLetra1;
   //     string _path;
   //     switch (_tipoTest)
   //     {
   //         case tipoTest.Nombre:
   //             _path = "inputs/letrasN";
   //             break;
   //         default:
   //             _path = "inputs/letrasS";
   //             break;
   //     }
   //     TextAsset _fileData = Resources.Load(_path) as TextAsset;
   //     string[] _lines = _fileData.text.Split("\n"[0]);
   //     //Texture2D _imageAux;
   //     for (int _i = 1; _i < _lines.Length; _i++)
   //     {

   //         string[] _lineData = (_lines[_i].Trim()).Split(";"[0]);
   //         //Debug.Log(_lineData.Length);
   //         switch (_tipoTest)
   //         {
   //             case tipoTest.Nombre:
   //                 _audioLetra1 = Resources.Load<AudioClip>("audios/letras/nombres/" + _lineData[13]);
   //                 break;
   //             default:
   //                 _audioLetra1 = Resources.Load<AudioClip>("audios/letras/sonidos/" + _lineData[13]);
   //                 break;
   //         }           

   //         _diccionario.agregarPalabra(_i - 1, new CPalabraLetras(_lineData[1], _lineData[2], _lineData[3], _lineData[4], _lineData[5], _audioLetra1));
   //         if (int.Parse(_lineData[12]) == 1)
   //         {
   //             _cantEjPra += 1;
   //         }
   //         else if (int.Parse(_lineData[12]) == 0)
   //         {
   //             _cantEjEje += 1;
   //             Debug.Log(_lineData[3]);
   //             Debug.Log(_lineData[4]);
   //             Debug.Log(_lineData[5]);
   //         }
   //         else if (int.Parse(_lineData[12]) == 2)
   //         {
   //             _cantEjTest += 1;
   //         }
   //     }
   //     _indicesTest = new int[_cantEjTest];
   //     for (int _i = _cantEjPra + _cantEjEje; _i < _cantEjPra + _cantEjEje + _cantEjTest; _i++)
   //     {
   //         _indicesTest[_i - (_cantEjPra + _cantEjEje)] = _i;
   //     }
   // }

   // // Carga de los audios
   // private void cargarAudios()
   // {
   //     switch (_tipoTest)
   //     {
   //         case tipoTest.Nombre:
   //             _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa");
   //             _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/LKnombre_I1");
   //             break;
   //         default:
   //             _ABienvenido = Resources.Load<AudioClip>("audios/instrucciones/bienvenidoacasa2");
   //             _AInstrucciones = Resources.Load<AudioClip>("audios/instrucciones/LKsonido_I1");
               
   //             break;
   //     }
   //     _AComenzar = Resources.Load<AudioClip>("audios/instrucciones/bien_pausa_comienzo");
   //     _AFin = Resources.Load<AudioClip>("audios/instrucciones/globo");
   //     _APausa = Resources.Load<AudioClip>("audios/instrucciones/Pausa");
   //     _AVolverAPracticar = Resources.Load<AudioClip>("audios/instrucciones/volverapracticar");
   //     _APulsaResp = Resources.Load<AudioClip>("audios/instrucciones/pulsarespuesta");
   //     _Abien = Resources.Load<AudioClip>("audios/instrucciones/Bien");
   //     _Amal = Resources.Load<AudioClip>("audios/instrucciones/Mal");


   // }

   // // Deshabilitar botones respuesta
   // private void deshabilitarRespuesta()
   // {
   //     for (int _i = 0; _i < _buttonsRespuesta.Length; _i++)
   //     {
   //         _buttonsRespuesta[_i].interactable = false;
   //     }
   // }
   // // habilitar botones respuesta
   // private void habilitarRespuesta()
   // {
   //     for (int _i = 0; _i < _buttonsRespuesta.Length; _i++)
   //     {
   //         _buttonsRespuesta[_i].interactable = true;
   //     }
   // }


   // // METODOS GENERICOS

   // // BOTONES
   // // Boton de no repetir la practica
   // public void botonContinuarTest()
   // {
   //     _practicarDeVuelta = false;
   //     _respondioPractica = true;
   // }

   // // Boton de repetir practica
   // public void botonRepetirPractica()
   // {
   //     _practicarDeVuelta = true;
   //     _respondioPractica = true;
   // }
    
   // // ESPERAS
   // // Esperar que responda si quiere practicar de vuelta
   // public IEnumerator esperarRespuestaPractica()
   // {
   //     while (!_respondioPractica)
   //     {
   //         yield return null;
   //     }
   //     _respondioPractica = false;
   //     yield break;
   // }


   // // AUDIO
   // // Reproducir audio
   // private IEnumerator reproducirAudio(AudioClip _audioAux)
   // {
   //     Debug.Log("reproduciendo: " + _audioAux);
   //     _audio.clip = _audioAux;
   //     _audio.Play();
   //     yield return StartCoroutine(esperarAudio(_audio));
   //     yield return new WaitForSeconds(0.2f);
   //     yield break;
   // }
   // // Esperar a que termine el audio
   // private IEnumerator esperarAudio(AudioSource _a)
   // {
   //     while (_a.isPlaying | _pausado)
   //     {
   //         // Debug.Log("Esperando a: " + _a);
   //         yield return null;
   //     }
   //     yield break;
   // }

    
   // // PAUSA
   // // Boton pausa
   // public void pausar()
   // {
   //     _pausado = true;
   //     _buttonPause.SetActive(false);
   //     _buttonUnPause.SetActive(true);
   //     _panelPausa.SetActive(true);
   //     if (_audio.isPlaying) _audio.Pause();
   //     //Time.timeScale = 0;  
   // }

   // // Boton unpausar
   // public void unPausar()
   // {
   //     _pausado = false;
   //     _tTotalPausa = Time.time - _tini;
   //     _tini = Time.time;
   //     _buttonPause.SetActive(true);
   //     _buttonUnPause.SetActive(false);
   //     _panelPausa.SetActive(false);
   //     _audio.UnPause();
   //     //Time.timeScale = 1;
   // }
   // // Mostrar pausa.
   // private IEnumerator mostrarPausa()
   // {
   //     _audio.clip = _APausa;
   //     _audio.Play();
   //     yield return new WaitForSeconds(1f);
   //     _panelPausa.SetActive(true);
   //     _buttonPause.SetActive(false);
   //     _buttonUnPause.SetActive(true);
   //     yield return StartCoroutine(esperarAudio(_audio));
   //     yield return new WaitForSeconds(1f);
   //     _panelPausa.SetActive(false);
   //     _buttonPause.SetActive(true);
   //     _buttonUnPause.SetActive(false);
   //     yield break;
   // }

   // // Activar visual
   // private void activarVisual()
   // {
   //     for (int _i = 0; _i < _estimuloVisualGO.Length; _i++)
   //     {
   //         _estimuloVisualGO[_i].SetActive(true);
   //     }
   // }

   // // DesActivar visual
   // private void desactivarVisual()
   // {
   //     for (int _i = 0; _i < _estimuloVisualGO.Length; _i++)
   //     {
   //         _estimuloVisualGO[_i].SetActive(false);
   //     }
   // }

   // // METODOS AUXILIARES PARA LOGICA

   // // Método para mezclar indices
   // protected int[] mezclarIndices(int[] _indMixed)
   // {        
   //     for (var i = _indMixed.GetLength(0) -1 ; i > 0; i--)
   //     {            
   //         var r = Random.Range(0, i+1);
   //         var tmp = _indMixed[i];
   //         _indMixed[i] = _indMixed[r];
   //         _indMixed[r] = tmp;
   //     }
   //     return _indMixed;
        
   // }


   // private int asignarOpciones(string[] _opciones, Text[] _textos, string _opCorrecta) // Ojo que también mezcla los audios!
   // {
   //     Debug.Log("opciones: " + _opciones.ToString());
   //     Debug.Log("correcta: " + _opCorrecta);
   //     int _pos = 0;
   //     int[] _indOrden = new int[3] { 0, 1, 2 };
   //     int[] _indDesorden;
   //     _indDesorden = mezclarIndices(_indOrden);
   //     for (int _i=0; _i<3; _i++)
   //     {
   //         _textos[_indDesorden[_i]].text = _opciones[_i];
   //         if (_opciones[_i] == _opCorrecta) _pos = _indDesorden[_i];
   //     }
   //     return _pos;
   // }


   // // Marcar botón correcto
   // private Sprite apretarBoton(Button _button)
   // {
   //     Sprite _imNormal, _imApretado;
   //     _imNormal = _button.GetComponent<Image>().sprite;
   //     _imApretado = _button.spriteState.pressedSprite;
   //     _button.GetComponent<Image>().sprite = _imApretado;
   //     _button.gameObject.transform.localScale = 1.3f * Vector3.one;
   //     return _imNormal;
   // }

   // // Espera respuesta del usuario
   // private IEnumerator esperarRespuesta()
   // {
   //     _respuesta = "";
   //     habilitarRespuesta();
   //     _respondio = false;
   //     _tini = Time.time;
   //     _tResp = 0f;
   //     _tTotalPausa = 0f;
   //     while (!_respondio & _tResp < _tiempoMaxResp)
   //     {
   //         if (!_pausado)
   //         {
   //             _tResp = Time.time - _tini;
   //         }
   //         yield return null;
   //     }
   //     if (!_respondio)
   //     {
   //         _tResp = _tiempoMaxResp;
   //     }
   //     deshabilitarRespuesta();
   //     if (_respondio & _respuesta == _letraCorrecta)
   //     {
   //         _estBien++;
   //         _acierto = true;
   //     }
   //     else
   //     {
   //         _estMal++;
   //         _acierto = false;
   //     }
   //     yield break;
   // }

   // // button 0
   // public void Button0()
   // {
   //     _respuesta = _opcionesLetraText[0].text;
   //     _respondio = true;
   // }
   // // button 1
   // public void Button1()
   // {
   //     _respuesta = _opcionesLetraText[1].text;
   //     _respondio = true;
   // }
   // // button 2
   // public void Button2()
   // {
   //     _respuesta = _opcionesLetraText[2].text;
   //     _respondio = true;
   // }






   // // FUNCIONES GRANDES -------------------------------------------------------------------------------------------------------------
   // // Ejemplo
   // private IEnumerator ejemplo(int _indPalabra, float _tResp)
   // {
   //     // Desabilitamos botones
   //     deshabilitarRespuesta();
   //     // Actualizamos info actual
   //     _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
   //     _letraCorrecta = _palabraActual.obtenerLetraCorrecta();
   //     _opcionesLetra = _palabraActual.obtenerEstimulos();
   //     _audioLetra = _palabraActual.obtenerAudio();
   //     _targetPos = asignarOpciones(_opcionesLetra, _opcionesLetraText, _letraCorrecta);
   //     _buttonCorrecto = _buttonsRespuesta[_targetPos];
        
   //     // Activamos visual
   //     activarVisual();

   //     // Activamos audios
   //     yield return StartCoroutine(reproducirAudio(_audioLetra));
        

   //     // Esperamos un momentiño
   //     yield return new WaitForSeconds(_tResp);

   //     //prendemos boton correcto
   //    Sprite _imNormal;
   //     _imNormal = apretarBoton(_buttonCorrecto);

   //     // Esperamos un rato
   //     yield return new WaitForSeconds(_tResp);

   //     // apagamos boton correcto
   //     _buttonCorrecto.GetComponent<Image>().sprite = _imNormal;
   //     _buttonCorrecto.gameObject.transform.localScale = 1f * Vector3.one;

   //     // Habilitamos botones nuevamente      
   //     desactivarVisual();

   //     // Log
   //     //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
   //     //    _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), "", _trialPos.ToString(), "NC", "NC", "NC");
   //     //_csvWriter.saveLine();
   //     //yield return new WaitForSeconds(1f);
   //     yield break;
   // }


   // // Ejemplo
   // private IEnumerator testPosta(int _indPalabra)
   // {
   //     // Desabilitamos botones
   //     deshabilitarRespuesta();
   //     // Actualizamos info actual
   //     _palabraActual = _diccionario.obtenerPalabra(_indPalabra);
   //     _letraCorrecta = _palabraActual.obtenerLetraCorrecta();
   //     _opcionesLetra = _palabraActual.obtenerEstimulos();
   //     _audioLetra = _palabraActual.obtenerAudio();
   //     _targetPos = asignarOpciones(_opcionesLetra, _opcionesLetraText, _letraCorrecta);
   //     _buttonCorrecto = _buttonsRespuesta[_targetPos];

   //     // Activamos visual
   //     activarVisual();

   //     // Activamos audios
   //     yield return StartCoroutine(reproducirAudio(_audioLetra));


   //     // Esperar respuesta
   //     yield return StartCoroutine(esperarRespuesta());


   //     // Habilitamos botones nuevamente      
   //     desactivarVisual();

   //     // Log
   //     //_csvWriter.AddTestRow(_pTask, _palabraActual.obtenerPalabra(), "", _nroSilabas.ToString(), "", _nroSilabas.ToString(), _palabraActual.obtenerStruct(), "0",
   //     //    _palabraActual.obtenerPalabra(), "", _tablero._TaskCount.ToString(), "", _trialPos.ToString(), "NC", "NC", "NC");
   //     //_csvWriter.saveLine();
   //     //yield return new WaitForSeconds(1f);
   //     yield break;
   // }













}
