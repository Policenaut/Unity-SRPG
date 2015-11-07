using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	#region Fields

	public int HP
	{
		get { return stats[StatTypes.HP]; }
		set { }
	}

	Stats stats;

	#endregion
}