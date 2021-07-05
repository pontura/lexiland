﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CListaTestDichListSyl
{

    // Atributos
    private int _cantEjemplos, _cantPractica, _cantTest;
    private Dictionary<int, CEstimuloDichListSyl> _estimulos;
    private AudioClip[] _audiosEjemplo;
    private AudioClip _audioInstrucciones;
    private float _tRespuestaMaximo, _tInterEnsayo;
    public enum TestTipe { SegmentationS, SegmentationF, AisolationS, AisolationF, STMsimon, STMorden, RAN, SintesisS, SintesisF };
    private TestTipe _tipoTest;
    private int[] _indicesTest, _indicesTestMixed;  


    // Constructor
    public CListaTestDichListSyl(string _pathInput, string _pathAudios)
    {
        // Inicializamos
        _cantEjemplos = 0;
        _cantPractica = 0;
        _cantTest = 0;
        // Creamo el diccionario de estimulos
        _estimulos = new Dictionary<int, CEstimuloDichListSyl>();
        // Leemos el csv
        TextAsset _fileData = Resources.Load(_pathInput) as TextAsset;
        // Linea a linea vamos creando el estimulo y rellenando el diccionario
        string[] _lines = _fileData.text.Split("\n"[0]);
        for (int _i = 1; _i < _lines.Length; _i++)
        {
            if (_lines[_i] != "")
            {
                CEstimuloDichListSyl _estimuloAux = new CEstimuloDichListSyl(_lines[_i], _pathAudios);
                _estimulos.Add(_i - 1, _estimuloAux);
                // Tipo de estimulo             
                switch (_estimuloAux.TipoEstimulo)
                {
                    case 0:
                        _cantEjemplos++;
                        break;
                    case 1:
                        _cantPractica++;
                        break;
                    case 2:
                        _cantTest++;
                        break;
                    default:
                        Debug.Log("Tipo Estímulo Incorrecto");
                        break;
                }
            }
        }
        _audiosEjemplo = new AudioClip[_cantEjemplos];
        _audioInstrucciones = null;
        _indicesTest = new int[_cantTest];
        for (int _i = _cantPractica + _cantEjemplos; _i < _cantPractica + _cantEjemplos + _cantTest; _i++)
        {
            _indicesTest[_i - (_cantPractica + _cantEjemplos)] = _i;
        }
        _tInterEnsayo = 0.1f;
    }

    // Metodos
    public int CantEjemplos
    {
        get { return _cantEjemplos; }
    }
    public int CantPractica
    {
        get { return _cantPractica; }
    }
    public int CantTest
    {
        get { return _cantTest; }
    }
    public int[] IndicesTest
    {
        get { return _indicesTest; }
    }
    public int[] IndicesTestMixed
    {
        get
        {
            int[] _indMixed = _indicesTest;
            for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
            {
                var r = Random.Range(0, i+1);
                var tmp = _indMixed[i];
                _indMixed[i] = _indMixed[r];
                _indMixed[r] = tmp;
            }
            return _indMixed;
        }
    }


    // Función para desplegar data del proyecto
    public void debugTestData()
    {
        Debug.Log("Contenido diccionario: ");
        for (int _i = 0; _i < _estimulos.Count; _i++)
        {
            Debug.Log(_estimulos[_i].LineaEntera);
        }
    }
    // Audios Ejemplo
    public AudioClip[] AudiosEjemplo
    {
        get { return _audiosEjemplo; }
        set { _audiosEjemplo = value; }
    }
    // Tiempo Respuesta
    public float TiempoRespMaximo
    {
        get { return _tRespuestaMaximo; }
        set { _tRespuestaMaximo = value; }
    }
    // Tiempo InterEnsayo
    public float TiempoInterEnsayo
    {
        get { return _tInterEnsayo; }
        set { _tInterEnsayo = value; }
    }
    // Audio Instrucciones
    public AudioClip AudioInstrucciones
    {
        get { return _audioInstrucciones; }
        set { _audioInstrucciones = value; }
    }
    public bool TieneInstrucciones()
    {
        if (_audioInstrucciones == null) return false;
        else return true;
    }

    // Obtener Estímulo entero
    public CEstimuloDichListSyl ObtenerEstimulo(int _ind)
    {
        return _estimulos[_ind];
    }
    // Tiempo Test
    public TestTipe TipoTest
    {
        get { return _tipoTest; }
        set { _tipoTest = value; }
    }


}