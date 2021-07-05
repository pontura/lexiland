using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour {

    public Transform estaciones;

    Animator animator;

    AnimatorStateInfo animationState;
    AnimatorClipInfo[] animatorClip;
    float prevTime = 0.0f;

	public void Awake()
	{
        animator = this.GetComponent<Animator>();
        animationState = animator.GetCurrentAnimatorStateInfo(0);
        animatorClip = animator.GetCurrentAnimatorClipInfo(0);
//        Debug.Log(animatorClip[0].GetHashCode() + " " + animatorClip[0].clip.name);
        prevTime = 0.0f;
	}

    public void Continuar(){

        Debug.Log(prevTime);
        animator.speed = 1.0f;
        GetComponent<animar_imagen>().enabled = true;
        //animator.Play(animatorClip[0].GetHashCode(), 0, prevTime);
        animator.Play(animatorClip[0].clip.name, 0, prevTime);
        //animator.playbackTime = prevTime;
    }

	public void LlegaACasa(int i){

        Estacion estacion = estaciones.GetChild(i).GetComponent<Estacion>();

        if (Juego.j.estaciones.Contains(estacion)){
            estacion.GetComponentInChildren<Animator>().SetTrigger("saltar");
            GetComponent<animar_imagen>().enabled = false;
            animator.speed = 0.0f;
            prevTime = animatorClip[0].clip.length * animationState.normalizedTime;
            Debug.Log("llega a casa " + i);
            estacion.casita.GetComponent<Button>().interactable = true;
        }

    }

    public void Finish(){
        Juego.j.EndGame();
    }

}
