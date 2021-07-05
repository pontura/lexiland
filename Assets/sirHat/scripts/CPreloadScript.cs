using UnityEngine;
using System.Collections;

public class CPreloadScript : MonoBehaviour {
	IEnumerator Start() {
        AsyncOperation async = Application.LoadLevelAsync("main");
        yield return async;
    }    
}
