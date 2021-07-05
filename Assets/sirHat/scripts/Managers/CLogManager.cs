using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CLogManager : MonoBehaviour {

   
    // Singleton
    public static CLogManager Instance;
    //public string _sess, _id;
    private string _pathCCF, _pathSTM, _pathLista, _pathIQ, _pathDichListSyl, _pathDichListMus, _pathVisEnt, _pathVSS, _pathhTapping, _pathRANs;
    public int _taskPos;
	public bool _archivoCCF, _archivoIQ, _archivoSTM, _archivoTapping, _archivoDLS, _archivoDLM, _archivoVisEnt, _archivoVSS, _archivoRAN;

	public string _textoID, _textoClase, _textoEscuela, _textoTurno, _textoSesion,_textoFecha,
	_textoHora,_textoAplicador, _lista, _textoTablet; //_sesion
    public string _AppVersion;


    void Awake()
    {
        Instance = this;
    }


    // Metodos Lista -----------------------------------------------------------------------------------------------------------

    // Guardado posta, se llama desde los métodos
    public void GuardarLista(string _taskDone)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[2] { _textoID, _taskDone };
        rowData.Add(rowDataTemp);
        // Guardamos 
        _pathLista = createPathLista();
        GuardarData(rowData, _pathLista);
    }

    // Following method is used to retrive the relative path as device platform
    private string createPathLista()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "TestsDoneLog.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/TestsDoneLog.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/TestsDoneLog.csv";
#else
        return Application.dataPath +  "/TestsDoneLog.csv";
#endif
    }

    // RANs ---------------------------------------------------------------------------------------------------------------

    // Encabezado
    public void saveLogInRANs()
    {
        Debug.Log("saveLogInRANs");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row	
        string[] rowDataTemp = new string[6] {"ID", " task", "subTask", "taskPos", "RT", "session" };
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathRANs = createPathRANs();
        // Guardamos 
        GuardarData(rowData, _pathRANs);
        _archivoRAN = true;

    }


    // Guardado posta, se llama desde los métodos
    public void GuardarEstimuloRAN(string _RANtype, string _RANsubtype, float _tiempoCrono)
    {
        if (!_archivoRAN)
        {
            CLogManager.Instance.saveLogInRANs();
        }
        Debug.Log("Guardando estimulo RANs");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(new string[6] {_textoID, _RANtype,_RANsubtype,_taskPos.ToString(), _tiempoCrono.ToString(),_textoSesion });
        GuardarData(rowData, _pathRANs);
    }



    // Following method is used to retrive the relative path as device platform
    private string createPathRANs()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_RANs.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_RANs.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_RANs.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_RANs .csv";
#endif
    }









    // Tapping ---------------------------------------------------------------------------------------------------------------

    // Encabezado
    public void saveLogInTapping()
    {
        Debug.Log("saveLogInDichListMus");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row	
        string[] rowDataTemp = new string[16] { " ID", "task", "trial_Code", "freq", "audioL", "audioR", "cond", "trialType", "audio1", "numPrac", "taskPos", "trialPos", "resp", "RT", "contador", "session" };
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathhTapping = createPathTapping();
        // Guardamos 
        GuardarData(rowData, _pathhTapping);
        _archivoTapping = true;

    }


    // Guardado posta, se llama desde los métodos
    public int GuardarEstimuloTapping(CEstimuloSegundaParte _estimulo, int _numPrac, int _trialPos, string _respuesta, List<float> taps, int contador)
    {
        if (!_archivoTapping)
        {
            CLogManager.Instance.saveLogInTapping();
        }
        Debug.Log("Guardando estimulo Mus");
        List<string[]> rowData = new List<string[]>();
        string _numPracString;
        if (_numPrac == -1)
        {
            _numPracString = "NA";
        }
        else
        {
            _numPracString = _numPrac.ToString();
        }


        for (int i = 0; i < taps.Count; i++)
        {
            rowData.Add(new string[16] {_textoID,  _estimulo.Task, _estimulo.TrialCode,_estimulo.Frec,_estimulo.AudioL, _estimulo.AudioR, _estimulo.Cond,
                _estimulo.TrialType, _estimulo.Audio1, _numPracString, _taskPos.ToString(), _trialPos.ToString(), _respuesta,  taps[i].ToString(),contador++.ToString(), _textoSesion });
        }
        GuardarData(rowData, _pathhTapping);
        return contador;
    }



    // Following method is used to retrive the relative path as device platform
    private string createPathTapping()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_Tappings.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_Tappings.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_Tappings.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_Tappings .csv";
