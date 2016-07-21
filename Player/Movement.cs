using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
  public float speed;
  
  //Simple script, needs to be updated for wall climbing and such later in development
  //When working on this script be careful to consider the method being used for gravity (may want to create a public bool about being grounded and have that change the gravity script)
  
	void Update () {
    
    //adjusts current rotation based on mouse lateral input, this is the way players control the camera
    Vector3 currentRotation = transform.rotation.eulerAngles;
    float currentYaw = currentRotation.y;
    float newYaw = currentYaw;
    newYaw += Input.GetAxis("Mouse X");
    transform.rotation = Quaternion.Euler(currentRotation.x, newYaw, currentRotation.z);
    
    //adjusts position based on arrow key inputs
    if (Mathf.Abs(Input.GetAxis("Vertical")) > 0){
      transform.position += Mathf.Sign(Input.GetAxis("Vertical"))*transform.forward*speed;
    }
    if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0){
      transform.position += Mathf.Sign(Input.GetAxis("Horizontal"))*transform.right*speed;
    }
  }
}
