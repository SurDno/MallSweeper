using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

public void LoadScene(string SceneName)
{
    SceneManager.LoadScene("SampleScene");
}

public void Quit()
{
    Application.Quit();
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
