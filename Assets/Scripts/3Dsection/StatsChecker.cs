using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsChecker : Singleton<StatsChecker> {
    public float finalHealthMultiplier,
        finalDamageMultiplier,
        finalSpeedMultiplier,
        finalStaminaRecovery,
        finalFireRate,
        finalBossHealthMultiplier,
        finalBossDamageMultiplier,
        finalBossSpeedMultiplier,
        finalBossAttackRate;

    public List<Image> organImages;
    
    void Start() {
        finalHealthMultiplier = finalDamageMultiplier = finalSpeedMultiplier =
            finalFireRate = finalBossHealthMultiplier =
                finalBossDamageMultiplier = finalBossSpeedMultiplier = finalStaminaRecovery = finalBossAttackRate = 1;
        var organs = PentagramManager.organs;
        foreach (var organ in organs) {
            finalHealthMultiplier *= organ.healthMultiplier;
            finalDamageMultiplier *= organ.damageMultiplier;
            finalSpeedMultiplier *= organ.speedMultiplier;
            finalStaminaRecovery *= organ.staminaRecovery;
            finalFireRate *= organ.fireRate;
            finalBossHealthMultiplier *= organ.bossHealthMultiplier;
            finalBossDamageMultiplier *= organ.bossDamageMultiplier;
            finalBossSpeedMultiplier *= organ.bossSpeedMultiplier;
            finalBossAttackRate *= organ.bossAttackRate;
        }

        for (int i = 0; i < organImages.Count; i++) {
            organImages[i].sprite = organs[i].Image;
        }
    }
    
}