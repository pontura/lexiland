using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWriteLinebyLine : MonoBehaviour {
    
    private List<string[]> rowData = new List<string[]>();
    private string filePath;
    //Variables to save
    //Log In
    private string _nombre;
	private string _apellido;
	public string _id;
	private string _clase;
	private string _maestro;
	private string _escuela;
	private string _turno;
	private string _modelo;
	private string _sesion;
	private string _fecha;
	private string _hora;
	private string _aplicador;
    
    //Tests Data
    private List<string> _task;
    private List<string> _word1;
    private List<string> _word2;
    private List<string> _corrResp;
    private List<string> _numPhon;
    private List<string> _numSyll;
    private List<string> _struct;
    private List<string> _trialType;
    private List<string> _audio1;
    private List<string> _audio2;
    private List<string> _taskPos;
    private List<string> _numPrac;
    private List<string> _trialPos;
    private List<string> _resp;
    private List<string> _acierto;
    private List<string> _RT;

    //Use this for initialization
    void Start () {
        _task = new List<string>();
        _word1 = new List<string>();
        _word2 = new List<string>();
        _corrResp = new List<string>();
        _numPhon = new List<string>();
        _numSyll = new List<string>();
        _struct = new List<string>();
        _trialType = new List<string>();
        _audio1 = new List<string>();
        _audio2 = new List<string>();
        _taskPos = new List<string>();
        _numPrac = new List<string>();
        _trialPos = new List<string>();
        _resp = new List<string>();
        _acierto = new List<string>();
        _RT = new List<string>();
    }
    
    public void Save(){

        // Creating first titles row
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = "Nombre";
        rowDataTemp[1] = "Apellido";
        rowDataTemp[2] = "ID";
        rowDataTemp[3] = "Clase";
        rowDataTemp[4] = "Maestro/a";
        rowDataTemp[5] = "Escuela";
        rowDataTemp[6] = "Turno";
        rowDataTemp[7] = "Modelo Tablet";
        rowDataTemp[8] = "Sesion";
        rowDataTemp[9] = "Fecha";
        rowDataTemp[10] = "Hora";
        rowDataTemp[11] = "Aplicador";        
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        //  for(int i = 0; i < 10; i++){
        //      rowDataTemp = new string[3];
        //      rowDataTemp[0] = "Sushanta"+i; // name
        //      rowDataTemp[1] = ""+i; // ID
        //      rowDataTemp[2] = "$"+UnityEngine.Random.Range(5000,10000); // Income
        //      rowData.Add(rowDataTemp);
        //  }
        
        // Write LogIn Data
        rowDataTemp = new string[12];
        rowDataTemp[0] = _nombre;
        rowDataTemp[1] = _apellido;
        rowDataTemp[2] = _id;
        rowDataTemp[3] = _clase;
        rowDataTemp[4] = _maestro;
        rowDataTemp[5] = _escuela;
        rowDataTemp[6] = _turno;
        rowDataTemp[7] = _modelo;
        rowDataTemp[8] = _sesion;
        rowDataTemp[9] = _fecha;
        rowDataTemp[10] = _hora;
        rowDataTemp[11] = _aplicador;   
        rowData.Add(rowDataTemp);
        
        // Creating second titles row
        rowDataTemp = new string[16];
        rowDataTemp[0] = "task";
        rowDataTemp[1] = "word1";
        rowDataTemp[2] = "word2";
        rowDataTemp[3] = "corrResp";
        rowDataTemp[4] = "numPhon";
        rowDataTemp[5] = "numSyll";
        rowDataTemp[6] = "struct";
        rowDataTemp[7] = "trialType";
        rowDataTemp[8] = "audio1";
        rowDataTemp[9] = "audio2";
        rowDataTemp[10] = "taskPos";
        rowDataTemp[11] = "numPrac";        
        rowDataTemp[12] = "trialPos";  
        rowDataTemp[13] = "resp";  
        rowDataTemp[14] = "acierto";  
        rowDataTemp[15] = "RT";  
        rowData.Add(rowDataTemp);
        
        // Write tests data rows
        for(int i = 0; i < _task.Count; i++)
        {
            rowDataTemp = new string[16];
            rowDataTemp[0] = _task[i];
            rowDataTemp[1] = _word1[i];
            rowDataTemp[2] = _word2[i];
            rowDataTemp[3] = _corrResp[i];
            rowDataTemp[4] = _numPhon[i];
            rowDataTemp[5] = _numSyll[i];
            rowDataTemp[6] = _struct[i];
            rowDataTemp[7] = _trialType[i];
            rowDataTemp[8] = _audio1[i];
            rowDataTemp[9] = _audio2[i];
            rowDataTemp[10] = _taskPos[i];
            rowDataTemp[11] = _numPrac[i];      
            rowDataTemp[12] = _trialPos[i]; 
            rowDataTemp[13] = _resp[i];
            rowDataTemp[14] = _acierto[i];  
            rowDataTemp[15] = _RT[i];
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        filePath = getPath();
        StreamWriter file2 = new StreamWriter(filePath, true);
        string aux;
        for (int index = 0; index < length; index++)
        {
            aux = string.Join(delimiter, output[index]);
            file2.WriteLine(aux);
        }
        file2.Close();
        ResetLogInData();

    }



    public void saveLogIn()
    {
        rowData = new List<string[]>();

        // Creating first titles row
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = "Nombre";
        rowDataTemp[1] = "Apellido";
        rowDataTemp[2] = "ID";
        rowDataTemp[3] = "Clase";
        rowDataTemp[4] = "Maestro/a";
        rowDataTemp[5] = "Escuela";
        rowDataTemp[6] = "Turno";
        rowDataTemp[7] = "Modelo Tablet";
        rowDataTemp[8] = "Sesion";
        rowDataTemp[9] = "Fecha";
        rowDataTemp[10] = "Hora";
        rowDataTemp[11] = "Aplicador";
        rowData.Add(rowDataTemp);

        // Write LogIn Data
        rowDataTemp = new string[12];
        rowDataTemp[0] = _nombre;
        rowDataTemp[1] = _apellido;
        rowDataTemp[2] = _id;
        rowDataTemp[3] = _clase;
        rowDataTemp[4] = _maestro;
        rowDataTemp[5] = _escuela;
        rowDataTemp[6] = _turno;
        rowDataTemp[7] = _modelo;
        rowDataTemp[8] = _sesion;
        rowDataTemp[9] = _fecha;
        rowDataTemp[10] = _hora;
        rowDataTemp[11] = _aplicador;
        rowData.Add(rowDataTemp);

        // Creating second titles row
        rowDataTemp = new string[17];
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "task";
        rowDataTemp[2] = "word1";
        rowDataTemp[3] = "word2";
        rowDataTemp[4] = "corrResp";
        rowDataTemp[5] = "numPhon";
        rowDataTemp[6] = "numSyll";
        rowDataTemp[7] = "struct";
        rowDataTemp[8] = "trialType";
        rowDataTemp[9] = "audio1";
        rowDataTemp[10] = "audio2";
        rowDataTemp[11] = "taskPos";
        rowDataTemp[12] = "numPrac";
        rowDataTemp[13] = "trialPos";
        rowDataTemp[14] = "resp";
        rowDataTemp[15] = "acierto";
        rowDataTemp[16] = "RT";
        rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        //StringBuilder sb = new StringBuilder();
        //for (int index = 0; index < length; index++)
        //    sb.AppendLine(string.Join(delimiter, output[index]));
        //filePath = getPath();
        //StreamWriter outStream = System.IO.File.CreateText(filePath);
        //outStream.WriteLine(sb);
        //outStream.Close();

        filePath = getPath();
        StreamWriter file2 = new StreamWriter(filePath, true);
        string aux;
        for (int index = 0; index < length; index++)
        {
            aux = string.Join(delimiter, output[index]);
            file2.WriteLine(aux);
        }
        file2.Close();
        ResetLogInData();
    }

    public void saveLine()
    {
        rowData = new List<string[]>();
        string[] rowDataTemp;
        for (int i = 0; i < _task.Count; i++)
        {
            rowDataTemp = new string[17];
            rowDataTemp[0] = _id;
            rowDataTemp[1] = _task[i];
            rowDataTemp[2] = _word1[i];
            rowDataTemp[3] = _word2[i];
            rowDataTemp[4] = _corrResp[i];
            rowDataTemp[5] = _numPhon[i];
            rowDataTemp[6] = _numSyll[i];
            rowDataTemp[7] = _struct[i];
            rowDataTemp[8] = _trialType[i];
            rowDataTemp[9] = _audio1[i];
            rowDataTemp[10] = _audio2[i];
            rowDataTemp[11] = _taskPos[i];
            rowDataTemp[12] = _numPrac[i];
            rowDataTemp[13] = _trialPos[i];
            rowDataTemp[14] = _resp[i];
            rowDataTemp[15] = _acierto[i];
            rowDataTemp[16] = _RT[i];
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";
        string aux;
        aux = string.Join(delimiter, output[0]);

        StreamWriter file2 = new StreamWriter(filePath, true);
        file2.WriteLine(aux);
        file2.Close();


        //StreamWriter outStream = System.IO.File.CreateText(filePath);
        //outStream.WriteLine(aux);
        //outStream.Close();

        ClearTestRows();

    }



    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath + "/CSV/"+ _id + "_" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _sesion + "_CCF.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath +"/" + _id + "_" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _sesion + "_CCF.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+ _id + "_" + DateTime.Now.ToString().Replace("/","").Replace(":","").Replace(" ","_") + "_" + _sesion + "_CCF";
#else
        return Application.dataPath +"/"+ _id + "_" + DateTime.Now.ToString().Replace("/","").Replace(":","").Replace(" ","_") + "_" + _sesion + "_CCF";
#endif
    }

    // Set log-in Data
    public void SetLogInData(string pNombre, string pApellido, string pID, string pClase, string pMaestro, string pEscuela, string pTurno, string pModelo, 
                             string pSesion, string pFecha, string pHora, string pAplicador)
    {
        _nombre = pNombre;
        _apellido = pApellido;
        _id = pID;
        _clase = pClase;
        _maestro = pMaestro;
        _escuela = pEscuela;
        _turno = pTurno;
        _modelo = pModelo;
        _sesion = pSesion;
        _fecha = pFecha;
        _hora = pHora;
        _aplicador = pAplicador;
    }
    
    // Reset log-in data
    public void ResetLogInData()
	{
		_nombre = "";
		_apellido = "";
		//_id = "";
		_clase = "";
		_maestro = "";
		_escuela = "";
		_turno = "";
		_modelo = "";
		_sesion = "";
		_fecha = "";
		_hora = "";
		_aplicador = "";	
	}
    
    // Set Test Row
    public void AddTestRow(string pTask, string pWord1, string pWord2, string pCorrResp, string pNumPhone, string pNumSyll, string pStruct, string pTrialType, 
                            string pAudio1, string pAudio2, string pTaskPos, string pNumPrac, string pTrialPos, string pResp, string pAcierto, string pRT)
    {       
        _task.Add(pTask);
        _word1.Add(pWord1);
        _word2.Add(pWord2);
        _corrResp.Add(pCorrResp);
        _numPhon.Add(pNumPhone);
        _numSyll.Add(pNumSyll);
        _struct.Add(pStruct);
        _trialType.Add(pTrialType);
        _audio1.Add(pAudio1);
        _audio2.Add(pAudio2);
        _taskPos.Add(pTaskPos);
        _numPrac.Add(pNumPrac);
        _trialPos.Add(pTrialPos);
        _resp.Add(pResp);
        _acierto.Add(pAcierto);
        _RT.Add(pRT);
    }
    
    // Clear Tests Data
    public void ClearTestRows()
    {
        for(int i = _task.Count-1; i >= 0; i--)
        {
            _task.RemoveAt(i);
            _word1.RemoveAt(i);
            _word2.RemoveAt(i);
            _corrResp.RemoveAt(i);
            _numPhon.RemoveAt(i);
            _numSyll.RemoveAt(i);
            _struct.RemoveAt(i);
            _trialType.RemoveAt(i);
            _audio1.RemoveAt(i);
            _audio2.RemoveAt(i);
            _taskPos.RemoveAt(i);
            _numPrac.RemoveAt(i);
            _trialPos.RemoveAt(i);
            _resp.RemoveAt(i);
            _acierto.RemoveAt(i);
            _RT.RemoveAt(i);
        }
    }
}

