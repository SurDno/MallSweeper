using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightRotation : MonoBehaviour {
    private Camera cam;
    public float rotationSpeed = 5f;
    public float positionOffset = 1f; 

    private void Start() {
        cam = Camera.main;
    }
    
    private void Update() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.parent.position;
        direction.z = 0;
        
        // Calculate rotation
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector2 offsetDirection = transform.up; 
        Vector3 newPosition = transform.parent.position + (Vector3)(offsetDirection * positionOffset);
        transform.position = newPosition;
    }
}