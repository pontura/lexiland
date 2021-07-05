using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CTappingOld : MonoBehaviour {

    public AudioSource audiosource;
    public Image estimuloVisual;
    public AudioClip tumpac;
    float T;
    float dt = 0.0f;
    enum estado {test, idle};
    estado Estado;
    int cantTapsTestsAsistidos = 10;
    public Sprite tapping0, tapping1;
    public System.Diagnostics.Stopwatch sw;
    List<float> taps;
    public Text tessto;
    int n;

    // Use this for initialization
    void Start () {

        taps = new List<float>();

        // Cantidad de tests asistidos
        int cantTests = 3;
        int cantTapsTests = 20;
        bool eVisual = true;
        T = (int)(0.25f*tumpac.samples/tumpac.length);
        Estado = estado.test;
        audiosource.clip = tumpac;
        //sw = new System.Diagnostics.Stopwatch();
        //sw.Start();
        Debug.Log(T);
        n = 0;
        // Application.targetFrameRate = 100;
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (Estado == estado.test)
        {
            if (!audiosource.isPlaying)
                audiosource.Play();
            if (audiosource.timeSamples > n*T)
            {
                //Debug.Log(" entra " + (audiosource.time-dt));
                estimuloVisual.sprite = tapping1;
                //sw.Reset();
                //sw.Start();
                n++;
            }
            if (2*audiosource.timeSamples > (2*n-1)*T)
            {
                estimuloVisual.sprite = tapping0;
            }
                       // Debug.Log(audiosource.timeSamples % T);//Time.time - dt - T);

        }


    }

    public void tap()
    {
        taps.Add(Time.time);
        Debug.Log(Time.time);
        tessto.text = (Time.time - dt).ToString();
        dt = Time.time;
    }

    public IEnumerator tapImage()
    {
        estimuloVisual.sprite = tapping1;
        yield return new WaitForSeconds(0.1f);
        estimuloVisual.sprite = tapping0;
    }

    public IEnumerator CorrerTest()
    {
        inicializar();
        // Instrucciones y ejemplos       
        yield return new WaitForSeconds(1f);

        // Cantidad de tests asistidos
        int cantTestsAsistidos = 3;
        int cantTapsTestsAsistidos = 10;
        // Cantidad de tests asistidos
        int cantTests = 3;
        int cantTapsTests = 40;
        bool eVisual = true;
        T = 0.27777f;

        for (int i=0; i<cantTestsAsistidos; i++)
        {
            for (int j = 0; j < cantTapsTestsAsistidos; j++)
            {
                Invoke("tap", j * T);
            }
            // Reproducir Audio
            // Enablear Tambor
            // Enable Estímulo AD
            // Start Audio (con freq[i] y duracion[i])
            // Start Video (con freq[i] y duracion[i])
            // Guardar Log
        }

        //estimuloVisual.gameobject.SetActive(false);
        eVisual = false;

        for (int i = 0; i < cantTests; i++)
        {
            // Reproducir Audio
            // Enablear Tambor
            // Enable Estímulo A
            // Start Audio (con freq[i] y duracion[i])
            // Guardar Log
        }


        salirTest();
    }

    private void inicializar()
    {
        // Carga de audios
        // Carga de Imagenes
        // Carga de log
    }

    private void salirTest()
    {
        // Rompe todo
    }

    

}
