using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayStop : MonoBehaviour {

    public string _target;

    public void PlayTarget()
    {
        CAudioManager.Instance.playRecord(_target);
    }

}
