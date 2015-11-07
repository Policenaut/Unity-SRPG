using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{

	#region Notifications
	public static string WillChangeNotificaion(StatTypes type)
	{
		if (!_willChangeNotifications.ContainsKey(type))
		{ _willChangeNotifications.Add(type, string.Format("Stats.{0}WillChange", type.ToString())); }
		return _willChangeNotifications[type];
	}

	public static string DidChangeNotificaion(StatTypes type)
	{
		if (!_didChangeNotifications.ContainsKey(type))
		{ _didChangeNotifications.Add(type, string.Format("Stats.{0}DidCHange", type.ToString())); }
		return _didChangeNotifications[type];
	}

	static Dictionary<StatTypes, string> _willChangeNotifications = new Dictionary<StatTypes, string>();
	static Dictionary<StatTypes, string> _didChangeNotifications = new Dictionary<StatTypes, string>();
	#endregion

	#region Fields / Properties
	public int this[StatTypes s]
	{
		get { return _data[(int)s]; }
		set { SetValue(s, value, true); }
	}
	int[] _data = new int[(int)StatTypes.Count];
	#endregion

	#region Public
	public void SetValue(StatTypes type, int value, bool allowExceptions)
	{
		int oldValue = this[type];
		if (oldValue == value)
		{ return; }

		if (allowExceptions)
		{
			//TODO: exceptions to the rules
		}

		_data[(int)type] = value;
		//TODO: Post Notificaion
	}
	#endregion
}
