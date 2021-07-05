using UnityEngine;
using System.Collections;

public class CGameManager : MonoBehaviour {

    public CmanejoDeGlobos _globosHanlder;
    public string _session, _lista;
	// Listas de tests
    public enum TestTipe { Segmentacion, Sintesis, Rima, STMs, Identificacion, LK, Vocabulario, VocabularioPPVT, STMo, Aislamiento, RAN ,
		SegmentacionF, SintesisF, RimaPW,LKs,AislamientoF, IdentificacionF,
		VisEn, VisSeg, Tapping, DichListSyl, DichListMus, Reading, IQ};
    private TestTipe[] _tableroA1, _tableroA2, _tableroA3, _tableroA4, _tableroA5,
        _tableroB1, _tableroB2, _tableroB3, _tableroB4, _tableroB5,
        _tableroC1, _tableroC2, _tableroC3, _tableroC4, _tableroC5;

    //_tablero1Tests,_tablero2Tests,_tablero3Tests,_tablero4Tests;
    // Scripts de tests
	public CTest5 _TestSTMorden;
    public CTest6 _TestSTMsimon;
    public CTestCCF _TestSegS, _TestSegF, _TestAisS, _TestAisF, _TestLetterKnowledgeS, _TestLetterKnowledgeN, _TestSintesisS,
        _TestSintesisF, _TestVocabulario, _TestVocabularioPPVT, _TestIdentificacionS, _TestIdentificacionF, _TestRimaW, _TestRimaPW;
	public CTestDichListMus3 _TestDichListMus;
	public CTestDichListSyl _TestDichListSyl;
	public CTapping2 _TestTapping;
	public CTestIQ _TestIQ;
	public TestRAN _TestRAN , _TestLectura;
    public CTestVisualSeg _TestVisSeg;
    public CTestVisualEnt _TestVisEn;
	// Paneles de tests
    public GameObject _panelSegS, _panelSegF, _panelAisS, _panelAisF, _panelSTMorden, _panelSTMsimon, _panelRAN,
        _panelLNS, _panelLNN, _panelSintesisS, _panelSintesisF, _panelVocabulario, _panelVocabularioPPVT, _panelIdentificacionS, _panelIdentificacionF, _panelRimaW, _panelRimaPW,
	_panelTapping, _panelVisSeg, _panelVisEn, _panelDichListSyl, _panelDichListMus, _panelReading,_panelIQ;
	// Tableros
    public GameObject _panelTablero1, _panelTablero2, _panelTablero3;
    private CTablero _tablero1, _tablero2, _tablero3;
    public CTablero _tableroActual;

    // Singleton
    public static CGameManager Instance;

    void Awake()
    {
        Instance = this;
        Input.multiTouchEnabled = false;
    }


