using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CListaTestVisSS
{

    // Atributos
    private AudioClip[] _audiosEjemplo;
    private AudioClip _audioInstrucciones;
    private float _tRespuestaMaximo, _tInterEnsayo;


    // Constructor
    public CListaTestVisSS()
    {
        _audiosEjemplo = null;
        _audioInstrucciones = null;
        _tInterEnsayo = 0.1f;
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




}