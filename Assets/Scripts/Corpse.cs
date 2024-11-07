using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Corpse : MonoBehaviour {
    public List<Sprite> sprites;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            this.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }
}
