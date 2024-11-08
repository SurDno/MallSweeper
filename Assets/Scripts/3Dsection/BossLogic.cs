using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BossLogic : MonoBehaviour {
    public enum BehaviourType {
        Circling,
        Towards,
        Idle
    }

    public BehaviourType currentType = BehaviourType.Towards;
    public float movementSpeed;

    public Rigidbody rb;
    public Animator anim;
    public Transform playerTransform;
    public float bossHealth;
    public Image bossHealthBar;
    public bool canKick = true;
    
    void Start() {
        bossHealth *= StatsChecker.Instance.finalBossHealthMultiplier;
        movementSpeed *= StatsChecker.Instance.finalBossSpeedMultiplier;
        
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerTransform = FirstPersonController.Instance.transform;
    }
    void Update() {
        if (bossHealth <= 0)
            return;
        
        var player = FirstPersonController.Instance.gameObject;
        
        transform.LookAt(player.transform);
        transform.eulerAngles += new Vector3(0, 90, 0);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (!canKick) {
            rb.velocity = Vector2.zero;
            return;
        }
        
        rb.velocity = currentType switch {
            BehaviourType.Circling => transform.forward * movementSpeed,
            BehaviourType.Towards => (-transform.right) * movementSpeed,
            BehaviourType.Idle => Vector2.zero,
            _ => Vector2.zero
        };

        if (Vector3.Distance(transform.position, playerTransform.position) < 2f ) {
            StartCoroutine(Kick());
        }
    }

    public IEnumerator Kick() {
        if (bossHealth <= 0)
            yield break;
        
        var bossDamage = 15f * StatsChecker.Instance.finalBossDamageMultiplier;
        FirstPersonController.Instance.DealDamage(bossDamage);
        anim.SetBool("Punch" + (UnityEngine.Random.Range(1, 5)).ToString(), true);
        canKick = false;
        yield return new WaitForSeconds(3f * StatsChecker.Instance.finalBossAttackRate);
        canKick = true;
        anim.SetBool("Punch1", false);
        anim.SetBool("Punch2", false);
        anim.SetBool("Punch3", false);
        anim.SetBool("Punch4", false);
    }

    public void OnTriggerEnter(Collider other) {
        if (bossHealth <= 0)
            return;
        
        if (other.CompareTag("Respawn")) {
            Destroy(other.gameObject);
            var damage = 1 * StatsChecker.Instance.finalDamageMultiplier;
            DealDamage(damage);
        }
    }

    public void DealDamage(float damage) {
        bossHealth -= damage;
        bossHealthBar.fillAmount = bossHealth / (100 * StatsChecker.Instance.finalBossHealthMultiplier);

        if (bossHealth <= 0) {
            EndingManager.Instance.ShowEnding();
            anim.SetBool("Death", true);
            Destroy(this.gameObject);
        }
    }
}
