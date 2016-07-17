using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemyAI : MonoBehaviour {
  
  public List<string> targetTags;
  public LayerMask targetLayer;
  GameObject currentTarget = null;
  
  public float sqrSightRadius;
  public float sqrWeaponRange;
  public float maxSightAngle;
  public float moveSpeed;
  public float rotationSpeed;
 
  GameObject[] specificTargetTags;
  List<GameObject> possibleTargets;
  
  void Start(){
    //create a list of possible game objects that can be targets based on tag
    foreach (string targetTag in targetTags){
      specificTargetTags = GameObject.FindGameObjectsWithTag(targetTag);
      foreach (GameObject possibleTarget in specificTargetTags){
        possibleTargets.Add(possibleTarget);
      }
    }
    StartCoroutine(Base());
  }
  
  IEnumerator Base(){
    while (true){
      //search for a target
      currentTarget = FindTarget();
      //if target has died, reset
      if (currentTarget != null){
        if (!currentTarget.activeSelf){
          currentTarget = null;
        }
        //move to target and attack if possible
        yield return StartCoroutine(MoveToTargetAndAttack(currentTarget));
      }
      yield return null;
    }
  }
  
  IEnumerator MoveToTargetAndAttack(GameObject currentTarget){
    //If able, use weapon
    if ((transform.position - currentTarget.transform.position).sqrMagnitude < sqrWeaponRange){
      UseWeapon(currentTarget);
    }
    //otherwise move towards target
    else{
      transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, moveSpeed);
      Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, transform.position-currentTarget.transform.position);
      transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }
    return null;
  }
  
  public GameObject FindTarget(){
    foreach (GameObject possibleTarget in possibleTargets){
      if ((possibleTarget.transform.position - transform.position).sqrMagnitude < sqrSightRadius && Mathf.Abs(Vector3.Angle(transform.forward, possibleTarget.transform.position)) < maxSightAngle){
        return possibleTarget;
      }
    }
    return null;
  }

  void UseWeapon(GameObject target){
    //some animation and create an event telling gaemobject target to take damage
  }
  
}
/*PseudoCode:
CREATE A STATE TREE.
1. Detect if player is in range using raycasts in a cone or a circle
  If player is detected:
    option a. Move if needed to get into weapon range, otherwise use weapon
    option b. Move into defensive formation around leader
    option c. Run
  If no player is detected move a set distance down the path or in set path (patrolling enemies)
  
  
  
Movement:
raycast forward distance that will be moved this frame.
If player is in the way only move far enough to be in weapon range (maybe slightly more to compensate for player movement)
If anything else is in the way turn slightly to try and avoid it and move forward less than normal

Once in weapon range:
Use weapon once
Recheck where player is
  */
