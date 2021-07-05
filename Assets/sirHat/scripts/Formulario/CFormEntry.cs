using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFormEntry : MonoBehaviour {

    public Text content;
    public GameObject timer;
    public GameObject obs;
    public GameObject botonera;
    public GameObject C;
    public GameObject D;
    public GameObject I;
    public GameObject L;
    public GameObject uM;
    public GameObject nc;

    public void setContent(string _content)
    {
        content.text = _content;
    }


    public void setButtons(int index, string _subTask)
    {
        CModo modo = CTarea.Inst._mode;
        int ind = index;
        if (modo._rt != RT_type.no)
            timer.SetActive(true);
        if (modo._ansType != ANS_type.no)
        {
            botonera.SetActive(true);
            //botonera.GetComponent<HorizontalLayoutGroup>().enabled = true;
            if (modo._ansType == ANS_type.CIDLuM)
            {
                C.SetActive(true);
                C.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.correcto));
                I.SetActive(true);
                I.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.incorrecto));
                if (_subTask == "pseudoword")
                {
                    L.SetActive(true);
                    L.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.lexicalization));
                }
                D.SetActive(true);
                D.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.duda));
                if (_subTask == "word")
                {
                    uM.SetActive(true);
                    uM.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.unMedio));
                }
            }
            if (modo._ansType == ANS_type.CInc)
            {
                C.SetActive(true);
                C.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.correcto));
                I.SetActive(true);
                I.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.incorrecto));
                nc.SetActive(true);
                nc.GetComponent<Button>().onClick.AddListener(() => CForm.Inst.setRespuesta(ind, CTarea.acierto.noContesta));
            }
            
        }
        //botonera.GetComponent<HorizontalLayoutGroup>().enabled = false;

    }


    public void setEditButtons(int index)
    {
        int ind = index;
        botonera.SetActive(true);
        if (CEdit.Inst._output.ContainsKey("RT"))
        {
            timer.SetActive(true);
            timer.transform.GetChild(1).GetComponent<Text>().text = CEdit.Inst._output["RT"][ind].Substring(0,4);
        }
        if (CEdit.Inst._output.ContainsKey("observations"))
        {
            obs.SetActive(true);
            obs.transform.GetChild(1).GetComponent<InputField>().text = CEdit.Inst._output["observations"][ind];
            obs.transform.GetChild(1).GetComponent<InputField>().onEndEdit.AddListener(val => CEdit.Inst.editObs(ind, obs.transform.GetChild(1).GetComponent<InputField>().text));
            
        }


        if (CEdit.Inst._output.ContainsKey("acierto"))
        {
            C.SetActive(true);
            C.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.correcto));
            if (CEdit.Inst._output["acierto"][ind] == "true")
            {
                C.transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            I.SetActive(true);
            I.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.incorrecto));
            if (CEdit.Inst._output["acierto"][ind] == "false")
            {
                I.transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            D.SetActive(true);
            D.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.duda));
            if (CEdit.Inst._output["acierto"][ind] == "NA")
            {
                D.transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
        }


        if (CEdit.Inst._output.ContainsKey("NC"))
        {
            nc.SetActive(true);
            nc.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.noContesta));
            if (CEdit.Inst._output["NC"][ind] == "true")
            {
                nc.transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
        }
        if (CEdit.Inst._output.ContainsKey("lexicalization/unMedio"))
        {
            if (CEdit.Inst._output["subTask"][ind] == "word")
            {
                L.SetActive(true);
                L.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.lexicalization));
                if (CEdit.Inst._output["lexicalization/unMedio"][ind] == "true")
                {
                    L.transform.GetChild(0).GetComponent<Image>().enabled = true;
                    I.transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
            }
            if (CEdit.Inst._output["subTask"][ind] == "pseudoword")
            {
                uM.SetActive(true);
                uM.GetComponent<Button>().onClick.AddListener(() => CEdit.Inst.setRespuesta(ind, CTarea.acierto.unMedio));
                if (CEdit.Inst._output["lexicalization/unMedio"][ind] == "true")
                {
                    uM.transform.GetChild(0).GetComponent<Image>().enabled = true;
                    I.transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
            }
        }



    }

}
