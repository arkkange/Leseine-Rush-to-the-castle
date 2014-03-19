using UnityEngine;
using System.Collections;

public class position_targeted : MonoBehaviour {

	public NetworkPlayer myPlayerController;
	
	Transform myTransform;
	public Transform MyTransform {
		get {
			return myTransform;
		}
		set {
			myTransform = value;
		}
	}

	//declaration de l'objet navmeshagent qui permet de créer le pathfinding
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {

		myTransform = this.transform;
		agent = GetComponent<NavMeshAgent>();
		agent.destination = myTransform.position;

		//la fonction player moove (issu du script networkInputManager)
		NetworkInputManager.player_moove += new NetworkInputManager.player_moove_delegate(player_have_to_moove); //delegate
	}


	// Update is called once per frame
	void Update () {

	}

	void player_have_to_moove(string PlayerTag, Vector3 final_position){
		if(PlayerTag == this.tag){
			agent.destination = final_position;	//set the festionation of player
		}
	}

}


