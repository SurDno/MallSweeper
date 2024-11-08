using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Singleton<PlayerControl> {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer organSprite;
	
    private Vector2 savedDirection;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void FixedUpdate() {
        if (PentagramManager.organs != null && PentagramManager.organs.Count == 5) {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 currentDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movementVector = currentDirection.normalized;

        if(currentDirection.x != 0 || currentDirection.y != 0)
            savedDirection = new Vector2(currentDirection.x, currentDirection.y);
		
        animator.SetFloat("Horizontal", savedDirection.x);
        animator.SetFloat("Vertical", savedDirection.y);
		
        animator.speed = (movementVector == Vector2.zero) ? 0 : 1;
		
        rb.velocity = movementVector * 5;
    }
}