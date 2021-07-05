using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CTapping : MonoBehaviour {

    public AudioSource audiosource;
    public Image estimuloVisual;
    public GameObject _feedback;
    public AudioClip[] audioInstrucciones;
    int Tsamples;
    float dt = 0;
    enum estado {test, idle, instrucciones, guardado, inicial};
    estado Estado;
    public Sprite[] tapping;
    List<float> taps;
    //public Text tessto;
    int n;
    int idT;
    public GameObject tambor;
    public GameObject mano;
    bool practica;
    private CListaTapping _listaTapping;
    private CEstimuloTapping _estimulo;
    string _path;
    public CConfiguracion _configH;
    public bool _conf;
    int contador = 0;
	float tiempoInicial = 0.0f;


    // Use this for initialization
    void Start () {
        _feedback.SetActive(false);
        //Estado = estado.inicial;
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (Estado == estado.test) {
			if (audiosource.timeSamples > n * Tsamples) {
                if (n == 0)
                {
                    _feedbackEN = true;
                   // StartCoroutine(FeedbackVisual());
                    tiempoInicial = Time.time;
                }
				n++;
				if (_estimulo.isVisual)
					estimuloVisual.sprite = tapping [0];
				if (practica){                   
                    mano.transform.localScale = Vector3.one * 0.8f;
					tambor.transform.localScale = 0.95f*Vector3.one;
				}
			}
			if (_estimulo.isVisual && 2 * audiosource.timeSamples > (2 * n - 1) * Tsamples) {
				estimuloVisual.sprite = tapping [1];
				if (practica){
					mano.transform.localScale = Vector3.one;
					tambor.transform.localScale = Vector3.one;
				}
			}
			if (!audiosource.isPlaying)
				Estado = estado.guardado;
		} else if (Estado == estado.idle) {
			if (idT < _listaTapping.cantEstimulos () - 1)
				StartCoroutine ("instrucciones");
			else {
				// Marcamos el test tapping como realizado en configuracion
				_configH.marcarTareaTapping ();
				// Guardamos en el archivo de log que está hecha
				CLogManager.Instance.GuardarLista ("Tapping");
                StartCoroutine(salirEsperando());
			}           
                
		} else if (Estado == estado.guardado) {
			guardarDatos ();
            _feedbackEN = false;
            Estado = estado.idle;
		} else if (Estado == estado.inicial) {


		}
    }


    public IEnumerator instrucciones()
    {
        Estado = estado.instrucciones;
        tambor.SetActive(false);
        mano.SetActive(false);
        estimuloVisual.gameObject.SetActive(false);
        idT++;
		if (idT < _listaTapping._cantEjemplos + _listaTapping._cantPractica) {
			_estimulo = _listaTapping.ObtenerEstimulo (idT);
		} else {
			int _aux = idT-_listaTapping._cantEjemplos - _listaTapping._cantPractica;
			Debug.Log ("indice de mix: " + _aux);
			Debug.Log ("indice mixed: " + _listaTapping.IndicesTestMixed[_aux]);
			_estimulo = _listaTapping.ObtenerEstimulo (_listaTapping.IndicesTestMixed[_aux]);
		}
        audiosource.clip = audioInstrucciones[idT];
        n = 0;
        audiosource.Play();
        yield return StartCoroutine("playAudio");
        tambor.SetActive(true);
        audiosource.clip = _estimulo.getAudioEstimulo;
        Tsamples = (int)(1/_estimulo.freq * _estimulo.getAudioEstimulo.samples / _estimulo.getAudioEstimulo.length);
        audiosource.Play();
        Estado = estado.test;
        practica = (int.Parse(_estimulo.TrialType) == 0);
        if (_estimulo.isVisual) estimuloVisual.gameObject.SetActive(true);
        mano.SetActive(practica);
    }

    public void guardarDatos()
    {
        List<string[]> tapsText = new List<string[]>();
        for (int i = 0; i < taps.Count; i++)
			tapsText.Add(new string[8] {CLogManager.Instance._textoID,   "tapping", _estimulo.TrialCode, _estimulo.TrialType, _estimulo.freq.ToString(),
                taps[i].ToString(), contador++.ToString() , CLogManager.Instance._textoSesion });
        GuardarData(tapsText, _path);
        taps.Clear();
    }

    public IEnumerator playAudio()
    {
        while (audiosource.isPlaying)
        {
            yield return null;
        }
        yield break;
    }

/*    public void tap()
    {
        taps.Add(audiosource.time);
        //tessto.text = (audiosource.time - dt).ToString();
        dt = audiosource.time;
    }
*/

	void OnGUI()
	{
		if (Event.current.type == EventType.MouseDown) {
            taps.Add(Time.time - tiempoInicial);
			tambor.transform.localScale = 0.95f*Vector3.one;
		}
		if (Event.current.type == EventType.MouseUp) {
			tambor.transform.localScale = Vector3.one;
		}
	}

    private bool _feedbackEN;
    float _tresp, _trespAnterior, _treal;
    float _tol = 50f / 1000f;
    private IEnumerator FeedbackVisual()
    {
        Debug.Log("corriendo corutina");
        _feedback.SetActive(true);
        while (_feedbackEN)
        {
            //if (Mathf.Abs(_tresp - _treal) < _tol && (Vector3.Magnitude(_feedback.transform.localScale) < 3f))
            //{
            //    _feedback.transform.localScale = 1.1f * _feedback.transform.localScale;
            //}
            //else
            //{
            //    _feedback.transform.localScale = 0.9f * _feedback.transform.localScale;
            //}
            yield return new WaitForSeconds(0.5f);
        }
        _feedback.SetActive(false);
        Debug.Log("Terminando corutina");
        yield break;
    }


    public void inicializar()
    {
		Debug.Log("En el start de Tapping");
		taps = new List<float>();
		Estado = estado.idle;
		idT = -1;
        _path = createPath();
        _listaTapping = new CListaTapping("inputs/tapping", "audiosTapping/");
        //_listaTapping.debugTestData();
        // Encabezado Log
        List<string[]> _cabecera = new List<string[]>();
        string[] rowDataTemp = CLogManager.Instance.HeaderLogin();
        _cabecera.Add(rowDataTemp);
        rowDataTemp = CLogManager.Instance.DataLogin();
        _cabecera.Add(rowDataTemp);
        _cabecera.Add(new string[8] {"ID" , "task", "trial_Code", "trial_type", "freq", "t", "contador", "session" });
        GuardarData(_cabecera, _path);
    }

    private IEnumerator salirEsperando()
    {
        yield return new WaitForSeconds(1f);
        salirTest();


    }

    public void salirTest()
    {
		Estado = estado.inicial;
        // Frenamos corutinas que puedan estar corriendo
        StopAllCoroutines();
        if (!_conf)
        {
            CCanvasManager.Instance.StartCoroutine("vueltaDeTest");
            CCanvasManager.Instance.DesactivarPanelComun();            
        }
        else
        {
            _configH.gameObject.SetActive(true);
            CCanvasManager.Instance.DesactivarPanelComun();
            _conf = false;
        }
        // Apagamos panel
        CCanvasManager.Instance.ApagarMano();
        this.gameObject.SetActive(false);



    }

    private void GuardarData(List<string[]> _data, string _path)
    {
        Debug.Log("GuardarData");
        string[][] output = new string[_data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = _data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";


        StreamWriter file2 = new StreamWriter(_path, true);
        string aux;
        for (int index = 0; index < length; index++)
        {
            aux = string.Join(delimiter, output[index]);
            file2.WriteLine(aux);
        }
        file2.Close();
    }


    private string createPath()
    {
        string _id = CLogManager.Instance._textoID;
        string _sess = CLogManager.Instance._textoSesion;
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _id + "_" + _sess + "_tapping.csv";
#elif UNITY_ANDROID
		return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _id + "_" + _sess + "_tapping.csv";
#elif UNITY_IPHONE
		return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_tapping.csv";
#else
		return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_tapping.csv";
#endif
    }

}
