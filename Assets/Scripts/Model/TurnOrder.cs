using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnOrder
{
	public readonly Unit unit;
	public int counter;

	public TurnOrder (Unit unit)
	{
		this.unit = unit;
	}

	public void Tick ()
	{
		counter += unit.Spd;
	}
}