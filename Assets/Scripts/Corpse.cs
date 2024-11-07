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
    public float detectionRange = 3f; // Distance at which player can see/interact with corpse
    private bool hasBeenSearched = false;
    private Transform playerTransform;
    
    void Start() {
        playerTransform = PlayerControl.Instance.transform;
        sprite.enabled = false;
        sprite.transform.eulerAngles = Vector2.zero;
    }
    
    void Update() {
        // Check distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        // Show sprite if player is close enough and corpse hasn't been searched
        if (!hasBeenSearched) {
            sprite.enabled = distanceToPlayer <= detectionRange;
        }
        
        // Check for interaction
        if (Input.GetKeyDown(KeyCode.E) && distanceToPlayer <= detectionRange && !hasBeenSearched) {
            Search();
        }
    }
    
    private void Search() {
        if (items.Count > 0) {
            Item foundItem = items[Random.Range(0, items.Count)];
            
            Debug.Log($"Found item: {foundItem.name}");
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        }
        
        hasBeenSearched = true;
        sprite.enabled = false;
    }
}