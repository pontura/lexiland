using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CEdit : MonoBehaviour {

    public static CEdit Inst;
    public GameObject form;
    public Dictionary<string, List<string>> _output;
    public string fp;
    public string[] header;
    public Text titulo;

    void Awake()
    {
        Inst = this;
    }

    public void editObs(int ind, string obs)
    {
        //        Debug.Log("tenemos: " + obs + ", en la fila " + ind);
        _output["observations"][ind] = obs;
    }

    public void setForm(string filepath, CModo _m)
    {
        fp = filepath;
        foreach (Transform child in form.transform)
            Destroy(child.gameObject);
        CAudioManager._aclips.Clear();

        header = new string[2];
        header[0] = File.ReadAllLines(filepath)[0];
        header[1] = File.ReadAllLines(filepath)[1];

        //Debug.Log(header[0]);
        //Debug.Log(header[1]);

        string[,] inputMatrix = CSVReader.SplitCsvGrid(File.ReadAllText(filepath));

        _output = new Dictionary<string, List<string>>();


        for (int _i = 0; _i < inputMatrix.GetLength(0); _i++)
        {
            List<string> _listaEstimulos = new List<string>();
            for (int _j = 3; _j < inputMatrix.GetLength(1); _j++)
            {
                //Debug.Log(inputMatrix[_i, _j]);
                _listaEstimulos.Add(inputMatrix[_i, _j]);
            }
            _output.Add(inputMatrix[_i, 2], _listaEstimulos);
//            Debug.Log(inputMatrix[_i, 0]);
        }

        for (int i=0; i<_output["task"].Count; i++)
        {
            GameObject f = Instantiate(Resources.Load("prefabs/formEntry") as GameObject);
            f.transform.SetParent(form.transform, false);
            if (_output.ContainsKey("audioPath"))
            {
                f.transform.GetChild(1).gameObject.SetActive(true);
                string key = _output["audioPath"][i];
                StartCoroutine(loadAudio(key));
                f.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => CAudioManager.Instance.playRecord(key));
            }
            if (_output.ContainsKey("oracion"))
            {
                f.transform.GetChild(0).GetComponent<Text>().text = _output["oracion"][i];
                f.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(380.0f, f.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
            }
            else
                f.transform.GetChild(0).GetComponent<Text>().text = _output["target"][i];
            f.transform.GetComponent<CFormEntry>().setEditButtons(i);
        }

        form.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
        this.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 1;


    }

    public IEnumerator loadAudio(string key)
    {
        //            WWW w = new WWW("file:///" + Application.persistentDataPath + "/Audios/" + CLogManager.Instance._textoID + "/" + key);
        WWW w = new WWW("file:///" + Application.persistentDataPath + "/Audios/" + key);
        //            Debug.Log(key);
        yield return w;
        if (!CAudioManager._aclips.ContainsKey(key))
            CAudioManager._aclips.Add(key, w.GetAudioClip());
    }

    public void setRespuesta(int ind, CTarea.acierto acierto)
    {
        switch (acierto)
        {
            case CTarea.acierto.correcto:
                _output["acierto"][ind] = "true";
                if (_output.ContainsKey("lexicalization/unMedio"))
                    _output["lexicalization/unMedio"][ind] = "NA";
                break;
            case CTarea.acierto.incorrecto:
                _output["acierto"][ind] = "false";
                if (_output.ContainsKey("lexicalization/unMedio"))
                    _output["lexicalization/unMedio"][ind] = "NA";
                break;
            case CTarea.acierto.duda:
                _output["acierto"][ind] = "NA";
                if (_output.ContainsKey("lexicalization/unMedio"))
                    _output["lexicalization/unMedio"][ind] = "false";
                break;
            case CTarea.acierto.lexicalization:
                _output["acierto"][ind] = "false";
                _output["lexicalization/unMedio"][ind] = "true";
                break;
            case CTarea.acierto.noContesta:
                _output["acierto"][ind] = "false";
                if (_output.ContainsKey("lexicalization/unMedio"))
                    _output["lexicalization/unMedio"][ind] = "false";
                break;
            case CTarea.acierto.unMedio:
                _output["acierto"][ind] = "false";
                _output["lexicalization/unMedio"][ind] = "true";
                break;

        }

    }

    public void guardar()
    {
        string aux = "";
        StreamWriter _file = new StreamWriter(fp, false);

        _file.WriteLine(header[0]);
        _file.WriteLine(header[1]);

        foreach (var param in _output.Keys)
        {
            aux += param + ",";
        }
        _file.WriteLine(aux.Substring(0, aux.Length - 1));

        for (int i=0; i< _output["task"].Count; i++)
        {
            aux = "";
            foreach (var param in _output.Keys)
            {
                aux += _output[param][i] + ",";
            }
            _file.WriteLine(aux.Substring(0, aux.Length - 1));
        }
        Debug.Log(aux);
        _file.Close();
    }

    }
