using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum Modo {Edit, Play}

public enum Tareas {aislamientoS, aislamientoF}

public class Juego : MonoBehaviour {

    const int MINESTACIONES = 3;

    Modo modo;

    public List<Estacion> estaciones = null;
    public Estacion editEstacion;
    public CConfiguracion conf;
    public PantallaTareas pantallaTareas;
    public Button comenzarJuego;
    public InputField nombreJuego;
    public Button borrarTablero;
    public GameObject cabezal;

    public GameObject intro;
    public GameObject globos;
    public GameObject final;
    public GameObject player;
    public GameObject panelGuardado;
    public GameObject panelPrevia;

    public static Juego j;
    public CheckPassword checkPassword;

    int indexEstacion;
    Dropdown juegos;
    int indexJuego;
    Coroutine nextStation;

    CJuego juego;
    GameObject CrearTablero;
    Text DropdownLabel;

	public void Awake()
	{
        j = this;
        juegos = GetComponentInChildren<Dropdown>();
        LoadJuegos();
	}

	public void OnEnable()
	{
        ClearAll();
        cabezal.SetActive(true);
        LoadJuegos();

        comenzarJuego.gameObject.SetActive(true);
        comenzarJuego.interactable = false;
        player.SetActive(false);
        panelGuardado.SetActive(false);
        panelPrevia.SetActive(false);
        final.SetActive(false);
        //player.GetComponent<Animator>().speed = 0;

        juegos.gameObject.SetActive(true);
        indexEstacion = -1;
	}

	public void ClearAll(){

        estaciones = new List<Estacion>();

        foreach (Estacion e in this.GetComponentsInChildren<Estacion>())
            e.ClearCasa();

        juego = null;

    }

    public void SiguienteEstacion(bool salidaForzada = false){

        if (estaciones != null)
            nextStation = StartCoroutine(NextStation(salidaForzada));


    }

    public IEnumerator NextStation(bool salidaForzada){
        
        indexEstacion++;



//        if (indexEstacion < estaciones.Count)
//            estaciones[indexEstacion].casita.GetComponent<Button>().interactable = true;


        if (indexEstacion > 0 && !salidaForzada){
            globos.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            globos.transform.GetChild(0).GetChild(indexEstacion - 1).GetChild(0).gameObject.SetActive(true);

            Debug.Log(indexEstacion + ", " + estaciones.Count);

            if (indexEstacion < estaciones.Count - 1)
                CAudioManager.Instance.FinTest();
            else
                CAudioManager.Instance.UltimoGloboSesion();
            yield return StartCoroutine(CAudioManager.Instance.Esperar());
            globos.SetActive(false);
        }

        foreach (Estacion e in estaciones)
            e.casita.GetComponent<Button>().interactable = false;

        CAudioManager.Instance.PrenderMusica();

        player.GetComponent<Jugador>().Continuar();

    }

    public void QuitarTarea(Estacion estacion){
        estaciones.Remove(estacion);
        CheckComenzar();

        juego = null;
        CrearTablero.SetActive(true);
        DropdownLabel.enabled = false;
    }

    public void AgregarTarea(Task task){
        AddTask(task._code);
        editEstacion.setTarea(task);
        //estaciones = estaciones.OrderBy(go => go.name).ToList();
        CheckComenzar();
        gameObject.SetActive(true);

        juego = null;
        CrearTablero.SetActive(true);
        DropdownLabel.enabled = false;
    }

    public void AddTask(string tarea)
    {
        switch (tarea)
        {
            case "isolationS":
                editEstacion.setTarea(conf.llamarAislamientoS);
                break;
            case "isolationP":
                editEstacion.setTarea(conf.llamarAislamientoF);
                break;
            case "blendingS":
                editEstacion.setTarea(conf.llamarSintesisS);
                break;
            case "blendingP":
                editEstacion.setTarea(conf.llamarSintesisF);
                break;
            case "segmentationS":
                editEstacion.setTarea(conf.llamarSegmentacionS);
                break;
            case "segmentationP":
                editEstacion.setTarea(conf.llamarSegmentacionF);
                break;
            case "rhymeW":
                editEstacion.setTarea(conf.llamarRimaW);
                break;
            case "rhymePW":
                editEstacion.setTarea(conf.llamarRimaPW);
                break;
            case "lettersN":
                editEstacion.setTarea(conf.llamarLetterKnowledgeN);
                break;
            case "lettersS":
                editEstacion.setTarea(conf.llamarLetterKnowledgeS);
                break;
            case "WIPPSI":
                editEstacion.setTarea(conf.llamarIQ);
                break;
            case "vocabularyPPVT":
                editEstacion.setTarea(conf.llamarVocabularioPPVT);
                break;
            case "tapping":
                editEstacion.setTarea(conf.llamarTapping);
                break;
            case "STMorder":
                editEstacion.setTarea(conf.llamarSTMorden);
                break;
            case "STMspatial":
                editEstacion.setTarea(conf.llamarSTMsimon);
                break;
            case "RAN":
                editEstacion.setTarea(conf.llamarRAN);
                break;
            case "ReadSS":
                editEstacion.setTarea(conf.llamalLecturaSS);
                break;
            case "ReadPRO":
                editEstacion.setTarea(conf.llamalLecturaPRO);
                break;
            case "DychSyll":
                editEstacion.setTarea(conf.llamarDichListSyl);
                break;
            case "DychMus":
                editEstacion.setTarea(conf.llamarDichListMus);
                break;
            case "visE":
                editEstacion.setTarea(conf.llamarVisEnt);
                break;
            case "visS":
                editEstacion.setTarea(conf.llamarVisSeg);
                break;
            case "vocabulary":
                editEstacion.setTarea(conf.llamarVocabulario);
                break;
            case "comprension":
                editEstacion.setTarea(conf.llamarComprension);
                break;
            case "BCBL":
                editEstacion.setTarea(conf.llamaBCBL);
                break;
            default:
                Debug.LogError("Cualquiera");
                break;

        }

    }