#endif
    }




    // Metodos Visual Segmentation -----------------------------------------------------------------------------------------------------------
    //task 	trialCode	offset	dur1	onsetMask	catch	inPhase	targ	corrResp	trialType	numPrac	taskPos	trialPos	targetPos	resp	acierto	RT
    // Encabezado
    public void saveLogInVisSeg()
    {
        Debug.Log("saveLogInVisSeg");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row		       									
        string[] rowDataTemp = new string[11] { "ID", "task", "trialCode", "trialType", "trialPos", "numPrac", "taskPos", "SOA", "resp", "RT", "sesion" };
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathVSS = createPathVisSeg();
        // Guardamos 
        GuardarData(rowData, _pathVSS);
        _archivoVSS = true;
    }


    // Guardar estímulo para práctica y test
    public void GuardarEstimuloVisSeg(int _tipoEjPrTest, int _trialPos, int _numPrac, int _SOA, string _respuesta, string _tResp)
    {
        if (!_archivoVSS)
        {
            CLogManager.Instance.saveLogInVisSeg();
        }
        List<string[]> rowData = new List<string[]>();
        // rowDataTemp = new string[11] { "ID", "task", "trialCode", "trialType", "trialPos", "numPrac", "taskPos", "SOA", "resp", "RT", "sesion" };
        string[] rowDataTemp = new string[11] { _textoID, "visSS", "NA", _tipoEjPrTest.ToString(), _trialPos.ToString(), _numPrac.ToString(), _taskPos.ToString(), _SOA.ToString(),_respuesta, _tResp, _textoSesion};
        rowData.Add(rowDataTemp);
        GuardarData(rowData, _pathVSS);
    }

    // Following method is used to retrive the relative path as device platform
    private string createPathVisSeg()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_VisSS.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_VisSS.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_VisSS.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_VisSS .csv";
#endif
    }




    // Metodos Visual Entrainment -----------------------------------------------------------------------------------------------------------
    //task 	trialCode	offset	dur1	onsetMask	catch	inPhase	targ	corrResp	trialType	numPrac	taskPos	trialPos	targetPos	resp	acierto	RT
    // Encabezado
    public void saveLogInVisEnt()
    {
        Debug.Log("saveLogInVisEnt");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row		
        //task 	trialCode	target	offset	dur1	onsetMask	catch	inPhase	corrResp	trialType	numPrac	taskPos	trialPos	targetPos	resp	acierto	RT	session
        string[] rowDataTemp = new string[19] { "ID", "task", "trialCode", "target", "offset", "dur1", "onsetMask", "catch", "inPhase", "corrResp", "trialType", "numPrac", "taskPos", "trialPos", "targetPos", "resp","acierto","RT","sesion" };
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathVisEnt = createPathVisEnt();
        // Guardamos 
        GuardarData(rowData, _pathVisEnt);
        _archivoVisEnt = true;
    }


    // Guardar estímulo para práctica y test
    public void GuardarEstimuloVisEnt(CEstimuloVisEnt _estimulo, int _numPrac, int _trialPos, string _tResp, string _respuesta, string _acierto)
    {
        if (!_archivoVisEnt)
        {
            CLogManager.Instance.saveLogInVisEnt();
        }
        CEstimuloVisEnt _estimuloActual;
        _estimuloActual = _estimulo;
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[19] { _textoID, _estimuloActual.Task, _estimuloActual.TrialCode, _estimuloActual.Target, _estimuloActual.Offset, _estimuloActual.Dur1, _estimuloActual.OnsetMask, _estimuloActual.Catch,
            _estimuloActual.InPhase, _estimuloActual.CorrResp,_estimuloActual.TrialType, _numPrac.ToString(),_taskPos.ToString(),  _trialPos.ToString(),
            _estimuloActual.TargetPosition,_respuesta, _acierto, _tResp, _textoSesion};
        rowData.Add(rowDataTemp);
        GuardarData(rowData, _pathVisEnt);
    }

    // Following method is used to retrive the relative path as device platform
    private string createPathVisEnt()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_VisE.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_VisE.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_VisE.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_VisE .csv";
