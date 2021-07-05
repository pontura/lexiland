using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTareaButton : MonoBehaviour {

    public CTareaData _tareaData;

    public void irAtarea()
    {
        CManager.Inst.irAtarea(_tareaData);
    }

}