    public void CheckComenzar(){
        comenzarJuego.interactable = estaciones.Count >= MINESTACIONES;
    }

    /*
    public void JuegoNuevo(){
        if (juegos.value != 0){
            juegos.value = 0;
            juegos.RefreshShownValue();
        }
    }
    */
    public void SkipPasswordAndExit() // pontura: salir automaticamente si solo querías grabar el tablero
    {
        checkPassword.SkipPasswordAndExit();
    }
    public void EndEdit(){
        if (juego != null)
            StartGame();
        else{
            panelGuardado.SetActive(true);
            panelGuardado.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void SaveJuego(bool saveAndExit = false){

        juego = ScriptableObject.CreateInstance<CJuego>();

        string nombre = nombreJuego.text;
        if (nombre == "")
            nombre = "Sin nombre";

        juego.nombre = nombre;
        juego.tareas = new Task[6];
        foreach (Estacion e in estaciones)
        {
            juego.tareas[e.index] = e.task;
        }
        juego.Save();

        if(saveAndExit)
            SkipPasswordAndExit();
        else
            StartGame();

    }

    public void StartGamePMD(){
        
        comenzarJuego.gameObject.SetActive(false);
        juegos.gameObject.SetActive(false);
        panelPrevia.SetActive(false);
        cabezal.SetActive(false);

        foreach (Estacion e in this.GetComponentsInChildren<Estacion>())
            e.setGameMode();

        foreach (Transform child in globos.transform.GetChild(0))
        {
            child.gameObject.SetActive(false);
            child.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < estaciones.Count; i++)
            globos.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

        if (juego != null)
            Main.m.s.SetUltimoJuego(juego.nombre);

        StartCoroutine(StartGameCorr());
    }

    public void StartGame(){

        panelPrevia.SetActive(true);

    }

    public IEnumerator StartGameCorr(){

        intro.SetActive(true);

        yield return StartCoroutine(intro.GetComponent<Cintro>().darlePlayIntro());

        player.SetActive(true);
        SiguienteEstacion();



    }

    public void EndGame(){

        final.SetActive(true);
        AudioSource audioSource = final.GetComponentInChildren<AudioSource>();
        audioSource.Play();
        estaciones = null;
        j.estaciones = null;
    }



    public void LoadJuegos()
    {

        juegos.options = new List<Dropdown.OptionData>();
        List<CJuego> listaJuegosFinal = new List<CJuego>();

        CrearTablero = juegos.transform.GetChild(0).gameObject;
        DropdownLabel = juegos.transform.GetChild(1).GetComponent<Text>();

        CJuego[] listaJuegos = Resources.LoadAll<CJuego>("Juegos");

        juegos.options.Add(new Dropdown.OptionData("---"));

        foreach (CJuego _juego in listaJuegos)
        {
            juegos.options.Add(new Dropdown.OptionData(_juego.nombre));
            listaJuegosFinal.Add(_juego);
        }
        int cantDefault = listaJuegosFinal.Count;

        List<CJuego> listaJuegosDisk = CJuego.LoadJuegosFromDisk();
        foreach (CJuego _juego in listaJuegosDisk)
        {
            juegos.options.Add(new Dropdown.OptionData(_juego.nombre));
            listaJuegosFinal.Add(_juego);
        }
        juegos.value = 0;
        RectTransform template = juegos.transform.GetChild(3).GetComponent<RectTransform>();
        template.sizeDelta = new Vector2(template.sizeDelta.x, (juegos.options.Count) * 50);
        RectTransform content = juegos.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, 50);

        juegos.onValueChanged.RemoveAllListeners();
        juegos.onValueChanged.AddListener(delegate
            {
                int juegosValue = juegos.value - 1;
                if (juegosValue < 0) return;
            Debug.Log(juegosValue);
            LoadJuego(listaJuegosFinal[juegosValue]);
            borrarTablero.gameObject.SetActive(juegosValue >= cantDefault);
            if (juegosValue >= cantDefault){
                borrarTablero.onClick.RemoveAllListeners();
                borrarTablero.onClick.AddListener(() => {
                    Debug.Log("Borrando " + listaJuegosFinal[juegosValue].nombre);
                    listaJuegosFinal[juegosValue].Delete();
                    LoadJuegos();
                });
            }
            });

        CrearTablero.SetActive(true);
        borrarTablero.gameObject.SetActive(false);

    }



    public void LoadJuego(CJuego _juego){

        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<Estacion>().ClearCasa();
            if (_juego.tareas[i] != null)
            {
                editEstacion = transform.GetChild(0).GetChild(i).GetComponent<Estacion>();
                AgregarTarea(_juego.tareas[i]);
            }
        }

        juego = _juego;
        CrearTablero.SetActive(false);
        DropdownLabel.enabled = true;

    }


    public void SalirDelJuego(){
        this.gameObject.SetActive(false);
        estaciones = null;
        CAudioManager.Instance.ApagarMusica();
        CCanvasManager.Instance.IrAMain();

    }



}
