using UnityEngine;
using System.Collections;
using System.Collections.Generic; //permet d'utiliser directement les dictionaires et listes;

public class NetworkInputManager : MonoBehaviour {


	//creation du delegate player moove (script position targeted)
	public delegate void player_moove_delegate(string PlayerName, Vector3 final_position);
	public static event player_moove_delegate player_moove; //playermoove delegated function

	//creation du delegate Target_position
	//public delegate void player_Target_change(string PlayerName, Vector3 final_position);
	//public static event player_Target_change Taget_Change; //playermoove delegated function


	//variable position for the player position needed in update
	Vector3 _TransFormPosition = Vector3.zero;

	///////////////////////////////////
	// player intentions
	///////////////////////////////////

	class sPlayerIntents //intentions du joueur sous forme de booléen
	{
		//cast de différentes sorts
		public bool _wantToCast_A = false;
		public bool _wantToCast_Z = false;
		public bool _wantToCast_E = false;
		public bool _wantToCast_R = false;
	}

	//un dictionaire de joueurs, et leurs intentions (sPlayerIntents)
	private Dictionary<NetworkPlayer, sPlayerIntents> _playersIntents;
	private Dictionary<NetworkPlayer, sPlayerIntents> PlayersIntents
	{
		get { return _playersIntents; }
		set { _playersIntents = value; }
	}
	
	private NetworkView _myNetworkView = null; //la networkview est le composant qui envoit les données server etc
	
	// Use this for initialization
	void Start () {	
		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 

		//creation du dictionaire sPlayersIntents 
		PlayersIntents = new Dictionary<NetworkPlayer, sPlayerIntents>();
		//recuperation de l'objet networkview
	}

	//ajotu du joueur conencté au dictionaire de joueurs + lancer "NewPlayerConnected" avec un [rpc]
	void OnPlayerConnected(NetworkPlayer p)
	{
		PlayersIntents.Add(p, new sPlayerIntents()); 	//ajout du joueur connecté au dictionnaire d'intentions de joueurs
		_myNetworkView.RPC("NewPlayerConnected", RPCMode.OthersBuffered, p); 	// RPC("nom de la fonctionenvoyée", parametres supplementaires...)
	}
	
	[RPC]
	void NewPlayerConnected(NetworkPlayer p)
	{
		PlayersIntents.Add(p, new sPlayerIntents()); //initialisation du nouveau couple Networkplayer et intentions
	}
	
	// Update is called once per frame
	void Update () {	//ici on envoit au serveur les intentions du joueur en fonction des touches eppuyées

		//ce bout de script sert a faire des test coté server 
		// il sert a recuperer le tranform du composant créé
		if (Network.isServer){
			 
		}

		if (Network.isClient){
			if(Input.GetMouseButtonDown(1)){

				_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				//RaycastHit hit;
				float hit = 0.0f;
				Plane playerPlane = new Plane(Vector3.up, _TransFormPosition); 	//changed
				if( playerPlane.Raycast(ray , out hit) ){
					_myNetworkView.RPC("PlayerWantToGo", RPCMode.Server, Network.player, ray.GetPoint(hit) );
				}

			}
			if (Input.GetMouseButton(1)){

				_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				//RaycastHit hit;
				float hit = 0.0f;
				Plane playerPlane = new Plane(Vector3.up, _TransFormPosition); 	//changed
				if( playerPlane.Raycast(ray , out hit) ){
					_myNetworkView.RPC("PlayerWantToGo", RPCMode.Server, Network.player, ray.GetPoint(hit) );
				}

			}
		}

	}


	//set the player moovement with the mesh to all the network when a player right clic
	[RPC]
	void PlayerWantToGo(NetworkPlayer p, Vector3 newPosition){
		if (Network.isServer)
		{	
			//set the destination of the player on the server only 
			player_moove(NetworkManager.PlayerList[p].tag, newPosition); // lancement du delegate
			//give the destination to the players with his tag (cause we don't have the list on client)
			_myNetworkView.RPC("PlayerSetDestination", RPCMode.Others, NetworkManager.PlayerList[p].tag, newPosition );
		}
	}
	// set the destination one the player
	[RPC]
	void PlayerSetDestination(string PlayerTag , Vector3 newposition){
		if(Network.isClient){
			//set destination to all clients with the tag name playername
			player_moove( PlayerTag, newposition); // lancement du delegate
		}
	}
	
	[RPC]
	void NeedPlayerPosition(NetworkPlayer p, Vector3 newPosition){
		if(Network.isServer){
				newPosition = NetworkManager.PlayerList[p].transform.position;
				_myNetworkView.RPC("NeedPlayerPosition", RPCMode.OthersBuffered, Network.player, newPosition);
		}
		if(Network.isClient){
			_TransFormPosition = newPosition;
		}
	}
	
	void FixedUpdate()	//effet de mouvement en fonction de l'etat
	{

	}
	
}
