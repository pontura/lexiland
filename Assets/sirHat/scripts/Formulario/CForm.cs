using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class CForm : MonoBehaviour {

    public static CForm Inst;
    public GameObject form;
    public GameObject ttPanel;
    public GameObject comenzarButton;
    public GameObject detenerButton;
    public Text Titulo;
    public Text guardarButton;
    public GameObject guardando;
    //public Text timerText;
    int countForm;
    int ind;
    float timer;
    float t_n;
    bool timerStopped;
    List<int> _completado;
    List<int> _indexes;
    string _subtarea;
    int trialPos;
    
    public enum answerTypes { correcto, incorrecto, lexicalization, otra };

    // Use this for initialization
    void Awake() {
        Inst = this;
    }

    public void setForm(string _subTask, bool edit)
    {
        _subtarea = _subTask;
        // Armado del formulario

        CLogManager.Instance.IncrementarTaskPos();
        
        Titulo.text = CTarea.Inst._name;

        // Seteos de canvas e inicialización de los index a correr
        ttPanel.SetActive(false);
        comenzarButton.SetActive(true);
        detenerButton.SetActive(false);
        form.GetComponent<CanvasGroup>().interactable = false;

        // Borrado del formulario y de los clips de audio
        foreach (Transform child in form.transform)
            Destroy(child.gameObject);

        _indexes = new List<int>();
        // Instanciado de entradas del formulario
        Dictionary<string, List<string>> input = CTarea.Inst._estimulos;
        for (countForm = 0; countForm < input["task"].Count; countForm++)
        if (_subTask == null || input["subTask"][countForm] == _subTask)
            {
                GameObject f = Instantiate(Resources.Load("prefabs/formEntry") as GameObject);
                f.transform.SetParent(form.transform, false);
                if (input.ContainsKey("oracion"))
                {
                    f.transform.GetChild(0).GetComponent<Text>().text = input["oracion"][countForm];
                    f.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(380.0f, f.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
                }
                else
                    f.transform.GetChild(0).GetComponent<Text>().text = input["target"][countForm];

                //f.transform.GetChild(1).GetComponent<CPlayStop>()._target = input["trialCode"][countForm];

                f.transform.GetComponent<CFormEntry>().setButtons(countForm, _subtarea);
                _indexes.Add(countForm);

            }

        // Inicialización del scrollrect
        this.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        
        _completado = new List<int>();
        trialPos = 0;
        guardarButton.text = "Salir";
        guardando.SetActive(false);

    }

    public void startTest()
    {
        timer = Time.time;
        guardarButton.text = "Guardar";
        
        
        form.GetComponent<CanvasGroup>().interactable = true;

        if (CTarea.Inst._mode._recAudio)
        {
            //Comienza la grabación
            CAudioManager.StartRecord();
        }

        

    }

/*    public void Update()
    {
        // Sólo para el timer
        if (ttPanel.activeSelf && !timerStopped)
        {
            int seconds = Mathf.RoundToInt(Time.time - timer);
            int minutes = seconds / 60;
            seconds = seconds % 60;
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }
*/

/*    public void stopTimer()
    {
        timerStopped = true;
        t_n = (Time.time - timer);
        // Habilitación del formulario
        form.GetComponent<CanvasGroup>().interactable = true;
    }
*/
    

    public void setRespuesta(int _index, CTarea.acierto acierto)
    {
        trialPos++;
        // Si estoy rellenando 'en vivo'
        if (!_completado.Contains(_index))
        {
            CTarea.Inst.agregarRespuesta();
            _completado.Add(_index);
            
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1].Add("index", _index.ToString());

            // Timer
            float timer_n = Time.time;
            t_n = (timer_n - timer);
            
            if (CTarea.Inst._mode._rt != RT_type.no)
            {
                ind = _indexes.FindIndex(a => a.Equals(_index));
                form.transform.GetChild(ind).GetComponent<CFormEntry>().timer.transform.GetChild(1).GetComponent<Text>().text = t_n.ToString();
                CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["RT"] = t_n.ToString();
            }



            if (CTarea.Inst._mode._recAudio)
            {
                // V2018
                //string _name = CLogManager.Instance._textoID + "_" + CTarea.Inst._code + "_" + CTarea.Inst._timeStamp + "_" + CTarea.Inst._estimulos["trialCode"][_index];
                string _name = PlayerPrefs.GetString("SubjectID") + "_" + CTarea.Inst._code + "_" + CTarea.Inst._timeStamp + "_" + CTarea.Inst._estimulos["trialCode"][_index];
                // Paro y guardo la grabación
                CAudioManager.GuardarRecord(_name);
                // Seteo de output audioPath
                CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["audioPath"] = _name + ".wav";
            }

            if (CTarea.Inst._mode._rt == RT_type.item || (CTarea.Inst._estimulos["trialType"][_index]=="1"))
            {
                // Cronómetro de item
                timer = timer_n;
            }



            if (CTarea.Inst._mode._recAudio) CAudioManager.StartRecord();

            // Seteo del taskPos
            CTarea.Inst._respuestas[CTarea.Inst._respuestas.Count - 1]["taskPos"] = CLogManager.Instance._taskPos.ToString();


        }

        int indux = CTarea.Inst._respuestas.FindIndex(a=> (int.Parse(a["index"]) == _index) );

        // Seteo del trialPos
        CTarea.Inst._respuestas[indux]["trialPos"] = trialPos.ToString();


        switch (acierto)
        {
            case CTarea.acierto.correcto:
                CTarea.Inst._respuestas[indux]["acierto"] = "true";
                if (CTarea.Inst._mode._ansType == ANS_type.CIDLuM)
                    CTarea.Inst._respuestas[indux]["lexicalization/unMedio"] = "NA";
                break;
            case CTarea.acierto.noContesta:
                CTarea.Inst._respuestas[indux]["acierto"] = "NC";
                break;
            case CTarea.acierto.duda:
                CTarea.Inst._respuestas[indux]["acierto"] = "NA";
                if (CTarea.Inst._mode._ansType == ANS_type.CIDLuM)
                    CTarea.Inst._respuestas[indux]["lexicalization/unMedio"] = "NA";
                break;
            case CTarea.acierto.incorrecto:
                CTarea.Inst._respuestas[indux]["acierto"] = "false";
                if (CTarea.Inst._mode._ansType == ANS_type.CIDLuM)
                    CTarea.Inst._respuestas[indux]["lexicalization/unMedio"] = "NA";
                break;
            case CTarea.acierto.unMedio:
                CTarea.Inst._respuestas[indux]["acierto"] = "false";
                if (CTarea.Inst._mode._ansType == ANS_type.CIDLuM)
                    CTarea.Inst._respuestas[indux]["lexicalization/unMedio"] = "true";
                break;
            case CTarea.acierto.lexicalization:
                CTarea.Inst._respuestas[indux]["acierto"] = "false";
                if (CTarea.Inst._mode._ansType == ANS_type.CIDLuM)
                    CTarea.Inst._respuestas[indux]["lexicalization/unMedio"] = "true";
                break;
        }

        debugLogDiccionario(CTarea.Inst._respuestas);

        // V2018
        //CTarea.Inst.guardarOutput();
        CTarea.Inst.saveXML();

        //Debug.Log(_completado.Count + ", " + _indexes.Count);
        if (_completado.Count == 8)
        {
            CTarea.Inst.marcarTarea(CTarea.Inst._code);
        }

    }

    public void guardar()
    {
        StartCoroutine(saveAudiosCorr());

        // Se saca la condicion de trialPos > 0
        //if (CTarea.Inst._subtareas != null & trialPos > 0)

        if (CTarea.Inst._subtareas != null)
        {
            int indSt = CTarea.Inst._subtareas.FindIndex(a => a.Equals(_subtarea));
            if (indSt != CTarea.Inst._subtareas.Count - 1)
                CManager.Inst.irASubtarea(CTarea.Inst._subtareas[indSt + 1]);
            else
            {
                this.gameObject.SetActive(false);
                if (Juego.j != null)
                    Juego.j.SiguienteEstacion();
                else
                    CManager.Inst.panelPostTarea.SetActive(true);

            }
        }
        // V2018
        /*
        else{
            if (CTarea.Inst._code != "ReadSEM")
                CManager.Inst.irAmenu();
            else
                CManager.Inst.irAtarea(Resources.Load<CTareaData>("ReadSEM/ReadSEM_MO"));            
        }
        */

    }

    public IEnumerator apagarGuardando()
    {
        yield return new WaitForSeconds(3.0f);
        guardando.SetActive(false);
    }

    public IEnumerator saveAudiosCorr()
    {
        if (trialPos > 0 && CTarea.Inst._mode._recAudio)
        {
            guardando.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            CTarea.Inst.saveAudios();
            yield return new WaitForSeconds(0.5f);
            //apagarGuardando();
            guardando.SetActive(false);
        }


    }

    public void debugLogDiccionario(List<Dictionary<string,string>> _dic)
    {
        string aux="";

        foreach (var entrada in _dic[0])
            aux += entrada.Key + "\t";
        aux += "\n";

        foreach (var line in _dic)
        {
            foreach(var key in line.Keys)
                aux += line[key] + "\t";
            aux += "\n";
        }
        Debug.Log(aux);

    }


}
