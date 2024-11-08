using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeCanvasManager : Singleton<FadeCanvasManager> {
	public Image blackScreen;
	
	public IEnumerator SwitchScenes() {
		DontDestroyOnLoad(this.gameObject);
		
		var a = 0f;
		while (a < 1) {
			a += Time.deltaTime;
			blackScreen.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("3DSection");
		
		while (a > 0) {
			a -= Time.deltaTime;
			blackScreen.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
	}
}