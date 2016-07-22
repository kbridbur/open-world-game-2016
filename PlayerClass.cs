using UnityEngine;
using System.Collections;
using System;

public class PlayerClass : LivingClass {
  public InventoryClass playerInventory;
  public event Action GameOver;
  public event Action Pause;
  public event Action Unpause;
  bool paused = false;
  
  //TODO create an event manager which will switch the camera to the UI and stop all objects upon pausing
  void Update(){
    if (Input.GetButtonDown(KeyCode.Escape.ToString())){
      if (paused){Unpause();}
      else{Pause();}
    }
  }
  
  public override void Die(){
    base.Die();
    GameOver();
  }
}