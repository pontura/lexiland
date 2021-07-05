using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRubyShared;

public class CVisual : MonoBehaviour {

    public static CVisual Inst;

    public GameObject palabraContenedor;
    public GameObject salirButton;
    public GameObject comenzarButton;
    public GameObject oracionContenedor;
    //public GameObject oracionPreviaContenedor;
    public GameObject fondo;
    public GameObject botonera;
    public GameObject panelPausa;
    int _index;
    List<int> _indexes;
    List<string> _estimulos;
    float timer;
    float t_n;
    string _subtarea;
    Dictionary<string, Sprite> _sprites;
    List<int> sumWrong;
    float tempTemp;
    float timeOut;
    float timeOffset;

    void Awake()
    {
        Inst = this;

        //SwipeDetector.OnSwipe += onSwipe;
        SwipeDetector.swipe += onSwipe;
    }

    public void setVisual(string _subTask)
    {
        CLogManager.Instance.IncrementarTaskPos();

        _subtarea = _subTask;
        _indexes = new List<int>();
        _sprites = new Dictionary<string, Sprite>();

        //this.GetComponent<FingerGestures>().enabled = false;
        //GetComponent<SwipeRecognizer>().enabled = false;
        //GetComponent<DigitalRubyShared.FingersScript>().enabled = false;
        GetComponent<SwipeDetector>().enabled = false;


        for (int countForm = 0; countForm < CTarea.Inst._estimulos["task"].Count; countForm++)
            if (_subTask == null || CTarea.Inst._estimulos["subTask"][countForm] == _subTask)
            {

                if (CTarea.Inst._mode._ansType == ANS_type.MO)
                {
                    for (int i=1; i<5; i++)
                    {
                        string name = "visual" + i.ToString();
                        if (CTarea.Inst._estimulos[name][countForm] != "NA" && !_sprites.ContainsKey(name))
                        {
                            Sprite s = Resources.Load<Sprite>(CTarea.Inst._code + "/visual/" + CTarea.Inst._estimulos[name][countForm]) as Sprite;
                            _sprites.Add(CTarea.Inst._estimulos[name][countForm], s);
                        }
                    }
                }

                _indexes.Add(countForm);
            }
        _index = -1;
        fondo.SetActive(true);
        if (CTarea.Inst._mode._ansType == ANS_type.MO)
            palabraContenedor.GetComponent<Text>().text = "Vamos a jugar...";
        else
            palabraContenedor.GetComponent<Text>().text = "Vamos a practicar...";
        palabraContenedor.transform.GetChild(0).gameObject.SetActive(false);
        palabraContenedor.transform.localPosition = new Vector3(0, 0, 0);

        oracionContenedor.SetActive(false);
        comenzarButton.SetActive(true);
        botonera.SetActive(false);
        //oracionPreviaContenedor.SetActive(false);
        panelPausa.SetActive(false);
        salirButton.SetActive(true);
        sumWrong = new List<int>();


        // Agregado para que comience automaticamente el readsem
        if (CTarea.Inst._code == "ReadSEM")
            comenzarButton.GetComponent<Button>().onClick.Invoke();
    }

    public void startTest()
    {
        timeOut = 0.0f;
        timeOffset = 0.0f;
        timer = Time.time;

        comenzarButton.SetActive(false);

        if (CTarea.Inst._mode._ansType != ANS_type.MO)
        {
            //GetComponent<FingerGestures>().enabled = true;
            //GetComponent<FingersScript>().enabled = true;
            GetComponent<SwipeDetector>().enabled = true;
            //GetComponent<SwipeRecognizer>().enabled = true;

            /*
            if (FingersScript.Instance.Gestures.Count == 0)
            {
                SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer();
                swipeGesture.Direction = SwipeGestureRecognizerDirection.Left;
                swipeGesture.StateUpdated += onSwipe;
                swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
                FingersScript.Instance.AddGesture(swipeGesture);
            }
            */

        }

        fondo.SetActive(false);
        if (CTarea.Inst._mode._ansType == ANS_type.MO)
        {
            botonera.SetActive(true);
            palabraContenedor.GetComponent<Text>().text = "";
            _index++;
            //displayOracion();
            displayStimulus();
        }
        if (CTarea.Inst._mode._ansType == ANS_type.no)
        {
            nextStimulus();
        }

    }

    public void salir()
    {
        panelPausa.SetActive(true);
        salirButton.SetActive(false);

        timeOut = Time.time;

        //if (_index>-1)

    }

    public void volver()
    {
        timeOffset += Time.time - timeOut;
        panelPausa.SetActive(false);
        salirButton.SetActive(true);

    }

