using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CTestIQ : MonoBehaviour {


    // Variables publicas a setear
    public Image[] _estimuloVisual;
    public Button[] _buttonsRespuesta;
    public GameObject[] _estimuloVisualGO, _buttonsRespuestaGO;
    public Text[] _opcionesLetraText;

    // Nuevo manejo data
    private CListaTestIQ _data;
    private CEstimuloIQ _estimuloActual;
    //Para log
    private int _trialPos, _numPrac,  _targetPos;
    private bool _acierto;
    public int[] _ultimasRespuestas = new int[5];

    // Variables internas
    private Button _buttonCorrecto;
    private bool _repetir;
    private string _respuesta, _respuestaCorrecta, _tResp;
    public enum tipoTestCCF { LetrasN, LetrasS, RimasW, RimasPW, IdentificacionS, IdentificacionF, AislamientoS, AislamientoF, SegmentacionS, SegmentacionF, SintesisS, SintesisF, VOcabulario, IQ };
    public tipoTestCCF _tipoTest;
    public enum tipoCanvas {VoF, Opciones, OpcionesTexto, OpcionesBotones, IQ}
    public tipoCanvas _tipoCanvas;
    public int _incorrectasSeguidas;


    // Datos para loggear
    public CConfiguracion _configH;
    public bool _conf;
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
        yield return new WaitForSeconds(1f);
        _panelCargando.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (_data.TieneInstrucciones())
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_data.AudioInstrucciones));
        }

        for (int _i = 0; _i < _data.CantEjemplos; _i++)
        {
            yield return StartCoroutine(CorrerEstimulo(_i, _data.AudiosEjemplo[_i], 0));
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
                    while (_repetir)
                    {
                        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
                        yield return StartCoroutine(CorrerEstimulo(_cont, null, 1));
                        _repetir = !(CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
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
        int[] _indicesTest;        
        if(_tipoTest == tipoTestCCF.IQ)
        {
            _indicesTest = _data.IndicesTest;
        }
        else
        {
            _indicesTest = _data.IndicesTestMixed;
        }
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        {
            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(CorrerEstimulo(_indicesTest[_cont],null,2));
        }
        _configH.marcarTareaIQ();
        CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarAudios();
        CAudioManager.Instance.SetearARepetir(null);
        //salirTest();
        Salir(false);
    }




    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        for (int _i=0; _i<_ultimasRespuestas.Length; _i++)
        {
            _ultimasRespuestas[_i] = 0;
        }
        CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.IQ);
        _incorrectasSeguidas = 0;
        CLogManager.Instance.IncrementarTaskPos();       
       _data = CInfoManager.Instance.DataIQ();
        CCanvasManager.Instance.SetearBotonesGO(_buttonsRespuestaGO);
        CCanvasManager.Instance.SetearEstimulosGO(_estimuloVisualGO);
        CCanvasManager.Instance.SetearEstimulos(_estimuloVisual);
        CCanvasManager.Instance.SetearBotones(_buttonsRespuesta);
        CCanvasManager.Instance.ApagarVisual();
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;   

        // V2018
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimuloIQ.GetHeaders());
        output.Save();

        
    }


    // INTERACCION CON BOTONES  --------------------------------------------------------------------------------

    // button 0
    public void Button0()
    {
        RespuestaBoton(0);
    }
    // button 1
    public void Button1()
    {
        RespuestaBoton(1);
    }
    // button 2
    public void Button2()
    {
        RespuestaBoton(2);
    }
    // button 3
    public void Button3()
    {
        RespuestaBoton(3);
    }
    // button 4
    public void Button4()
    {
        RespuestaBoton(4);
    }
    // button 5
    public void Button5()
    {
        RespuestaBoton(5);
    }
    // button 6
    public void Button6()
    {
        RespuestaBoton(6);
    }

    private void RespuestaBoton(int _i)
    {
        switch (_tipoCanvas)
        {
            case tipoCanvas.Opciones:
                _respuesta = _estimuloVisual[_i].sprite.name;
                break;
            case tipoCanvas.OpcionesTexto:
                _respuesta = _estimuloVisual[_i].GetComponentInChildren<Text>().text;
                break;
            case tipoCanvas.VoF:
                _respuesta = (_i == 0) ? "1" : "0"; // El primer botón tiene que ser el de verdadero
                break;
            case tipoCanvas.OpcionesBotones:
                _respuesta = _buttonsRespuestaGO[_i].GetComponent<Image>().sprite.name;
                break;
            case tipoCanvas.IQ:
                _respuesta = _buttonsRespuestaGO[_i].GetComponent<Image>().sprite.name;
                break;
            default:
                Debug.Log("Error al asignar estimulo visual");
                break;
        }
        CCanvasManager.Instance.Respondio = true;
    }


    // Salir
    public void salirTest()    {
        
        StopAllCoroutines();
        if (!_conf)
        {
            switch (_tipoTest)
            {                
                case tipoTestCCF.LetrasN:
                    CGameManager.Instance.llamarTest(CGameManager.TestTipe.LKs);
                    break;
                case tipoTestCCF.RimasW:
                    CGameManager.Instance.llamarTest(CGameManager.TestTipe.RimaPW);
                    break;
                case tipoTestCCF.AislamientoS:
                    CGameManager.Instance.llamarTest(CGameManager.TestTipe.AislamientoF);
                    break;
                case tipoTestCCF.SintesisS:
                    CGameManager.Instance.llamarTest(CGameManager.TestTipe.SintesisF);
                    break;
                case tipoTestCCF.SegmentacionS:
                    CGameManager.Instance.llamarTest(CGameManager.TestTipe.SegmentacionF);
                    break;
                default:
                    CCanvasManager.Instance.StartCoroutine("vueltaDeTest");
                    CCanvasManager.Instance.DesactivarPanelComun();
                    break;
            }            
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


    // Reproduccion de estimulos auditivos
    private IEnumerator ReproducirAudiosEstimulo()
    {
        // Reproducimos audios    
        for (int _i = 0; _i < _estimuloActual.Audios.Length; _i++)
        {
            yield return StartCoroutine(CAudioManager.Instance.Reproducir(_estimuloActual.Audios[_i]));
        }
        CAudioManager.Instance.SetearARepetir(_estimuloActual.Audios);
        yield break;
    }



    // FUNCIONES GRANDES - EJEMPLO, PRACTIVA Y TEST --------------------------------------------------


   private IEnumerator CorrerEstimulo(int _indPalabra, AudioClip _audioAux, int _tipoEjPrTest)
    {
        // Desabilitamos botones
        CCanvasManager.Instance.DeshabilitarRespuesta();
        // Actualizamos info actual
        _estimuloActual = _data.ObtenerEstimulo(_indPalabra);
        _respuestaCorrecta = _estimuloActual.Respuesta;
        // Actualizar estimulo visual
        switch (_tipoCanvas)
        {
            case tipoCanvas.Opciones:
                _targetPos = CCanvasManager.Instance.asignarOpciones(_estimuloActual.VisualesImagenes, _estimuloVisual, _respuestaCorrecta);
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                break;
            case tipoCanvas.OpcionesTexto:
                _targetPos = CCanvasManager.Instance.asignarOpcionesTexto(_estimuloActual.Visuales, _opcionesLetraText, _respuestaCorrecta);
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                break;
            case tipoCanvas.OpcionesBotones:
                _targetPos = CCanvasManager.Instance.asignarOpcionesBotones(_buttonsRespuestaGO, _respuestaCorrecta, _estimuloVisual, _estimuloActual.VisualesImagenes); // Para segmentacion
                Debug.Log(_targetPos);
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                _targetPos = -1;
                break;
            case tipoCanvas.IQ:
                _targetPos = CCanvasManager.Instance.asignarOpcionesIQ(_buttonsRespuestaGO, _respuestaCorrecta, _estimuloVisual, _estimuloActual.VisualesImagenes); // Para IQ
                Debug.Log(_targetPos);
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                //_targetPos = -1;
                break;
            case tipoCanvas.VoF:
                for (int _i = 0; _i < _estimuloActual.VisualesImagenes.Length; _i++)
                {
                    _estimuloVisual[_i].sprite = _estimuloActual.VisualesImagenes[_i];
                }
                _buttonCorrecto = (_respuestaCorrecta == "1") ? _buttonsRespuesta[0] : _buttonsRespuesta[1];
                break;
            default:
                Debug.Log("Error al asignar estimulo visual");
                break;
        }
        // Reproducimos Audio ejemoplo si lo es
        if(_tipoEjPrTest == 0)
        {
            CAudioManager.Instance.ReproducirSinEspera(_audioAux);
        }        
        // Activamos visual        
        if (_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF )
        {
            CCanvasManager.Instance.PrenderVisualLargoFijo(_estimuloActual.VisualesImagenes.Length);
        }
        else
        {
            if (_tipoTest == tipoTestCCF.IQ)
            {
               CCanvasManager.Instance.PrenderBotonesLargoFijo(_estimuloActual.VisualesImagenes.Length-1);
               CCanvasManager.Instance.PrenderEstimulosLargoFijo(_estimuloActual.VisualesImagenes.Length);
            }
            else
            {
                CCanvasManager.Instance.PrenderVisual();
            }
           
        }
        // Msotramos respuesta si es ejemplo
        if (_tipoEjPrTest == 0)
        {
            // esperamos a que termine el audio y un ratito más
            yield return StartCoroutine(CAudioManager.Instance.Esperar());
            yield return new WaitForSeconds(0.8f);
            // prendemos boton correcto
            if(_tipoTest == tipoTestCCF.IQ)
            {
                yield return StartCoroutine(CCanvasManager.Instance.apretarBoton2(_buttonCorrecto));
            }
            else
            {
                yield return StartCoroutine(CCanvasManager.Instance.apretarBoton(_buttonCorrecto));
            }
            
        }
        else // Sino esperamos respuesta, etc
        {
            // Reproducimos audios          
            if(_estimuloActual.Audios != null) StartCoroutine(ReproducirAudiosEstimulo());
            // Esperamos la respuesta
            _respuesta = "NA";
            yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
            _tResp = CCanvasManager.Instance.TLog;
            // Respondemos segun si le pego o no            
            _acierto = (CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
            // Lógica de salida rpematura
            if (_tipoEjPrTest == 2)
            {
                for (int _i = _ultimasRespuestas.Length-1; _i >0; _i--)
                {
                    _ultimasRespuestas[_i] = _ultimasRespuestas[_i - 1];
                }
                if (!_acierto)
                {
                    _ultimasRespuestas[0] = 1;
                }
                else
                {
                    _ultimasRespuestas[0] = 0;
                }
                _incorrectasSeguidas = 0;
                for (int _i = 0; _i < _ultimasRespuestas.Length; _i++)
                {
                    _incorrectasSeguidas += _ultimasRespuestas[_i];
                }
            }               

            if (_tipoEjPrTest == 1)
            {
                yield return StartCoroutine(CAudioManager.Instance.ReproducirResultado(_acierto));
            }
            
        }

        // Habilitamos botones nuevamente 
        CCanvasManager.Instance.ApagarVisual();
        // Loggeo        
        if (_tipoEjPrTest == 0)
        {
            CLogManager.Instance.GuardarEjemploIQ(_estimuloActual, _numPrac,  _trialPos, _targetPos);
            output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _targetPos, "NA", "NA", "NA"));
        }
        else
        {
            CLogManager.Instance.GuardarEstimuloIQ(_estimuloActual, _numPrac,  _trialPos, _targetPos, _tResp, _respuesta, _acierto);
            output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _targetPos, _tResp, _respuesta, _acierto.ToString()));
        }
        if (_incorrectasSeguidas == 4)
        {
            _configH.marcarTareaIQ();
            CLogManager.Instance.GuardarLista(_estimuloActual.Task);
            //salirTest();
            Salir(false);
        }

        yield return new WaitForSeconds(_data.TiempoInterEnsayo);
        yield break;
    }

    //-------------------------------------------------------------------------------------------------------------------------------



}