#endif
    }




    // Metodos Dich List Mus -----------------------------------------------------------------------------------------------------------

    // Encabezado
    public void saveLogInDichListMus()
    {
        Debug.Log("saveLogInDichListMus");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row	
        string[] rowDataTemp = new string[16] { " ID", "task", "trial_Code", "freq", "audioL", "audioR", "cond", "trialType", "audio1", "numPrac", "taskPos", "trialPos", "resp", "RT", "contador", "session" };
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathDichListMus = createPathDichListMus();
        // Guardamos 
        GuardarData(rowData, _pathDichListMus);
		_archivoDLM = true;

    }


    // Guardado posta, se llama desde los métodos
    public int GuardarEstimuloDichListMus(CEstimuloSegundaParte _estimulo,int _numPrac, int _trialPos, string _respuesta, List<float> taps, int contador)
    { 
		if (!_archivoDLM) {
			CLogManager.Instance.saveLogInDichListMus();
		}
		Debug.Log ("Guardando estimulo Mus");
        List<string[]> rowData = new List<string[]>();
        string _numPracString;
        if(_numPrac == -1)
        {
            _numPracString = "NA";
        }
        else
        {
            _numPracString = _numPrac.ToString();
        }


        for (int i = 0; i < taps.Count; i++) {
            rowData.Add(new string[16] {_textoID,  _estimulo.Task, _estimulo.TrialCode,_estimulo.Frec,_estimulo.AudioL, _estimulo.AudioR, _estimulo.Cond,
                _estimulo.TrialType, _estimulo.Audio1, _numPracString, _taskPos.ToString(), _trialPos.ToString(), _respuesta,  taps[i].ToString(),contador++.ToString(), _textoSesion });
        }
        GuardarData(rowData, _pathDichListMus);
        return contador;
    }



    // Following method is used to retrive the relative path as device platform
    private string createPathDichListMus()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_DichListMus.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_DichListMus.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_DichListMus.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_DichListMus .csv";
