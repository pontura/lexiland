using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CmanejoDeGlobos : MonoBehaviour {

    public Image[] _globos4, _globos5;
    private Image[] _globos;
	public Sprite _globoAOn, _globoAOff,_globoBOn, _globoBOff,_globoCOn, _globoCOff,_globoDOn, _globoDOff;
	private int  _contIntraSesion, _cantMaxGlobos;
	private Sprite _globoOn, _globoOff;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {	
	}

	public void SetearTablero(string _tablero){
        _cantMaxGlobos = 4;
        if (_tablero == "A1" || _tablero == "A2" || _tablero == "A3" || _tablero == "A4" || _tablero == "A5")
        {
            _globoOn = _globoAOn;
            _globoOff = _globoAOff;
            _cantMaxGlobos = 4;
        }
        else if (_tablero == "B1" || _tablero == "B2" || _tablero == "B3" || _tablero == "B4" || _tablero == "B5")
        {
            _globoOn = _globoBOn;
            _globoOff = _globoBOff;
            _cantMaxGlobos = 5;
        }
        else if (_tablero == "C1" || _tablero == "C2" || _tablero == "C3" || _tablero == "C4" || _tablero == "C5")
        {
            _globoOn = _globoCOn;
            _globoOff = _globoCOff;
            _cantMaxGlobos = 4;
        }
        else 
        {
            _globoOn = _globoDOn;
            _globoOff = _globoDOff;
        }
        // --- Según cantidad de globos
        if(_cantMaxGlobos == 5)
        {
            _globos = _globos5;
            for (int _i = 0; _i < _globos4.Length; _i++)
            {
                _globos4[_i].gameObject.SetActive(false);                
            }
        }
        else 
        {
            _globos = _globos4;
            for (int _i = 0; _i < _globos5.Length; _i++)
            {
                _globos5[_i].gameObject.SetActive(false);
            }
        }
        
		_contIntraSesion = 0;
		for (int _i=0; _i<_globos.Length; _i++) {
			if(_i<_cantMaxGlobos){
				_globos [_i].gameObject.SetActive (true);
                _globos[_i].sprite = _globoOff;
            }
            else{
				_globos [_i].gameObject.SetActive (false);
			}
		}
	}


    public IEnumerator aumentarGlobo()
    {
		if (_contIntraSesion < _cantMaxGlobos-1)
        {
            CAudioManager.Instance.FinTest();
        }
        else
        {
            CAudioManager.Instance.UltimoGloboSesion();
        }        
        CAudioManager.Instance.PrenderMusica();
        yield return new WaitForSeconds(1f);
		_globos[_contIntraSesion].sprite = _globoOn;
        _contIntraSesion += 1;
        yield return StartCoroutine(CAudioManager.Instance.Esperar());
        this.gameObject.SetActive(false);
    }

   




}
