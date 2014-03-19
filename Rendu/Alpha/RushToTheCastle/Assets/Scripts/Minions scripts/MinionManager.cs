using UnityEngine;
using System.Collections;
using System.Collections.Generic; //permet d'utiliser directement les dictionaires et listes;

public class MinionManager : MonoBehaviour {

	#region serialized Stats

		[SerializeField]
		private double _Domages;
		[SerializeField]
		private double _AttackSpeed;
		[SerializeField]
		private double _CriticalHitChance;
		[SerializeField]
		private double _CriticalHitBonus;
		[SerializeField]
		private double _Range;

	#endregion

	[SerializeField]
	AudioSource sword;

	//ScriptableObject of the target
	public HealthManager script;

	[SerializeField]
	Transform Cart;

	//navmeshagent for the pathfinding calculation
	[SerializeField]
	public NavMeshAgent agent;
	//agent.stoppingDistance

	[SerializeField]
	private NetworkView _myNetworkView;

	//list of targets in the order
	List<Transform> _Targets = new List<Transform>();

	public delegate void I_killed_a_minion(Transform minion);  	//déclaration du type I_killed_a_minion  comme une fonction
	public static event I_killed_a_minion Minion_have_been_killed;   	//variable membre déclaré avec le type I_killed_a_minion qui s'appelle Minion_have_been_killed  

	// Use this for initialization
	void Start () {

			#region itnitializing nav mesh and audio source and networkview
				
			sword = this.GetComponent<AudioSource>();
			agent = this.GetComponent<NavMeshAgent>();
			_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 
	
			#endregion

			#region we get the minion destination's in function of the minion's color (two position around the cart)
				if(this.tag == "Red_Minion"){
					Cart = GameObject.Find("RedPush").transform;
				}
				else if(this.tag == "Blue_Minion"){
					Cart = GameObject.Find("BluePush").transform;
				}
				agent.destination = Cart.position; 
				_myNetworkView.RPC("SetAgentDestination", RPCMode.Others, Cart.position);
			#endregion

	}


	// Update is called once per frame
	void Update () {
		if (Network.isServer){

			#region attack timer clock maj
			if(AttackSpeedTiming)
			{
				countdown -= Time.deltaTime;
				if(countdown <= 0 && (_Targets.Count != 0)){
					AttackSpeedTiming = false;
				}
			}
			#endregion

			#region clearing dead targets in _Targets
			List<Transform> _refresher = new List<Transform>();
			foreach (Transform current in _Targets){
				if(!current){
					_refresher.Add(current);
				}
			}
			foreach( Transform current in _refresher){
				_Targets.Remove(current);
			}
			#endregion

			#region Time to attack
			if( (!AttackSpeedTiming) && (_Targets.Count != 0) ){		//if the attack speed timer is'nt waiting we can attack and there is a target in the list

				#region if we have a Target we attack it and set position to it
				if(_Targets.Count != 0){
					script = _Targets[0].GetComponent<HealthManager>();			//we get the script of the target to domage him
					script.LooseHealth( CalculatedDamages() );		//we damage him

					//reset attack speed timer
					StartAttackSpeedTimer(_AttackSpeed);

					//reset position to target's one
					agent.destination = _Targets[0].position;
					_myNetworkView.RPC("SetAgentDestination", RPCMode.OthersBuffered, _Targets[0].position);

				}
				#endregion
			}
			#endregion

			#region no Target set destination to Cart
			if( _Targets.Count == 0 ){
				agent.destination = Cart.position;
				_myNetworkView.RPC("SetAgentDestination", RPCMode.Others, Cart.position);
			}
			else{
				agent.destination = _Targets[0].position;
				_myNetworkView.RPC("SetAgentDestination", RPCMode.Others, _Targets[0].position);
			}
			#endregion


		}
	}

	#region Attack Timer
	bool AttackSpeedTiming = false;
	double countdown;
	void StartAttackSpeedTimer(double time)
	{
		AttackSpeedTiming = true;
		countdown = time;
	}
	#endregion

	void OnTriggerEnter(Collider other){
		if (Network.isServer){

			#region minions collider's actions
				if(this.tag == "Red_Minion"){
					if(other.gameObject.tag == "Blue_Minion" || other.gameObject.tag == "Player_blue_1" || other.gameObject.tag == "Player_blue_2" ||  other.gameObject.tag ==  "Player_blue_3" ||  other.gameObject.tag == "Player_blue_4" ||  other.gameObject.tag == "Player_blue_5"){
						_Targets.Add(other.transform);
					}
				}
				else if(this.tag == "Blue_Minion"){
					if(other.gameObject.tag == "Red_Minion" || other.gameObject.tag =="Player_red_1" || other.gameObject.tag =="Player_red_2" || other.gameObject.tag =="Player_red_3" || other.gameObject.tag =="Player_red_4" || other.gameObject.tag =="Player_red_5") {
						_Targets.Add(other.transform);
					}
				}
			#endregion

		}
	}

	void OnTriggerExit(Collider other){
		if (Network.isServer){

			#region minions collider's actions
				if(this.tag == "Red_Minion"){
					if(other.gameObject.tag == "Blue_Minion" || other.gameObject.tag == "Player_blue_1" || other.gameObject.tag == "Player_blue_2" ||  other.gameObject.tag ==  "Player_blue_3" ||  other.gameObject.tag == "Player_blue_4" ||  other.gameObject.tag == "Player_blue_5"){
						if(_Targets.Count != 0) {
							_Targets.Remove(other.transform);
						}
					}
				}
				else if(this.tag == "Blue_Minion"){
					if(other.gameObject.tag == "Red_Minion" || other.gameObject.tag =="Player_red_1" || other.gameObject.tag =="Player_red_2" || other.gameObject.tag =="Player_red_3" || other.gameObject.tag =="Player_red_4" || other.gameObject.tag =="Player_red_5") {
						if(_Targets.Count != 0) {
							_Targets.Remove(other.transform);
						}
					}
				}
			#endregion

		}
	}

	#region stats calculate

		double CalculatedDamages(){
			double _Critic = Random.value;

			if(_Critic <= _CriticalHitChance){
				return _Domages*_CriticalHitBonus;
			}
			else{
				return _Domages;
			}
		}

	#endregion
	

	#region RPC

		[RPC]
		void SetAgentDestination( Vector3 _newPosition ){
			agent.destination = _newPosition;
		}

	#endregion

}
