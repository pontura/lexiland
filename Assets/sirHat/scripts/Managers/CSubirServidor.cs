using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;


public class CSubirServidor : MonoBehaviour
{

	public Text mensaje;
	public Text mensaje2;
	public GameObject panel1;
    private string csv_file, csv_dir, audio_file, audio_dir;


    void Start(){

        // Path a los archivos CSV
#if UNITY_EDITOR
        csv_file = Application.dataPath + "/CSV/";
        csv_dir = Application.dataPath + "/CSV";
        audio_file = Application.persistentDataPath + "/Audios/";
        audio_dir = Application.persistentDataPath + "/Audios";
#elif UNITY_ANDROID
         csv_file = Application.persistentDataPath + "/";
         csv_dir = Application.persistentDataPath;
        audio_file = Application.persistentDataPath + "/Audios/";
        audio_dir = Application.persistentDataPath + "/Audios";
#elif UNITY_IPHONE
        path_file = Application.persistentDataPath + "/CSV/";
        path_dir = Application.persistentDataPath + "/CSV";
#else
        csv_file = Application.dataPath + "/CSV/";
        csv_dir = Application.dataPath + "/CSV";
#endif


    }


    // Volver al panel de Log In : la llama el botón de vovler.
    public void volverLogin()
    {
        this.gameObject.SetActive(false);
        mensaje.text = "Pronto para subir...";
        panel1.SetActive(true);
    }

    // SUBIDA DE ARCHIVOS AL SERVIDOR  -----------------------------------------------------------------------------------------------------------------
    public void SaveCSV(){
        StartCoroutine(SubirRutina(tipoArchivo.csv));

    }
    public void SaveAudio()
    {
        StartCoroutine(SubirRutina(tipoArchivo.audio));

    }

    enum tipoArchivo {audio, csv}

    private IEnumerator SubirRutina(tipoArchivo ta)
    {
        mensaje.text = "Subiendo...";
        yield return null;
        print("www created");
        DirectoryInfo dir;
        FileInfo[] info;
        if (ta == tipoArchivo.csv)
        {
            dir = new DirectoryInfo(csv_dir);
            info = dir.GetFiles("*.csv");
            Debug.Log("audios");
        }
        else
        {
            dir = new DirectoryInfo(audio_dir);
            info = dir.GetFiles("*.wav");
        }
        int cant = 0;
        bool exito = true;
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
            print("www creadsdsdted");
            byte[] data;
            if (ta == tipoArchivo.csv)
                data = File.ReadAllBytes(csv_file + f.Name);
            else
                data = File.ReadAllBytes(audio_file + f.Name);
            WWWForm form = new WWWForm();
            print("form created ");
            form.AddField("action", "level upload");
            form.AddField("file", "file");
            form.AddBinaryData("file", data, f.Name, "text/csv");
            print("binary data added ");
            WWW w = new WWW("http://www.tifon.psico.edu.uy/insert_datos.php", form);
            yield return w;
            Debug.Log(w.error);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                mensaje.text = "ERROR, no se pudo realizar la operación con éxito.";
                exito = false;
                break;
            }
            if (!String.IsNullOrEmpty(w.error))
            {
                mensaje.text = w.error + ", no se completo la operación con exito, la cantidad de archivos subida es: " + cant.ToString();
                exito = false;
                break;
            }
            if (ta == tipoArchivo.csv)
                File.Delete(csv_file + f.Name);
            else
                File.Delete(audio_file + f.Name);

            cant = cant + 1;
        }
        if (exito)
        {
            mensaje.text = "La cantidad de archivos subidos al servidor es: " + cant.ToString() + "\nSubidos todos los archivos con exito";
        }
//        mensaje.text = "Verificando...";
        yield return null;
        //StartCoroutine(GetInfo(ta));

        yield break;
    }



	// OBTENER INFORMACIÓN DE ARCHIVOS SUBIDOS EN EL SERVIDOR ---------------------------------------------------------------------------------
	public void getInfoServer(){
        mensaje.text = "Conectando...";
        StartCoroutine(GetInfo(tipoArchivo.csv));
	}

    public void getInfoServerAudios()
    {
        mensaje.text = "Conectando...";
        StartCoroutine(GetInfo(tipoArchivo.audio));
    }

    private IEnumerator GetInfo(tipoArchivo ta){
        DirectoryInfo dir;
        FileInfo[] info;
        if (ta == tipoArchivo.csv)
        {
            dir = new DirectoryInfo(csv_dir);
            info = dir.GetFiles("*.csv");
        }
        else
        {
            dir = new DirectoryInfo(audio_dir);
            info = dir.GetFiles("*.wav");
        }

        /*        string names="";
                foreach (FileInfo f in info) 
                { Debug.Log(f.Name);
                    names = names + f.Name + "-";
                }
                Debug.Log(names);
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {	
                    mensaje.text  = "ERROR, no se pudo realizar la operación con éxito.";
                }
                else{			
                    string url ="http://www.tifon.psico.edu.uy/obtener_info.php"+"?"+"nombres="+WWW.EscapeURL(names);
                    WWW getInfo = new WWW(url);
                    yield return getInfo;
                    mensaje.text  = getInfo.text;

                }
        */

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            mensaje.text = "ERROR, no se pudo realizar la operación con éxito.";
        }
        else
        {
            int cant_exist = 0;
            foreach (FileInfo f in info)
            {
                string url = "http://www.tifon.psico.edu.uy/check.php" + "?" + "filename=" + WWW.EscapeURL(f.Name);
                WWW getInfo = new WWW(url);
                yield return getInfo;
                if (getInfo.text == "true ") cant_exist++;
            }

            mensaje.text = "Hay " + cant_exist + " de "  + info.Length + " archivos subidos";
        }



    }


}