#endif
    }




    // Metodos Dich List Syl -----------------------------------------------------------------------------------------------------------
    // Encabezado
    public void saveLogInDichListSyl()
    {
        Debug.Log("saveLogInDichListSyl");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row	
        string[] rowDataTemp = new string[16] {" ID", "task", "trial_Code", "freq", "audioL", "audioR", "cond", "trialType", "audio1", "numPrac","taskPos", "trialPos","resp", "RT", "contador" , "session"};
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathDichListSyl = createPathDichListSyl();
        // Guardamos 
        GuardarData(rowData, _pathDichListSyl);
		_archivoDLS = true;
    }


    // Guardar estímulo para práctica y test
    public void GuardarEstimuloDichListSyl(CEstimuloSegundaParte _estimulo, int _numPrac, int _trialPos, string _tResp, string _respuesta, int _contador)
    {
        // Guardamos log in si no existe el archivo
		if (!_archivoDLS) {
			CLogManager.Instance.saveLogInDichListSyl();
		}
        //Metemos toda la data en estructura estímulo
        string _numPracString;
        if (_numPrac == -1)
        {
            _numPracString = "NA";
        }
        else
        {
            _numPracString = _numPrac.ToString();
        }

        CEstimuloSegundaParte _estimuloActual;
        _estimuloActual = _estimulo;
        _estimuloActual.NumPractica = _numPracString;
        _estimuloActual.TaskPosition = _taskPos.ToString();
        _estimuloActual.TrialPosition = _trialPos.ToString();       
        _estimuloActual.Resp = _respuesta;
        _estimuloActual.RT = _tResp;
        _estimuloActual.Contador = _contador.ToString();
        // Guardamos
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[16] { _textoID, _estimuloActual.Task, _estimuloActual.TrialCode, _estimuloActual.Frec, _estimuloActual.AudioL, _estimuloActual.AudioR, _estimuloActual.Cond,
            _estimuloActual.TrialType, _estimuloActual.Audio1, _estimuloActual.NumPractica, _estimuloActual.TaskPosition, _estimuloActual.TrialPosition,  _estimuloActual.Resp, _estimuloActual.RT,
             _estimuloActual.Contador, _textoSesion };
        rowData.Add(rowDataTemp);
        GuardarData(rowData, _pathDichListSyl);
    }


    // Following method is used to retrive the relative path as device platform
    private string createPathDichListSyl()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_DichListSyl.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_DichListSyl.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_DichListSyl.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_DichListSyl .csv";
#endif
    }






    // Metodos IQ -----------------------------------------------------------------------------------------------------------
    // Encabezado
    public void saveLogInIQ()
    {
        Debug.Log("saveLogInIQ");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row		
        string[] rowDataTemp = new string[27] { "ID", "task", "trialCode", "target", "visual1", "visual2", "visual3", "visual4","visual5","visual6", "corrResp", "corrResp2", "numSyll", "numPhon", "struct",
            "syllPos", "trialType","audio1", "audio2", "numPrac", "taskPos", "trialPos", "targetPos", "resp", "acierto", "RT","session"};
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathIQ = createPathIQ();
        // Guardamos 
        GuardarData(rowData, _pathIQ);
		_archivoIQ = true;
    }

    // Guardar estímulo para los casos de ejemplo que tienen varios NA
    public void GuardarEjemploIQ(CEstimuloIQ _estimulo, int _numPrac, int _trialPos, int _targetPos)
    {
		if (!_archivoIQ) {
			CLogManager.Instance.saveLogInIQ();
		}
        string _resplog;
        if (_targetPos == -1)
            _resplog = "NA";
        else
            _resplog = _targetPos.ToString();
        CEstimuloIQ _estimuloActual;
        _estimuloActual = _estimulo;
        _estimuloActual.NumPractica = _numPrac.ToString();
        _estimuloActual.TaskPosition = _taskPos.ToString();
        _estimuloActual.TrialPosition = _trialPos.ToString();
        _estimuloActual.TargetPosition = _resplog;
        _estimuloActual.Resp = "NA";
        _estimuloActual.Acierto = "NA";
        _estimuloActual.RT = "NA";
        CLogManager.Instance.GuardarEstimuloPrivadoIQ(_estimuloActual);
    }

    // Guardar estímulo para práctica y test
    public void GuardarEstimuloIQ(CEstimuloIQ _estimulo, int _numPrac, int _trialPos, int _targetPos, string _tResp, string _respuesta, bool _acierto)
    {
		if (!_archivoIQ) {
			CLogManager.Instance.saveLogInIQ();
		}
        string _resplog;
        if (_targetPos == -1)
            _resplog = "NA";
        else
            _resplog = _targetPos.ToString();
        CEstimuloIQ _estimuloActual;
        _estimuloActual = _estimulo;
        _estimuloActual.NumPractica = _numPrac.ToString();
        _estimuloActual.TaskPosition = _taskPos.ToString();
        _estimuloActual.TrialPosition = _trialPos.ToString();
        _estimuloActual.TargetPosition = _resplog;
        _estimuloActual.Resp = _respuesta;
        _estimuloActual.Acierto = _acierto.ToString();
        _estimuloActual.RT = _tResp;
        CLogManager.Instance.GuardarEstimuloPrivadoIQ(_estimuloActual);
    }

    // Guardado posta, se llama desde los métodos
    private void GuardarEstimuloPrivadoIQ(CEstimuloIQ _estimulo)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[27] { _textoID, _estimulo.Task, _estimulo.TrialCode, _estimulo.Target, _estimulo.Visual1, _estimulo.Visual2, _estimulo.Visual3, _estimulo.Visual4, _estimulo.Visual5, _estimulo.Visual6,
            _estimulo.CorrResp,_estimulo.CorrRespAux, _estimulo.NumSyll, _estimulo.NumPhon, _estimulo.Struct, _estimulo.SyllPos, _estimulo.TrialType, _estimulo.Audio1, _estimulo.Audio2, _estimulo.NumPractica,
            _estimulo.TaskPosition, _estimulo.TrialPosition, _estimulo.TargetPosition, _estimulo.Resp, _estimulo.Acierto, _estimulo.RT, _textoSesion};
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathIQ);

    }

    // Following method is used to retrive the relative path as device platform
    private string createPathIQ()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_IQ.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_IQ.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_IQ.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_IQ.csv";
