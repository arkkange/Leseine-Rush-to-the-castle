using UnityEngine;
using System.Collections;
using System.Collections.Generic; //permet d'utiliser directement les dictionaires et listes;

public class NetworkManager : MonoBehaviour {

	//player start position


	#region red team positions
		[SerializeField]
		public Transform _positionPlayer1_red;
		[SerializeField]
		public Transform _positionPlayer2_red;
		[SerializeField]
		public Transform _positionPlayer3_red;
		[SerializeField]
		public Transform _positionPlayer4_red;
		[SerializeField]
		public Transform _positionPlayer5_red;
	#endregion

	#region blue team positions
		[SerializeField]
		public Transform _positionPlayer1_blue;
		[SerializeField]
		public Transform _positionPlayer2_blue;
		[SerializeField]
		public Transform _positionPlayer3_blue;
		[SerializeField]
		public Transform _positionPlayer4_blue;
		[SerializeField]
		public Transform _positionPlayer5_blue;
	#endregion

	[SerializeField]
	private GameObject _CartPrefab;

	[SerializeField]
	private Transform _CartStartPosition;

	//player prefab
		[SerializeField]
		private GameObject _PlayerPrefab;
		string tag;

	//networkview
		private NetworkView _myNetworkView = null;

	//List of player object instanciated
		public static Dictionary<NetworkPlayer, GameObject> PlayerList = new Dictionary<NetworkPlayer, GameObject>();
	
	//Datas for the server (is a server or is a client?)
		[SerializeField]
		private bool _isServer = true;
		public bool IsServer
		{
			get { return _isServer; }
			set { _isServer = value; }
		}
		
		[SerializeField]
		public Transform _camera;

	// Use this for initialization
	void Start () {

		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 
		Application.runInBackground = true;

		#region Network creation
			if (IsServer)
			{
				//initialisation de la player list a chaque debut de server
				PlayerList = new Dictionary<NetworkPlayer, GameObject>();

				Network.InitializeSecurity();
				Network.InitializeServer(2, 25000, true);
				Network.maxConnections = 10;
			}
			else
			{
				//player se connecte au serveur a cette adresse
				Network.Connect("127.0.0.1", 25000); //
			}
		#endregion

		Network.Instantiate(_CartPrefab, _CartStartPosition.position ,_CartStartPosition.rotation,0);

	}


	void OnConnectedToServer()
	{	
		// set camer'as position to the spawn
		if( (Network.connections.Length%2) == 1){	//if last conection's blue team
			_camera.position = new Vector3(125, 30, 80);
		}
		else{										//if last conection's red team
			_camera.position = new Vector3(17, 30, 370);
		}

	}

	
	void OnPlayerConnected(NetworkPlayer player)
	{	
		//initialisation
		if(Network.connections.Length == 1)
		{
			tag = "Player_blue_1";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer1_blue.position ,_positionPlayer1_blue.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 2)
		{
			tag = "Player_red_1";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer1_red.position ,_positionPlayer1_red.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 3)
		{
			tag = "Player_blue_2";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer2_blue.position ,_positionPlayer2_blue.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 4)
		{
			tag = "Player_red_2";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer2_red.position ,_positionPlayer2_red.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 5)
		{
			tag = "Player_blue_3";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer3_blue.position ,_positionPlayer3_blue.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);

		}
		if (Network.connections.Length == 6)
		{
			tag = "Player_red_3";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer3_red.position ,_positionPlayer3_red.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 7)
		{
			tag = "Player_blue_4";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer4_blue.position ,_positionPlayer4_blue.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 8)
		{
			tag = "Player_red_4";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer4_red.position ,_positionPlayer4_red.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 9)
		{
			tag = "Player_blue_5";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer5_blue.position ,_positionPlayer5_blue.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}
		if (Network.connections.Length == 10)
		{
			tag = "Player_red_6";
			_myNetworkView.RPC("ChangePlayerPrefabTag", RPCMode.All, tag);
			GameObject newplayer = Network.Instantiate(_PlayerPrefab, _positionPlayer5_red.position ,_positionPlayer5_red.rotation,0) as GameObject;
			PlayerList.Add(player , newplayer);
		}

	}

	[RPC]
	void ChangePlayerPrefabTag(string tag){	//modifie le playerprefab.tag de tous les client et du server
		_PlayerPrefab.tag = tag;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
