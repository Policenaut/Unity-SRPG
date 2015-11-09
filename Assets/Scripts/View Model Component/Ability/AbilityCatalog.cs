using UnityEngine;
using System.Collections;

/// <summary>
/// Assumes that all direct children are categories
/// and that the direct children of categories
/// are abilities
/// </summary>
public class AbilityCatalog : MonoBehaviour 
{
	public int CategoryCount ()
	{
		return transform.childCount;
	}

	public GameObject GetCategory (int index)
	{
		if (index < 0 || index >= transform.childCount)
			return null;
		return transform.GetChild(index).gameObject;
	}

	public int AbilityCount (GameObject category)
	{
		return category != null ? category.transform.childCount : 0;
	}

	public Ability GetAbility (int categoryIndex, int abilityIndex)
	{
		GameObject category = GetCategory(categoryIndex);
		if (category == null || abilityIndex < 0 || abilityIndex >= category.transform.childCount)
			return null;
		return category.transform.GetChild(abilityIndex).GetComponent<Ability>();
	}
}