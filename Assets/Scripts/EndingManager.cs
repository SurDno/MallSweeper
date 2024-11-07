using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {
    public Image endingImage;

    private void Start() => ShowEnding();
    
    public void ShowEnding() {
        StartCoroutine(EndingCoroutine());
    }

    private IEnumerator EndingCoroutine() {
        var timer = 3f;
        while (timer > 0) {
            timer -= Time.deltaTime;
            var color = endingImage.color;
            color.a = Mathf.Lerp(1f, 0f, timer);
            endingImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        timer = 3f;
        while (timer > 0) {
            timer -= Time.deltaTime;
            var color = endingImage.color;
            color.a = Mathf.Lerp(0f, 1f, timer);
            endingImage.color = color;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
