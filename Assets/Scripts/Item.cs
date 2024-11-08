using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Mallsweeper Unique Shit/Item", order = 1)]
public class Item : ScriptableObject {
    public Sprite Image;
    public string Name;
    public string abilityDescription;
    public float healthMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;
    public float damageReceivedMultiplier = 1.0f;
    public float damageIgnoreChance = 0.0f;
    public float doubleAttackChance = 0.0f;
    public float blockIgnoreChance = 0.0f;
    public float attackThroughDamageMultiplier = 0.0f;
}