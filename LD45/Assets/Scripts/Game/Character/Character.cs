using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
	public BaseStat HitPoints;
	public BaseStat StaminaPoints;
	public BaseStat ManaPoints;
	public BaseStat SleepPoints;
	public BaseStat HungerPoints;
	public ExperienceStat ExperiencePoints;

	public float speed;
}