#endif
    }


    // Metodos CCF ---------------------------------------------------------------------------------------------------------

    // Encabezado
    public void saveLogIn()
    {
        Debug.Log("saveLogIn");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row
        string[] rowDataTemp = new string[24] { "ID", "task", "trialCode", "target", "visual1", "visual2", "visual3", "visual4", "corrResp", "numSyll", "numPhon", "struct",
            "syllPos", "trialType","audio1", "audio2", "numPrac", "taskPos", "trialPos", "targetPos", "resp", "acierto", "RT","session"};
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathCCF = createPath();
        // Guardamos 
        GuardarData(rowData, _pathCCF);
		_archivoCCF = true;
    }

    // Guardar estímulo para los casos de ejemplo que tienen varios NA
    public void GuardarEjemplo(CEstimulo _estimulo, int _numPrac, int _trialPos, int _targetPos)
    {
		if (!_archivoCCF) {
			CLogManager.Instance.saveLogIn();
		}
        string _resplog;
        if (_targetPos == -1)
            _resplog = "NA";
        else
            _resplog = _targetPos.ToString();
        CEstimulo _estimuloActual;
        _estimuloActual = _estimulo;
        _estimuloActual.NumPractica = _numPrac.ToString();
        _estimuloActual.TaskPosition = _taskPos.ToString();
        _estimuloActual.TrialPosition = _trialPos.ToString();
        _estimuloActual.TargetPosition = _resplog;
        _estimuloActual.Resp = "NA";
        _estimuloActual.Acierto = "NA";
        _estimuloActual.RT = "NA";
        CLogManager.Instance.GuardarEstimuloPrivado(_estimuloActual);
    }

    // Guardar estímulo para práctica y test
    public void GuardarEstimulo(CEstimulo _estimulo, int _numPrac, int _trialPos, int _targetPos, string _tResp, string _respuesta, bool _acierto)
    {
		if (!_archivoCCF) {
			CLogManager.Instance.saveLogIn();
		}
        string _resplog;
        if (_targetPos == -1)
            _resplog = "NA";
        else
            _resplog = _targetPos.ToString();
        CEstimulo _estimuloActual;
        _estimuloActual = _estimulo;
        _estimuloActual.NumPractica = _numPrac.ToString();
        _estimuloActual.TaskPosition = _taskPos.ToString();
        _estimuloActual.TrialPosition = _trialPos.ToString();
        _estimuloActual.TargetPosition = _resplog;
        _estimuloActual.Resp = _respuesta;
        _estimuloActual.Acierto = _acierto.ToString();
        _estimuloActual.RT = _tResp;
        CLogManager.Instance.GuardarEstimuloPrivado(_estimuloActual);
    }

    // Guardado posta, se llama desde los métodos
    private void GuardarEstimuloPrivado(CEstimulo _estimulo)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[24] { _textoID, _estimulo.Task, _estimulo.TrialCode, _estimulo.Target, _estimulo.Visual1, _estimulo.Visual2, _estimulo.Visual3, _estimulo.Visual4,
            _estimulo.CorrResp, _estimulo.NumSyll, _estimulo.NumPhon, _estimulo.Struct, _estimulo.SyllPos, _estimulo.TrialType, _estimulo.Audio1, _estimulo.Audio2, _estimulo.NumPractica,
            _estimulo.TaskPosition, _estimulo.TrialPosition, _estimulo.TargetPosition, _estimulo.Resp, _estimulo.Acierto, _estimulo.RT, _textoSesion};
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathCCF);

    }

    // Following method is used to retrive the relative path as device platform
    private string createPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_CCF.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_CCF.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_CCF.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_CCF.csv";
