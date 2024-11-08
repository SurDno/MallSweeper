using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PentagramManager : Singleton<PentagramManager> {
	private static readonly int UseOutline = Shader.PropertyToID("_UseOutline");
	public SpriteRenderer pentagramImage;
	public float detectionRange = 8f;
	public Material organsGlow;
	public bool canPlaceOrgans;
	public List<SpriteRenderer> organPositions;
	private int index;
	public static List<Item> organs = new List<Item>();
	private static readonly int GlowAmount = Shader.PropertyToID("_GlowAmount");
	public Material pentagramMat;
	public Light2D pentagramLight;
	public GameObject mole;
	public Sprite uiAfterRitual;
	public Image bottombar;

	public void Start() {
		organs = new List<Item>();
	}
	
	public void OrganCollected() {
		var col = Color.white;
		col.a = Mathf.Log(Inventory.Instance.items.Count) / 6f;
		if (Inventory.Instance.allOrgansCollected)
			col.a = 1;
		
		pentagramImage.color = col;
	}
	
	void Update() {
		var distanceToPlayer = Vector2.Distance(transform.position, PlayerControl.Instance.transform.position);
		bool originalValue = canPlaceOrgans;
		canPlaceOrgans = (distanceToPlayer <= detectionRange) && Inventory.Instance.allOrgansCollected;
		organsGlow.SetFloat(UseOutline, canPlaceOrgans ? 1 : 0);
		if (canPlaceOrgans) {
			if (index < 5) {
				var organsLeftToPlace = 5 - index;
				GoalManager.Instance.SetNewGoal($"Choose {organsLeftToPlace} more organs for the ritual.");
			}
		} else if (originalValue && !canPlaceOrgans) { 
			if (index < 5) {
				GoalManager.Instance.SetNewGoal($"Find the pentagram to perform the ritual.");
			}
		}
	}
	
	public void PlaceOrgan(Item organ) {
		if (index == 5)
			return;
		
		organs.Add(organ);
		organPositions[index].sprite = organ.Image;
		index++;
		pentagramMat.SetFloat(GlowAmount, Mathf.Pow(index / 5f, 5f));
		pentagramLight.intensity = Mathf.Pow(index / 5f, 5f) * 3;

		if (index == 5) {
			SummonCreature();
		}
	}

	private void SummonCreature() {
		GoalManager.Instance.SetNewGoal($"HE IS COMING.");
		bottombar.sprite = uiAfterRitual;
		StartCoroutine(MoleCoroutine());
	}

	private IEnumerator MoleCoroutine() {
		yield return new WaitForSeconds(1.5f);
		mole.SetActive(true);		
		yield return new WaitForSeconds(4f);
		FadeCanvasManager.Instance.StartCoroutine(FadeCanvasManager.Instance.SwitchScenes());
	}

	public void OnDisable() {
		pentagramMat.SetFloat(GlowAmount, 0);
	}
}