    public void setTime()
    {
        CTarea.Inst.agregarRespuesta();
        Debug.Log(_indexes[_index]);
        // ULTIMO CAMBIO
        CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1].Add("index", _indexes[_index].ToString());

        float timer_n = (Time.time - timeOffset);

        if (CTarea.Inst._mode._rt == RT_type.item)
        {
            // Cronómetro
            t_n = (timer_n - timer);
            //Debug.Log(t_n);
        }


        if (CTarea.Inst._mode._rt != RT_type.no)
        {
            // Seteo del tiempo
            if (CTarea.Inst._mode._rt2)
            {
                CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["RT_2"] = t_n.ToString();
                CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["RT"] = tempTemp.ToString();
            }
            else
                CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["RT"] = t_n.ToString();

        }

        CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["trialPos"] = _index.ToString();
        CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["taskPos"] = CLogManager.Instance._taskPos.ToString();

        // V2018
        //CTarea.Inst.guardarOutput();
        CTarea.Inst.saveXML();

        debugLogDiccionario(CTarea.Inst._respuestas);

        timer = timer_n;
    }


    public void onSwipe(SwipeDetector.DraggedDirection direction)
    {
        Debug.Log(direction);
        if (direction == SwipeDetector.DraggedDirection.Left)
            if (CTarea.Inst._mode._ansType != ANS_type.MO) nextStimulus();
            else displayStimulus();
    }

    /*
    public void onSwipe(SwipeGesture gesture)
    {
        Debug.Log(gesture.Direction);
        if (gesture.Direction == FingerGestures.SwipeDirection.Left)
            if (CTarea.Inst._mode._ansType != ANS_type.MO) nextStimulus();
            else displayStimulus();
    }

    public void onSwipe(DigitalRubyShared.GestureRecognizer gesture)
    {
        //Debug.Log("Swipe papa " + gesture.State + " " + gesture.);

        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (CTarea.Inst._mode._ansType != ANS_type.MO) nextStimulus();
            else displayStimulus();
        }
    }
    */


    public void nextStimulus()
    {
            if (_index < _indexes.Count-1)
            {
                if (_index > -1)
                {
                    setTime();
                }
            /*            Animation a = palabraContenedor.GetComponent<Animation>();
                        a.clip = a.GetClip("swipeIn");
                        a.Play();
                        palabraContenedor.text = CTarea.Inst._estimulos["target"][_indexes[_index + 1]];
                        a.clip = a.GetClip("swipeOut");
                        a.Play();
            _index++;
            */
                StartCoroutine("changueText");
                if (_index == 8) CTarea.Inst.marcarTarea(CTarea.Inst._code);
            }
            else
            {
            comenzarButton.SetActive(true);
                
            // AGREGADO el setTime()
            if (_index > -1){
                setTime();
                //V2018
                // CTarea.Inst._file.Close();
            }
            if (CTarea.Inst._subtareas != null)
            {
                int indSt = CTarea.Inst._subtareas.FindIndex(a => a.Equals(_subtarea));

                if ((indSt < CTarea.Inst._subtareas.Count - 1))
                    CManager.Inst.irASubtarea(CTarea.Inst._subtareas[indSt + 1]);
                else{
                    // V2018
                    // CManager.Inst.irAmenu();

                    if (CTarea.Inst._code == "ReadBCBL-ch")
                        CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadBCBL/ReadBCBL2018"));
                    else{
                        this.gameObject.SetActive(false);
                        if (Juego.j != null)
                            Juego.j.SiguienteEstacion();
                        else
                            CManager.Inst.panelPostTarea.SetActive(true);

                    }

                }
            }
            else
            {
                // V2018
                //CManager.Inst.irAmenu();
                this.gameObject.SetActive(false);
                if (Juego.j != null)
                    Juego.j.SiguienteEstacion();
                else
                    CManager.Inst.panelPostTarea.SetActive(true);

            }

        }
    }

    public IEnumerator changueText()
    {
        Animation a = palabraContenedor.GetComponent<Animation>();
        a.clip = a.GetClip("swipeIn");
        a.Play();
        while (a.isPlaying)
            yield return null;
        if (_index>0 && CTarea.Inst._estimulos["trialType"][_indexes[_index]] == "1" && CTarea.Inst._estimulos["trialType"][_indexes[_index + 1]] == "2")
        {
            fondo.SetActive(true);
            palabraContenedor.GetComponent<Text>().text= "";
            palabraContenedor.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            palabraContenedor.GetComponent<Text>().text = CTarea.Inst._estimulos["visual1"][_indexes[_index + 1]];
            _index++;
        }
        a.clip = a.GetClip("swipeOut");
        a.Play();
        
    }

    public void repetir()
    {
        palabraContenedor.transform.GetChild(0).gameObject.SetActive(false);
        fondo.SetActive(false);
        _index = -1;
        timer = Time.time;
        StartCoroutine("changueText");
    }

    public void jugar()
    {
        palabraContenedor.transform.GetChild(0).gameObject.SetActive(false);
        fondo.SetActive(false);
        palabraContenedor.GetComponent<Text>().text = CTarea.Inst._estimulos["visual1"][_indexes[_index + 1]];
        timer = Time.time;
        _index++;
    }

