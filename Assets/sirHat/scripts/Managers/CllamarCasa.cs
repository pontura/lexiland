using UnityEngine;
using System.Collections;

public class CllamarCasa : MonoBehaviour {

	public CsaltarCasas _casa1, _casa2, _casa3, _casa4, _casa5, _casa6;
    public CTablero _tableroManager;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void llamarCasa1()
    {
        _casa1.empezarASaltar();
    }

    public void llamarCasa2()
    {
        _casa2.empezarASaltar();
    }
    public void llamarCasa3()
    {
        _casa3.empezarASaltar();
    }
    public void llamarCasa4()
    {
        _casa4.empezarASaltar();
    }
	public void llamarCasa5()
	{
		_casa5.empezarASaltar();
	}
	public void llamarCasa6()
	{
		_casa6.empezarASaltar();
	}

    public void llamarFin()
    {
		if (CGameManager.Instance._session == "3") {
			_tableroManager.pantallaFinFin();
		} else {
			_tableroManager.pantallaFin();
		}        
    }

}
