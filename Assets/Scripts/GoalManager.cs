using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalManager : Singleton<GoalManager> {
	public List<TMP_Text> goals;

	public void SetNewGoal(string newGoal) {
		foreach (var goal in goals) {
			goal.text = newGoal;
		}
	}
}
