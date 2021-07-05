using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged {

    [SerializeField] Transform _slots;
    [SerializeField] Text _inventoryText;
    public string _respuesta;
    public string[] _respCadena;
    public int _nroResp ;
    public CTest5 _CTest5;

    // Use this for initialization
    void Start()
    {
        HasChanged();
    }

    public void HasChanged()
    {
        _nroResp = 0;
        _respCadena = new string[9];
        System.Text.StringBuilder _builder = new System.Text.StringBuilder();
        foreach (Transform slotTransform in _slots)
        {
            GameObject item = slotTransform.GetComponent<CSlot>().item;
            if (item)
            {
                _respCadena[_nroResp] = item.name;
                _builder.Append(item.name);
                _nroResp += 1;
            }

        }
        _inventoryText.text = _builder.ToString();
        _respuesta = _builder.ToString();
        _CTest5.compararRespuesta(_respuesta,_nroResp, _respCadena);
    }


}


namespace UnityEngine.EventSystems
{
    public interface IHasChanged: IEventSystemHandler
    {
        void HasChanged();
    }
}

