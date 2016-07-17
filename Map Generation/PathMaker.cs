using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathMaker {
  
  public static int[,] GeneratePath(float[,] noiseMap, int mapWidth, int mapHeight, int seed){
    
    int[,] pathPoints = new int[mapWidth,mapHeight];
    
    System.Random prng = new System.Random (seed);
    
    List<Vector2> pointsToConnect = GenerateStartingPoints(mapWidth, mapHeight, noiseMap);
    
    for (int i = 0; i < pointsToConnect.Count-1; i++){
      List<Vector2> AtoBPoints = new List<Vector2>();
      //Create a list of valid points connecting A and B
      AtoBPoints = GetConnectingPoints(pointsToConnect[i], pointsToConnect[i+1], mapWidth, mapHeight, noiseMap);
      //Set those points as points in the path to be returned
      foreach (Vector2 point in AtoBPoints){
        pathPoints[(int)point.x, (int)point.y] = 1;
      }
    }
    
    return pathPoints;
  }
  //Test a points to see if it is valid to be in the path
  public static bool IsValidPoint(Vector2 pointToCheck, int mapWidth, int mapHeight, float[,] noiseMap){
    //Is in boundries?
    if (pointToCheck.x >= mapWidth-1 || pointToCheck.x <= 0 || pointToCheck.y >= mapHeight-1 || pointToCheck.y <=0){
      return false;
    } 
    //Is height correct?
    if (noiseMap[(int)pointToCheck.x, (int)pointToCheck.y] > .6f || noiseMap[(int)pointToCheck.x, (int)pointToCheck.y] < .4f){
      return false;
    }
    return true;
  }
  
  //Use a* to find best valid path between points
  public static List<Vector2> GetConnectingPoints(Vector2 startPoint, Vector2 goalPoint, int mapWidth, int mapHeight, float[,] noiseMap){
    /*Debug.Log(startPoint);
    Debug.Log(goalPoint);
    List<Vector2> connectingPoints = new List<Vector2>();
    List<Node> toCheckQueue = new List<Node>();
    List<Vector2> checkedLocations = new List<Vector2>();
    Node startNode = new Node();
    startNode.location = startPoint;
    startNode.parent = null;
    startNode.cost = Vector2.Distance(startPoint, goalPoint);
    toCheckQueue.Add(startNode);
    int i = 0;
    while (toCheckQueue.Count > 0){
      //Find lowest cost node of the open nodes
      /*Node lowestCostNode = startNode;
      float lowestCost = Mathf.Infinity;
      foreach (Node _node in toCheckQueue){
        if (_node.cost < lowestCost){
          lowestCost = _node.cost;
          lowestCostNode = _node;
        }
      }
      //Remove the lowest cost node from the queue and add its successors
      Node checking = toCheckQueue[0];
      toCheckQueue.Remove(toCheckQueue[0]);
      checkedLocations.Add(checking.location);
      foreach (Node _node in GetSuccessors(checking, goalPoint, startPoint)){
        Node replaceNode = null;
        bool replace = false;
        //if a successor is the goal point then return the path of node locations
        if (_node.location == goalPoint){
          Node currentNode = _node; 
          while (currentNode != startNode){
            connectingPoints.Add(currentNode.location);
            currentNode = currentNode.parent;
          }
          return connectingPoints;
        }
        if (!checkedLocations.Contains(_node.location) && IsValidPoint(_node.location, mapWidth, mapHeight, noiseMap)){
          foreach (Node node2 in toCheckQueue){
            if (node2.location == _node.location){
              if (_node.cost < node2.cost){
                replaceNode = node2;
                replace = true;
              }
            }
          }
          if (replace){
            toCheckQueue.Add(_node);
            toCheckQueue.Remove(replaceNode);
          }
          else{
            toCheckQueue.Add(_node);
          }
        } 
      }
      i++;
    }
    return connectingPoints;*/
  List<Vector2> connectingPoints = new List<Vector2>();
  List<Vector2> closedLocs = new List<Vector2>();
  List<Node> openNodes = new List<Node>();
  Node startNode = new Node(startPoint, null, 0f, Vector2.Distance(startPoint, goalPoint));
  openNodes.Add(startNode);
  int sth = 0;
  while (sth < 500){
    sth ++;
    Node currentNode = openNodes[0];
    
    foreach (Node node in openNodes){
      if (node.fCost < currentNode.fCost || node.fCost == currentNode.fCost && node.gCost < currentNode.gCost){
        currentNode = node;
      }
    }
    Debug.Log(currentNode.location);
    openNodes.Remove(currentNode);
    closedLocs.Add(currentNode.location);
    
    foreach (Node childNode in GetSuccessors(currentNode, startPoint, goalPoint)){
      
      bool alreadyIn = false;
      
      if (childNode.location == goalPoint){
        Node currentParentNode = childNode;
        while (currentParentNode != null){
          connectingPoints.Add(currentParentNode.location);
          currentParentNode = currentParentNode.parent;
        }
        return connectingPoints;
      }
      
      if (!IsValidPoint(childNode.location, mapWidth, mapHeight, noiseMap) && closedLocs.Contains(childNode.location)){
        continue;
      }
      
      foreach (Node sameNode in openNodes){
        if (sameNode.location == childNode.location){
          alreadyIn = true;
          if (sameNode.sCost > childNode.sCost){
            sameNode.parent = childNode.parent;
            sameNode.sCost = childNode.sCost;
          }
          break;
        }
      }
      
      if (!alreadyIn){
        openNodes.Add(childNode);
      }
    }
  }
  return connectingPoints;
  }
  
  public static List<Vector2> GenerateStartingPoints(int mapWidth, int mapHeight, float[,] noiseMap){
    List<Vector2> startingPoints = new List<Vector2>();
    
    System.Random rng = new System.Random (0);
    
    /*for (float i = 1f; i < 8f; i++){
    for (float j = 1f; j < 8f; j++){
      Vector2 baseLoc = new Vector2((int)Mathf.Lerp(0, mapWidth, i/8f), (int)Mathf.Lerp(0, mapHeight, j/8f));
      if (IsValidPoint(baseLoc, mapWidth, mapHeight, noiseMap)){
        startingPoints.Add(baseLoc);
        Debug.Log(baseLoc);
      }
    }
    }*/
    while (startingPoints.Count < 2){
      Vector2 checkPoint = new Vector2(rng.Next(0, mapWidth-1), rng.Next(0, mapHeight-1));
      if (IsValidPoint(checkPoint, mapWidth, mapHeight, noiseMap)){
        startingPoints.Add(checkPoint);
      }
    }
    return startingPoints;
  }
  
  //Check points around a point to see if they are valid
  public static List<Node> GetSuccessors(Node parentNode, Vector2 start, Vector2 goal){
    List<Node> successors = new List<Node>();
    
    float parentLocX = parentNode.location.x;
    float parentLocY = parentNode.location.y;
    
    for (int i = -1; i < 1; i++){
      for (int j = -1; j < 1; j++){
        if (i == 0 && j == 0){
          continue;
        }
        Vector2 childLoc = new Vector2(parentLocX + i, parentLocY + j);
        float sCost = Mathf.Abs(childLoc.x - start.x) + Mathf.Abs(childLoc.y - start.y);
        float gCost = Mathf.Abs(childLoc.x - goal.x) + Mathf.Abs(childLoc.y - goal.y); 
        successors.Add(new Node(childLoc, parentNode, sCost, gCost));
      }
    }
    return successors;
  }
  
  public class Node{
    public Vector2 location;
    public Node parent;
    public float sCost;
    public float gCost;
    
    public Node(Vector2 _location, Node _parent, float _sCost, float _gCost){
      location = _location;
      parent = _parent;
      sCost = _sCost;
      gCost = _gCost;
    }
    
    public float fCost{
      get{
        return sCost + gCost;
      }
    }
  }
}
