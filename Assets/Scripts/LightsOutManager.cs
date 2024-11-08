using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsOutManager : Singleton<LightsOutManager> {
    public Light2D globalLight;
    public bool isMattsLaptop;

    public void TurnLightsOff() => StartCoroutine(LightsOff());
    
    IEnumerator LightsOff() {
        var minValue = isMattsLaptop ? 1 / 255f : 10 / 255f;
        while (globalLight.color.r > minValue) {
            var color = globalLight.color;
            color.r = color.b = color.g = color.r - (0.33f * Time.deltaTime);
            globalLight.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}