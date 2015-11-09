using UnityEngine;
using System.Collections;

public abstract class AbilityEffectTarget : MonoBehaviour 
{
	public abstract bool IsTarget (Tile tile);
}