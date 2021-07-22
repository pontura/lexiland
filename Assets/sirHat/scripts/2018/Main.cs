using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main m;
    public InputField escuelaInput;

    public GameObject listaSujetos;
    public GameObject DatosSujeto;
    public GameObject agregar;
    public GameObject panelBorrado;
    public GameObject panelSubida;
    public GameObject botonSubida;
    public GameObject popUpSelect;

    Color azulcito = new Color(3.0f / 256.0f, 158.0f / 256.0f, 219.0f / 256.0f, 1.0f);
    string directory;

    public Sujeto s;

	public void Awake()
	{
        m = this;
	}

	public void OnEnable()
	{
        Debug.Log("Intentando subida de datos");
        SubidaDatos.sd.SubirArchivos(false);
	}

	public void Initiate(Sujeto sujeto = null)
	{
        escuelaInput.text = PlayerPrefs.GetString("escuela");
        panelBorrado.SetActive(false);
        popUpSelect.SetActive(false);
        //DatosSujeto.SetActive(false);
        directory = Application.persistentDataPath + "/Users/" + PlayerPrefs.GetString("UserID");
        UpdateListaSujetos(sujeto);
        //UpdateArchivosASubir();    
    }
    public void RefreshEscuela()
    {
        PlayerPrefs.SetString("escuela", escuelaInput.text);
    }

    public void SubirArchivos(){
        SubidaDatos.sd.SubirArchivos(true);
    }


	public void LogOut(){
        PlayerPrefs.SetString("UserID", "");
        CCanvasManager.Instance.IrALogin();
    }

    public void UpdateListaSujetos(Sujeto selectedSujeto = null){

        foreach (Transform child in listaSujetos.transform)
            if (child.tag == "caseEntry") Destroy(child.gameObject);
        PlayerPrefs.SetString("SubjectID", "");

        DirectoryInfo di = new DirectoryInfo(directory);

        FileInfo[] fi;

        foreach (DirectoryInfo d in di.GetDirectories().OrderByDescending(d => d.CreationTime)){
            fi = d.GetFiles("sujeto.xml");

            if (fi.Length != 0){
                Sujeto sujeto = Sujeto.Load(fi[0].FullName);
                GameObject f = Instantiate(Resources.Load("prefabs/caseEntry") as GameObject);
                f.transform.SetParent(listaSujetos.transform, false);
                f.transform.SetAsLastSibling();
                //yield return new WaitForEndOfFrame();
                f.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = sujeto.ID.Substring(4);
                f.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = sujeto.nombre;
                f.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = sujeto.apellido;
                f.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = sujeto.escuela;
                f.transform.GetChild(5).gameObject.SetActive(false);

                Button ver = f.transform.GetChild(5).GetComponent<Button>();
                ver.onClick.AddListener(() => DatosSujeto.transform.parent.gameObject.SetActive(true));


                foreach (Text t in f.GetComponentsInChildren<Text>())
                    t.color = Color.gray;

                f.GetComponent<Button>().onClick.AddListener(() => UpdateDatosSujeto(sujeto));

                f.GetComponent<Button>().onClick.AddListener(() =>
                {
            

                    foreach (Transform child in listaSujetos.transform)    
                        if (child.tag == "caseEntry") {
                            child.GetComponent<Image>().color = Color.white;
                            foreach (Text t in child.GetComponentsInChildren<Text>())
                                t.color = Color.gray;
                            child.transform.GetChild(0).GetComponent<Button>().enabled = false;
                            child.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                            child.transform.GetChild(5).gameObject.SetActive(false);
                        }

                    f.GetComponent<Image>().color = azulcito;
                    f.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    f.transform.GetChild(5).gameObject.SetActive(true);

                    Button botonBorrado = f.transform.GetChild(0).GetComponent<Button>();
                    botonBorrado.enabled = true;
                    botonBorrado.onClick.AddListener(() => {
                        panelBorrado.SetActive(true);
                        panelBorrado.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                        panelBorrado.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(()=> {
                            BorrarSujeto(d);
                            panelBorrado.SetActive(false);
                            UpdateListaSujetos();
                        });
                    } );

                    foreach (Text t in f.transform.GetComponentsInChildren<Text>())
                    t.color = Color.white;

                    PlayerPrefs.SetString("SubjectID", sujeto.ID);
                    s = sujeto;

            });

                if (selectedSujeto != null && sujeto.ID == selectedSujeto.ID)
                    f.GetComponent<Button>().onClick.Invoke();
            }
        }

        //yield return new WaitForEndOfFrame();
        //agregar.transform.SetAsLastSibling();
        //yield return new WaitForEndOfFrame();

    }

    public void UpdateDatosSujeto(Sujeto s){
        //Debug.Log(s.ID);
        //DatosSujeto.SetActive(true);
        DatosSujeto.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = s.nombre;
        DatosSujeto.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = s.apellido;
        DatosSujeto.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = s.escuela;
        DatosSujeto.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = s.bornDate.ToShortDateString();
        DatosSujeto.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<Text>().text = s.genero.ToString();
        DatosSujeto.transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<Text>().text = s.escolaridad;
        DatosSujeto.transform.GetChild(0).GetChild(6).GetChild(1).GetComponent<Text>().text = s.ultimoTablero;
        DatosSujeto.transform.GetChild(0).GetChild(7).GetChild(1).GetComponent<Text>().text = s.custom;
    }

    public void IrAtest(){
        if (PlayerPrefs.GetString("SubjectID") == "")
            popUpSelect.SetActive(true);
        else
            CCanvasManager.Instance.IrATareas(false);
    }

    public void IrAJuego(){
        if (PlayerPrefs.GetString("SubjectID") == "")
            popUpSelect.SetActive(true);
        else
            CCanvasManager.Instance.IrAJuego();   
    }

    public void BorrarSujeto(DirectoryInfo di){

        string erasePath = directory + "/borrados/";
        Directory.Move(di.FullName, erasePath + di.Name);

    }


}
