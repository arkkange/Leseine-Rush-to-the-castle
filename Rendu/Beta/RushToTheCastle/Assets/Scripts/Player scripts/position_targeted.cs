using UnityEngine;
using System.Collections;

public class position_targeted : MonoBehaviour {

	private NetworkPlayer myPlayerController;

	[SerializeField]
	Animator _controller;

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


	void Start () {

		myTransform = this.transform;
		agent = GetComponent<NavMeshAgent>();
		agent.destination = myTransform.position;

		//la fonction player moove (issu du script networkInputManager)
		NetworkInputManager.player_moove += new NetworkInputManager.player_moove_delegate(player_have_to_moove); //delegate

	}
	

	void Update () {

		#region Change Annimation boolean
			if( agent.remainingDistance < 0.2f ){
				_controller.SetBool("IsMooving", false);	//set the animation bool to stop
			}
		#endregion

	}

	#region delegate used for agent destination set (and begin animation)
		void player_have_to_moove(string PlayerTag, Vector3 final_position){
			if(PlayerTag == this.tag){
				agent.destination = final_position;			//set the destionation of player
				_controller.SetBool("IsMooving", true);		//set the animation bool to walking
			}
		}
	#endregion

}


