using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class CFormOneEntry : MonoBehaviour {

    public static CFormOneEntry Inst;
    public GameObject comenzarButton;
    public GameObject ttPanel;
    public Text Titulo;
    public Text timerText;
    public bool timerStopped;
    Animation a;

    float timer;
    float t_n;
    

    // Use this for initialization
    void Awake() {
        Inst = this;
    }

    public void setForm()
    {
        CLogManager.Instance.IncrementarTaskPos();


        Titulo.text = CTarea.Inst._name;
        timerStopped = false;
        // Seteos de canvas e inicialización de los index a correr
        comenzarButton.SetActive(true);
        timerText.text = "0.0";
        ttPanel.SetActive(false);
        CTarea.Inst._respuestas.Add(new Dictionary<string, string>());
        CTarea.Inst._respuestas[0].Add("errores", "NA");
        CTarea.Inst._respuestas[0].Add("palabras", "NA");

        foreach (var child in this.GetComponentsInChildren<InputField>())
        {
            child.text = "";
        }

    }

    public void startTest()
    {
        timer = Time.time;

        ttPanel.SetActive(true);
        ttPanel.GetComponent<Image>().color = Color.white;

        a = ttPanel.GetComponent<Animation>();
        a.clip = a.GetClip("colorChangue");

    }

    public void Update()
    {
        // Sólo para el timer
        if (ttPanel.activeSelf && !timerStopped)
        {
            int seconds = Mathf.RoundToInt(Time.time - timer);
            int minutes = seconds / 60;
            seconds = seconds % 60;
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if ((Time.time - timer) > 2*60)
            {
                if (!a.isPlaying)
                {
                    //ttPanel.GetComponent<Animation>().clip = ttPanel.GetComponent<Animation>().GetClip("cronometro");
                    a.Play();
                }
            }

                if ((Time.time - timer) > 3*60)
            {
                timerStopped = true;
                ttPanel.GetComponent<Image>().color = Color.cyan;
                ttPanel.GetComponent<Animation>().Stop();
            }

        }
    }

    public void setErrores(InputField _ifield)
    {
        CTarea.Inst._respuestas[0]["errores"] = _ifield.text;
    }

    public void setCantPalabras(InputField _ifield)
    {
        CTarea.Inst._respuestas[0]["palabras"] = _ifield.text;
    }

    public void guardar()
    {
        CTarea.Inst._respuestas[0]["trialPos"] = "1";
        CTarea.Inst._respuestas[0]["taskPos"] = CLogManager.Instance._taskPos.ToString();
        CTarea.Inst._respuestas[0]["index"] = "0";
        CTarea.Inst.marcarTarea(CTarea.Inst._code);
        // V2018
        //CTarea.Inst.guardarOutput();
        //CManager.Inst.irAmenu();
        CTarea.Inst.saveXML();
        this.gameObject.SetActive(false);
        if (Juego.j != null)
            Juego.j.SiguienteEstacion();
        else
            CManager.Inst.panelPostTarea.SetActive(true);


    }

    public void salir(){
        this.gameObject.SetActive(false);
        if (Juego.j != null)
            Juego.j.SiguienteEstacion();
        else
            CManager.Inst.panelPostTarea.SetActive(true);

    }

    /*    public void debugLogDiccionario(List<Dictionary<string,string>> _dic)
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
    */

}
