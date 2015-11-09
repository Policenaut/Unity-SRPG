using UnityEngine;
using System.Collections;

public class BaseException : MonoBehaviour
{
	public bool toggle { get; private set; }
	private bool defaultToggle;

	public BaseException(bool defaultToggle)
	{
		this.defaultToggle = defaultToggle;
		toggle = defaultToggle;
	}

	public void FlipToggle()
	{
		toggle = !defaultToggle;
	}
}
