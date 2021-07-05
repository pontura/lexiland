using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAudioManager : MonoBehaviour
{

    // Singleton
    public static CAudioManager Instance;
    // Audios en comun
    private AudioClip _bienvenida, _aPracticar, _aPracticarConPausa, _fin, _aPulsaResp, _aVolverAPracticar, _aComenzar, _aBien, _aMal,
        _aPausa, _pantallaFinal, _tetocati, _finsesion, _Alexayuda, _RAN, _ultimoGloboSesion, _audioWelcomeBack;
    public AudioClip _MusicaVisSeg, _musicaFondoTablero;
    // Audiosource
    public AudioSource _audioSource, _MusicaFondo;
    // Variables internas
    private AudioClip _audioActual;
    private AudioClip[] _aRepetir;
    public bool RepetirSimon;
    public CTest6 _testSimon;

    private bool _pausado;

    // MT
    static AudioClip _ac;
    public static Dictionary<string, AudioClip> _aclips;

    private static int minFreq;
    private static int maxFreq;
    public static int _lenght = 7;


    void Awake()
    {
        Instance = this;
    }
        

    // Use this for initialization
    void Start()
    {
        //_bienvenida = Resources.Load<AudioClip>("audiosGenerales/bienvenidoacasa");
        _aPracticar = Resources.Load<AudioClip>("audiosGenerales/practicar");
        _aPulsaResp = Resources.Load<AudioClip>("audiosGenerales/pulsarespuesta");
       // _aPracticarConPausa = Resources.Load<AudioClip>("audiosGenerales/bien_pausa_comienzo");
        _aVolverAPracticar = Resources.Load<AudioClip>("audiosGenerales/volverapracticar");
        _aComenzar = Resources.Load<AudioClip>("audiosGenerales/listos");
        _aBien = Resources.Load<AudioClip>("audiosGenerales/Bien");
        _aMal = Resources.Load<AudioClip>("audiosGenerales/Mal");
        _fin = Resources.Load<AudioClip>("audiosGenerales/globo2");
        _ultimoGloboSesion = Resources.Load<AudioClip>("audiosGenerales/globo_casa_4");
        _pantallaFinal = Resources.Load<AudioClip>("audiosGenerales/pantallaFinal");
        _aPausa = Resources.Load<AudioClip>("audiosGenerales/Pausa");
        _tetocati = Resources.Load<AudioClip>("audiosGenerales/tetocati");
        _finsesion = Resources.Load<AudioClip>("audiosGenerales/proximasesioncapl");
        _Alexayuda = Resources.Load<AudioClip>("audiosGenerales/Alexayuda");
		_RAN = Resources.Load<AudioClip>("audiosInstrucciones/RAN_I");
        _aRepetir = new AudioClip[0];
        _audioWelcomeBack = Resources.Load<AudioClip>("audiosGenerales/hola");


        // MT
        _aclips = new Dictionary<string, AudioClip>();

        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present  
        {
            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0) maxFreq = 44100;

        }

    }

    // MT
    public static void StartRecord()
    {
        if (!Microphone.IsRecording(null))
        {
            _ac = Microphone.Start(null, false, _lenght, maxFreq);
        }
    }

    public static void StopRecord()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }
        // TODO: Guardar el clip

    }

    public static void GuardarRecord(string _name)
    {
        StopRecord();
        // TODO: Verificar _ac
        _aclips.Add(_name, _ac);
    }

    public void playRecord(string _name)
    {
        Debug.Log(_name);
        this.GetComponent<AudioSource>().clip = _aclips[_name];
        this.GetComponent<AudioSource>().Play();

    }
    // END MT



    // Musica fondo
    public void PrenderMusica()
    {
        _MusicaFondo.Play();
    }
    public void ApagarMusica()
    {
        _MusicaFondo.Stop();
    }
    public void ReproducirWelcomeBack()
    {
        _audioSource.clip = _audioWelcomeBack;
        _audioSource.Play();

    }

    public int TimeSample()
    {
        return _audioSource.timeSamples;
    }

    public float TimeAudioSource()
    {
        return _audioSource.time;
    }


    // Reproduccion privada
    private IEnumerator reproducirAudio()
    {
        ApagarMusica();
        _audioSource.clip = _audioActual;
        _audioSource.Play();
        yield return StartCoroutine(esperarAudio());
        yield return new WaitForSeconds(0.2f);
        yield break;
    }
    // Reproduccion privada sin espera
    private void reproducirAudioSinEspera()
    {
        _audioSource.clip = _audioActual;
        _audioSource.Play();      
    }
    // Esperar a que termine el audio
    private IEnumerator esperarAudio()
    {
        while (_audioSource.isPlaying | _pausado)
        {
            yield return null;
        }
        yield break;
    }

    // Reproduccion privada sin espera
    public void reproducirRAN()
    {
        _audioSource.clip = _RAN;
        _audioSource.Play();
    }

    // Reproduccion musica de vis seg
    public void reproducirVisSeg()
    {
        _MusicaFondo.clip = _MusicaVisSeg;
        _MusicaFondo.Play();
        _MusicaFondo.volume = 0.2f;
    }
    public void finreproducirVisSeg()
    {
        _MusicaFondo.clip = _musicaFondoTablero;
        _MusicaFondo.volume = 0.8f;
    }


    // Metodos llamados desde afuera
    //Bienvenida
    public IEnumerator Bienvenida()
    {
        _audioActual = _bienvenida;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    public void FinTest()
    {
        _audioActual = _fin;
        StartCoroutine(reproducirAudio());
    }
    public void UltimoGloboSesion()
    {
        _audioActual = _ultimoGloboSesion;
        StartCoroutine(reproducirAudio());
    }
    public IEnumerator AlexAyuda()
    {
        _audioActual = _Alexayuda;
        yield return  StartCoroutine(reproducirAudio());
        yield break;
    }
    public void FinSesion()
    {
        _audioActual = _finsesion;
        StartCoroutine(reproducirAudio());
    }
    public IEnumerator Tetocati()
    {
        _audioActual = _tetocati;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    public void ProximaSesion()
    {
        _audioActual = _finsesion;
        StartCoroutine(reproducirAudio());
    }

    // Reproducir audio en particular
    public IEnumerator Reproducir(AudioClip _clip)
    {
		Debug.Log (_clip);
        _audioActual = _clip;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // Reproducir audio en particular sin espera
    public void ReproducirSinEspera(AudioClip _clip)
    {
        _audioActual = _clip;
        reproducirAudioSinEspera();
    }
    public IEnumerator Esperar()
    {
        yield return StartCoroutine(esperarAudio());
        yield break;
    }
    // Querés practicar
    public IEnumerator APracticar()
    {
        _audioActual = _aPracticar;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // Pulsa la respuesta correcta
    public IEnumerator PulsaCorrecta()
    {
        _audioActual = _aPulsaResp;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // Volver a practicar
    public IEnumerator VolverAPracticar()
    {
        _audioActual = _aVolverAPracticar;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // Coomenzar
    public IEnumerator Comenzar()
    {
        _audioActual = _aComenzar;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // reproducir bien!
    public IEnumerator Bien()
    {
        _audioActual = _aBien;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // reproducir bien!
    public IEnumerator Mal()
    {
        _audioActual = _aMal;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // Reproducir resultado
    public IEnumerator ReproducirResultado(bool _res)
    {
        _audioActual = _res ? _aBien : _aMal;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
    // reproducir bien!
    public IEnumerator MostrarPausa()
    {
        _audioActual = _aPausa;
        yield return StartCoroutine(reproducirAudio());
        yield break;
    }
   

    public void Pausar()
    {
        _pausado = true;
        _audioSource.Pause();
    }
    public void UnPausar()
    {
        _audioSource.UnPause();
        _pausado = false;
        
    }



    // Repetida de audio estímulo por Alex en Ventana---------------------------------------------------------------------------
    // Habilitar Alex
    private bool _repeticionAlex;
    public bool RepeticionAlex
    {
        get { return _repeticionAlex; }
        set { _repeticionAlex = value; }
    } 
    // Reproducir actual
    public void ReproducirActual()
    {

        if (_repeticionAlex && _aRepetir != null)
        {
            FrenarAudios();
            _repeticionAlex = false;
            StartCoroutine(RepetirAudio());
        }
        if (RepetirSimon) StartCoroutine(_testSimon.RepetirSecuencia());

    }

    public void FrenarAudios()
    {
        StopAllCoroutines();
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    // seteamos el audio que se repetirá
    public void SetearARepetir(AudioClip[] _audiosEstimulos)
    {
        FrenarRepeticion();
        _aRepetir = _audiosEstimulos;

    }
    // Repetir Audio
    public IEnumerator RepetirAudio()
    {
        if (_aRepetir != null) {
            AudioClip[] _aRepetirAux = _aRepetir;
            for (int _i = 0; _i < _aRepetirAux.Length; _i++)
            {
                if (_aRepetirAux[_i] != null)
                {
                    yield return StartCoroutine(Reproducir(_aRepetirAux[_i]));
                }
                else
                {
                    break;
                }
            }
        }
        _repeticionAlex = true;
        yield break;
    }

    public void FrenarRepeticion()
    {
        StopCoroutine(RepetirAudio());
    }

}

