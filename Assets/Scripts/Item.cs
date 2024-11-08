using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Mallsweeper Unique Shit/Item", order = 1)]
public class Item : ScriptableObject {
    public Sprite Image;
    public string Name;
    public string abilityDescription;
    public float healthMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;
    public float speedMultiplier = 1.0f;
    public float staminaRecovery = 1.0f;
    public float fireRate = 1.0f;
    public float bossHealthMultiplier = 1.0f;
    public float bossDamageMultiplier = 1.0f;
    public float bossSpeedMultiplier = 1.0f;
    public float bossAttackRate = 1.0f;
}