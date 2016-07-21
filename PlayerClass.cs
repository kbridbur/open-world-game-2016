using UnityEngine;
using System.Collections;

public class PlayerClass : LivingClass {
  public InventoryClass playerInventory;
  
  public override void Die(){
    base.Die();
    //create an event to end the game
  }
}