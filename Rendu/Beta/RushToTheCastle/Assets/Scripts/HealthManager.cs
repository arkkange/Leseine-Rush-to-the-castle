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

	#endregion

	#region variables for Player Only
		[SerializeField]
		Transform RespawnPoint = null;	//this is the respawn point for player
	#endregion

	#region variables for Minions Only
		private bool _Proxy_cart = false;	//this variable is used for minions and update with cart manager
	#endregion

	#region Delegates
		//this delegate is used for minions and update with cart manager
		public delegate void Minion_died_delegate(string Tag);
		public static event Minion_died_delegate Minion_died;
	#endregion
	

	void Start () {
		SetMaxHealthy();
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

			#region Player mecanics

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

			#region Player mecanics
			
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

		void GainHealth(float quantity){
			if(Health > MaxHealth){
				SetMaxHealthy();
			}
			
		}

	#endregion

}
