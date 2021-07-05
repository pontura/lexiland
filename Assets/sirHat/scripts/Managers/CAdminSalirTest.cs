using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CAdminSalirTest : MonoBehaviour {

    //public GameObject _adminPanel;
	public string _password;		
	public InputField _textoPassword;	
	public GameObject _wrongPasswordMsg;
   // public GameObject _panelPausa, _unpauseButon;
    public CTest5 _TestSTMorden;
    public CTest6 _TestSTMsimon;
    public CTestCCF _TestSegS, _TestSegF, _TestAisS, _TestAisF, _TestLetterKnowledgeS, _TestLetterKnowledgeN, _TestSintesisS,
        _TestSintesisF, _TestVocabulario, _TestVocabularioPPVT, _TestIdentificacionS, _TestIdentificacionF, _TestRimaW, _TestRimaPW;
    public TestRAN _TestRAN, _TestLectura;
    public CTestIQ _TestIQ;
    public CTapping2 _TestTapping;
    public CTestDichListSyl _TestDichListSyl;
    public CTestDichListMus3 _TestDichListMus;
    public CTestVisualEnt _TestVisEnt;
    public CTestVisualSeg _TestVisSeg;

    //public GameObject panelPostTarea;
    //public GameObject panelPostJuego;

    // Singleton
    public static CAdminSalirTest Instance;

    void Awake()
    {
        Instance = this;
    }


    public void CheckPassword()
	{
		if(_password == _textoPassword.text)
		{
			Debug.Log("Correctooooo maldito");
			_wrongPasswordMsg.SetActive(false);
            // llamar panel de admin           
            _textoPassword.text = "";
            CCanvasManager.Instance.unPausar();
            CAudioManager.Instance.RepeticionAlex = false;
            CAudioManager.Instance.FrenarAudios();
            CAudioManager.Instance.SetearARepetir(null);
            SalirDelTest();
            // _panelPausa.SetActive(false);
            // _unpauseButon.SetActive(false);
        }
		else
		{
            _textoPassword.text = "";
            _wrongPasswordMsg.SetActive(true);
		}
	}
	
	public void Return()
	{
        // volver a pantalla anterior
        _textoPassword.text = "";
        this.gameObject.SetActive(false);
        CAudioManager.Instance.PrenderMusica();
	}


    private CGameManager.TestTipe _testActual;
    public void SetTestActual(CGameManager.TestTipe _test)
    {
        _testActual = _test;
        CCanvasManager.Instance.apagarPanelPractica();
    }


    // V2018

    private void SalirDelTest()
    {
        //panelPostJuego.SetActive(false);

        switch (_testActual)
        {
            case CGameManager.TestTipe.Segmentacion:
                _TestSegS._salirDesdePausa = true;
                //_TestSegS.salirTest();
                _TestSegS.Salir(true);
                break;
            case CGameManager.TestTipe.SegmentacionF:
                //_TestSegF.salirTest();
                _TestSegF.Salir(true);
                break;
            case CGameManager.TestTipe.Sintesis:
                _TestSintesisS._salirDesdePausa = true;
                //_TestSintesisS.salirTest();
                _TestSintesisS.Salir(true);
                break;
            case CGameManager.TestTipe.SintesisF:
                //_TestSintesisF.salirTest();
                _TestSintesisF.Salir(true);
                break;
            case CGameManager.TestTipe.Rima:
                _TestRimaW._salirDesdePausa = true;
                //_TestRimaW.salirTest();
                _TestRimaW.Salir(true);
                break;
            case CGameManager.TestTipe.RimaPW:
                //_TestRimaPW.salirTest();
                _TestRimaPW.Salir(true);
                break;
            case CGameManager.TestTipe.STMs:
                //_TestSTMsimon.salirTest();
                _TestSTMsimon.Salir(true);
                break;
            case CGameManager.TestTipe.Identificacion:
                //_TestIdentificacionF.salirTest();
                _TestIdentificacionF.Salir(true);
                break;
            case CGameManager.TestTipe.IdentificacionF:
                //_TestIdentificacionF.salirTest();
                _TestIdentificacionF.Salir(true);
                break;
            case CGameManager.TestTipe.LK:
                _TestLetterKnowledgeN._salirDesdePausa = true;
                //_TestLetterKnowledgeN.salirTest();
                _TestLetterKnowledgeN.Salir(true);
                break;
            case CGameManager.TestTipe.LKs:
                //_TestLetterKnowledgeS.salirTest();
                _TestLetterKnowledgeS.Salir(true);
                break;
            case CGameManager.TestTipe.Vocabulario:
                //_TestVocabulario.salirTest();
                _TestVocabulario.Salir(true);
                break;
            case CGameManager.TestTipe.VocabularioPPVT:
                //_TestVocabularioPPVT.salirTest();
                _TestVocabularioPPVT.Salir(true);
                break;
            case CGameManager.TestTipe.STMo:
                //_TestSTMorden.salirTest();
                _TestSTMorden.Salir(true);
                break;
            case CGameManager.TestTipe.Aislamiento:
                _TestAisS._salirDesdePausa = true;
                //_TestAisS.salirTest();
                _TestAisS.Salir(true);
                break;
            case CGameManager.TestTipe.AislamientoF:
                //_TestAisF.salirTest();
                _TestAisF.Salir(true);
                break;
            case CGameManager.TestTipe.RAN:
                //_TestRAN.salirTest();
                _TestRAN.Salir(true);
                break;
            case CGameManager.TestTipe.VisSeg:
                //_TestVisSeg.salirTest();
                _TestVisSeg.Salir(true);
                break;
            case CGameManager.TestTipe.VisEn:
                //_TestVisEnt.salirTest();
                _TestVisEnt.Salir(true);
                break;
            case CGameManager.TestTipe.Tapping:
                //_TestTapping.salirTest();
                _TestTapping.Salir(true);
                break;
            case CGameManager.TestTipe.DichListSyl:
                //_TestDichListSyl.salirTest();
                _TestDichListSyl.Salir(true);
                break;
            case CGameManager.TestTipe.DichListMus:
                //_TestDichListMus.salirTest();
                _TestDichListMus.Salir(true);
                break;
            case CGameManager.TestTipe.Reading:
                //_TestLectura.salirTest();
                _TestLectura.Salir(true);
                break;
            case CGameManager.TestTipe.IQ:
                //_TestIQ.salirTest();
                _TestIQ.Salir(true);
                break;
        }
    
    }

}
