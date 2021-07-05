using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CTestCCF : MonoBehaviour {


    // Variables publicas a setear
    public Image[] _estimuloVisual;
    public Button[] _buttonsRespuesta;
    public GameObject[] _estimuloVisualGO, _buttonsRespuestaGO;    
    public Text[] _opcionesLetraText;

    // Nuevo manejo data
    private CListaTest _data;
    private CEstimulo _estimuloActual;
    //Para log
    private int _trialPos, _numPrac,  _targetPos;
    private bool _acierto;
   

    // Variables internas
    private Button _buttonCorrecto;
    private bool _repetir;
    private string _respuesta, _respuestaCorrecta, _tResp;
    public enum tipoTestCCF { LetrasN, LetrasS, RimasW, RimasPW, IdentificacionS, IdentificacionF, AislamientoS, AislamientoF, SegmentacionS, SegmentacionF, SintesisS, SintesisF, VOcabulario, VocabularioPPVT };
    public tipoTestCCF _tipoTest;
    public enum tipoCanvas {VoF, Opciones, OpcionesTexto, OpcionesBotones}
    public tipoCanvas _tipoCanvas;

    // Datos para loggear
    public CConfiguracion _configH;
    public bool _conf;
    public GameObject _panelCargando;

    // Contador de errores para Vocabulario PPVT
    List<int> sumWrong;

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
            yield return StartCoroutine(ejemplo(_i, _data.AudiosEjemplo[_i]));
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
                        yield return StartCoroutine(practica(_cont));
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
        if(_tipoTest == tipoTestCCF.VocabularioPPVT)
        {
            _indicesTest = _data.IndicesTest;
        }
        else
        {
            _indicesTest = _data.IndicesTestMixed;
        }
        // Prueba
        for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
//        for (int _cont = 0; _cont < 5; _cont++)
        {
            // Para vocabulario PPVT
            if (_tipoTest == tipoTestCCF.VocabularioPPVT)
            {
                int CantWrong = 0;
                for (int i = sumWrong.Count - 1; i >= 0 && i > sumWrong.Count - 1 - 5; i--)
                {
                    CantWrong += sumWrong[i];
                    //Debug.Log(sumWrong[i]);
                }
                //Debug.Log("Cant wrong: " + CantWrong);
                if (CantWrong >= 4) break;
            }
            

            _trialPos += 1;
            yield return new WaitForSeconds(_data.TiempoInterEnsayo);
            yield return StartCoroutine(testPosta(_indicesTest[_cont]));
        }
        _configH.marcarTareaCCF(_tipoTest);
        switch (_tipoTest)
        {
            case tipoTestCCF.SegmentacionF:
                CLogManager.Instance.GuardarLista("segmentF");
                break;
            case tipoTestCCF.SegmentacionS:
                CLogManager.Instance.GuardarLista("segmentS");
                break;
            default:
                CLogManager.Instance.GuardarLista(_estimuloActual.Task);
                break;
        }
        // V2018
        // CLogManager.Instance.GuardarLista(_estimuloActual.Task);
        // salirTest();

        Finish();

    }


    private void Finish(){

        StopAllCoroutines();
        CCanvasManager.Instance.DesactivarPanelComun();
        this.gameObject.SetActive(false);

        switch (_tipoTest)
        {
            case tipoTestCCF.SintesisS:
                _configH.llamarSintesisF();
                break;
            case tipoTestCCF.AislamientoS:
                _configH.llamarAislamientoF();
                break;
            case tipoTestCCF.SegmentacionS:
                _configH.llamarSegmentacionF();
                break;
            case tipoTestCCF.RimasW:
                _configH.llamarRimaPW();
                break;
            case tipoTestCCF.LetrasN:
                _configH.llamarLetterKnowledgeS();
                break;
            default:
                Salir(false);
                break;
        }                

    }

    public void Salir(bool salidaForzada){
        StopAllCoroutines();
        CCanvasManager.Instance.DesactivarPanelComun();

        this.gameObject.SetActive(false);
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);


    }

    // INICIALIZACIONES  --------------------------------------------------------------------------------
    private void inicializar()
    {
        
        CLogManager.Instance.IncrementarTaskPos();
        switch (_tipoTest)
        {
            case tipoTestCCF.LetrasS:
                _data = CInfoManager.Instance.DataLetterKnowledgeS();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.LKs);
                break;
            case tipoTestCCF.LetrasN:
                _data = CInfoManager.Instance.DataLetterKnowledgeN();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.LK);
                break;
            case tipoTestCCF.RimasW:
                _data = CInfoManager.Instance.DataRimaW();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Rima);
                break;
            case tipoTestCCF.RimasPW:
                _data = CInfoManager.Instance.DataRimaPW();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.RimaPW);
                break;
            case tipoTestCCF.AislamientoF:
                _data = CInfoManager.Instance.DataAisalamientoF();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.AislamientoF);
                break;
            case tipoTestCCF.AislamientoS:
                _data = CInfoManager.Instance.DataAisalamientoS();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Aislamiento);
                break;
            case tipoTestCCF.SintesisF:
                _data = CInfoManager.Instance.DataSintesisF();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.SintesisF);
                break;
            case tipoTestCCF.SintesisS:
                _data = CInfoManager.Instance.DataSintesisS();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Sintesis);
                break;
            case tipoTestCCF.SegmentacionF:
                _data = CInfoManager.Instance.DataSegmentacionF();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.SegmentacionF);
                break;
            case tipoTestCCF.SegmentacionS:
                _data = CInfoManager.Instance.DataSegmentacionS();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Segmentacion);
                break;
            case tipoTestCCF.VOcabulario:
                _data = CInfoManager.Instance.DataVocabulario();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Vocabulario);
                break;
            case tipoTestCCF.VocabularioPPVT:
                _data = CInfoManager.Instance.DataVocabularioPPVT();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.VocabularioPPVT);
                break;
            case tipoTestCCF.IdentificacionF:
                _data = CInfoManager.Instance.DataIdentificacionF();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.IdentificacionF);
                break;
            case tipoTestCCF.IdentificacionS:
                _data = CInfoManager.Instance.DataIdentificacionS();
                CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Identificacion);
                break;
            default:
                Debug.Log("Error al cargar datos de test");
                break;
        }

        // Canvas   
		CCanvasManager.Instance.PrenderRepeticionAlex ();
        CCanvasManager.Instance.SetearBotonesGO(_buttonsRespuestaGO);
        CCanvasManager.Instance.SetearEstimulosGO(_estimuloVisualGO);
        CCanvasManager.Instance.SetearEstimulos(_estimuloVisual);
        CCanvasManager.Instance.SetearBotones(_buttonsRespuesta);
        CCanvasManager.Instance.ApagarVisual();
        CCanvasManager.Instance.ActivarPanelComun();
        _targetPos = -1;
        sumWrong = new List<int>();
        // Canvas               

        // V2018
        // Inicializacion Output