    // Use this for initialization
    void Start () {
        _tableroA3 = new TestTipe[4] { TestTipe.Sintesis, TestTipe.VisSeg, TestTipe.Tapping, TestTipe.Rima };
        _tableroA1 = new TestTipe[4] { _tableroA3[3], _tableroA3[0], _tableroA3[1], _tableroA3[2] };
        _tableroA2 = new TestTipe[4] { _tableroA3[0], _tableroA3[1], _tableroA3[3], _tableroA3[2] };
        _tableroA4 = new TestTipe[4] { _tableroA3[0], _tableroA3[3] ,_tableroA3[2], _tableroA3[1] };
        _tableroA5 = new TestTipe[4] { _tableroA3[0], _tableroA3[2], _tableroA3[1], _tableroA3[3] };
        _tableroB3 = new TestTipe[5] { TestTipe.LK, TestTipe.Segmentacion, TestTipe.STMs, TestTipe.VocabularioPPVT, TestTipe.VisEn };
        _tableroB1 = new TestTipe[5] { _tableroB3[3], _tableroB3[4], _tableroB3[0], _tableroB3[1], _tableroB3[2] };
        _tableroB2 = new TestTipe[5] { _tableroB3[0], _tableroB3[1], _tableroB3[4], _tableroB3[3], _tableroB3[2] };
        _tableroB4 = new TestTipe[5] { _tableroB3[0], _tableroB3[4], _tableroB3[3], _tableroB3[2], _tableroB3[1] };
        _tableroB5 = new TestTipe[5] { _tableroB3[0], _tableroB3[2], _tableroB3[1], _tableroB3[4], _tableroB3[3] };
        _tableroC3 = new TestTipe[4] { TestTipe.Vocabulario, TestTipe.Aislamiento, TestTipe.IQ, TestTipe.STMo };
        _tableroC1 = new TestTipe[4] { _tableroC3[3], _tableroC3[0], _tableroC3[1], _tableroC3[2] };
        _tableroC2 = new TestTipe[4] { _tableroC3[0], _tableroC3[1], _tableroC3[3], _tableroC3[2] };
        _tableroC4 = new TestTipe[4] { _tableroC3[0], _tableroC3[3], _tableroC3[2], _tableroC3[1] };
        _tableroC5 = new TestTipe[4] { _tableroC3[0], _tableroC3[2], _tableroC3[1], _tableroC3[3] };
        // Handlers Tableros
        _tablero1 = _panelTablero1.GetComponent<CTablero>();
        _tablero2 = _panelTablero2.GetComponent<CTablero>();
        _tablero3 = _panelTablero3.GetComponent<CTablero>();
    }


	//private TestTipe[] ListaMixed(TestTipe[] _listaOriginal)
	//{
	//	TestTipe[] _listaDestino; 
	//	int[] _indOriginal;
 //       if(_listaOriginal.Length == 4)
 //       {
 //           _indOriginal = new int[3] { 1, 2, 3 };
 //           _listaDestino = new TestTipe[4];
 //       }
 //       else
 //       {
 //           _indOriginal = new int[2] { 1, 2 };
 //           _listaDestino = new TestTipe[3];
 //       }
		
	//	int[] _indMixed = _indOriginal;
	//	for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
	//	{
	//		var r = Random.Range(0, i+1);
	//		var tmp = _indMixed[i];
	//		_indMixed[i] = _indMixed[r];
	//		_indMixed[r] = tmp;
	//	}
	//	_listaDestino [0] = _listaOriginal [0];
	//	for (int _i = 0; _i<_indMixed.Length; _i++) {
	//		_listaDestino[_i+1] = _listaOriginal [_indMixed[_i]];
	//	}
	//	return _listaDestino;
	//}


    public TestTipe[] ObtenerLIstaTests()
    {
        TestTipe[] _listaMezclada;
        switch (_lista)
        {
            case "A1":
                _listaMezclada = _tableroA1;
                break;
            case "A2":
                _listaMezclada = _tableroA2;
                break;
            case "A3":
                _listaMezclada = _tableroA3;
                break;
            case "A4":
                _listaMezclada = _tableroA4;
                break;
            case "A5":
                _listaMezclada = _tableroA5;
                break;
            case "B1":
                _listaMezclada = _tableroB1;
                break;
            case "B2":
                _listaMezclada = _tableroB2;
                break;
            case "B3":
                _listaMezclada = _tableroB3;
                break;
            case "B4":
                _listaMezclada = _tableroB4;
                break;
            case "B5":
                _listaMezclada = _tableroB5;
                break;
            case "C1":
                _listaMezclada = _tableroC1;
                break;
            case "C2":
                _listaMezclada = _tableroC2;
                break;
            case "C3":
                _listaMezclada = _tableroC3;
                break;
            case "C4":
                _listaMezclada = _tableroC4;
                break;
            case "C5":
                _listaMezclada = _tableroC5;
                break;
            default:
                Debug.Log("Error al cargar la lista de Tests");
                _listaMezclada = null;
                break;
        }
        _globosHanlder.SetearTablero (_lista);
		return _listaMezclada;
    }



