using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
	[MenuItem("Assets/Create/Conversation Data")]
	public static void CreateConversationData ()
	{
		ScriptableObjectUtility.CreateAsset<ConversationData> ();
	}
}