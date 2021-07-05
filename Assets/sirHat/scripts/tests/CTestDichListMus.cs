using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CTestDichListMus : MonoBehaviour {
	
	public AudioSource audiosource;
	public Image estimuloVisual;
	//public AudioClip[] audioInstrucciones;
	int Tsamples;
	float dt = 0;
	enum estado {test, idle, instrucciones, guardado};
	estado Estado;
	//public Sprite[] tapping;
	List<float> taps;
	//public Text tessto;
	int n;
	int idT;
	public GameObject tambor;
	public GameObject mano;
	bool practica;
	private CListaTestDichListMus _listaTapping;
	private CEstimuloDichListMus _estimulo;
	string _path;
	public CConfiguracion _configH;
	public bool _conf;
	int contador = 0;
	
	
	// Use this for initialization
	void Start () {
		Debug.Log("En el start de DichListMus");
		taps = new List<float>();
		SetearEstado (estado.idle);
		idT = -1;
		inicializar();
	}

	
	private void inicializar()
	{
		_path = createPath();
		//_listaTapping = CInfoManager.Instance.DataDichListMus ();
		List<string[]> _cabecera = new List<string[]>();
		_cabecera.Add(new string[7] { "task", "trial_Code", "trial_type", "freq", "session", "t", "contador" });
		GuardarData(_cabecera, _path);
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (Estado == estado.test)
		{
			if (practica && audiosource.timeSamples > n*Tsamples)
			{
				n++;
				//if (_estimulo.isVisual) estimuloVisual.sprite = tapping[0];
				mano.transform.localScale = Vector3.one * 0.8f;
			}
			if (practica && 2*audiosource.timeSamples > (2*n-1)*Tsamples)
			{
				//estimuloVisual.sprite = tapping[1];
				mano.transform.localScale = Vector3.one;
			}
			if (!audiosource.isPlaying)
				Estado = estado.guardado;
		}
		else if (Estado == estado.idle)
		{
			if (idT < _listaTapping.cantEstimulos() - 1)
				StartCoroutine("instrucciones");
			else
			{
				// Marcamos el test tapping como realizado en configuracion
				_configH.marcarTareaDichListMus();
				// Guardamos en el archivo de log que está hecha
				//CLogManager.Instance.GuardarLista("Tapping");   Esto tiene que ir !!!!!!!!!!!!!!!!!!!!
				salirTest();
			}           
			
		}
		else if (Estado == estado.guardado)
		{
			guardarDatos();
			Estado = estado.idle;
		}
	}


	// Cosas a hacer cuando me voy de un estado
	private void SalirDeEstado(estado _estadoOrigen){
		switch (_estadoOrigen) {

		case estado.idle:

			break;

		case estado.test:
			guardarDatos();
			SetearEstado (estado.idle);
			break;
			
			
		}

	}

	// Cosas a hacer a la entrada de un estado
	private void SetearEstado(estado _estadoDestino){
		switch (_estadoDestino) {

		case estado.idle:
			Estado = estado.idle;
			break;

		case estado.test:
			break;


		}

	}
	
	
	public IEnumerator instrucciones()
	{
		Estado = estado.instrucciones;
		tambor.SetActive(false);
		mano.SetActive(false);
		//estimuloVisual.gameObject.SetActive(false);
		idT++;
		_estimulo = _listaTapping.ObtenerEstimulo(idT);
		if (idT == 0) {
			audiosource.clip = _listaTapping.AudioInstrucciones;
			audiosource.Play();
			yield return StartCoroutine("playAudio");
		}
		n = 0;
		tambor.SetActive(true);
		audiosource.clip = _estimulo.Audios[0];
		Tsamples = (int)(1/_estimulo.FreqInt * _estimulo.Audios[0].samples / _estimulo.Audios[0].length);
		audiosource.Play();
		Estado = estado.test;
		practica = (int.Parse(_estimulo.TrialType) == 0);
		//if (_estimulo.isVisual) estimuloVisual.gameObject.SetActive(true);
		mano.SetActive(practica);
	}











	public void guardarDatos()
	{
		int session = 1;
		List<string[]> tapsText = new List<string[]>();
		for (int i = 0; i < taps.Count; i++)
			tapsText.Add(new string[7] {"dichMus", _estimulo.TrialCode, _estimulo.TrialType, _estimulo.Freq, session.ToString(), taps[i].ToString(), contador++.ToString() });
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
	
	public void tap()
	{
		taps.Add(audiosource.time);
		//tessto.text = (audiosource.time - dt).ToString();
		dt = audiosource.time;
	}

	
	public void salirTest()
	{
		
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
		string _id = "estaID";
		string _sess = "estaSession";
		#if UNITY_EDITOR
		return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _id + "_" + _sess + "_DichListMus.csv";
		#elif UNITY_ANDROID
		return Application.persistentDataPath + "/" + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + "_" + _id + "_" + _sess + "_DichListMus.csv";
		#elif UNITY_IPHONE
		return Application.persistentDataPath + "/CSV/"  + System.DateTime.Now.ToString("MMddyy") + "_" + System.DateTime.Now.ToString("HHmmss") + _id + "_" + _sess + "_DichListMus.csv";
		#else
		return Application.dataPath + "/CSV/" + System.DateTime.Now.ToString("MMddyyHHmmss") + "_" + _id + "_" + _sess + "_DichListMus.csv";
		#endif
	}
	
}