/*    public void displayOracion()
    {
        botonera.SetActive(false);
        oracionContenedor.SetActive(false);
        oracionPreviaContenedor.SetActive(true);
        oracionPreviaContenedor.GetComponent<Text>().text = CTarea.Inst._estimulos["oracion"][_index];
        this.GetComponent<FingerGestures>().enabled = true;
    }
*/
    public void displayStimulus()
    {

        botonera.SetActive(true);

        //GetComponent<FingerGestures>().enabled = false;
        //GetComponent<SwipeRecognizer>().enabled = false;
        GetComponent<SwipeDetector>().enabled = false;

        //oracionPreviaContenedor.SetActive(false);
        oracionContenedor.SetActive(true);
        foreach (Transform child in botonera.transform)
        {
            Destroy(child.gameObject);
        }
        int cant = 0;
        oracionContenedor.GetComponent<Text>().text = CTarea.Inst._estimulos["oracion"][_index];
        for (int i=1; i<5; i++)
        {
            string visual = "visual" + i.ToString();
            if (CTarea.Inst._estimulos[visual][_index] != "NA")
            {
                cant++;
                GameObject g = Instantiate(Resources.Load("prefabs/Button") as GameObject);
                g.transform.SetParent(botonera.transform, false);
                string option = "visual"+ i.ToString();
                g.GetComponent<Button>().onClick.AddListener(() => setOption(option));
                //Debug.Log(CTarea.Inst._estimulos[visual][_index]);
                g.GetComponent<Image>().sprite = _sprites[CTarea.Inst._estimulos[visual][_index]];
            }
        }

        if (cant > 3)
        {
            botonera.GetComponent<GridLayoutGroup>().constraintCount = 2;
            botonera.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 200);
        }
        else
        {
            botonera.GetComponent<GridLayoutGroup>().constraintCount = 3;
            botonera.GetComponent<GridLayoutGroup>().cellSize = new Vector2(300, 130);
        }


        float timer_n = (Time.time - timeOffset);

        if (CTarea.Inst._mode._rt == RT_type.item)
        {
            // Cronómetro
            t_n = (timer_n - timer);
            //Debug.Log(t_n);
        }
        tempTemp = t_n;
        timer = timer_n;

    }

    public void setOption(string option)
    {
        Debug.Log(_indexes[_index]);

        setTime();
        if (CTarea.Inst._estimulos[option][_indexes[_index]] == CTarea.Inst._estimulos["target"][_indexes[_index]])
        {
            CTarea.Inst._respuestas[_indexes[_index]]["acierto"] = "true";
            sumWrong.Add(0);
            //Debug.Log("Correcto");
        }
        else
        {
            CTarea.Inst._respuestas[_indexes[_index]]["acierto"] = "false";
            sumWrong.Add(1);
            //Debug.Log("Incorrecto");
        }

        // V2018
        //CTarea.Inst.guardarOutput();

        CTarea.Inst.saveXML();

        _index++;

        int CantWrong=0;
        for (int i = sumWrong.Count - 1; i >= 0 && i > sumWrong.Count - 1 - 5; i--)
        {
            CantWrong += sumWrong[i];
            //Debug.Log(sumWrong[i]);
        }

        if (_index < _indexes.Count)// && CantWrong<4)
            //displayOracion();
            displayStimulus();
        else
        {

            // V2018
            //CTarea.Inst.marcarTarea(CTarea.Inst._code);
            //CManager.Inst.irAmenu();

            if (CTarea.Inst._code == "ReadSINT")
                CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSEM/ReadSEM_MO"));
            else
            {
                this.gameObject.SetActive(false);
                if (Juego.j != null)
                    Juego.j.SiguienteEstacion();
                else
                    CManager.Inst.panelPostTarea.SetActive(true);
            }

        }

        
    }

    public void debugLogDiccionario(List<Dictionary<string, string>> _dic)
    {
        string aux = "";

        foreach (var entrada in _dic[0])
            aux += entrada.Key + "\t";
        aux += "\n";

        foreach (var line in _dic)
        {
            foreach (var key in line.Keys)
                aux += line[key] + "\t";
            aux += "\n";
        }
        Debug.Log(aux);

    }


}
