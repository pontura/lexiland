﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Registro : MonoBehaviour {

    public InputField nombre;
    public InputField apellido;
    public InputField escuela;
    public InputField custom;
    public InputField alumnoID;

    public Dropdown genero;
    public Dropdown curso;
    public Dropdown escolaridad;
    public Paroxe.SuperCalendar.Calendar fechaNacimiento;
    public GameObject camposFaltantes;
    Text calendarPlaceholder;
    Text calendarText;

    bool enableRegister;

	private void Awake()
	{
        //fechaNacimiento.OnDaySelected();

        calendarText = fechaNacimiento.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        calendarPlaceholder = fechaNacimiento.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Text>();
	}

	public void OnEnable()
	{
        ClearForm();
	}

    public void CheckRegister(){

        //Debug.Log("Chequeando");

        calendarPlaceholder.enabled = fechaNacimiento.SelectionState != Paroxe.SuperCalendar.Calendar.SelectionStateType.DaySelected;
        calendarText.enabled = fechaNacimiento.SelectionState == Paroxe.SuperCalendar.Calendar.SelectionStateType.DaySelected;

        bool checkNombre = nombre.text != "";
        nombre.gameObject.transform.GetChild(nombre.gameObject.transform.childCount - 1).gameObject.SetActive(checkNombre);

        bool checkApellido = apellido.text != "";
        apellido.gameObject.transform.GetChild(apellido.gameObject.transform.childCount - 1).gameObject.SetActive(checkApellido);

        bool checkEscuela = escuela.text != "";
        escuela.gameObject.transform.GetChild(escuela.gameObject.transform.childCount - 1).gameObject.SetActive(checkEscuela);

        bool checkGenero = genero.value != 0;
        genero.gameObject.transform.GetChild(0).gameObject.SetActive(checkGenero);
        bool checkEscolaridad = escolaridad.value != 0;
        escolaridad.gameObject.transform.GetChild(0).gameObject.SetActive(checkEscolaridad);
        bool checkDOB = fechaNacimiento.SelectionState == Paroxe.SuperCalendar.Calendar.SelectionStateType.DaySelected;
        fechaNacimiento.gameObject.transform.GetChild(fechaNacimiento.gameObject.transform.childCount - 1).gameObject.SetActive(checkDOB);

        enableRegister = (checkNombre && checkApellido && checkEscuela && checkGenero && checkEscolaridad && checkDOB);

        if (enableRegister)
            camposFaltantes.SetActive(false);


    }

    public void ClearForm(){

        fechaNacimiento.onDateTimeChanged.RemoveAllListeners();

        nombre.text = "";
        apellido.text = "";
        escuela.text = PlayerPrefs.GetString("escuela");
        custom.text = "";
        alumnoID.text = "";
        curso.value = 0;
        genero.value = 0;
        escolaridad.value = 0;
        fechaNacimiento.SelectionState = Paroxe.SuperCalendar.Calendar.SelectionStateType.None;
        fechaNacimiento.GoTop();

        calendarPlaceholder.enabled = true;
        calendarText.enabled = false;


        CheckRegister();

        camposFaltantes.SetActive(false);

        fechaNacimiento.onDateTimeChanged.AddListener(() => CheckRegister());

    }

    public void AttempToRegister(){

        if (enableRegister){

            Sujeto sujeto = new Sujeto
            {
                nombre = nombre.text,
                apellido = apellido.text,
                escuela = escuela.text,
                custom = custom.text,
                alumno_id = int.Parse(alumnoID.text),
                curso = (cursos)(curso.value - 1),
                teacher_id = DatabaseManager.Instance.GetData().id.ToString(),
                bornDate = fechaNacimiento.DateTime,
                genero = (gender)(genero.value - 1),
                escolaridad = escolaridad.options[escolaridad.value].text,
                usuarioID = PlayerPrefs.GetString("UserID")
            };

            sujeto.Save();

            this.gameObject.SetActive(false);
            Main.m.Initiate(sujeto);
        }
        else{
            camposFaltantes.SetActive(true);
        }


    }


}
