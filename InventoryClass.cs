using UnityEngine;
using System.Collections;

public class InventoryClass : MonoBehaviour {
  GameObject[] itemArray;
  int rows;
  int columns;
  int maxItemCapacity = rows*columns;
  int numItems;
  
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
    if (itemArray.Contains(item)){
      itemArray.Remove(item);
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
  
  public void DisplayInventory{
    //something
  }
  
  void CondenseArray(){
    for (int i = 1; i < numItems; i ++){
      if (itemArray[i-1] == null){
        SwapItemIndices(i-1, i);
      }
    }
  }
  
}