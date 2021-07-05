using UnityEngine;
using System.Collections;

public class CInfoManager : MonoBehaviour
{
    public static CInfoManager Instance;
    private AudioClip[] _audiosAuxEjemplos;
    private AudioClip _audioAuxInstrucciones; 

    void Awake()
    {
        Instance = this;
    }

    // ---------- CON CARGA POR ACTIVIDAD -------------------------------------------------------

    public CListaTest DataRimaW()
    {
        CListaTest _data = new CListaTest("inputs/rimaW", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/rimaW_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/rimaW_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/rimaW_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("RimaW Cargado");
        return _data;
    }
    public CListaTest DataRimaPW()
    {
        CListaTest _data = new CListaTest("inputs/rimaPW", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[0];
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/rimaPW_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("RimaPW Cargado");
        return _data;
    }
    public CListaTest DataVocabulario()
    {
        CListaTest _data = new CListaTest("inputs/vocabulario", "audiosVocabulario/", "imagenesVocabulario/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/vocabulario_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/vocabulario_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/vocabulario_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("Vocabulario Cargado");
        return _data;
    }

    public CListaTest DataVocabularioPPVT()
    {
        CListaTest _data = new CListaTest("inputs/vocabularioPPVT", "audiosVocabularioPPVT/", "imagenesVocabularioPPVT/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/vocabulario_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/vocabulario_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/vocabulario_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("Vocabulario PPVT Cargado");
        return _data;
    }



    public CListaTest DataIdentificacionS()
    {
        CListaTest _data = new CListaTest("inputs/identificacionS", "audiosIdentificacion/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[3];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/identificacionS_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/identificacionS_E2");
        _audiosAuxEjemplos[2] = Resources.Load<AudioClip>("audiosEjemplos/identificacionS_E3");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/identificacionS_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("IdentifiacionS Cargado");
        return _data;
    }
    public CListaTest DataIdentificacionF()
    {
        CListaTest _data = new CListaTest("inputs/identificacionF", "audiosIdentificacion/", "imagenes/");
        _data.TiempoRespMaximo = 60f;
        Debug.Log("IdentifiacionF Cargado");
        return _data;
    }
    public CListaTest DataSintesisS()
    {
        CListaTest _data = new CListaTest("inputs/sintesisS", "audiosSintesis/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/sintesisS_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/sintesisS_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/sintesisS_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("SintesisS Cargado");
        return _data;
    }
    public CListaTest DataSintesisF()
    {
        CListaTest _data = new CListaTest("inputs/sintesisF", "audiosSintesis/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/sintesisF_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/sintesisF_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/sintesisF_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("SintesisF Cargado");
        return _data;
    }
    public CListaTest DataAisalamientoS()
    {
        CListaTest _data = new CListaTest("inputs/aislamientoS", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[3];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoS_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoS_E2");
        _audiosAuxEjemplos[2] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoS_E3");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/aislamientoS_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("AisalmeientoS Cargado");
        return _data;
    }
    public CListaTest DataAisalamientoF()
    {
        CListaTest _data = new CListaTest("inputs/aislamientoF", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[3];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoF_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoF_E2");
        _audiosAuxEjemplos[2] = Resources.Load<AudioClip>("audiosEjemplos/aislamientoF_E3");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/aislamientoF_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("AisalmeientoF Cargado");
        return _data;
    }
    public CListaTest DataSegmentacionS()
    {
        CListaTest _data = new CListaTest("inputs/segmentacionS", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[3];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/segmentacionS_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/segmentacionS_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/segmentacionS_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("SegmentacionS Cargado");
        return _data;
    }
    public CListaTest DataSegmentacionF()
    {
        CListaTest _data = new CListaTest("inputs/segmentacionF", "audios/", "imagenes/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/segmentacionF_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/segmentacionF_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/segmentacionF_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("SegmentacionF Cargado");
        return _data;
    }

    public CListaTest DataLetterKnowledgeN()
    {
        CListaTest _data = new CListaTest("inputs/letrasN", "audiosLetras/nombres/", "");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/LKnombre_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/LKnombre_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/LKn_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("LN N Cargado");
        return _data;
    }
    public CListaTest DataLetterKnowledgeS()
    {
        CListaTest _data = new CListaTest("inputs/letrasS", "audiosLetras/sonidos/", "");
        _audiosAuxEjemplos = new AudioClip[0];
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/LKs_I"); ;
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("LN S Cargado");
        return _data;
    }
    public CListaTestSTMorden DataSTMorden()
    {
        CListaTestSTMorden _data = new CListaTestSTMorden("inputs/STMo", "audiosSTMorden/", "imagenesSTMorden/");
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/STMOrden_E1");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/STMOrden_E2");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/STMo_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        _data.TiempoInterEnsayo = 1f;
        Debug.Log("STMorden Cargado");
        return _data;
    }
    public CListaTestSTMsimon DataSTMsimon()
    {
        CListaTestSTMsimon _data = new CListaTestSTMsimon("inputs/STMs");
        _audiosAuxEjemplos = new AudioClip[0];
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/STMs_I_chanchos");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        _data.TiempoInterEnsayo = 0.5f;
        _data.DingSound = Resources.Load<AudioClip>("audiosSTMsimon/ding");
        _data.PressSound = Resources.Load<AudioClip>("audiosSTMsimon/press");
        Debug.Log("STMsimon Cargado");
        return _data;
    }
    public CListaTestIQ DataIQ()
    {
        CListaTestIQ _data = new CListaTestIQ("inputs/WIPPSI", "audios/", "imagenesIQ/");
        _audiosAuxEjemplos = new AudioClip[1];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/IQ_E1");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/IQ_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 300f;
        _data.TiempoInterEnsayo = 0.5f;
        Debug.Log("IQ Cargado");
        return _data;
    }
    public CListaTestSegundaParte DataDLM()
    {
        CListaTestSegundaParte _data = new CListaTestSegundaParte("inputs/dichmus", "audiosDichmus/");
        _audiosAuxEjemplos = null;
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/DLM_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        _data.TiempoInterEnsayo = 1f;
        Debug.Log("DLM Cargado");
        return _data;
    }
    public CListaTestSegundaParte DataTapping()
    {
        CListaTestSegundaParte _data = new CListaTestSegundaParte("inputs/tapping", "audiosTapping/");
        _audiosAuxEjemplos = null;
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/tapping_I");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        _data.TiempoInterEnsayo = 1f;
        Debug.Log("Tapping Cargado");
        return _data;
    }
    public CListaTestSegundaParte DataDichListSyl()
    {
        CListaTestSegundaParte _data = new CListaTestSegundaParte("inputs/dichsyll_b", "audiosDichsyll/");
        _audiosAuxEjemplos = null;
        _audioAuxInstrucciones = null;
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        _data.TiempoInterEnsayo = 0.2f;
        Debug.Log("DichListSyl Cargado");
        //_dataDichListSyl.debugTestData();
        return _data;
    }
    public CListaTestVisEnt DataVisEnt()
    {
        CListaTestVisEnt _data = new CListaTestVisEnt("inputs/visEn2017", "imagenesVisEnt/");
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/VEn_I"); ;
        _audiosAuxEjemplos = new AudioClip[2];
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/VEn_E_si");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/VEn_E_no");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("Vis Ent Cargado");
        return _data;
    }
    public CListaTestVisSS DataVisSeg()
    {
        CListaTestVisSS _data = new CListaTestVisSS();
        _audiosAuxEjemplos = new AudioClip[2];
        _audioAuxInstrucciones = Resources.Load<AudioClip>("audiosInstrucciones/VSS_I");
        _audiosAuxEjemplos[0] = Resources.Load<AudioClip>("audiosEjemplos/VSS_E_una");
        _audiosAuxEjemplos[1] = Resources.Load<AudioClip>("audiosEjemplos/VSS_E_dos");
        _data.AudiosEjemplo = _audiosAuxEjemplos;
        _data.AudioInstrucciones = _audioAuxInstrucciones;
        _data.TiempoRespMaximo = 60f;
        Debug.Log("Vis Seg Cargado");
        return _data;
    }


    

}
