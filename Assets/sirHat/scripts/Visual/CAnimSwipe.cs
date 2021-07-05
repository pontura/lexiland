using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAnimSwipe : MonoBehaviour {

    public string nextWord;

    public void changueWord()
    {
        GetComponent<Text>().text = nextWord;
    }

}
