using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class animar_imagen : MonoBehaviour {
	
	public List<Sprite> _sprites =  new List<Sprite>();
	public float _cadencia;
	private Image _imagen;
	private float timer;

	void Start () {
		//timer = Random.Range(0.5f,1f);
		timer = _cadencia;
		_imagen = GetComponent<Image> ();
		_imagen.sprite = _sprites[0]; 
	}
	

	void Update () {

		timer -= Time.deltaTime;
		if(timer<0){
			_imagen.sprite = _sprites[Random.Range(0,_sprites.Count)];
			//timer = Random.Range(0.5f,1f);
			timer = _cadencia;
		} 
	} 


	void OnDisable(){
		//_imagen.sprite = _sprites[0]; 
	}


}