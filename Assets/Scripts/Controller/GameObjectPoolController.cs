using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolController : MonoBehaviour 
{
	#region Fields / Properties
	static GameObjectPoolController Instance
	{
		get
		{
			if (instance == null)
				CreateSharedInstance();
			return instance;
		}
	}
	static GameObjectPoolController instance;
	
	static Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();
	#endregion
	
	#region MonoBehaviour
	void Awake ()
	{
		if (instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}
	#endregion
	
	#region Public
	public static void SetMaxCount (string key, int maxCount)
	{
		if (!pools.ContainsKey(key))
			return;
		PoolData data = pools[key];
		data.maxCount = maxCount;
	}

	public static bool AddEntry (string key, GameObject prefab, int prepopulate, int maxCount)
	{
		if (pools.ContainsKey(key))
			return false;
		
		PoolData data = new PoolData();
		data.prefab = prefab;
		data.maxCount = maxCount;
		data.pool = new Queue<Poolable>(prepopulate);
		pools.Add(key, data);
		
		for (int i = 0; i < prepopulate; ++i)
			Enqueue( CreateInstance(key, prefab) );
		
		return true;
	}
	
	public static void ClearEntry (string key)
	{
		if (!pools.ContainsKey(key))
			return;
		
		PoolData data = pools[key];
		while (data.pool.Count > 0)
		{
			Poolable obj = data.pool.Dequeue();
			GameObject.Destroy(obj.gameObject);
		}
		pools.Remove(key);
	}
	
	public static void Enqueue (Poolable sender)
	{
		if (sender == null || sender.isPooled || !pools.ContainsKey(sender.key))
			return;
		
		PoolData data = pools[sender.key];
		if (data.pool.Count >= data.maxCount)
		{
			GameObject.Destroy(sender.gameObject);
			return;
		}
		
		data.pool.Enqueue(sender);
		sender.isPooled = true;
		sender.transform.SetParent(Instance.transform);
		sender.gameObject.SetActive(false);
	}
	
	public static Poolable Dequeue (string key)
	{
		if (!pools.ContainsKey(key))
			return null;
		
		PoolData data = pools[key];
		if (data.pool.Count == 0)
			return CreateInstance(key, data.prefab);
		
		Poolable obj = data.pool.Dequeue();
		obj.isPooled = false;
		return obj;
	}
	#endregion
	
	#region Private
	static void CreateSharedInstance ()
	{
		GameObject obj = new GameObject("GameObject Pool Controller");
		DontDestroyOnLoad(obj);
		instance = obj.AddComponent<GameObjectPoolController>();
	}
	
	static Poolable CreateInstance (string key, GameObject prefab)
	{
		GameObject instance = Instantiate(prefab) as GameObject;
		Poolable p = instance.AddComponent<Poolable>();
		p.key = key;
		return p;
	}
	#endregion
}