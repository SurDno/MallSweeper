using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Corpse : MonoBehaviour {
    public List<Sprite> sprites;
    public SpriteRenderer sprite;
    public List<Item> items;
    public float detectionRange = 3f; 
    private bool hasBeenSearched = false;
    private Transform playerTransform;
    
    void Start() {
        playerTransform = PlayerControl.Instance.transform;
        sprite.enabled = false;
        sprite.transform.eulerAngles = Vector2.zero;
    }
    
    void Update() {
        if (hasBeenSearched)
            return;
        
        var distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        sprite.enabled = distanceToPlayer <= detectionRange;
        

        if (Input.GetKeyDown(KeyCode.E) && distanceToPlayer <= detectionRange) 
            Search();
    }
    
    private void Search() {
        if (items.Count > 0) {
            Item foundItem = items[Random.Range(0, items.Count)];
            
            Inventory.Instance.AddItem(foundItem);
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        }
        
        hasBeenSearched = true;
        sprite.enabled = false;
    }
}