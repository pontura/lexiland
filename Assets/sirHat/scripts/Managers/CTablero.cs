using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CTablero : MonoBehaviour {

   
    public GameObject _panelTablero, _panelFin, _butonFIn;
    public CsaltarCasas[] _casas;
    public Button[] _ButCasas;
    public Animator _Jugador;
    public CGameManager.TestTipe[] _ordenTests;
    public CGameManager.TestTipe _testActual;
    private int _casaActual;
    public int _TaskCount; // Solo para que no me puteen los scripts viejos


    // Use this for initialization
    void Start () {
        for (int _i = 0; _i < _ButCasas.Length; _i++)
        {
            _ButCasas[_i].interactable = false;
        }
        _ordenTests =CGameManager.Instance.ObtenerLIstaTests();
        empezarJuego();
    }
	


    // Empezar el juego
	public void empezarJuego(){
		_Jugador.SetTrigger ("avanzar");
        _ButCasas[0].interactable = true;
    }


    // funciones botones cada casa
    public void botonCasa0(){
        llamarTestPorNumeroCasa(0);             
    }
	public void botonCasa1(){    
        llamarTestPorNumeroCasa(1);
    }   
    public void botonCasa2()
    {       
        llamarTestPorNumeroCasa(2);
    }   
    public void botonCasa3()
    {
        llamarTestPorNumeroCasa(3);
    }   
	public void botonCasa4()
	{
		llamarTestPorNumeroCasa(4);
	}
	public void botonCasa5()
	{
		llamarTestPorNumeroCasa(5);
	}

    public void ApagarBotonCasa()
    {
        _ButCasas[_casaActual].interactable = false;
    }


    // Llamada desde los tests cuando terminan.
    public void vueltaDeTest()
    {
        _ButCasas[_casaActual].interactable = false;
        // Avanzamos Alex
        _Jugador.SetTrigger("avanzar");
        // Botones casas
        
        if (_casaActual < _ButCasas.Length-1) _ButCasas[_casaActual + 1].interactable = true;
        // Saltada de casas
        _casas[_casaActual].terminarDeSaltar();
        if (_casaActual < _ButCasas.Length-1) _casas[_casaActual + 1].empezarASaltar();
        
    }


    // Pantalla final sesion
    public void pantallaFin()
    {
        CAudioManager.Instance.FinSesion();
        _butonFIn.SetActive(true);
    }





    // Pantalla final final
    public void pantallaFinFin()
    {
        _panelFin.SetActive(true);
        _panelFin.GetComponent<AudioSource>().Play();
        _panelFin.GetComponentInChildren<Animator>().SetTrigger("activar");
        _panelTablero.gameObject.SetActive(false);
    }

    // Llamar test i
    private void llamarTestPorNumeroCasa(int _pos)
    {
        _casaActual = _pos;
        _testActual = _ordenTests[_pos];
        _casas[_casaActual].terminarDeSaltar();
        CAudioManager.Instance.ApagarMusica();
        CGameManager.Instance.llamarTest(_testActual);
    }







}
