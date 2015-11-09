using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConversationPanel : MonoBehaviour
{
	public Text message;
	public Image speaker;
	public GameObject arrow;
	public Panel panel;

	void Start()
	{
		Vector3 pos = arrow.transform.localPosition;
		arrow.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z);
		Tweener t = arrow.transform.MoveToLocal(new Vector3(pos.x, pos.y - 5, pos.z), 0.5f, EasingEquations.EaseInQuad);
		t.loopType = EasingControl.LoopType.PingPong;
		t.loopCount = -1;
	}

	public IEnumerator Display(SpeakerData sd)
	{
		speaker.sprite = sd.speaker;
		speaker.SetNativeSize();

		for (int i = 0 ; i < sd.messages.Count ; ++i)
		{
			message.text = sd.messages[i];
			arrow.SetActive(i + 1 < sd.messages.Count);
			yield return null;
		}
	}
}