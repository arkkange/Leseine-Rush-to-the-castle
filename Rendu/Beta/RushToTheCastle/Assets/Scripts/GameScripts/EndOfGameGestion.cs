using UnityEngine;
using System.Collections;

public class EndOfGameGestion : MonoBehaviour {

	private bool _end=false;
	private string MonTexte;

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Cart") ){
			Debug.Log (other.tag);
			if(this.CompareTag("Finish_Blue") ){
				MonTexte = "Victoire des Rouges !";
			}
			if(this.CompareTag("Finish_Red") ){
				MonTexte = "Victoire des Bleus !";
			}
			_end=true;

		}

	}

	void OnGUI()
	{
		
		if (_end) {
			GUI.Box (new Rect (100, 70, 250, 190), MonTexte);
			
			if (GUI.Button (new Rect (200, 100, 130, 50), "Fin de la partie")) //clic du joueur (en local)
			{
				Application.LoadLevel (0);
			}
			
		}
	}

}