    public void ReiniciarJuego()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void llamarTablero()
    {
        if (_lista == "A1" || _lista == "A2" || _lista == "A3" || _lista == "A4" || _lista == "A5")
        {
            _panelTablero1.SetActive(true);
            _tableroActual = _tablero1;
        }
        else if (_lista == "B1" || _lista == "B2" || _lista == "B3" || _lista == "B4" || _lista == "B5")
        {
            _panelTablero2.SetActive(true);
            _tableroActual = _tablero2;
        }
        else if (_lista == "C1" || _lista == "C2" || _lista == "C3" || _lista == "C4" || _lista == "C5")
        {
            _panelTablero3.SetActive(true);
            _tableroActual = _tablero3;
        }
        else
        {
            Debug.Log("Error en llamarTablero");
        }

    }


    public void llamarTest(CGameManager.TestTipe _testAux)
    {
        switch (_testAux)
        {
            case CGameManager.TestTipe.Segmentacion:
                _panelSegS.SetActive(true);
                _TestSegS.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.SegmentacionF:
                _panelSegF.SetActive(true);
                _TestSegF.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.Sintesis:
                _panelSintesisS.SetActive(true);
                _TestSintesisS.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.SintesisF:
                _panelSintesisF.SetActive(true);
                _TestSintesisF.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.Rima:
                _panelRimaW.SetActive(true);
                _TestRimaW.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.RimaPW:
                _panelRimaPW.SetActive(true);
                _TestRimaPW.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.STMs:
                _panelSTMsimon.SetActive(true);
                _TestSTMsimon.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.Identificacion:
                _panelIdentificacionS.SetActive(true);
                _TestIdentificacionS.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.IdentificacionF:
                _panelIdentificacionF.SetActive(true);
                _TestIdentificacionF.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.LK:
                _panelLNN.SetActive(true);
                _TestLetterKnowledgeN.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.LKs:
                _panelLNS.SetActive(true);
                _TestLetterKnowledgeS.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.Vocabulario:
                _panelVocabulario.SetActive(true);
                _TestVocabulario.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.VocabularioPPVT:
                _panelVocabularioPPVT.SetActive(true);
                _TestVocabularioPPVT.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.STMo:
                _panelSTMorden.SetActive(true);
                _TestSTMorden.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.Aislamiento:
                _panelAisS.SetActive(true);
                _TestAisS.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.AislamientoF:
                _panelAisF.SetActive(true);
                _TestAisF.StartCoroutine("CorrerTest");
                break;
            case CGameManager.TestTipe.RAN:
                _panelRAN.SetActive(true);
                _TestRAN.StartCoroutine("CorrerTest");
                break;
		case CGameManager.TestTipe.VisSeg:
			_panelVisSeg.SetActive(true);
		    _TestVisSeg.StartCoroutine("CorrerTest");
			break;
		case CGameManager.TestTipe.VisEn:
            _panelVisEn.SetActive(true);
            _TestVisEn.StartCoroutine("CorrerTest");
            break;
		case CGameManager.TestTipe.Tapping:
			_panelTapping.SetActive(true);
			_TestTapping.StartCoroutine("CorrerTest");
                break;
		case CGameManager.TestTipe.DichListSyl:
			_panelDichListSyl.SetActive(true);
			_TestDichListSyl.StartCoroutine("CorrerTest");
			break;
		case CGameManager.TestTipe.DichListMus:
			_panelDichListMus.SetActive(true);
			_TestDichListMus.StartCoroutine("CorrerTest");
			break;
		case CGameManager.TestTipe.Reading:
			_panelReading.SetActive(true);
			_TestLectura.StartCoroutine("CorrerTest");
			break;
		case CGameManager.TestTipe.IQ:
			_panelIQ.SetActive(true);
			_TestIQ.StartCoroutine("CorrerTest");
			break;
        }
    }









}
