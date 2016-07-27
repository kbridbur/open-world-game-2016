 using UnityEngine;
 using System.Collections;

 public class AlternativeCamera : MonoBehaviour {
     public Transform target;        //an Object to follow
     public float damping = 6.0f;    //to control the rotation 
     public bool smooth = true;
     public float minDistance = 10.0f;    //How far the target is from the camera
     public string property = "";
     public float baseRadius;
     public float baseHeight;
     float scrollModifier = 1f;
     float height;
     float radius;
     private Color color;
     private float alpha = 1.0f;
     private Transform CameraTransform;
     
     void Awake() {
         height = baseHeight;
         radius = baseRadius;
         CameraTransform = transform;
     }
     // Update is called once per frame
     void Update () {
       float scroll = Input.GetAxis("Mouse ScrollWheel");
       scrollModifier -= scroll*.5f;
       scrollModifier = Mathf.Clamp(scrollModifier, .2f, 1f);
       transform.position = Vector3.Lerp(transform.position, GetTransform(), Time.deltaTime * damping);
       height = baseHeight*scrollModifier;
       radius = baseRadius*scrollModifier;
     }
     void LateUpdate() {
         if(target) {
             if(smooth) {
                 
                 //Look at and dampen the rotation
                 //Look slightly above the target
                 Vector3 camTargetPosition = target.position + target.up*height/4f;
                 Quaternion rotation = Quaternion.LookRotation(camTargetPosition - CameraTransform.position);
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
      backDistance = radius;
      xDistance = -Mathf.Sin((targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      zDistance = -Mathf.Cos((targetVector3.y)*Mathf.Deg2Rad)*backDistance;
      RaycastHit hit;
      Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit, Mathf.Infinity, 1<<8);
      yDistance = transform.position.y - hit.distance + height;
      return new Vector3(xDistance + target.transform.position.x, yDistance, zDistance + target.transform.position.z);
  }
 }