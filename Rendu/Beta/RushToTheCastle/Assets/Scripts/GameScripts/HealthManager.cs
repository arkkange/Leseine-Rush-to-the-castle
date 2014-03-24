using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	#region variables for everyone

		[SerializeField]
		private double Health;

		[SerializeField]
		double maxHealth;
		public double MaxHealth {
			get {
				return maxHealth;
			}
			set {
				maxHealth = value;
			}
		}

		[SerializeField]
		bool Is_Player;

	#endregion

	#region variables for Player Only

		[SerializeField]
		Transform _Red_Team_Respawn;

		[SerializeField]
		Transform _Blue_Team_Respawn;

		[SerializeField]
		Transform camera;


	#endregion

	#region variables for Minions Only
		private bool _Proxy_cart = false;	//this variable is used for minions and update with cart manager
	#endregion

	#region Delegates
		//this delegate is used for minions and update with cart manager
		public delegate void Minion_died_delegate(string Tag);
		public static event Minion_died_delegate Minion_died;

		//creation du delegate player moove (script position targeted)
		public delegate void player_moove_delegate(string PlayerName, Vector3 final_position);
		public static event player_moove_delegate player_moove; //playermoove delegated function
	#endregion
	

	void Start () {
		SetMaxHealthy();
		//if player
		if(this.Is_Player = true){	
			_Red_Team_Respawn = GameObject.Find("PlayerStartPositionsRed").transform;
			_Blue_Team_Respawn = GameObject.Find("PlayerStartPositionsBlue").transform;
			camera = GameObject.Find("Main Camera").transform;
		}
	}

	#region triggers

		void OnTriggerEnter(Collider other){
			#region Minions mecanics
				if(this.tag == "Red_Minion" || this.tag =="Blue_Minion"){
					if(other.tag == "Cart"){
						_Proxy_cart = true;	//permet de savoir si le cart est a proximité
					}
				}
			#endregion

			#region Player mecanics (not used)
			if(this.tag == "Player_red_1" || this.tag == "Player_red_2" || this.tag == "Player_red_3" || this.tag == "Player_red_4" || this.tag == "Player_red_5"){
				if( other.gameObject.tag == "Player_blue_1" || other.gameObject.tag == "Player_blue_2" ||  other.gameObject.tag ==  "Player_blue_3" ||  other.gameObject.tag == "Player_blue_4" ||  other.gameObject.tag == "Player_blue_5"){
					
				}
			}

			if(this.tag == "Player_blue_1" || this.tag == "Player_blue_2" || this.tag == "Player_blue_3" || this.tag == "Player_blue_4" || this.tag == "Player_blue_5"){
				if( other.gameObject.tag =="Player_red_1" || other.gameObject.tag =="Player_red_2" || other.gameObject.tag =="Player_red_3" || other.gameObject.tag =="Player_red_4" || other.gameObject.tag =="Player_red_5") {
					
				}
			}
			#endregion
		}

		void OnTriggerExit(Collider other){
			#region Minions mecanics
				if(this.tag == "Red_Minion" || this.tag =="Blue_Minion"){
					if(other.tag == "Cart"){
						_Proxy_cart = false;
					}
				}
			#endregion

	
		}

	#endregion
	
	// Update is called once per frame
	void Update() {

		#region dead
			if(Health <= 0){
				#region Cart decreese a minion (if proxy cart)
					if(this.tag == "Red_Minion" || this.tag == "Blue_Minion"){
						if(_Proxy_cart == true){
							Minion_died(this.tag);	//delegate envoyé au cart por lui faire un -- du coté de la couleur du cart
						}
						Network.Destroy(this.transform.gameObject);	//we destroy the object
					}
				#endregion

				#region Player mecanics
				if(this.tag == "Player_red_1" || this.tag == "Player_red_2" || this.tag == "Player_red_3" || this.tag == "Player_red_4" || this.tag == "Player_red_5"){
					//respawn red
					this.transform.position = _Red_Team_Respawn.position;
					camera.position = _Red_Team_Respawn.position + Vector3.back * 10;
					SetMaxHealthy();
					player_moove(this.tag, _Red_Team_Respawn.position);
				}
				
				if(this.tag == "Player_blue_1" || this.tag == "Player_blue_2" || this.tag == "Player_blue_3" || this.tag == "Player_blue_4" || this.tag == "Player_blue_5"){
					//respawn blue
					this.transform.position = _Blue_Team_Respawn.position;
					camera.position = _Blue_Team_Respawn.position  + Vector3.back * 10;;
					SetMaxHealthy();
					player_moove(this.tag, _Blue_Team_Respawn.position);
				}
				#endregion

			}
		#endregion
	}

	void SetMaxHealthy(){
		Health = maxHealth;
	}

	#region Functions

		public bool LooseHealth(double quantity){	//return true if the object died
			Health -= quantity;
			if(Health <= 0){
				return true;
			}
			else{
				return false;
			}		
		}

		void GainHealth(double quantity){
			if(Health > MaxHealth){
				SetMaxHealthy();
			}
			
		}

	#endregion

}
