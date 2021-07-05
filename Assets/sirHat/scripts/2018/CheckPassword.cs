using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CheckPassword : MonoBehaviour {

    public InputField passwordField;
    public string password;

    public UnityEvent evento;

    public void CheckingPassword(){

        if (passwordField.text == password)
            evento.Invoke();

    }


}
