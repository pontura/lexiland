using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class Estacion : MonoBehaviour {

    public GameObject casita;
    public GameObject codeTarea;
    public GameObject agregarTarea;
    public GameObject eliminarTarea;
    public int index;

    GameObject tarea;

    public Task task;

    public void ClearCasa(){
        agregarTarea.SetActive(true);
        eliminarTarea.SetActive(false);
        casita.SetActive(false);
        codeTarea.SetActive(false);
        casita.GetComponent<Button>().onClick.RemoveAllListeners();

        if (tarea != null)
            tarea.SetActive(true);

        Juego.j.QuitarTarea(this);

    }

    public void IrATareas(){

        Juego.j.editEstacion = this;
        CCanvasManager.Instance.IrATareas(true);

    }

    /*
     public void setTarea(GameObject _tarea){
        tarea = _tarea;
        _tarea.SetActive(false);
    }
    */

    public void setTarea(Task _task){
        task = _task;
        codeTarea.GetComponentInChildren<Text>().text = task._name;
    }

    public void setTarea(UnityAction action){

        casita.GetComponent<Button>().onClick.RemoveAllListeners();
        casita.GetComponent<Button>().onClick.AddListener(action);
        casita.SetActive(true);
        casita.GetComponent<Button>().interactable = false;
        agregarTarea.SetActive(false);
        eliminarTarea.SetActive(true);
        codeTarea.SetActive(true);

        Juego.j.estaciones.Add(this);

    }

    public void setGameMode(){

        casita.SetActive(Juego.j.estaciones.Contains(this));

        codeTarea.SetActive(false);
        agregarTarea.SetActive(false);
        eliminarTarea.SetActive(false);

    }



}