#endif
    }


    // Métodos STM ----------------------------------------------------------------------------------------------------------------------

    // Encabezado
    public void saveLogInSTM()
    {
        Debug.Log("saveLogIn STM");
        List<string[]> rowData = new List<string[]>();
        rowData.Add(HeaderLogin());
        rowData.Add(DataLogin());
        // Creating second titles row
        string[] rowDataTemp = new string[27] { "ID",  "task", "word1", "word2", "word3", "word4", "word5", "word6", "word7", "word8", "word9", "numEl", "trialType", "taskPos", "trialPos",
            "resp1", "resp2", "resp3", "resp4", "resp5", "resp6", "resp7", "resp8", "resp9", "acierto", "RT" ,"session"};
        rowData.Add(rowDataTemp);
        // Actualizamos path y guardamos encabezado
        _pathSTM = createPathSTM();
        GuardarData(rowData,_pathSTM);
		_archivoSTM = true;
    }


    // Guardar estímulo para los casos de ejemplo que tienen varios NA
    public void GuardarEjemploSTMorden(CEstimuloSTMorden _estimuloActual, int _numPrac, int _trialPos)
    {
		if (!_archivoSTM) {
			CLogManager.Instance.saveLogInSTM();
		}
        string[] rowDataTemp = new string[27] { _textoID,  _estimuloActual.Task, _estimuloActual.Visual1, _estimuloActual.Visual2, _estimuloActual.Visual3, _estimuloActual.Visual4, _estimuloActual.Visual5, _estimuloActual.Visual6 , "", "", "",
            _estimuloActual.NumEl, _estimuloActual.TrialType,_taskPos.ToString(), _trialPos.ToString(), "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA" , _textoSesion};
        List<string[]> rowData = new List<string[]>();
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathSTM);
    }

    // Guardar estímulo para práctica y test
    public void GuardarEstimuloSTM(CEstimuloSTMorden _estimuloActual, int _numPrac, int _trialPos, string _tResp, string[] _palabrasRespuesta, bool _acierto)
    {
		if (!_archivoSTM) {
			CLogManager.Instance.saveLogInSTM();
		}
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _estimuloActual.NumEstimulos)
            {
                _respLog[_i] = _palabrasRespuesta[_i];
            }
            else
            {
                _respLog[_i] = "";
            }
        }
        string[] rowDataTemp = new string[27] { _textoID,  _estimuloActual.Task, _estimuloActual.Visual1, _estimuloActual.Visual2, _estimuloActual.Visual3, _estimuloActual.Visual4, _estimuloActual.Visual5, _estimuloActual.Visual6 , "", "", "",
            _estimuloActual.NumEl, _estimuloActual.TrialType,_taskPos.ToString(), _trialPos.ToString(), _respLog[0], _respLog[1], _respLog[2], _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8],
            _acierto.ToString(), _tResp, _textoSesion};
        List<string[]> rowData = new List<string[]>();
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathSTM);

    }

    // Guardar estímulo para los casos de ejemplo que tienen varios NA
    public void GuardarEjemploSTMsimon(CEstimuloSTMsimon _estimuloActual, int _taskPos, int _trialPos)
    {
		if (!_archivoSTM) {
			CLogManager.Instance.saveLogInSTM();
		}
        string[] rowDataTemp = new string[27] { _textoID,  _estimuloActual.Task, _estimuloActual.Word1, _estimuloActual.Word2, _estimuloActual.Word3, _estimuloActual.Word4, _estimuloActual.Word5, _estimuloActual.Word6 ,
            _estimuloActual.Word7 , _estimuloActual.Word8, _estimuloActual.Word9, _estimuloActual.NumEl, _estimuloActual.TrialType,_taskPos.ToString(), _trialPos.ToString(), "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA" , _textoSesion};
        List<string[]> rowData = new List<string[]>();
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathSTM);
    }

    // Guardar estímulo para práctica y test
    public void GuardarEstimuloSTMsimon(CEstimuloSTMsimon _estimuloActual, int _trialPos, string _tResp, int[] _flashRespuesta,int _nroRespuesta, bool _acierto)
    {
		if (!_archivoSTM) {
			CLogManager.Instance.saveLogInSTM();
		}
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _nroRespuesta)
                _respLog[_i] = (_flashRespuesta[_i]+1).ToString();
            else
                _respLog[_i] = "";
        }       
        string[] rowDataTemp = new string[27] { _textoID,   _estimuloActual.Task, _estimuloActual.Word1, _estimuloActual.Word2, _estimuloActual.Word3, _estimuloActual.Word4, _estimuloActual.Word5, _estimuloActual.Word6 ,
             _estimuloActual.Word7 , _estimuloActual.Word8, _estimuloActual.Word9, _estimuloActual.NumEl, _estimuloActual.TrialType, _taskPos.ToString(), _trialPos.ToString(),
            _respLog[0], _respLog[1], _respLog[2], _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto.ToString(), _tResp, _textoSesion};
        List<string[]> rowData = new List<string[]>();
        rowData.Add(rowDataTemp);
        // Guardamos 
        GuardarData(rowData, _pathSTM);
    }

    public void IncrementarTaskPos()
    {
        _taskPos += 1;
    }

    // Following method is used to retrive the relative path as device platform
    private string createPathSTM()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_STM.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath +"/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _textoID + "_" + _textoSesion + "_STM.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _id + "_" + _sess + "_STM.csv";
#else
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_STM.csv";
#endif
    }



    // Métodos Privado genéricos------------------------------------------------------------------------------------
    // Guardar data
    private void GuardarData(List<string[]> _data, string _path)
    {
        Debug.Log("GuardarData");
        string[][] output = new string[_data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = _data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";


        StreamWriter file2 = new StreamWriter(_path, true);
        string aux;
        for (int index = 0; index < length; index++)
        {
            aux = string.Join(delimiter, output[index]);
            file2.WriteLine(aux);
        }
        file2.Close();
    }

    public string[] HeaderLogin()
    {
        string[] rowDataTemp = new string[10] { "ID", "Clase", "Escuela", "Turno", "Sesion", "Fecha", "Hora", "Aplicador", "Tablet", "Versión App" };
        return rowDataTemp;
    }
    public string[] DataLogin()
    {
        string[]  rowDataTemp = new string[10] { _textoID, _textoClase, _textoEscuela, _textoTurno, _textoSesion, _textoFecha, _textoHora, _textoAplicador, _textoTablet, _AppVersion };
        return rowDataTemp;
    
    }



}
