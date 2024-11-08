using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameRandomizer : MonoBehaviour {
    public List<GameObject> names;

    void Start() {
        foreach (var o in names)
            o.SetActive(false);
        
        names[Random.Range(0, names.Count)].SetActive(true);
    }
}