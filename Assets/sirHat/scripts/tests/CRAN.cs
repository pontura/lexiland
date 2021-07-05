using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRAN : MonoBehaviour {

    public GameObject letras, numeros, colores, objetos;
    public GameObject cronometroLetras, cronometroNumeros, cronometroColores, cronometroObjetos;
    public GameObject timerButton;
    public Sprite faro, lupa, mesa, pato, queso;
    bool tranka = false;
    int trialPos = 0;
    bool letrasC = false;
    bool numerosC = false;
    bool coloresC = false;
    bool objetosC = false;

    List<int> indexesLetras, indexesNumeros, indexesColores, indexesObjetos;
    List<bool> aciertos;

    public GameObject panelInicio;
    public GameObject siguiente;
    public GameObject content;
    int indexContent;

    float timer;
    bool timeando;

	// Update is called once per frame
	void Update () {
		if (timerButton.activeSelf)
        {
            int seconds = Mathf.RoundToInt(Time.time - timer);
            int minutes = seconds / 60;
            seconds = seconds % 60;
            timerButton.transform.GetChild(0).GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
	}

    public void UpdateContent(){

        foreach (Transform child in content.transform){
            child.gameObject.SetActive(false);
        }

        if (indexContent > -1)
            content.transform.GetChild(indexContent).gameObject.SetActive(true);

        panelInicio.SetActive(indexContent == -1);
        siguiente.SetActive(indexContent > -1 && indexContent < content.transform.childCount - 1);

    }

    public void Next(){
        indexContent++;
        UpdateContent();
    }

	public void setForm()
    {
        CLogManager.Instance.IncrementarTaskPos();
        indexContent = -1;
        UpdateContent();

        timeando = false;
        timerButton.SetActive(false);

        //this.GetComponentInChildren<ScrollRect>().horizontalNormalizedPosition = 0.0f;

        int letrasCont = 0;
        int numerosCont = 0;
        int coloresCont = 0;
        int objetosCont = 0;
        letrasC = false;
        numerosC = false;
        coloresC = false;
        objetosC = false;
        indexesLetras = new List<int>();
        indexesNumeros = new List<int>();
        indexesColores = new List<int>();
        indexesObjetos = new List<int>();
        aciertos = new List<bool>();
        cronometroColores.SetActive(true);
        cronometroLetras.SetActive(true);
        cronometroNumeros.SetActive(true);
        cronometroObjetos.SetActive(true);

        for (int i = 0; i < CTarea.Inst._estimulos["task"].Count; i++)
        {
            int c = i;
            aciertos.Add(true);
            switch (CTarea.Inst._estimulos["subTask"][i])
            {
                case "Letras":
                    indexesLetras.Add(c);
                    int il = letrasCont;
                    letras.transform.GetChild(letrasCont).GetChild(0).GetComponent<Text>().text = CTarea.Inst._estimulos["target"][i];
                    letras.transform.GetChild(letrasCont).GetComponent<Button>().onClick.RemoveAllListeners();
                    letras.transform.GetChild(il).GetChild(1).gameObject.SetActive(false);
                    letras.transform.GetChild(letrasCont).GetComponent<Button>().onClick.AddListener(() => { setTrue(c);
                        letras.transform.GetChild(il).GetChild(1).gameObject.SetActive(!letras.transform.GetChild(il).GetChild(1).gameObject.activeSelf); });
                    letrasCont++;
                    break;
                case "Numeros":
                    indexesNumeros.Add(c);
                    int nc = numerosCont;
                    numeros.transform.GetChild(numerosCont).GetChild(0).GetComponent<Text>().text = CTarea.Inst._estimulos["target"][i];
                    numeros.transform.GetChild(numerosCont).GetComponent<Button>().onClick.RemoveAllListeners();
                    numeros.transform.GetChild(nc).GetChild(1).gameObject.SetActive(false);
                    numeros.transform.GetChild(numerosCont).GetComponent<Button>().onClick.AddListener(() => {
                        setTrue(c);
                        numeros.transform.GetChild(nc).GetChild(1).gameObject.SetActive(!numeros.transform.GetChild(nc).GetChild(1).gameObject.activeSelf);
                    });
                    numerosCont++;
                    break;
                case "Colores":
                    indexesColores.Add(c);
                    int cc = coloresCont;
                    colores.transform.GetChild(coloresCont).GetComponent<Image>().color = pintame(CTarea.Inst._estimulos["target"][i]);
                    colores.transform.GetChild(coloresCont).GetComponent<Button>().onClick.RemoveAllListeners();
                    colores.transform.GetChild(cc).GetChild(0).gameObject.SetActive(false);
                    colores.transform.GetChild(coloresCont).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        setTrue(c);
                        colores.transform.GetChild(cc).GetChild(0).gameObject.SetActive(!colores.transform.GetChild(cc).GetChild(0).gameObject.activeSelf);
                    });
                    coloresCont++;
                    break;
                case "Objetos":
                    indexesObjetos.Add(c);
                    int oc = objetosCont;
                    objetos.transform.GetChild(objetosCont).GetComponent<Image>().sprite= objetame(CTarea.Inst._estimulos["target"][i]);
                    objetos.transform.GetChild(objetosCont).GetComponent<Button>().onClick.RemoveAllListeners();
                    objetos.transform.GetChild(oc).GetChild(0).gameObject.SetActive(false);
                    objetos.transform.GetChild(objetosCont).GetComponent<Button>().onClick.AddListener(() => {
                        setTrue(c);
                        objetos.transform.GetChild(oc).GetChild(0).gameObject.SetActive(!objetos.transform.GetChild(oc).GetChild(0).gameObject.activeSelf);
                    });
                    objetosCont++;
                    break;
            }
        }


    }

    public void startTest()
    {
        timer = Time.time;
        tranka = true;
        timerButton.SetActive(true);

        siguiente.SetActive(false);
   }

    public void endTest(string subtask)
    {
        timer = Time.time - timer;
        save(subtask);
        tranka = false;
        timerButton.SetActive(false);

        siguiente.SetActive(true);

    }

    

    public void setTrue(int i)
    {
        aciertos[i] = !aciertos[i];
    }

    public void save(string subtask)
    {
        List<int> _indexes;
        _indexes = new List<int>();
        switch (subtask)
        {
            case "Letras":
                _indexes = indexesLetras;
                letrasC = true;
                break;
            case "Numeros":
                _indexes = indexesNumeros;
                numerosC = true;
                break;
            case "Colores":
                _indexes = indexesColores;
                coloresC = true;
                break;
            case "Objetos":
                _indexes = indexesObjetos;
                objetosC = true;
                break;
            
        }

        foreach (var index in _indexes)
        {
            CTarea.Inst.agregarRespuesta();
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["RT"] = timer.ToString();
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["acierto"] = aciertos[index].ToString();
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["trialPos"] = trialPos.ToString();
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["taskPos"] = CLogManager.Instance._taskPos.ToString();
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1].Add("index", index.ToString());

            // V2018
            // CTarea.Inst.guardarOutput();
        }

        // V2018
        CTarea.Inst.saveXML();

        trialPos++;
//        CTarea.Inst._file.Close();

    }

    public void salir(bool salidaForzada)
    {
        if (coloresC && letrasC && numerosC && objetosC)
            CTarea.Inst.marcarTarea(CTarea.Inst._code);
        // CManager.Inst.irAmenu();
        this.gameObject.SetActive(false);
        if (Juego.j != null)
            Juego.j.SiguienteEstacion(salidaForzada);
        else
            CManager.Inst.panelPostTarea.SetActive(!salidaForzada);

    }

    public Color pintame(string color)
    {
        switch (color)
        {
            case "AZUL":
                return Color.blue;
            case "NEGRO":
                return Color.black;
            case "BLANCO":
                return Color.white;
            case "ROJO":
                return Color.red;
            case "VERDE":
                return Color.green;
            default:
                return Color.white;
        }
    }

    public Sprite objetame(string objeto)
    {
        switch (objeto)
        {
            case "FARO":
                return faro;
            case "LUPA":
                return lupa;
            case "MESA":
                return mesa;
            case "PATO":
                return pato;
            case "QUESO":
                return queso;
            default:
                return null;
        }
    }
}
