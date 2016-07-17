using UnityEngine;
using System.Collections;
using System;

public class CameraBehavior : MonoBehaviour {
  public Camera mainCamera;
  public GameObject target;
  public float slopeConstant;
  public float maxRadiusConstant;
  float radiusConstant;
  RaycastHit[] playerRayCasts;
  float slopeAngle;
  public AnimationCurve pitchToFOV;
  public float basePitch;
  float pitch;
  public float height;
  public float yaw;
  float prevYaw;
  Quaternion _rotation;
  Quaternion prevRotation;
  Vector3 _transform;
  Vector3 prevTransform;
  float FOV = 60;
  float prevFOV;
  float radius;
  float backDistance;
  float xDistance;
  float yDistance;
  float zDistance;
  float yTransform;
	void Start () {
    prevTransform = transform.position;
    prevRotation = transform.rotation;
    playerRayCasts = new RaycastHit[1];
	}
	
	void Update () {
    //Set current quaternion and transform as previous
    GetPlayerRayCasts();
    //I say prev but in reality they're current
    prevYaw = yaw;
    prevRotation = _rotation;
    prevTransform = _transform;
    //pitch = pitchFromRays();
    radiusConstant = maxRadiusConstant;
    radius = radiusConstant;
    CheckBlockedVeiw();
    _transform = GetTransform();
    pitch = PitchFromTransform();
    _rotation = GetRotation();
    SetTransform();
    //set position to somewhere between previous and newly calculated position based on their difference
	}
  
  //Gathers data regarding the surroundings of the player to be used in determining camera angle
  public void GetPlayerRayCasts(){
    //Ray only detects ground
    RaycastHit hit;
    Vector3 origin = target.transform.position;
    Vector3 worldDown = new Vector3(0,-1,0);
    Physics.Raycast(origin, worldDown, out hit, Mathf.Infinity, 1<<8);
    playerRayCasts[0] = hit;
  }
  
  public void CheckBlockedVeiw(){
    Vector3[] yawChecks = new Vector3[3];
    Vector3 origin = target.transform.position;
    Vector3 direction = transform.position - origin;
    float directionMagnitute = direction.magnitude;
    Vector3 Offset = new Vector3(direction.x * Mathf.Tan(30*Mathf.Deg2Rad),0, direction.z * Mathf.Tan(30*Mathf.Deg2Rad));
    yawChecks[0] = direction;
    //Vectors currently wrong, go up and down instead of to the sides
    yawChecks[1] = (direction + Offset).normalized * directionMagnitute;
    yawChecks[2] = (direction - Offset).normalized * directionMagnitute;  
    float bestRadius = 0f;
    float _radius;
    Vector3 bestDirection = direction;
    RaycastHit hit;
    foreach (Vector3 vector in yawChecks){
      _radius = radius;
      for (float i = 1; i >= .25f; i -= .05f){
        Debug.DrawRay(origin, vector, Color.red, i*radius, false);
        if (Physics.Raycast(origin, vector, out hit, i*radius)){
          _radius *= (i-.05f);
        }
        else{
          if (_radius > bestRadius){
            bestRadius = _radius;
            bestDirection = vector;
          }
          break;
        }
      }
    }
    radius = bestRadius;
    if (bestDirection == yawChecks[1]){
      yaw += 30;
    }
    if (bestDirection == yawChecks[1]){
      yaw -= 30;
    }
  }
  //TRY CHANGING THIS TO PITCH DEPENDS ON HEIGHT AND HEIGHT DEPENDS SOLELY ON THE CAMERA BEING A CERTAIN DISTANCE OFF THE GROUND
  float pitchFromRays(){
    float _pitch;
    Vector3 groundNormal = playerRayCasts[0].normal;
    float slopeAngle = Vector3.Angle(groundNormal, new Vector3(0,1,0));
    _pitch = basePitch + slopeAngle * slopeConstant;
    if (_pitch > 60f){
      return 60f;
    }
    if (_pitch < 0f){
      return 0f;
    }
    return _pitch;
  }
  
  float PitchFromTransform(){
    return Vector3.Angle(transform.forward, transform.position-target.transform.position);
  }
    
  Vector3 GetTransform(){
    Quaternion targetQuaternion = target.transform.rotation;
    Vector3 targetVector3 = targetQuaternion.eulerAngles;
    /*//get distance to be from the object based on pitch
    yDistance = Mathf.Sign(pitch)*Mathf.Sin(Mathf.Abs(pitch*Mathf.Deg2Rad))*radius;
    backDistance = -Mathf.Cos(Mathf.Abs(pitch*Mathf.Deg2Rad))*radius;
    xDistance = Mathf.Sin((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
    zDistance = Mathf.Cos((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
    return new Vector3(xDistance + target.transform.position.x, yDistance + target.transform.position.y, zDistance + target.transform.position.z);*/
    backDistance = radius;
    xDistance = Mathf.Sin((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
    zDistance = Mathf.Cos((yaw+targetVector3.y)*Mathf.Deg2Rad)*backDistance;
    RaycastHit hit;
    Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit, Mathf.Infinity, 1<<8);
    yDistance = transform.position.y - hit.distance + height;
    return new Vector3(xDistance + target.transform.position.x, yDistance, zDistance + target.transform.position.z);
  }
  
  Quaternion GetRotation(){
    Quaternion targetQuaternion = target.transform.rotation;
    Vector3 targetVector3 = targetQuaternion.eulerAngles;
    if (targetVector3.y == 360f){
      return Quaternion.Euler(pitch + targetVector3.x, prevYaw + targetVector3.y, 180);
    }
    return Quaternion.Euler(pitch + targetVector3.x, yaw + targetVector3.y, 180);
  }
  
  float GetFOV(){
    return 60*pitchToFOV.Evaluate((pitch+10f)/70f);
  }
  
  void SetTransform(){
    //redo this
    Vector3 transformDiff = _transform - prevTransform;
    Vector3 targetQuat = _rotation.eulerAngles;
    Vector3 prevQuat = prevRotation.eulerAngles;
    Vector3 rotationDiff = targetQuat-prevQuat;
    transform.rotation = _rotation;//Quaternion.RotateTowards(prevRotation, _rotation, .00005f);
    transform.position = _transform;//Vector3.MoveTowards(prevTransform , _transform, .001f);
    //mainCamera.fieldOfView = FOV - .25f*(FOV - prevFOV);
  }
}