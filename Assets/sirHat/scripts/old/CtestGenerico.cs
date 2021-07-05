using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;



public class CtestGenerico : MonoBehaviour {


    // Variables logica común
    protected int _cantEjEje, _cantEjPra, _cantEjTest;
    protected float _tInterEnsayo;

    // Pausa
    protected bool _pausado;
    public GameObject _buttonPause, _buttonUnPause, _panelPausa;
    protected AudioClip _APausa;

    // Práctica
    protected AudioClip _APracticar;
    protected bool _respondioPractica, _practicarDeVuelta, _repetir, _respondio;
    protected int _numPrac, _trialPos;
    public GameObject _panelFinPractica;

    // Variables log
    protected float _tTotalPausa, _tini;

    // Variables UI a setear
    public AudioSource _audio;







    //----------------------METODOS -----------------------------

    // Test
    private IEnumerator Test()
    {
        // Seteos iniciales
        inicializacion();

        ////// Instrucciones y ejemplos
        yield return StartCoroutine(EjemploYBienvenida());

        // Previa practica
        yield return StartCoroutine(previaPractica());

        //// Practica
        //_practicarDeVuelta = true;
        //_numPrac = -1;
        //while (_practicarDeVuelta)
        //{
        //    _numPrac += 1;
        //    _trialPos = 0;
        //    activarVisual();
        //    activarBotones();
        //    if (_panelFinPractica.activeSelf) _panelFinPractica.SetActive(false);
        //    for (int _cont = _cantEjEje; _cont < _cantEjEje + _cantEjPra; _cont++)
        //    {
        //        _trialPos += 1;
        //        _repetir = true;

        //        while (_repetir)
        //        {
        //            yield return new WaitForSeconds(_tInterEnsayo);
        //            yield return StartCoroutine(practica(_cont));
        //            if (_respondio & _respuesta == _nroSilabas)
        //            {
        //                _repetir = false;
        //            }
        //            yield return null;
        //        }
        //    }
        //    desactivarVisual();
        //    desactivarBotones();
        //    _panelFinPractica.SetActive(true);
        //    yield return StartCoroutine(reproducirAudio(_AVolverAPracticar));
        //    yield return StartCoroutine(esperarRespuestaPractica());
        //}
        //_panelFinPractica.SetActive(false);

        //// Test
        //_trialPos = 0;
        //desactivarVisual();
        //activarBotones();
        //_audio.clip = _AComenzar;
        //_audio.Play();
        //yield return StartCoroutine(esperarAudio(_audio));
        //_indicesTest = mezclarIndices(_indicesTest);
        //for (int _cont = 0; _cont < _indicesTest.Length; _cont++)
        //{
        //    _trialPos += 1;
        //    yield return new WaitForSeconds(_tInterEnsayo);
        //    yield return StartCoroutine(testPosta(_indicesTest[_cont]));
        //}
        //desactivarVisual();
        //desactivarBotones();
        ////_audio.clip = _AFin;
        ////_audio.Play();
        ////yield return StartCoroutine(esperarAudio(_audio));
        //salirTestSegmentacion();

    }

    // A implementar ----------------------------------------------------
    // Inicializacion
    protected void inicializacion()
    {

    }

    // Ejemplo y Bienvenida
    protected IEnumerator EjemploYBienvenida()
    {
        yield break;
    }
    // Previa práctica
    protected IEnumerator previaPractica()
    {
        // Quieres practicar ahroa te toca a ti
        // demostracion pausa
        yield break;
    }

    //--------------------------------------------------------------------




    // BOTONES
    // Boton de no repetir la practica
    public void botonContinuarTest()
    {
        _practicarDeVuelta = false;
        _respondioPractica = true;
    }

    // Boton de repetir practica
    public void botonRepetirPractica()
    {
        _practicarDeVuelta = true;
        _respondioPractica = true;
    }








    // ESPERAS
    // Esperar que responda si quiere practicar de vuelta
    public IEnumerator esperarRespuestaPractica()
    {
        while (!_respondioPractica)
        {
            yield return null;
        }
        _respondioPractica = false;
        yield break;
    }
















    // AUDIO
    // Reproducir audio
    private IEnumerator reproducirAudio(AudioClip _audioAux)
    {
        Debug.Log("reproduciendo: " + _audioAux);
        _audio.clip = _audioAux;
        _audio.Play();
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(0.2f);
        yield break;
    }
    // Esperar a que termine el audio
    private IEnumerator esperarAudio(AudioSource _a)
    {
        while (_a.isPlaying | _pausado)
        {
            // Debug.Log("Esperando a: " + _a);
            yield return null;
        }
        yield break;
    }











    // PAUSA
    // Boton pausa
    public void pausar()
    {
        _pausado = true;
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        _panelPausa.SetActive(true);
        if (_audio.isPlaying) _audio.Pause();
        //Time.timeScale = 0;  
    }

    // Boton unpausar
    public void unPausar()
    {
        _pausado = false;
        _tTotalPausa = Time.time - _tini;
        _tini = Time.time;
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        _panelPausa.SetActive(false);
        _audio.UnPause();
        //Time.timeScale = 1;
    }
    // Mostrar pausa.
    private IEnumerator mostrarPausa()
    {
        _audio.clip = _APausa;
        _audio.Play();
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(true);
        _buttonPause.SetActive(false);
        _buttonUnPause.SetActive(true);
        yield return StartCoroutine(esperarAudio(_audio));
        yield return new WaitForSeconds(1f);
        _panelPausa.SetActive(false);
        _buttonPause.SetActive(true);
        _buttonUnPause.SetActive(false);
        yield break;
    }











    // METODOS AUXILIARES PARA LOGICA

    // Método para mezclar indices
    protected int[] mezclarIndices(int[] _indMixed)
    {
        for (var i = _indMixed.GetLength(0) - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = _indMixed[i];
            _indMixed[i] = _indMixed[r];
            _indMixed[r] = tmp;
        }
        return _indMixed;
    }





}
