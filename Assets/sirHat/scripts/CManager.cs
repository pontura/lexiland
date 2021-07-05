using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class CManager : MonoBehaviour {


    public static CManager Inst;

    public GameObject menuPanel;
    public GameObject menuSPanel;
    public GameObject tareaPanel;
    public GameObject visualPanel;
    public GameObject ssPanel;
    public GameObject listTasksPanel;
    public GameObject guardarButton;
    public GameObject panelPostTarea;
    
    void Awake()
    {
        Inst = this;
    }

    public void Start()
    {

        menuSPanel.SetActive(false);
        tareaPanel.SetActive(false);
        visualPanel.SetActive(false);
        ssPanel.SetActive(false);
        listTasksPanel.SetActive(false);
    }

/*    IEnumerator pruebaServidor()
    {
        string _path = Application.persistentDataPath + "/Data/" + "centroData/";
        string _filePath = _path + CData.Inst.centro._ID + ".xml";
        byte[] file = File.ReadAllBytes(_filePath);
        Dictionary<string,string> headers = new Dictionary<string, string>();
        headers.Add("content","")
        WWW w = new WWW("http://xyzdevelop.com/bcbl/update.php", file, );
        yield return w;

        WWWForm form = new WWWForm();
        print("form created ");
        form.AddField("action", "level upload");
        form.AddField("file", "file");
        form.AddBinaryData("file", file, "prueba.xml", "application/xml");
        print("binary data added ");
        WWW w = new WWW("http://xyzdevelop.com/bcbl/update.php", form);
        yield return w;


        Debug.Log(w.text);
    }
*/

    public void irAtarea(CTareaData _tarea)
    {
        CTarea.Inst.LoadTarea(_tarea);
        menuPanel.SetActive(false);
        tareaPanel.SetActive(false);

        if (CTarea.Inst._subtareas == null)
            irASubtarea(null);
        else
            irASubtarea(CTarea.Inst._subtareas[0]);
            //irASubtareas();
    }

    public void irASubtareas()
    {

        tareaPanel.SetActive(false);
        visualPanel.SetActive(false);
        listTasksPanel.SetActive(false);
        menuSPanel.SetActive(true);

        foreach (Transform child in menuSPanel.transform.GetChild(1))
            Destroy(child.gameObject);

        GameObject f;
        foreach (string st in CTarea.Inst._subtareas)
        {
            f = Instantiate(Resources.Load("prefabs/subtarea") as GameObject);
            f.transform.SetParent(menuSPanel.transform.GetChild(1), false);
            f.GetComponent<CSubtarea>()._subtarea = st;
            f.transform.GetChild(0).GetComponent<Text>().text = st;
        }
    }

    public void irASubtarea(string _subtarea)
    {
        // TODO: Mejorar el tema de los paneles
        menuSPanel.SetActive(false);
        if (CTarea.Inst._mode._form)
        {
            tareaPanel.SetActive(true);
            CForm.Inst.setForm(_subtarea, false);
        }
        if (CTarea.Inst._mode._visualInput)
        {
            visualPanel.SetActive(true);
            CVisual.Inst.setVisual(_subtarea);
        }
        if (CTarea.Inst._mode._oneEntry)
        {
            ssPanel.SetActive(true);
            CFormOneEntry.Inst.setForm();
           
        }



    }

    public void irAmenu()
    {
        menuSPanel.SetActive(false);
        tareaPanel.SetActive(false);
        visualPanel.SetActive(false);
        ssPanel.SetActive(false);
        menuPanel.SetActive(true);
        listTasksPanel.SetActive(false);
    }

    public void irAEdit(CTareaData td)
    {
        //        string _path = Application.persistentDataPath + "/CSV";
        string _path = Application.persistentDataPath;
/*        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
*/
        DirectoryInfo dir = new DirectoryInfo(_path);
        FileInfo[] info = dir.GetFiles("*.csv");

        guardarButton.SetActive(false);
        menuSPanel.SetActive(false);
        tareaPanel.SetActive(false);
        visualPanel.SetActive(false);
        menuPanel.SetActive(false);
        listTasksPanel.SetActive(true);
        //Debug.Log(_path);
        CEdit.Inst.titulo.text = td._name;



        //Debug.Log(info.Length);

        foreach (Transform child in listTasksPanel.transform.GetChild(0).GetChild(0).GetChild(0))
            Destroy(child.gameObject);

        GameObject button;
        foreach (FileInfo f in info)
        {
            string fn = f.Name;
            string[] fnData = fn.Split("_"[0]);
            //for (int i = 0; i < fnData.Length; i++) Debug.Log(fnData[i]);
            if (fnData.Length==5 && fnData[4].Contains(td._code) && fnData[2].Equals(CLogManager.Instance._textoID))
            {
                button = Instantiate(Resources.Load("prefabs/entry") as GameObject);
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                button.transform.SetParent(listTasksPanel.transform.GetChild(0).GetChild(0).GetChild(0), false);
                button.transform.GetChild(0).GetComponent<Text>().text = "Día: " + fnData[0].Substring(2,2) + "/" + fnData[0].Substring(0, 2) + " - Hora: " + fnData[1].Substring(0,2) + ":" + fnData[1].Substring(2, 2) + " - Sesión: " + fnData[3];
                button.GetComponent<Button>().onClick.AddListener(() => editTask(fn));
            }
        }
        listTasksPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<VerticalLayoutGroup>().childControlWidth = false;


    }

    public void editTask(string filepath)
    {
        //string path = Application.persistentDataPath + "/CSV/" + filepath;
        string path = Application.persistentDataPath + "/" + filepath;
        CEdit.Inst.setForm(path, CTarea.Inst._mode);
        listTasksPanel.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        guardarButton.SetActive(true);
    }

}
