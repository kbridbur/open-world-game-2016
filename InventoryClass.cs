using UnityEngine;
using System.Collections;

public class InventoryClass : MonoBehaviour {
  GameObject[] itemArray;
  public int rows;
  public int columns;
  int maxItemCapacity;
  int numItems;
  
  void Awake(){
    itemArray = new GameObject[rows*columns];
    maxItemCapacity = rows*columns;
  }
  
  public void AddItemToArray(GameObject item){
    if (numItems != maxItemCapacity){
      itemArray[numItems + 1] = item;
      CondenseArray();
    }
    else{
      //some method of erroring or just not accpeting the item
    }
    numItems++;
  }
  
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
  
  public string GetItemNameAtIndex(int index){
    return itemArray[index].ToString();
  }
  
  public GameObject GetItemAtIndex(int index){
    return itemArray[index];
  }

  public void SwapItemIndices(int index1, int index2){
    GameObject temp = itemArray[index1];
    itemArray[index1] = itemArray[index2];
    itemArray[index2] = temp;
    CondenseArray();
  }
  
  public void DisplayInventory(){
    //something
  }
  
  public bool CheckDoesInventoryContain(GameObject item){
    foreach(GameObject checkingFor in itemArray){
      if (checkingFor == item){
        return true;
      }
    }
    return false;
  }
  
  void CondenseArray(){
    for (int i = 1; i < numItems; i ++){
      if (itemArray[i-1] == null){
        SwapItemIndices(i-1, i);
      }
    }
  }
  
}