//        output = new Output(CLogManager.Instance._textoID, System.DateTime.Now);
        output = new Output(PlayerPrefs.GetString("task"), System.DateTime.Now);
        output.setParametros(CEstimulo.GetHeaders());
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

    private void RespuestaBoton(int _i)
    {
        switch (_tipoCanvas)
        {
            case tipoCanvas.Opciones:
                _respuesta = _estimuloVisual[_i].sprite.name;
                Debug.Log("Sprite.name: " + _respuesta);
                break;
            case tipoCanvas.OpcionesTexto:
                _respuesta = _estimuloVisual[_i].GetComponentInChildren<Text>().text;
                break;
            case tipoCanvas.VoF:
                _respuesta = (_i == 0) ? "1" : "0"; // El primer botón tiene que ser el de verdadero
                break;
            case tipoCanvas.OpcionesBotones:
                _respuesta = _buttonsRespuestaGO[_i].GetComponent<Image>().sprite.name;
                Debug.Log("Sprite.name: " + _respuesta);
                break;
            default:
                Debug.Log("Error al asignar estimulo visual");
                break;
        }
        CCanvasManager.Instance.Respondio = true;
    }


    // Salir
    public bool _salirDesdePausa;
    public void salirTest()    {
        
        StopAllCoroutines();
        if (!_conf)
        {
            if (!_salirDesdePausa)
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
                _salirDesdePausa = false;
                CCanvasManager.Instance.StartCoroutine("vueltaDeTest");
                CCanvasManager.Instance.DesactivarPanelComun();
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

    // Ejemplos
    private IEnumerator ejemplo(int _indPalabra, AudioClip _audioAux)
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
        
        // Activamos visual        
        if (_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF)
        {
            CCanvasManager.Instance.PrenderVisualLargoFijo(_estimuloActual.VisualesImagenes.Length);
        }
        else
        {
            CCanvasManager.Instance.PrenderVisual();
        }
        if (_tipoTest == tipoTestCCF.VOcabulario || _tipoTest == tipoTestCCF.VocabularioPPVT)
        {
            yield return new WaitForSeconds(1f);
        }
        // Reproducimos Audio
        CAudioManager.Instance.ReproducirSinEspera(_audioAux);
        // esperamos a que termine el audio y un ratito más
        yield return StartCoroutine(CAudioManager.Instance.Esperar());
        yield return new WaitForSeconds(0.8f);
        // prendemos boton correcto
        yield return StartCoroutine(CCanvasManager.Instance.apretarBoton(_buttonCorrecto));
        // Habilitamos botones nuevamente 
        CCanvasManager.Instance.ApagarVisual();
        // Loggeo        
        CLogManager.Instance.GuardarEjemplo(_estimuloActual, _numPrac,  _trialPos, _targetPos);

        // V2018
        // TODO: Guardado Ejemplo
        output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _targetPos, "NA", "NA", "NA"));

        yield return new WaitForSeconds(0.8f);
        yield break;
    }


    // Practica
    private IEnumerator practica(int _indPalabra)
    {
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
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                _targetPos = -1;
                break;
            case tipoCanvas.VoF:
                for (int _i = 0; _i < _estimuloActual.VisualesImagenes.Length; _i++)
                {
                    _estimuloVisual[_i].sprite = _estimuloActual.VisualesImagenes[_i];
                }
                _buttonCorrecto = (_respuestaCorrecta == "1") ? _buttonsRespuesta[0] : _buttonsRespuesta[1]; // Primer boton es de tick
                break;
            default:
                Debug.Log("Error al asignar estimulo visual");
                break;
        }
        // Mostramos imagenes        
        if(_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF)
        {
            CCanvasManager.Instance.PrenderVisualLargoFijo(_estimuloActual.VisualesImagenes.Length);
        }
        else
        {
            CCanvasManager.Instance.PrenderVisual();
        }
        if (_tipoTest == tipoTestCCF.VOcabulario || _tipoTest == tipoTestCCF.VocabularioPPVT)
        {
            yield return new WaitForSeconds(1f);
        }
        // Reproducimos audios    
        if(_tipoTest == tipoTestCCF.RimasW || _tipoTest == tipoTestCCF.RimasPW || _tipoTest == tipoTestCCF.AislamientoF || _tipoTest == tipoTestCCF.AislamientoS)
        {
            yield return StartCoroutine(ReproducirAudiosEstimulo());
        }
        else
        {
            StartCoroutine(ReproducirAudiosEstimulo());
        }
       
        // Esperamos la respuesta
        _respuesta = "NA";
        if(_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF)
        {
            yield return new WaitForSeconds(3f);
        }
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
        _tResp = CCanvasManager.Instance.TLog;
        // Respondemos segun si le pego o no
        _acierto = (CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);
        yield return StartCoroutine(CAudioManager.Instance.ReproducirResultado(_acierto));
        // Apagamos Visual
        CCanvasManager.Instance.ApagarVisual();
        // Loggeo 
        CLogManager.Instance.GuardarEstimulo(_estimuloActual, _numPrac,  _trialPos, _targetPos, _tResp, _respuesta, _acierto);

        // V2018
        // TODO: Guardado Practica
        output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _targetPos, _tResp, _respuesta, _acierto.ToString()));

        yield break;
    }


    // Test
    private IEnumerator testPosta(int _indPalabra)
    {
        // Actualizamos info actual
        Debug.Log("Estímulo actual: " + _indPalabra);
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
                _buttonCorrecto = _buttonsRespuesta[_targetPos];
                _targetPos = -1;
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
        // Mostramos imagenes
        if (_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF)
        {
            CCanvasManager.Instance.PrenderVisualLargoFijo(_estimuloActual.VisualesImagenes.Length);
        }
        else
        {
            CCanvasManager.Instance.PrenderVisual();
        }
        if(_tipoTest == tipoTestCCF.VOcabulario || _tipoTest == tipoTestCCF.VocabularioPPVT)
        {
            yield return new WaitForSeconds(1f);
        }

        // Reproducimos audios        
        StartCoroutine(ReproducirAudiosEstimulo());
        // Esperamos la respuesta
        _respuesta = "NA";
        if (_tipoTest == tipoTestCCF.SintesisS || _tipoTest == tipoTestCCF.SintesisF)
        {
            yield return new WaitForSeconds(3f);
        }
        yield return StartCoroutine(CCanvasManager.Instance.esperarRespuesta(_estimuloActual.Audios, _data.TiempoRespMaximo));
        _tResp = CCanvasManager.Instance.TLog;
        //Actualizamos estadisticas
        _acierto = (CCanvasManager.Instance.Respondio & _respuesta == _respuestaCorrecta);

        sumWrong.Add(_acierto?0:1);

        // Apagamos visual
        CCanvasManager.Instance.ApagarVisual();
        // loggeo
        CLogManager.Instance.GuardarEstimulo(_estimuloActual, _numPrac,  _trialPos, _targetPos, _tResp, _respuesta, _acierto);

        // V2018
        // TODO: Guardado Test Posta
        output.addItem(_estimuloActual.GetEstimulo(_numPrac, _trialPos, _targetPos, _tResp, _respuesta, _acierto.ToString()));

        yield break;

    }


}
