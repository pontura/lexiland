using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaTareas : MonoBehaviour {

    public Transform listaTareas;
    public GameObject verBetas;
    public InputField passwordBetas;
    public GameObject panelPrevio;
    public GameObject panelPost;
    public Button irATareaButton;

    public CConfiguracion conf;

    /*
	public void Awake()
	{
        foreach (Transform child in listaTareas){
            child.GetChild(1).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            child.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(() => child.GetComponent<Task>().AgregarTarea());
        }
	}
	*/

	public void OnEnable()
	{
        UpdateList();
        this.GetComponentInChildren<ScrollRect>().horizontalNormalizedPosition = 0.0f;
        verBetas.SetActive(true);
        listaTareas.GetChild(1).gameObject.SetActive(false);
        passwordBetas.text = "";

        panelPrevio.SetActive(false);
        panelPost.SetActive(false);

	}

    /*
	public void SetMode(bool juego){

        foreach (Transform child in listaTareas){
            child.GetChild(1).GetChild(2).gameObject.SetActive(juego);
        }

    }
    */

    /*
    public void ActivateAll(){
        foreach (Transform child in listaTareas)
            child.gameObject.SetActive(true);
    }
    */

    public void UpdateList(){

        //Debug.Log(Juego.j.estaciones.Count);

        bool modoJuego = (Juego.j != null && Juego.j.estaciones != null);

        // Tareas alfa
        foreach (Transform child in listaTareas.GetChild(0)){
            // Se cambia el eliminado por el deshabilitado
            //child.gameObject.SetActive( ( Juego.j== null || Juego.j.estaciones == null) ||
            //                           (Juego.j.estaciones !=null && Juego.j.estaciones.FindIndex((Estacion e) => e.task._code == child.name) == -1));

            child.GetComponent<CanvasGroup>().interactable = (!modoJuego ||
                                       (Juego.j.estaciones.FindIndex((Estacion e) => e.task._code == child.name) == -1));


            child.GetChild(1).GetChild(2).gameObject.SetActive(modoJuego);
            child.GetChild(1).GetChild(1).gameObject.SetActive(!modoJuego);
        }

        // Tareas beta
        foreach (Transform child in listaTareas.GetChild(1))
        {
            // Se cambia el eliminado por el deshabilitado
            //child.gameObject.SetActive((Juego.j == null || Juego.j.estaciones == null) ||
            //                           (Juego.j.estaciones != null && Juego.j.estaciones.FindIndex((Estacion e) => e.task._code == child.name) == -1));

            child.GetComponent<CanvasGroup>().interactable = ((Juego.j == null || Juego.j.estaciones == null) ||
                                       (Juego.j.estaciones != null && Juego.j.estaciones.FindIndex((Estacion e) => e.task._code == child.name) == -1));


            child.GetChild(1).GetChild(2).gameObject.SetActive(Juego.j != null && Juego.j.estaciones != null);
            child.GetChild(1).GetChild(1).gameObject.SetActive(!(Juego.j != null && Juego.j.estaciones != null));
        }


    }


    public void verTareasBetas(){
        if (passwordBetas.text == "alex"){
            listaTareas.GetChild(1).gameObject.SetActive(true);
            verBetas.SetActive(false);
            StartCoroutine(acomodarScroll());
        }
    }

    public IEnumerator acomodarScroll(){
        yield return new WaitForEndOfFrame();
        float position = (float)listaTareas.GetChild(0).childCount / (float)(listaTareas.GetChild(0).childCount + listaTareas.GetChild(1).childCount);
        listaTareas.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = position;
        Debug.Log(position);
    }


    public void IrATarea(Task task)
    {
        panelPrevio.SetActive(true);

        irATareaButton.onClick.RemoveAllListeners();

        switch (task._code)
        {
            case "isolationS":
                irATareaButton.onClick.AddListener(conf.llamarAislamientoS);
                break;
            case "isolationP":
                irATareaButton.onClick.AddListener(conf.llamarAislamientoF);
                break;
            case "blendingS":
                irATareaButton.onClick.AddListener(conf.llamarSintesisS);
                break;
            case "blendingP":
                irATareaButton.onClick.AddListener(conf.llamarSintesisF);
                break;
            case "segmentationS":
                irATareaButton.onClick.AddListener(conf.llamarSegmentacionS);
                break;
            case "segmentationP":
                irATareaButton.onClick.AddListener(conf.llamarSegmentacionF);
                break;
            case "rhymeW":
                irATareaButton.onClick.AddListener(conf.llamarRimaW);
                break;
            case "rhymePW":
                irATareaButton.onClick.AddListener(conf.llamarRimaPW);
                break;
            case "lettersN":
                irATareaButton.onClick.AddListener(conf.llamarLetterKnowledgeN);
                break;
            case "lettersS":
                irATareaButton.onClick.AddListener(conf.llamarLetterKnowledgeS);
                break;
            case "WIPPSI":
                irATareaButton.onClick.AddListener(conf.llamarIQ);
                break;
            case "vocabularyPPVT":
                irATareaButton.onClick.AddListener(conf.llamarVocabularioPPVT);
                break;
            case "tapping":
                irATareaButton.onClick.AddListener(conf.llamarTapping);
                break;
            case "STMorder":
                irATareaButton.onClick.AddListener(conf.llamarSTMorden);
                break;
            case "STMspatial":
                irATareaButton.onClick.AddListener(conf.llamarSTMsimon);
                break;
            case "RAN":
                irATareaButton.onClick.AddListener(conf.llamarRAN);
                break;
            case "ReadSS":
                irATareaButton.onClick.AddListener(conf.llamalLecturaSS);
                break;
            case "ReadPRO":
                irATareaButton.onClick.AddListener(conf.llamalLecturaPRO);
                break;
            case "DychSyll":
                irATareaButton.onClick.AddListener(conf.llamarDichListSyl);
                break;
            case "DychMus":
                irATareaButton.onClick.AddListener(conf.llamarDichListMus);
                break;
            case "visE":
                irATareaButton.onClick.AddListener(conf.llamarVisEnt);
                break;
            case "visS":
                irATareaButton.onClick.AddListener(conf.llamarVisSeg);
                break;
            case "vocabulary":
                irATareaButton.onClick.AddListener(conf.llamarVocabulario);
                break;
            case "comprension":
                irATareaButton.onClick.AddListener(conf.llamarComprension);
                break;
            case "BCBL":
                irATareaButton.onClick.AddListener(conf.llamaBCBL);
                break;
            default:
                Debug.LogError("Cualquiera");
                break;

        }

        irATareaButton.onClick.AddListener(() =>
        {
            panelPrevio.SetActive(false);
        });

        if (task.material)
            irATareaButton.onClick.Invoke();

    }

}
