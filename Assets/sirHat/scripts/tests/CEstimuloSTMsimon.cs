using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CEstimuloSTMsimon
{
    // Strings para logeo fácil
    private string _task, _word1, _word2, _word3, _word4, _word5, _word6, _word7, _word8, _word9, _numEl,_trialType, _taskPos, _trialPos,
        _resp1, _resp2, _resp3, _resp4, _resp5, _resp6, _resp7, _resp8, _resp9, _acierto, _RT;
    private string _lineaEntera;
    private string[] _lineaArray;
    // Variables utiles para test
    private int[] _secuenciaCorrecta;
    private string[] _secuenciaCorrectaString;
    private int[] _secuenciaRespuesta;
    private string[] _secuenciaRespuestaString;
    private int _tipoEstimulo, _numEstimulos;

    public static string[] GetHeaders()
    {
        string[] rowDataTemp = {"word1", "word2", "word3", "word4", "word5", "word6", "word7", "word8", "word9", "numEl", "trialType", "taskPos", "trialPos",
            "resp1", "resp2", "resp3", "resp4", "resp5", "resp6", "resp7", "resp8", "resp9", "acierto", "RT"};
        return rowDataTemp;
    }

    public string[] GetEstimulo(int _trialPos, string _tResp, int[] _flashRespuesta, int _nroRespuesta, string _acierto)
    {
        string[] _respLog = new string[9];
        for (int _i = 0; _i < 9; _i++)
        {
            if (_i < _nroRespuesta)
                _respLog[_i] = (_flashRespuesta[_i] + 1).ToString();
            else
                _respLog[_i] = "";

            if (TrialType.Equals("0"))
                _respLog[_i] = "NA";

        }
        string[] rowDataTemp = {Word1, Word2, Word3, Word4, Word5, Word6, Word7, Word8, Word9, NumEl, TrialType, _taskPos, _trialPos.ToString(),
            _respLog[0], _respLog[1], _respLog[2], _respLog[3], _respLog[4], _respLog[5], _respLog[6], _respLog[7], _respLog[8], _acierto, _tResp};
        return rowDataTemp;
    }


    // Constructor
    public CEstimuloSTMsimon(string _linea)
    {
        //Debug.Log("creando estimulo simon " + _linea);

        _lineaEntera = _linea;
        string[] _lineaData = (_linea.Trim()).Split(","[0]);
        _task = _lineaData[0];
        _word1 = _lineaData[1];
        _word2 = _lineaData[2];
        _word3 = _lineaData[3];
        _word4 = _lineaData[4];
        _word5 = _lineaData[5];
        _word6 = _lineaData[6];
        _word7 = _lineaData[7];
        _word8 = _lineaData[8];
        _word9 = _lineaData[9];
        _numEl = _lineaData[10];
        _trialType = _lineaData[11];
        _tipoEstimulo = int.Parse(_trialType);
        _numEstimulos = int.Parse(_numEl);
        _secuenciaCorrecta = new int[_numEstimulos];
        _secuenciaCorrectaString = new string[_numEstimulos];
        for (int _i = 0; _i < _numEstimulos; _i++)
        {
            _secuenciaCorrectaString[_i] = _lineaData[_i + 1];
            _secuenciaCorrecta[_i] = int.Parse(_lineaData[_i + 1])-1;
            //Debug.Log("sec correcta: " + _secuenciaCorrecta[_i]);
        }
    }



    // Accesos a las variables
    public string[] LineaArray
    {
        get
        {
            _lineaArray = new string[25] { _task, _word1, _word2, _word3, _word4, _word5, _word6, _word7, _word8, _word9, _numEl,_trialType, _taskPos, _trialPos,
                _resp1, _resp2, _resp3, _resp4, _resp5, _resp6, _resp7, _resp8, _resp9, _acierto, _RT };
            return _lineaArray;
        }
    }
    public string Word1
    {
        get { return _word1; }
    }
    public string Word2
    {
        get { return _word2; }
    }
    public string Word3
    {
        get { return _word3; }
    }
    public string Word4
    {
        get { return _word4; }
    }
    public string Word5
    {
        get { return _word5; }
    }
    public string Word6
    {
        get { return _word6; }
    }
    public string Word7
    {
        get { return _word7; }
    }
    public string Word8
    {
        get { return _word8; }
    }
    public string Word9
    {
        get { return _word9; }
    }

    public int NumEstimulos
    {
        get { return _numEstimulos; }
    }
    public int TipoEstimulo
    {
        get { return _tipoEstimulo; }
    }
    public string LineaEntera
    {
        get { return _lineaEntera; }
    }
    public string Task
    {
        get { return _task; }
    }
    public string TrialType
    {
        get { return _trialType; }
    }
    public string TaskPosition
    {
        get { return _taskPos; }
        set { _taskPos = value; }
    }
    public string TrialPosition
    {
        get { return _trialPos; }
        set { _trialPos = value; }
    }
    public string Acierto
    {
        get { return _acierto; }
        set { _acierto = value; }
    }
    public string RT
    {
        get { return _RT; }
        set { _RT = value; }
    }
    public string NumEl
    {
        get { return _numEl; }
    }
    public int[] SecuenciaCorrecta
    {
        get { return _secuenciaCorrecta; }
    }
}
