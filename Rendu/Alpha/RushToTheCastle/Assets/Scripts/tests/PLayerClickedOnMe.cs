using UnityEngine;
using System.Collections;

public class PLayerClickedOnMe : MonoBehaviour {

	public NetworkView _networkView;

	//delegate


	void Start(){
		_networkView = this.GetComponent<NetworkView>();

	}

	void Update(){
		if(Network.isClient){

			/*if (Input.GetMouseButtonDown(1)){

				Debug.Log("rozuihvisdvoishegiprho");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				Physics.Raycast(ray, out hit, 500f);
				if (hit.collider == this.collider){
					networkView.RPC("ClientClicked", RPCMode.Server, Network.player);
				}
				Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa");

			}*/

		}

	}
	/*
	void OnMouseDown()
	{
		if (Network.isClient){
			//networkView.RPC("ClientClicked", RPCMode.Server, Network.player);
		}
	}

	[RPC]
	void ClientClicked(NetworkPlayer _NetworkPlayer){
		string _Tag = NetworkManager.PlayerList[_NetworkPlayer].tag;
		if(Target_Change != null)
			Target_Change( _Tag, this.transform );
		networkView.RPC("ServerInformClicked", RPCMode.Others, _Tag );

	}

	[RPC]
	void ServerInformClicked(string PlayerTag){
		if(Target_Change != null)
			Target_Change( PlayerTag, this.transform );
	}
*/
}
