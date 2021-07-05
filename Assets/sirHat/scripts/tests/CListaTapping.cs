using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CListaTapping
{

    // Atributos
    public int _cantEjemplos, _cantPractica, _cantTest;
    private Dictionary<int, CEstimuloTapping> _estimulos;
    private AudioClip _audioInstrucciones;
	public int[] _indicesTest, _indMixed; 

    // Constructor
    public CListaTapping(string _pathInput, string _pathAudios)
    {
        // Inicializamos
        _cantEjemplos = 0;
        _cantPractica = 0;
        _cantTest = 0;
        // Creamo el diccionario de estimulos
        _estimulos = new Dictionary<int, CEstimuloTapping>();
        // Leemos el csv
        TextAsset _fileData = Resources.Load(_pathInput) as TextAsset;
        // Linea a linea vamos creando el estimulo y rellenando el diccionario
        string[] _lines = _fileData.text.Split("\n"[0]);
        for (int _i = 1; _i < _lines.Length; _i++)
        {
            if (_lines[_i] != "")
            {
                CEstimuloTapping _estimuloAux = new CEstimuloTapping(_lines[_i], _pathAudios);
                _estimulos.Add(_i - 1, _estimuloAux);
				// Tipo de estimulo             
				switch (_estimuloAux.TrialType)
				{
				case "0":
					_cantEjemplos++;
					break;
				case "1":
					_cantPractica++;
					break;
				case "2":
					_cantTest++;
					break;
				default:
					Debug.Log("Tipo Estímulo Incorrecto");
					break;
				}
            }
        }
		_indicesTest = new int[_cantTest];
		for (int _i = _cantPractica + _cantEjemplos; _i < _cantPractica + _cantEjemplos + _cantTest; _i++)
		{
			_indicesTest[_i - (_cantPractica + _cantEjemplos)] = _i;
		}
		_indMixed = _indicesTest;
		for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
		{
			var r = Random.Range(0, i+1);
			var tmp = _indMixed[i];
			_indMixed[i] = _indMixed[r];
			_indMixed[r] = tmp;
		}
//		for (int _i=0;_i<_indMixed.Length;_i++){
//			Debug.Log ("indMixed " + _i + ": " + _indMixed[_i]);
//		}

		_audioInstrucciones = null;
    }

    // Metodos	
	public int[] IndicesTestMixed
	{
		get
		{
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
    public AudioClip AudioInstrucciones
    {
        get { return _audioInstrucciones; }
        set { _audioInstrucciones = value; }
    }
    // Obtener Estímulo entero
    public CEstimuloTapping ObtenerEstimulo(int _ind)
    {
        return _estimulos[_ind];
    }

    public int cantEstimulos()
    {
        return _estimulos.Count;
    }

}