using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestRAN : MonoBehaviour {

    public int _recordTIme;
    public Toggle _indicador;
    public Text _textoBoton;
    private AudioClip myAudioClip;
    private string _fileName;
    private bool _grabando;
    private List<AudioClip> _audiosAgrabar;
    public bool _conf;
    public CConfiguracion _configH;
    public enum tipoTestRAN { RAN, Lectura };
    public tipoTestRAN _tipoTest;


    // Use this for initialization
    void Start () {
        _audiosAgrabar = new List<AudioClip>();
        _grabando = false;
        _indicador.isOn = false;
        _textoBoton.text = "Grabar";
    }


    public void Record()
    {
        if (!_grabando)
        {
            _grabando = true;   
            _indicador.isOn = true;
            _textoBoton.text = "Detener";
            StartCoroutine(Grabando());
        }
        else
        {
            _grabando = false;
            _indicador.isOn = false;
            _textoBoton.text = "Grabar";
        }       

    }

    public IEnumerator CorrerTest()
    {
        CLogManager.Instance.IncrementarTaskPos();
        CAudioManager.Instance.reproducirRAN();
        CCanvasManager.Instance.ApagarRepeticionAlex();
        CCanvasManager.Instance.ActivarPanelComun();
        if (_tipoTest == tipoTestRAN.RAN)
        {
            CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.RAN);
        }
        else
        {
            CAdminSalirTest.Instance.SetTestActual(CGameManager.TestTipe.Reading);
        }

        yield break;
    }

    // Salir
    public void salirTest(bool salidaForzada)
    {
        if(_tipoTest == tipoTestRAN.RAN)
        {
            _configH.marcarTareaRAN();
            CLogManager.Instance.GuardarLista("RAN");
        }
        else
        {
            _configH.marcarTareaLectura();
            CLogManager.Instance.GuardarLista("Lectura");
        }
        
        StopAllCoroutines();
        if (!_conf)
        {
            CCanvasManager.Instance.StartCoroutine("vueltaDeTest");
        }
        else
        {
            _configH.gameObject.SetActive(true);
            _conf = false;
        }
        //this.gameObject.SetActive(false);
        Salir(salidaForzada);
        
    }

    public void Salir(bool salidaForzada){
        StopAllCoroutines();
        this.gameObject.SetActive(false);
        CCanvasManager.Instance.DesactivarPanelComun();
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);

    }


    private IEnumerator Grabando()
    {
        _audiosAgrabar = new List<AudioClip>();
        while (_grabando)
        {
            myAudioClip = Microphone.Start(null, false, _recordTIme, 44100);
            while (_grabando & Microphone.IsRecording(null))
            {
                yield return null;
            }            
            if(!_grabando) Microphone.End(null); // Si salgo xq toque el boton freno el micro antes de tocar el audioclíp
            _audiosAgrabar.Add(myAudioClip);
            Debug.Log(myAudioClip.length);
        }        
        AudioClip _audiosCombinados;
        _audiosCombinados = Combine(_audiosAgrabar.ToArray());
        _fileName = getPath();
       // SavWav.Save(_fileName, _audiosCombinados);
        yield break;
    }








    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/audios/" + "RAN_" + System.DateTime.Now.ToString("MMddyyHHmmss");
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + "RAN_" + System.DateTime.Now.ToString("MMddyyHHmmss");
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/" + "RAN_" + System.DateTime.Now.ToString("MMddyyHHmmss");
#else
        return Application.dataPath + "/audios/" + "RAN_" + System.DateTime.Now.ToString("MMddyyHHmmss");
#endif
    }



    public static AudioClip Combine(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;
        AudioClip result = AudioClip.Create("Combine", length, 2, 44100/2, false, false);
        Debug.Log(result.length);
        result.SetData(data, 0);

        return result;
    }





}
