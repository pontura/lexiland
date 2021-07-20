using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SubidaDatos : MonoBehaviour {

    public static SubidaDatos sd;

    public GameObject panelGeneral;

    public GameObject panelPrevio;
    public GameObject panelSubida;
    public GameObject panelError;

    public Button botonSubida;

    public int cantCorrectos;

    List<string> listaArchivos;
    float totalSize;

	public void Awake()
	{
        Debug.Log("Awake");
        sd = this;
	}

    public void SubirArchivos(bool front)
    {

        panelGeneral.SetActive(front);
        panelError.SetActive(false);
        panelSubida.SetActive(false);
        panelPrevio.SetActive(false);

        StartCoroutine(DBServices.db.CheckInternetConnection((isConnected) => {
            Debug.Log("Checkeando conexion para subida de datos: isConnected:" + isConnected + " - front:" + front);
            //SubidaDatos.sd.panelError.SetActive(!isConnected);
            if (isConnected)
                UpdateArchivosASubir(front);

            panelError.SetActive(!isConnected);
        
        }));
    }

    public void UpdateArchivosASubir(bool front)
    {

        totalSize = 0;
        listaArchivos = new List<string>();

        string directorio = Application.persistentDataPath + "/Users/";// + PlayerPrefs.GetString("UserID");

        DirectoryInfo d = new DirectoryInfo(directorio);

        foreach (DirectoryInfo di in d.GetDirectories())
        {
            //            DirectoryInfo di = new DirectoryInfo(directorio);
            foreach (DirectoryInfo dinfo in di.GetDirectories())
            {

                string filename = dinfo.FullName + "/" + dinfo.Name + ".xml";
                if (File.Exists(filename))
                {
                    //Debug.Log(filename);
                    listaArchivos.Add(filename);
                    FileInfo fi = new FileInfo(filename);
                    totalSize += fi.Length;
                }


                if (Directory.Exists(dinfo.FullName + "/outputs"))
                {
                    DirectoryInfo dio = new DirectoryInfo(dinfo.FullName + "/outputs");
                    FileInfo[] fi = dio.GetFiles("*.xml");

                    foreach (FileInfo f in fi)
                    {
                        Debug.Log(f.FullName);
                        listaArchivos.Add(f.FullName);
                        FileInfo fio = new FileInfo(f.FullName);
                        totalSize += fio.Length;
                    }

                }

            }
        }

        Debug.Log("Hay " + listaArchivos.Count);

        panelPrevio.SetActive(front);
        if (listaArchivos.Count > 0)
                panelPrevio.transform.GetChild(0).GetComponent<Text>().text = "Se subirán " + listaArchivos.Count + " archivos.";
        else
                panelPrevio.transform.GetChild(0).GetComponent<Text>().text = "No hay archivos nuevos para subir";

        panelPrevio.transform.GetChild(1).gameObject.SetActive(listaArchivos.Count > 0);
        panelPrevio.transform.GetChild(2).gameObject.SetActive(listaArchivos.Count == 0);

        if (!front && listaArchivos.Count > 0)
            SubiendoDatos();
        
    }

    public void SubiendoDatos(){

        Debug.Log("Attemp to subir datos");
        panelPrevio.SetActive(false);
        StartCoroutine(SubidaDatosServidor());

    }

    public IEnumerator SubidaDatosServidor(){

        panelGeneral.SetActive(true);
        panelSubida.SetActive(true);
       
        cantCorrectos = 0;
        panelSubida.transform.GetChild(1).GetComponent<Image>().enabled = true;
        this.GetComponent<CanvasGroup>().interactable = false;
        yield return StartCoroutine(DBServices.db.UploadFiles(listaArchivos));
        this.GetComponent<CanvasGroup>().interactable = true;
        Debug.Log("Todo Subido");
        panelSubida.transform.GetChild(1).GetComponent<Image>().enabled = false;

        if (cantCorrectos > 0){
            panelSubida.transform.GetChild(0).GetComponent<Text>().text = "Se subieron " + cantCorrectos + "/" + listaArchivos.Count + " archivos correctamente.";
            if (cantCorrectos != listaArchivos.Count)
                panelSubida.transform.GetChild(0).GetComponent<Text>().text += "/nPrueba más tarde para subir los archivos restantes.";
        }
        else
            panelSubida.transform.GetChild(0).GetComponent<Text>().text = "No se pudieron subir los archivos.\nPrueba más tarde.";

    }

    public void Subiendo(int index, float size){

        Debug.Log("Subiendo archivo " + index + "/" + listaArchivos.Count);
        Debug.Log(size / totalSize);

        panelSubida.transform.GetChild(0).GetComponent<Text>().text = "Subiendo archivo " + index + "/" + listaArchivos.Count;
        panelSubida.transform.GetChild(1).GetComponent<Image>().fillAmount = size / totalSize;


    }

}
