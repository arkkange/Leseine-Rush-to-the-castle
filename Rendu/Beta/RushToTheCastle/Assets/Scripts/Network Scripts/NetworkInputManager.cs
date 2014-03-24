using UnityEngine;
using System.Collections;
using System.Collections.Generic; //permet d'utiliser directement les dictionaires et listes;

public class NetworkInputManager : MonoBehaviour {

	#region delegate functions
		//creation du delegate player moove (script position targeted)
		public delegate void player_moove_delegate(string PlayerName, Vector3 final_position);
		public static event player_moove_delegate player_moove; //playermoove delegated function

		//creation du delegate skillShotResolution
	public delegate void skill_Shot_Resolution_delegate(string PlayerName, Vector3 mouse_position, string compétence);
		public static event skill_Shot_Resolution_delegate Skill_Shot;

	#endregion

	Vector3 _PlayerPosition = Vector3.zero;	//variable position for the player position needed in update

	///////////////////////////////////
	// player intentions
	///////////////////////////////////

	#region dictionaries (player and player intents)

		//un dictionaire de joueurs, et leurs intentions (sPlayerIntents)
		private Dictionary<NetworkPlayer, sPlayerIntents> _PlayersIntents;

		class sPlayerIntents //intentions du joueur sous forme de booléen
		{
			//cast de différentes sorts
			public bool _wantToCast_A = false;
			public bool _wantToCast_Z = false;
			public bool _wantToCast_E = false;
			public bool _wantToCast_R = false;
		}

	#endregion
	
	private NetworkView _myNetworkView = null; //la networkview est le composant qui envoit les données server etc
	

	#region initialisation
	void Start () {	
		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 	//recuperation de l'objet networkview

		//creation du dictionaire sPlayersIntents 
		_PlayersIntents	 = new Dictionary<NetworkPlayer, sPlayerIntents>();

	}
	#endregion

	#region Player connections
		
	void OnPlayerConnected(NetworkPlayer p)	//ajout du joueur conencté au dictionaire de joueurs + lancer "NewPlayerConnected" avec un [rpc]
		{
			_PlayersIntents.Add(p, new sPlayerIntents()); 	//ajout du joueur connecté au dictionnaire d'intentions de joueurs
			_myNetworkView.RPC("NewPlayerConnected", RPCMode.Others, p); 	// RPC("nom de la fonctionenvoyée", parametres supplementaires...)
		}
	#endregion

	void Update () {	
		#region network side
			if (Network.isServer){
				 
			}
		#endregion

		#region client side
			if (Network.isClient){
				#region Players's Mooves requests (inputs)

					#region Mouse inputs
					if(Input.GetMouseButtonDown(1)){

						_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						//RaycastHit hit;
						float hit = 0.0f;
						Plane playerPlane = new Plane(Vector3.up, _PlayerPosition); 	//changed
						if( playerPlane.Raycast(ray , out hit) ){
							_myNetworkView.RPC("PlayerWantToGo", RPCMode.Server, Network.player, ray.GetPoint(hit) );
						}

					}
					if (Input.GetMouseButton(1)){

						_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						//RaycastHit hit;
						float hit = 0.0f;
						Plane playerPlane = new Plane(Vector3.up, _PlayerPosition); 	//changed
						if( playerPlane.Raycast(ray , out hit) ){
							_myNetworkView.RPC("PlayerWantToGo", RPCMode.Server, Network.player, ray.GetPoint(hit) );
						}
					}
					#endregion
				
					#region keyboard inputs
						//skills
						if(Input.GetKeyDown(KeyCode.A)){
							System.Console.WriteLine("detection de keydown A");
							_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
							Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							//RaycastHit hit;
							float hit = 0.0f;
							Plane playerPlane = new Plane(Vector3.up, _PlayerPosition); 	//changed
							if( playerPlane.Raycast(ray , out hit) ){
								//envoi de la position ( et donc rotation de la destionation )
								_myNetworkView.RPC("ASkillResolution", RPCMode.Server, Network.player, ray.GetPoint(hit), "A");
							}
						}
						
						if(Input.GetKeyDown(KeyCode.Z)){
							_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Server, Network.player, Vector3.zero);
							Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							//RaycastHit hit;
							float hit = 0.0f;
							Plane playerPlane = new Plane(Vector3.up, _PlayerPosition); 	//changed
							if( playerPlane.Raycast(ray , out hit) ){
								//envoi de la position ( et donc rotation de la destionation )
								_myNetworkView.RPC("ASkillResolution", RPCMode.Server, Network.player, ray.GetPoint(hit), "Z");
							}
						}
						
						if(Input.GetKeyDown(KeyCode.E)){
							
						}
						
						if(Input.GetKeyDown(KeyCode.E)){
							
						}

					#endregion

				#endregion
			}
		#endregion

	}

	#region RPC

		#region Skills resolutions RPC

		[RPC]//server
		void ASkillResolution ( NetworkPlayer player, Vector3 MousePosition, string compétence){
			Skill_Shot( NetworkManager.PlayerList[player].tag , MousePosition, compétence );	//the mouse position can give us the rotation start of the skill
		
		}

		#endregion
		
		#region connections RPC
		
			[RPC]
			void NewPlayerConnected(NetworkPlayer p)
			{
				_PlayersIntents.Add(p, new sPlayerIntents()); //initialisation du nouveau couple Networkplayer et intentions
			}

		#endregion
		
		#region player moove RPC

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
					player_moove( PlayerTag, newposition); // launch du delegate
				}
			}

			//this RPC gets the player position from the list of players for the raycasthit
			[RPC]
			void NeedPlayerPosition(NetworkPlayer p, Vector3 newPosition){
				if(Network.isServer){
						newPosition = NetworkManager.PlayerList[p].transform.position;
						_myNetworkView.RPC("NeedPlayerPosition", RPCMode.Others, Network.player, newPosition);
				}
				if(Network.isClient){
					_PlayerPosition = newPosition;
				}
			}

		#endregion

	#endregion

	
}
