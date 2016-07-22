using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class LivingClass : MonoBehaviour {
  public float maxHealth;
  float health;
  public float speed;
  Rigidbody body;
  public float WeaponDamage;
  //Add animation for taking damage and dying
  
  void Awake(){
    health = maxHealth;
    body = GetComponent<Rigidbody>();
  }
  
  public void TakeDamage(float damageAmount){
    health -= damageAmount;
    if (health <= 0f){
      Die();
    }
  }
  
  public virtual void Die(){
    Destroy(gameObject);
  }
}