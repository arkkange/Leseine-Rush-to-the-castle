using UnityEngine;
using System.Collections;

public class AntreDuel : MonoBehaviour {

	// script qui permettra de lancer le duel face au chef d'une antre
	bool _enter = false; // variable local qui permet de savoir si le joueur à lance le duel ou pas
	bool _duel = false; // variable réseau qui permet de savoir si un joueur du réseau à lancer le duel ou pas

	[SerializeField]
	public Transform ChampionPrefab;
	
	[SerializeField]
	public Transform ChampionSpawnPoint;

	public Transform _Last_Hit_player;

	private NetworkView _myNetworkView = null;

	void Start(){
		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 
	}
	

	void OnTriggerEnter(Collider other)
	{
		if( !((other.tag == "Red_Minion") || (other.tag == "Blue_Minion")) ){
			_enter = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if( !((other.tag == "Red_Minion") || (other.tag == "Blue_Minion")) ){
			_enter = false;
			_duel = false;
		}
	}

	void OnGUI()
	{

		if (_enter && !_duel) {
				GUI.Box (new Rect (10, 10, 100, 90), "Duel Antre");

			if (GUI.Button (new Rect (20, 40, 80, 20), "Duel")) //clic du joueur (en local)
			{
				_duel = true;
				_myNetworkView.RPC("Instantiate_Champion", RPCMode.All);
			}

		}
	}


	[RPC]
	void Instantiate_Champion(){
		Network.Instantiate(ChampionPrefab, ChampionSpawnPoint.position, ChampionSpawnPoint.rotation, 0);
	}


	//recupérer le dernier qui a frappé
	void Set_Last_hit(Transform player){
		_Last_Hit_player = player;
	}


}
