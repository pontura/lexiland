using UnityEngine;
using System.Collections;

public class Cintro : MonoBehaviour {

    public GameObject _alex, _plus1, _plus2, _casa, _globo, _globos;
    public AudioSource _audio;
    public float _tAlex, _tCasas, _tGlobo, _tGlobos;


    // Use this for initialization
    void Start()
    {
        //StartCoroutine(darlePlayIntro());
    }



    // Use this for initialization
    void inicializacion()
    {
        _alex.SetActive(false);
        _plus1.SetActive(false);
        _plus2.SetActive(false);
        _casa.SetActive(false);
        _globo.SetActive(false);
        _globos.SetActive(false);
        //_tAlex   = 1f;
        //_tCasas  = 3f;
        //_tGlobo  = 1f;
        //_tGlobos = 1f;
    }


    public IEnumerator darlePlayIntro()
    {
        inicializacion();
        // Toda la animacion
        _audio.Play();
        yield return new WaitForSeconds(_tAlex);
        _alex.SetActive(true);
        yield return new WaitForSeconds(_tCasas);
        _plus1.SetActive(true);
        _casa.SetActive(true);
        yield return new WaitForSeconds(_tGlobo);
        _plus2.SetActive(true);
        _globo.SetActive(true);
        yield return new WaitForSeconds(_tGlobos);
        _globos.SetActive(true);
        while (_audio.isPlaying)
        {
            yield return null;
        }

        // V2018
        // CGameManager.Instance.llamarTablero();
        this.gameObject.SetActive(false);
        yield break;
    }

    public void Salir(){
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
        
        
        
         
}
