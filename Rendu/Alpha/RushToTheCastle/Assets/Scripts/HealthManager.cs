using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	[SerializeField]
	private double Health;

	private bool _Proxy_cart = false;

	[SerializeField]
	double maxHealth = 100;
	public double MaxHealth {
		get {
			return maxHealth;
		}
		set {
			maxHealth = value;
		}
	}

	public delegate void Minion_died_delegate(string Tag);
	public static event Minion_died_delegate Minion_died; //playermoove delegated function

	// Use this for initialization
	void Start () {
		SetMaxHealthy();
	}

	void OnTriggerEnter(Collider other){
		if(this.tag == "Red_Minion" || this.tag =="Blue_Minion"){
			if(other.tag == "Cart"){
				_Proxy_cart = true;	//permet de savoir si le cart est a proximité
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(this.tag == "Red_Minion" || this.tag =="Blue_Minion"){
			if(other.tag == "Cart"){
				_Proxy_cart = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if(Health <= 0){
			#region Cart decreese a minion (if proxy cart)
			if(_Proxy_cart == true){
				Minion_died(this.tag);	//delegate envoyé au cart por lui faire un -- du coté de la couleur du cart
			}
			#endregion
			Network.Destroy(this.transform.gameObject);	//we destroy the object
		}
	}

	void SetMaxHealthy(){
		Health = maxHealth;
	}

	public bool LooseHealth(double quantity){	//return true if the object died
		Health -= quantity;
		if(Health <= 0){/*
			#region Cart decreese a minion (if proxy cart)
			if(_Proxy_cart == true){
				Minion_died(this.tag);	//delegate envoyé au cart por lui faire un -- du coté de la couleur du cart
			}
			#endregion
			Network.Destroy(this.transform.gameObject);	//we destroy the object
			*/
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



}
