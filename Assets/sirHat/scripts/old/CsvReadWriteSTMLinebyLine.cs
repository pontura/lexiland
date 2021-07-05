using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWriteSTMLinebyLine : MonoBehaviour {
    
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
    private List<string> _word3;
    private List<string> _word4;
    private List<string> _word5;
    private List<string> _word6;
    private List<string> _word7;
    private List<string> _word8;
    private List<string> _word9;
    private List<string> _numEl;
    private List<string> _trialType;
    private List<string> _taskPos;
    private List<string> _trialPos;
    private List<string> _resp1;
    private List<string> _resp2;
    private List<string> _resp3;
    private List<string> _resp4;
    private List<string> _resp5;
    private List<string> _resp6;
    private List<string> _resp7;
    private List<string> _resp8;
    private List<string> _resp9;
    private List<string> _acierto;
    private List<string> _RT;


    //Use this for initialization
    void Start () {
        _task = new List<string>();
        _word1 = new List<string>();
        _word2 = new List<string>();
        _word3 = new List<string>();
        _word4 = new List<string>();
        _word5 = new List<string>();
        _word6 = new List<string>();
        _word7 = new List<string>();
        _word8 = new List<string>();
        _word9 = new List<string>();
        _numEl = new List<string>();
        _trialType = new List<string>();
        _taskPos = new List<string>();
        _trialPos = new List<string>();
        _resp1 = new List<string>();
        _resp2 = new List<string>();
        _resp3 = new List<string>();
        _resp4 = new List<string>();
        _resp5 = new List<string>();
        _resp6 = new List<string>();
        _resp7 = new List<string>();
        _resp8 = new List<string>();
        _resp9 = new List<string>();
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
        rowDataTemp = new string[26];
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "task";
        rowDataTemp[2] = "word1";
        rowDataTemp[3] = "word2";
        rowDataTemp[4] = "word3";
        rowDataTemp[5] = "word4";
        rowDataTemp[6] = "word5";
        rowDataTemp[7] = "word6";
        rowDataTemp[8] = "word7";
        rowDataTemp[9] = "word8";
        rowDataTemp[10] = "word9";
        rowDataTemp[11] = "numEl";
        rowDataTemp[12] = "trialType";
        rowDataTemp[13] = "taskPos";
        rowDataTemp[14] = "trialPos";
        rowDataTemp[15] = "resp1";
        rowDataTemp[16] = "resp2";
        rowDataTemp[17] = "resp3";
        rowDataTemp[18] = "resp4";
        rowDataTemp[19] = "resp5";
        rowDataTemp[20] = "resp6";
        rowDataTemp[21] = "resp7";
        rowDataTemp[22] = "resp8";
        rowDataTemp[23] = "resp9";
        rowDataTemp[24] = "acierto";  
        rowDataTemp[25] = "RT";  
        rowData.Add(rowDataTemp);
        
        // Write tests data rows
        for(int i = 0; i < _task.Count; i++)
        {
            rowDataTemp = new string[26];
            rowDataTemp[0] =_id;
            rowDataTemp[1] = _task[i];
            rowDataTemp[2] = _word1[i];
            rowDataTemp[3] = _word2[i];
            rowDataTemp[4] = _word3[i];
            rowDataTemp[5] = _word4[i];
            rowDataTemp[6] = _word5[i];
            rowDataTemp[7] = _word6[i];
            rowDataTemp[8] = _word7[i];
            rowDataTemp[9] = _word8[i];
            rowDataTemp[10] = _word9[i];
            rowDataTemp[11] = _numEl[i];
            rowDataTemp[12] = _trialType[i];
            rowDataTemp[13] = _taskPos[i];
            rowDataTemp[14] = _trialPos[i]; 
            rowDataTemp[15] = _resp1[i];
            rowDataTemp[16] = _resp2[i];
            rowDataTemp[17] = _resp3[i];
            rowDataTemp[18] = _resp4[i];
            rowDataTemp[19] = _resp5[i];
            rowDataTemp[20] = _resp6[i];
            rowDataTemp[21] = _resp7[i];
            rowDataTemp[22] = _resp8[i];
            rowDataTemp[23] = _resp9[i];
            rowDataTemp[24] = _acierto[i];  
            rowDataTemp[25] = _RT[i];
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

        //StreamWriter outStream = System.IO.File.CreateText(filePath);
        //outStream.WriteLine(sb);
        //outStream.Close();

        StreamWriter file2 = new StreamWriter(filePath, true);
        file2.WriteLine(sb.ToString());
        file2.Close();

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
        rowDataTemp = new string[26];
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "task";
        rowDataTemp[2] = "word1";
        rowDataTemp[3] = "word2";
        rowDataTemp[4] = "word3";
        rowDataTemp[5] = "word4";
        rowDataTemp[6] = "word5";
        rowDataTemp[7] = "word6";
        rowDataTemp[8] = "word7";
        rowDataTemp[9] = "word8";
        rowDataTemp[10] = "word9";
        rowDataTemp[11] = "numEl";
        rowDataTemp[12] = "trialType";
        rowDataTemp[13] = "taskPos";
        rowDataTemp[14] = "trialPos";
        rowDataTemp[15] = "resp1";
        rowDataTemp[16] = "resp2";
        rowDataTemp[17] = "resp3";
        rowDataTemp[18] = "resp4";
        rowDataTemp[19] = "resp5";
        rowDataTemp[20] = "resp6";
        rowDataTemp[21] = "resp7";
        rowDataTemp[22] = "resp8";
        rowDataTemp[23] = "resp9";
        rowDataTemp[24] = "acierto";
        rowDataTemp[25] = "RT";
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

        // Write tests data rows
        for (int i = 0; i < _task.Count; i++)
        {
            rowDataTemp = new string[26];
            rowDataTemp[0] = _id;
            rowDataTemp[1] = _task[i];
            rowDataTemp[2] = _word1[i];
            rowDataTemp[3] = _word2[i];
            rowDataTemp[4] = _word3[i];
            rowDataTemp[5] = _word4[i];
            rowDataTemp[6] = _word5[i];
            rowDataTemp[7] = _word6[i];
            rowDataTemp[8] = _word7[i];
            rowDataTemp[9] = _word8[i];
            rowDataTemp[10] = _word9[i];
            rowDataTemp[11] = _numEl[i];
            rowDataTemp[12] = _trialType[i];
            rowDataTemp[13] = _taskPos[i];
            rowDataTemp[14] = _trialPos[i];
            rowDataTemp[15] = _resp1[i];
            rowDataTemp[16] = _resp2[i];
            rowDataTemp[17] = _resp3[i];
            rowDataTemp[18] = _resp4[i];
            rowDataTemp[19] = _resp5[i];
            rowDataTemp[20] = _resp6[i];
            rowDataTemp[21] = _resp7[i];
            rowDataTemp[22] = _resp8[i];
            rowDataTemp[23] = _resp9[i];
            rowDataTemp[24] = _acierto[i];
            rowDataTemp[25] = _RT[i];
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
                return Application.dataPath + "/CSV/" + _id + "_" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _sesion + "_STM.csv";
#elif UNITY_ANDROID
                return Application.persistentDataPath + "/" + _id + "_"  + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _sesion + "_STM.csv";
#elif UNITY_IPHONE
                return Application.persistentDataPath+"/"+ _id + "_" + DateTime.Now.ToString().Replace("/","").Replace(":","").Replace(" ","_") + "_" + _sesion + "_STM";
#else
                return Application.dataPath +"/"+ _id + "_" + DateTime.Now.ToString().Replace("/","").Replace(":","").Replace(" ","_") + "_" + _sesion + "_STM";
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
    public void AddTestRow(string pTask, string pWord1, string pWord2, string pWord3, string pWord4, string pWord5, string pWord6, string pWord7, 
                            string pWord8, string pWord9, string pnumEl, string ptrialType,string ptaskPos, string pTrialPos, string pResp1, string pResp2,
         string pResp3, string pResp4, string pResp5, string pResp6, string pResp7, string pResp8, string pResp9, string pAcierto, string pRT)
    {       
        _task.Add(pTask);
        _word1.Add(pWord1);
        _word2.Add(pWord2);
        _word3.Add(pWord3);
        _word4.Add(pWord4);
        _word5.Add(pWord5);
        _word6.Add(pWord6);
        _word7.Add(pWord7);
        _word8.Add(pWord8);
        _word9.Add(pWord9);
        _numEl.Add(pnumEl);
        _trialType.Add(ptrialType);
        _taskPos.Add(ptaskPos);
        _trialPos.Add(pTrialPos);
        _resp1.Add(pResp1);
        _resp2.Add(pResp2);
        _resp3.Add(pResp3);
        _resp4.Add(pResp4);
        _resp5.Add(pResp5);
        _resp6.Add(pResp6);
        _resp7.Add(pResp7);
        _resp8.Add(pResp8);
        _resp9.Add(pResp9);
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
            _word3.RemoveAt(i);
            _word4.RemoveAt(i);
            _word5.RemoveAt(i);
            _word6.RemoveAt(i);
            _word7.RemoveAt(i);
            _word8.RemoveAt(i);
            _word9.RemoveAt(i);
            _numEl.RemoveAt(i);
            _trialType.RemoveAt(i);
            _trialPos.RemoveAt(i);
            _resp1.RemoveAt(i);
            _resp2.RemoveAt(i);
            _resp3.RemoveAt(i);
            _resp4.RemoveAt(i);
            _resp5.RemoveAt(i);
            _resp6.RemoveAt(i);
            _resp7.RemoveAt(i);
            _resp8.RemoveAt(i);
            _resp9.RemoveAt(i);
            _acierto.RemoveAt(i);
            _RT.RemoveAt(i);
        }
    }
}

