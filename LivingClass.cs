using UnityEngine;
using System.Collections;

public class LivingClass: MonoBehaviour {
  public float maxHealth;
  float health;
  public float speed;
  Rigidbody body;
  public float WeaponDamage;
  
  void Awake(){
    health = maxHealth;
    body = gameObject.AddComponent<Rigidbody>();
  }
  
  //figure out some way to pass a damage value to this trigger or figure out how events work and see if its possible to do it like that
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