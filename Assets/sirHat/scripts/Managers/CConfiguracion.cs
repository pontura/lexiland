using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CConfiguracion : MonoBehaviour {

   // private bool[] _testsCorridos;
   // private Toggle[] _toggles;
    public Toggle _toggleSegS, _toggleSegF, _toggleAisS, _toggleAisF, _toggleSTMorden, _toggleSTMsimon, _toggleRAN,
        _toggleLNS, _toggleLNN, _toggleSintesisS, _toggleSintesisF, _toggleVocabulario, _toggleVocabularioPPVT, _toggleIdentificacionS, _toggleIdentificacionF, _toggleRimaW , _toggleRimaPW,
	_toggleLectura, _toggleIQ, _toggleTapping, _toggleDichListSyl, _toggleDichListMus, _toggleVisE, _toggleVisSeg,
        _toggleLecturaBCBL, _toggleLecturaPRO, _toggleLecturaSEM, _toggleLecturaSINT, _toggleLecturaSS;

    public GameObject _butonVuelta;
    public CTest5 _TestSTMorden;
    public CTest6 _TestSTMsimon;
    public CTestCCF _TestSegS, _TestSegF,_TestAisS, _TestAisF, _TestLetterKnowledgeS, _TestLetterKnowledgeN, _TestSintesisS,
        _TestSintesisF, _TestVocabulario, _TestVocabularioPPVT, _TestIdentificacionS, _TestIdentificacionF, _TestRimaW, _TestRimaPW;
    public TestRAN _TestRAN , _TestLectura;
    public CTestIQ _TestIQ;
    public CTapping2 _TestTapping;
    public CTestDichListSyl _TestDichListSyl;
	public CTestDichListMus3 _TestDichListMus;
    public CTestVisualEnt _TestVisEnt;
    public CTestVisualSeg _TestVisSeg;
    public GameObject _panelSegS, _panelSegF, _panelAisS, _panelAisF, _panelSTMorden, _panelSTMsimon,_panelRAN,
        _panelLNS, _panelLNN, _panelSintesisS, _panelSintesisF, _panelVocabulario, _panelVocabularioPPVT, _panelIdentificacionS, _panelIdentificacionF, _panelRimaW, _panelRimaPW,
	_panelLectura, _panelIQ, _panelTapping, _panelDichListSyl, _panelDichListMus, _panelVisEnt, _panelVisSeg;
    private bool _modoConf;

    public GameObject _panelJuego;

    void OnEnable()
    {
        CCanvasManager.Instance.DesactivarPanelComun();
        CCanvasManager.Instance.ApagarMano();
        CCanvasManager.Instance.PrenderRepeticionAlex();
        CAudioManager.Instance.RepeticionAlex = false;
        CAudioManager.Instance.FrenarAudios();
        CAudioManager.Instance.SetearARepetir(null);
    }

    public void SalirDeConfiguracion()
    {
        CAudioManager.Instance.PrenderMusica();
        this.gameObject.SetActive(false);
    }    

    // Setear modo configuracion
    public bool ModoConf
    {
        get { return _modoConf; }
        set { _modoConf = value; }
    }
    public void DesactivarVuelta()
    {
        _butonVuelta.SetActive(false);
    }

   

    // lalamr tests
    public void llamarSegmentacionS()
    {
        PlayerPrefs.SetString("task", "SegmentacionS");
        CAudioManager.Instance.ApagarMusica();
        _panelSegS.SetActive(true);
        _TestSegS._conf = true;
        _TestSegS.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarSegmentacionF()
    {
        PlayerPrefs.SetString("task", "SegmentacionF");
        CAudioManager.Instance.ApagarMusica();
        _panelSegF.SetActive(true);
        _TestSegF._conf = true;
        _TestSegF.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarAislamientoS()
    {
        PlayerPrefs.SetString("task", "AislamientoS");
        CAudioManager.Instance.ApagarMusica();
        _panelAisS.SetActive(true);
        _TestAisS._conf = true;
        _TestAisS.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarAislamientoF()
    {
        PlayerPrefs.SetString("task", "AislamientoF");
        CAudioManager.Instance.ApagarMusica();
        _panelAisF.SetActive(true);
        _TestAisF._conf = true;
        _TestAisF.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarSTMorden()
    {
        PlayerPrefs.SetString("task", "STMorden");
        CAudioManager.Instance.ApagarMusica();
        _panelSTMorden.SetActive(true);
        _TestSTMorden._conf = true;
        _TestSTMorden.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarSTMsimon()
    {
        PlayerPrefs.SetString("task", "STMsimon");
        CAudioManager.Instance.ApagarMusica();
        _panelSTMsimon.SetActive(true);
        _TestSTMsimon._conf = true;
        _TestSTMsimon.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarSintesisS()
    {
        PlayerPrefs.SetString("task", "SintesisS");
        CAudioManager.Instance.ApagarMusica();
        _panelSintesisS.SetActive(true);
        _TestSintesisS._conf = true;
        _TestSintesisS.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarSintesisF()
    {
        PlayerPrefs.SetString("task", "SintesisF");
        CAudioManager.Instance.ApagarMusica();
        _panelSintesisF.SetActive(true);
        _TestSintesisF._conf = true;
        _TestSintesisF.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarLetterKnowledgeN()
    {
        PlayerPrefs.SetString("task", "LKN");
        CAudioManager.Instance.ApagarMusica();
        _panelLNN.SetActive(true);
        _TestLetterKnowledgeN._conf = true;
        _TestLetterKnowledgeN.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarLetterKnowledgeS()
    {
        PlayerPrefs.SetString("task", "LKS");
        CAudioManager.Instance.ApagarMusica();
        _panelLNS.SetActive(true);
        _TestLetterKnowledgeS._conf = true;
        _TestLetterKnowledgeS.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarIdentificacionS()
    {
        PlayerPrefs.SetString("task", "IdentificacionS");
        CAudioManager.Instance.ApagarMusica();
        _panelIdentificacionS.SetActive(true);
        _TestIdentificacionS._conf = true;
        _TestIdentificacionS.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarIdentificacionF()
    {
        PlayerPrefs.SetString("task", "IdentificacionF");
        CAudioManager.Instance.ApagarMusica();
        _panelIdentificacionF.SetActive(true);
        _TestIdentificacionF._conf = true;
        _TestIdentificacionF.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarVocabulario()
    {
        PlayerPrefs.SetString("task", "Vocabulario");
        CAudioManager.Instance.ApagarMusica();
        _panelVocabulario.SetActive(true);
        _TestVocabulario._conf = true;
        _TestVocabulario.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarVocabularioPPVT()
    {
        PlayerPrefs.SetString("task", "VocabularioPPVT");
        CAudioManager.Instance.ApagarMusica();
        _panelVocabularioPPVT.SetActive(true);
        _TestVocabularioPPVT._conf = true;
        _TestVocabularioPPVT.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarRimaW()
    {
        PlayerPrefs.SetString("task", "RimaW");
        CAudioManager.Instance.ApagarMusica();
        _panelRimaW.SetActive(true);
        _TestRimaW._conf = true;
        _TestRimaW.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarRimaPW()
    {
        PlayerPrefs.SetString("task", "RimaPW");
        CAudioManager.Instance.ApagarMusica();
        _panelRimaPW.SetActive(true);
        _TestRimaPW._conf = true;
        _TestRimaPW.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarLectura()
    {
        PlayerPrefs.SetString("task", "Lectura");
        CAudioManager.Instance.ApagarMusica();
        _panelLectura.SetActive(true);
        _TestLectura._conf = true;
        _TestLectura.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarIQ()
    {
        PlayerPrefs.SetString("task", "IQ");
        CAudioManager.Instance.ApagarMusica();
        _panelIQ.SetActive(true);
        _TestIQ._conf = true;
        _TestIQ.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarTapping()
    {
        PlayerPrefs.SetString("task", "Tapping");
        CAudioManager.Instance.ApagarMusica();
        _panelTapping.SetActive(true);
        _TestTapping._conf = true;
        _TestTapping.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarDichListSyl()
    {
        PlayerPrefs.SetString("task", "DLS");
        CAudioManager.Instance.ApagarMusica();
        _panelDichListSyl.SetActive(true);
        _TestDichListSyl._conf = true;
        _TestDichListSyl.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
	public void llamarDichListMus()
	{
        PlayerPrefs.SetString("task", "DLM");
		CAudioManager.Instance.ApagarMusica();
		_panelDichListMus.SetActive(true);
		_TestDichListMus._conf = true;
		_TestDichListMus.StartCoroutine("CorrerTest");
		this.gameObject.SetActive(false);
	}
    public void llamarVisEnt()
    {
        PlayerPrefs.SetString("task", "VisEnt");
        CAudioManager.Instance.ApagarMusica();
        _panelVisEnt.SetActive(true);
        _TestVisEnt._conf = true;
        _TestVisEnt.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }
    public void llamarVisSeg()
    {
        PlayerPrefs.SetString("task", "VisSeg");
        CAudioManager.Instance.ApagarMusica();
        _panelVisSeg.SetActive(true);
        _TestVisSeg._conf = true;
        _TestVisSeg.StartCoroutine("CorrerTest");
        this.gameObject.SetActive(false);
    }

    // Llamadas a las nuevas Tareas  -----------------------------------------
    public void llamalLecturaPRO()
    {
        PlayerPrefs.SetString("task", "LecturaPro");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadPRO/ReadPRO"));

        this.gameObject.SetActive(false);
    }

    /*
    public void llamalLecturaBCBL()
    {
        PlayerPrefs.SetString("task", "LecturaBCBL");
        CAudioManager.Instance.ApagarMusica();
        
        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadBCBL/ReadBCBL"));

        this.gameObject.SetActive(false);
    }

    public void llamalLecturaBCBL_v()
    {
        PlayerPrefs.SetString("task", "LecturaBCBL");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadBCBL/ReadBCBL_v"));

        this.gameObject.SetActive(false);
    }
    */

    public void llamaBCBL(){
        PlayerPrefs.SetString("task", "LecturaBCBL");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadBCBL/ReadBCBL_v"));

        this.gameObject.SetActive(false);
    }

    public void llamarRAN()
    {
        PlayerPrefs.SetString("task", "RAN");
        CAudioManager.Instance.ApagarMusica();

        CTarea.Inst.LoadTarea(Resources.Load<CTareaData>("RAN/RAN"));

        _panelRAN.SetActive(true);
        _panelRAN.GetComponent<CRAN>().setForm();

        this.gameObject.SetActive(false);
    }

    public void llamarComprension(){
        PlayerPrefs.SetString("task", "Comprension");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSINT/ReadSINT"));

        this.gameObject.SetActive(false);

    }


    public void llamalLecturaSEM()
    {
        PlayerPrefs.SetString("task", "LecturaSEM");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSEM/ReadSEM_form"));

        this.gameObject.SetActive(false);
    }

    public void llamalLecturaSEM_MO()
    {
        PlayerPrefs.SetString("task", "LecturaSEM");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSEM/ReadSEM_MO"));

        this.gameObject.SetActive(false);
    }


    public void llamalLecturaSINT()
    {
        PlayerPrefs.SetString("task", "LecturaSINT");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSINT/ReadSINT"));

        this.gameObject.SetActive(false);
    }

    public void llamalLecturaSS()
    {
        PlayerPrefs.SetString("task", "LecturaSS");
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSS/ReadSS"));

        this.gameObject.SetActive(false);
    }

    public void llamaleditar(CTareaData td)
    {
        CAudioManager.Instance.ApagarMusica();

        CManager.Inst.irAEdit(td);

        this.gameObject.SetActive(false);
    }


    // --------------------------------------------------------------
    // toggles
    public void marcarTareaVisSeg()
    {
        _toggleVisSeg.isOn = true;
    }
    public void marcarTareaSegS()
    {
        _toggleSegS.isOn = true;
    }
    public void marcarTareaSegF()
    {
        _toggleSegF.isOn = true;
    }
    public void marcarTareaAisS()
    {
        _toggleAisS.isOn = true;
    }
    public void marcarTareaAisF()
    {
        _toggleAisF.isOn = true;
    }
    public void marcarTareaSTMorden()
    {
        _toggleSTMorden.isOn = true;
    }
    public void marcarTareaSTMsimon()
    {
        _toggleSTMsimon.isOn = true;
    }
    public void marcarTareaRAN()
    {
        _toggleRAN.isOn = true;
        CLogManager.Instance.GuardarLista("RAN");
    }
    public void marcarTareaSintesisS()
    {
        _toggleSintesisS.isOn = true;
    }
    public void marcarTareaSintesisF()
    {
        _toggleSintesisF.isOn = true;
    }
    public void marcarTareaLNN()
    {
        _toggleLNN.isOn = true;
    }
    public void marcarTareaLNS()
    {
        _toggleLNS.isOn = true;
    }
    public void marcarTareaIdentificacionS()
    {
        _toggleIdentificacionS.isOn = true;
    }
    public void marcarTareaIdentificacionF()
    {
        _toggleIdentificacionF.isOn = true;
    }
    public void marcarTareaVocabulario()
    {
        _toggleVocabulario.isOn = true;
    }
    public void marcarTareaLectura()
    {
        _toggleLectura.isOn = true;
    }
    public void marcarTareaIQ()
    {
        _toggleIQ.isOn = true;
    }
    public void marcarTareaTapping()
    {
        _toggleTapping.isOn = true;
    }
    public void marcarTareaDichListSyl()
    {
        _toggleDichListSyl.isOn = true;
    }
	public void marcarTareaDichListMus()
	{
		_toggleDichListMus.isOn = true;
	}
    public void marcarTareVisEnt()
    {
        _toggleVisE.isOn = true;
    }

    public void marcarTareaLecturaBCBL()
    {
        _toggleLecturaBCBL.isOn = true;
        CLogManager.Instance.GuardarLista("ReadBCBL");
    }
    public void marcarTareaLecturaPRO()
    {
        _toggleLecturaPRO.isOn = true;
        CLogManager.Instance.GuardarLista("ReadPRO");
    }
    public void marcarTareaLecturaSEM()
    {
        _toggleLecturaSEM.isOn = true;
        CLogManager.Instance.GuardarLista("ReadSEM");
    }
    public void marcarTareaLecturaSINT()
    {
        _toggleLecturaSINT.isOn = true;
        CLogManager.Instance.GuardarLista("ReadSINT");
    }
    public void marcarTareaLecturaSS()
    {
        _toggleLecturaSS.isOn = true;
        CLogManager.Instance.GuardarLista("ReadSS");

    }




    public void marcarTareaCCF(CTestCCF.tipoTestCCF _tipoTestCCF)
    {
        switch (_tipoTestCCF)
        {
            case CTestCCF.tipoTestCCF.LetrasS:
                _toggleLNS.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.LetrasN:
                _toggleLNN.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.RimasW:
                _toggleRimaW.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.RimasPW:
                _toggleRimaPW.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.AislamientoF:
                _toggleAisF.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.AislamientoS:
                _toggleAisS.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.SintesisF:
                _toggleSintesisF.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.SintesisS:
                _toggleSintesisS.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.SegmentacionF:
                _toggleSegF.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.SegmentacionS:
                _toggleSegS.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.VOcabulario:
                _toggleVocabulario.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.VocabularioPPVT:
                _toggleVocabularioPPVT.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.IdentificacionF:
                _toggleIdentificacionF.isOn = true;
                break;
            case CTestCCF.tipoTestCCF.IdentificacionS:
                _toggleIdentificacionS.isOn = true;
                break;
            default:
                Debug.Log("Error al cargar datos de test");
                break;
        }
    }
    
}
