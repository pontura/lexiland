using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSubtarea : MonoBehaviour {

    public string _subtarea;

    public void irAsubtarea()
    {
        if (_subtarea != null)
            CManager.Inst.irASubtarea(_subtarea);
        else
            CManager.Inst.irAmenu();
    }

}
