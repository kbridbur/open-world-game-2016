 using UnityEngine;
 using System.Collections;

 public class AlternativeCamera : MonoBehaviour {
     public Transform target;        //an Object to follow
     public float damping = 6.0f;    //to control the rotation 
     public bool smooth = true;
     public float minDistance = 10.0f;    //How far the target is from the camera
     public string property = "";
     public float radius;
     public float height;
     
     private Color color;
     private float alpha = 1.0f;
     private Transform CameraTransform;
     
     void Awake() {
         CameraTransform = transform;
     }
     // Update is called once per frame
     void Update () {
         transform.position = Vector3.Lerp(transform.position, GetTransform(), Time.deltaTime * damping);
     }
     void LateUpdate() {
         if(target) {
             if(smooth) {
                 
                 //Look at and dampen the rotation
                 Quaternion rotation = Quaternion.LookRotation(target.position - CameraTransform.position);
                 CameraTransform.rotation = Quaternion.Slerp(CameraTransform.rotation, rotation, Time.deltaTime * damping);
             }
             else { //Just look at
                 CameraTransform.rotation = Quaternion.FromToRotation(-Vector3.forward, (new Vector3(target.position.x, target.position.y, target.position.z) - CameraTransform.position).normalized);
                 
                 float distance = Vector3.Distance(target.position, CameraTransform.position);
                 
                 if(distance < minDistance) {
                     alpha = Mathf.Lerp(alpha, 0.0f, Time.deltaTime * 2.0f);
                 }
                 else {
                     alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime * 2.0f);
                     
                 }
             }
         }
     }
     
    Vector3 GetTransform(){
      float xDistance;
      float yDistance;
      float zDistance;
      float backDistance;
      Quaternion targetQuaternion = target.transform.rotation;
      Vector3 targetVector3 = targetQuaternion.eulerAngles;
      /*//get distance to be from the object based on pitch
      yDistance = Mathf.Sign(pitch)*Mathf.Sin(Mathf.Abs(pitch*Mathf.Deg2Rad))*radius;
      backDistance = -Mathf.Cos(Mathf.Abs(pitch*Mathf.Deg2Rad))*radius;
      xDistance = Mathf.Sin((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      zDistance = Mathf.Cos((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      return new Vector3(xDistance + target.transform.position.x, yDistance + target.transform.position.y, zDistance + target.transform.position.z);*/
      backDistance = radius;
      xDistance = -Mathf.Sin((targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      zDistance = -Mathf.Cos((targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      RaycastHit hit;
      Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit, Mathf.Infinity, 1<<8);
      yDistance = transform.position.y - hit.distance + height;
      return new Vector3(xDistance + target.transform.position.x, yDistance, zDistance + target.transform.position.z);
  }
 }