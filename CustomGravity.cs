using UnityEngine;
using System.Collections;


//Allows characters to orient to the ground slope
public class CustomGravity : MonoBehaviour {
  Vector3 down;
	
	// Update is called once per frame
	void FixedUpdate () {
    down = GetGroundNormal();
    RotateToNormal(-down);
	}
  
  Vector3 GetGroundNormal(){
    Vector3 origin = transform.position;
    RaycastHit hit;
    Physics.Raycast(origin, -transform.up, out hit, Mathf.Infinity, 1<<8);
    Debug.DrawRay(origin, -hit.normal, Color.red, 15, false);
    return -hit.normal;
  }
  
  void RotateToNormal(Vector3 direction){
    transform.rotation = Quaternion.FromToRotation(transform.up, direction)*transform.rotation;
  }
}
