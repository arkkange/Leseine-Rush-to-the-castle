using UnityEngine;
using System.Collections;

public class RtsCam : MonoBehaviour {

	private int largeur = 5; //la zone de pixel dans laquel la camera va pouvoir bouger
	private int moveSpeed = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(Network.isClient){
			var translation = Vector3.zero;
		
		
				//gestion de la camera dans l'espace
				
			if(Input.mousePosition.x <largeur || Input.GetKey(KeyCode.LeftArrow) ) //vertical donc ici vers la gauche
				{	
					if(camera.transform.position.x > 100){
						translation += Vector3.right * -moveSpeed * Time.deltaTime;
					}
				}
				
			if(Input.mousePosition.x >= Screen.width - largeur || Input.GetKey(KeyCode.RightArrow ))//vertical donc ici vers la droite
				{
					if(camera.transform.position.x < 200){
						translation += Vector3.right * moveSpeed * Time.deltaTime;
					}
				}
				
			if(Input.mousePosition.y < largeur || Input.GetKey(KeyCode.DownArrow ))	//vers l'arriere
				{
					if(camera.transform.position.z > 80){
						translation += Vector3.forward * -moveSpeed * Time.deltaTime;
					}
				}
				
			if(Input.mousePosition.y > Screen.height - largeur || Input.GetKey(KeyCode.UpArrow))	// vers l'avant
				{
					if(camera.transform.position.z < 355){
						translation += Vector3.forward * moveSpeed * Time.deltaTime;
					}
				}

				camera.transform.position += translation;
	
		}

	//}
}
