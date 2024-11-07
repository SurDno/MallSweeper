using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement> {
    public Camera cam;
    public PlayerControl player;
    
    void Start() {
        cam = this.GetComponent<Camera>();
        player = PlayerControl.Instance;
    }

    void Update() {
        this.transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.3f);
        var vector3 = transform.position;
        vector3.z = -10;
        transform.position = vector3;
    }
}