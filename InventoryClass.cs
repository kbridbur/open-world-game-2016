using UnityEngine;
using System.Collections;
using System;

public class InventoryClass : MonoBehaviour {
  GameObject[] itemArray;
  public int rows;
  public int columns;
  int maxItemCapacity;
  int numItems;
  public event Action CannotAddToFullInventory;
  
  //Create an item array of the correct size
  void Awake(){
    itemArray = new GameObject[rows*columns];
    maxItemCapacity = rows*columns;
  }
  
  //Add an item to the next available slot in the array
  public void AddItemToArray(GameObject item){
    if (numItems != maxItemCapacity){
      itemArray[numItems + 1] = item;
      CondenseArray();
    }
    //If there is no space in the inventory, invoke event. This event should cause the item to drop back onto the ground
    else{
      if (CannotAddToFullInventory != null){
        CannotAddToFullInventory();
      }
    }
    numItems++;
  }
  
  //Removes an item from the array and condenses the array to fill the empty space
  public void RemoveItemFromArray(GameObject item){
    if (CheckDoesInventoryContain(item)){
      for (int i = 0; i < numItems; i++){
        if (itemArray[i] != item){
          itemArray[i] = itemArray[i];
        }
        else{itemArray[i] = null;}
      }
      CondenseArray();
    }
    numItems--;
  }
  
  //Returns a string which is the name of the item at any index in the inventory
  public string GetItemNameAtIndex(int index){
    return itemArray[index].ToString();
  }
  
  //Returns the item at any index of the inventory (will return null if there is no item at this index)
  public GameObject GetItemAtIndex(int index){
    return itemArray[index];
  }
  
  //Swaps the indexes of two items
  public void SwapItemIndices(int index1, int index2){
    GameObject temp = itemArray[index1];
    itemArray[index1] = itemArray[index2];
    itemArray[index2] = temp;
    CondenseArray();
  }
  
  //Checks if an item is within the inventory
  public bool CheckDoesInventoryContain(GameObject item){
    foreach(GameObject checkingFor in itemArray){
      if (checkingFor == item){
        return true;
      }
    }
    return false;
  }
  
  //Checks for null spots in the inventory and backfills them using other items
  void CondenseArray(){
    for (int i = 1; i < numItems; i ++){
      if (itemArray[i-1] == null){
        SwapItemIndices(i-1, i);
      }
    }
  }
  
}