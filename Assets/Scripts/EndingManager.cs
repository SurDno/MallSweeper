using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {
    public List<Image> endingImages;

    private void Start() => ShowEnding();
    
    public void ShowEnding() {
        StartCoroutine(EndingCoroutine());
    }

    private IEnumerator EndingCoroutine() {
        var timer = 3f;
        while (timer > 0) {
            timer -= Time.deltaTime;
            var color = Color.white;
            color.a = Mathf.Lerp(1f, 0f, timer);
            foreach(var endingImage in endingImages)
                endingImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        timer = 3f;
        while (timer > 0) {
            timer -= Time.deltaTime;
            var color = Color.white;
            color.a = Mathf.Lerp(0f, 1f, timer);
            foreach(var endingImage in endingImages)
                endingImage.color = color;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
