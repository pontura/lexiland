using UnityEngine;
using System.Collections;

public class CFinFinalAnimation : MonoBehaviour {

    public CanvasHandler _canvasH;
    public void finAnimation()
    {
        CGameManager.Instance.ReiniciarJuego();
        //_canvasH.reiniciarJuego();
    }
}
