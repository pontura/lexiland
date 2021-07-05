using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CListaTestSegundaParte
{

    // Atributos
    private int _cantEjemplos, _cantPractica, _cantTest, _cantFillers;
    private Dictionary<int, CEstimuloSegundaParte> _estimulos;
    private AudioClip[] _audiosEjemplo;
    private AudioClip _audioInstrucciones;    
    private float _tRespuestaMaximo, _tInterEnsayo;
    private int[] _indicesTest, _indicesTestMixed, _indicesFillers;  


    // Constructor
    public CListaTestSegundaParte(string _pathInput, string _pathAudios)
    {
        // Inicializamos
        _cantEjemplos = 0;
        _cantPractica = 0;
        _cantTest = 0;
        _cantFillers = 0;
        // Creamo el diccionario de estimulos
        _estimulos = new Dictionary<int, CEstimuloSegundaParte>();
        // Leemos el csv
        TextAsset _fileData = Resources.Load(_pathInput) as TextAsset;
        // Linea a linea vamos creando el estimulo y rellenando el diccionario
        string[] _lines = _fileData.text.Split("\n"[0]);
        for (int _i = 1; _i < _lines.Length; _i++)
        {
            if (_lines[_i] != "")
            {
                CEstimuloSegundaParte _estimuloAux = new CEstimuloSegundaParte(_lines[_i], _pathAudios);
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
                    case 3:
                        _cantFillers++;
                        break;
                    default:
                        Debug.Log("Tipo Estímulo Incorrecto");
                        break;
                }
            }
        }
        _audiosEjemplo = new AudioClip[_cantEjemplos];
        _audioInstrucciones = null;
        if(_estimulos[0].Task == "dichMus")
        {
            Debug.Log("Aleatorizando Dich List Music");
            _estimulos = Aleatorizar(_estimulos);
        }        
        ArmarIndicesTest();
        _tInterEnsayo = 0.1f;
    }



    private void ArmarIndicesTest()
    {
        _indicesTest = new int[_cantTest];
        int _contAux = 0;
        for (int _i = 0; _i < _estimulos.Count; _i++)
        {
            if(_estimulos[_i].TipoEstimulo == 2)
            {
                _indicesTest[_contAux] = _i;
                _contAux++;
            }            
        }
        _indicesFillers = new int[_cantFillers];
        _contAux = 0;
        for (int _i = 0; _i < _estimulos.Count; _i++)
        {
            if (_estimulos[_i].TipoEstimulo == 3)
            {
                _indicesFillers[_contAux] = _i;
                _contAux++;
            }
        }


    }


    private Dictionary<int, CEstimuloSegundaParte> Aleatorizar(Dictionary<int, CEstimuloSegundaParte> _estimulosOrigen)
    {
        int _largoTotal = _estimulosOrigen.Count;
        int _count = 0;
        Dictionary<int, CEstimuloSegundaParte> _estimulosDestino = new Dictionary<int, CEstimuloSegundaParte>();
        // Primero metemos ejemplos y práctica
        for (int _i = 0; _i < _cantPractica + _cantEjemplos; _i++)
        {
            _estimulosDestino.Add(_count, _estimulosOrigen[_i]);
            _count += 1;
            _estimulosOrigen.Remove(_i);
        }

        // Aleatorizamos canciones y frecuencia
        int[] _indMixed = new int[] { 0, 1, 2, 3, 4, 5 };
        for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
        {
            var r = Random.Range(0, i + 1);
            var tmp = _indMixed[i];
            _indMixed[i] = _indMixed[r];
            _indMixed[r] = tmp;
        }



        // Ahora hacemos random de test
        string _cancion, _frecuencia;
        for (int _i = 0; _i < 6; _i++)
        {
            // Aleatorizamos cancion y frecuencia
            switch (_indMixed[_i])
            {
                case 0:
                    _cancion = "manuelita";
                    _frecuencia = "1";
                    break;
                case 1:
                    _cancion = "manuelita";
                    _frecuencia = "2";
                    break;
                case 2:
                    _cancion = "manuelita";
                    _frecuencia = "3";
                    break;
                case 3:
                    _cancion = "reves";
                    _frecuencia = "1";
                    break;
                case 4:
                    _cancion = "reves";
                    _frecuencia = "2";
                    break;
                case 5:
                    _cancion = "reves";
                    _frecuencia = "3";
                    break;
                default:
                    _cancion = "";
                    _frecuencia = "";
                    Debug.Log("Todo mal con lo que hay que buscar");
                    break;
            }

            // Debug.Log("buscando " + _cancion + _frecuencia);

            // Aleatorizamos canales
            int _aux = Random.Range(0, 3);
            int[] _indAmixar = { 0, 1, 2 };
            for (var i = _indAmixar.GetLength(0) - 1; i > 0; i--)
            {
                var r = Random.Range(0, i + 1);
                var tmp = _indAmixar[i];
                _indAmixar[i] = _indAmixar[r];
                _indAmixar[r] = tmp;
            }
            string[] _canalesMixed = new string[3];
            _canalesMixed[_indAmixar[0]] = "bi";
            _canalesMixed[_indAmixar[1]] = "b";
            _canalesMixed[_indAmixar[2]] = "m";
            // Buscamos canal por canal

            // Buscamos el bi
            for (int _indcanal = 0; _indcanal < _canalesMixed.Length; _indcanal++)
            {
                // Debug.Log("buscando el bi");
                for (int _j = 0; _j < _largoTotal; _j++)
                {
                    if (_estimulosOrigen.ContainsKey(_j))
                    {
                        if (_estimulosOrigen[_j].Cond == _cancion && _estimulosOrigen[_j].Frec == _frecuencia && _estimulosOrigen[_j].AudioL == _canalesMixed[_indcanal])
                        {

                            _estimulosDestino.Add(_count, _estimulosOrigen[_j]);
                            //  Debug.Log("encontre el bi");
                            _count += 1;
                            _estimulosOrigen.Remove(_j);
                            break;
                        }
                    }
                }

            }
            // Debug.Log("buscando el bi");
            for (int _j = 0; _j < _largoTotal; _j++)
            {
                if (_estimulosOrigen.ContainsKey(_j))
                {
                    if (_estimulosOrigen[_j].Cond == _cancion && _estimulosOrigen[_j].Frec == _frecuencia && _estimulosOrigen[_j].AudioL == "bi")
                    {

                        _estimulosDestino.Add(_count, _estimulosOrigen[_j]);
                        //  Debug.Log("encontre el bi");
                        _count += 1;
                        _estimulosOrigen.Remove(_j);
                        break;
                    }
                }
            }
        }
        for (int _j = 0; _j < _largoTotal; _j++)
        {
            if (_estimulosOrigen.ContainsKey(_j))
            {
                if (_estimulosOrigen[_j].TrialType == "3")
                {

                    _estimulosDestino.Add(_count, _estimulosOrigen[_j]);
                    _count += 1;
                    _estimulosOrigen.Remove(_j);
                    break;
                }
            }

        }

        return _estimulosDestino;
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
    public int[] IndicesFiller
    {
        get { return _indicesFillers; }
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
    public CEstimuloSegundaParte ObtenerEstimulo(int _ind)
    {
        return _estimulos[_ind];
    }


}