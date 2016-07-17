using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
	public float dayLength;
  bool isday = true;
  Light lt;

	void Start(){
    lt = GetComponent<Light>();
    StartCoroutine(Cycler());
  }
  
  IEnumerator Cycler(){
    while(true){
      yield return new WaitForSeconds(dayLength);
      yield return StartCoroutine(SwitchDayNight(isday));
      isday = !isday;
    }
  }
  
  IEnumerator SwitchDayNight(bool _isday){
    if (_isday){
      for (float i = 1f; i >= 0f; i -= .01f){
        lt.intensity = i;
        yield return new WaitForSeconds(6f/5f);
      }
    }
    if (!_isday){
      for (float i = 0f; i <= 1f; i += .01f){
        lt.intensity = i;
        yield return new WaitForSeconds(6f/5f);
      }
    }
  }
}