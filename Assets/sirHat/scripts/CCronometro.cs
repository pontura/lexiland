using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CCronometro : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
	}

    private bool _corriendo;
    private float _tCrono, _tini;
    public string _taskType, _taskSubType;
    public void EmpezarTerminar()
    {
        if (_corriendo)
        {
            // Terminar
            _corriendo = false;
            _tCrono = Time.time - _tini;
            _tini = 0;
            CLogManager.Instance.GuardarEstimuloRAN(_taskType, _taskSubType, _tCrono);
            this.GetComponentInChildren<Text>().text = "Comenzar";
        }
        else
        {
            // Empezar
            _corriendo = true;
            _tini = Time.time;
            this.GetComponentInChildren<Text>().text = "Terminar";

        }


    }

   



}
