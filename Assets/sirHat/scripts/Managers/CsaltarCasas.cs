using UnityEngine;
using System.Collections;

public class CsaltarCasas : MonoBehaviour {

    private RectTransform _rt;
    private float _xini, _yini;
    private float _yfin;
    private float overTime = 0.25f;


    // Use this for initialization
    void Start()
    {
        _rt = this.GetComponent<RectTransform>();
        _xini = _rt.anchoredPosition.x;
        _yini = _rt.anchoredPosition.y;
        _yfin = _yini + 20f;
    }


    public void empezarASaltar()
    {
        StartCoroutine(saltar());

    }

    private IEnumerator saltar()
    {

        float startTime;
        

        while (true) { 
            // Subida
            startTime = Time.time;
            while (Time.time < startTime + overTime)
            {
                _rt.anchoredPosition = new Vector2(_xini, Mathf.Lerp(_yini, _yfin, (Time.time - startTime) / overTime));
                yield return null;
            }
            // Bajada
            startTime = Time.time;
            while (Time.time < startTime + overTime)
            {
                _rt.anchoredPosition = new Vector2(_xini, Mathf.Lerp(_yfin, _yini, (Time.time - startTime) / overTime));
                yield return null;
            }
            // Esperamos un poquito abajo
            yield return new WaitForSeconds(2f);
        }
    }



    // Terminar con el saltito
    public void terminarDeSaltar()
    {
        StopAllCoroutines();
        _rt.anchoredPosition = new Vector2(_xini,_yini);

    }







}
