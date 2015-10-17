using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Demo : MonoBehaviour 
{
	public Spacer spacer;

	IEnumerator Start ()
	{
		bool show = true;
		for (int i = 0; i < 10; ++i)
		{
			spacer.Toggle(show);
			show = !show;
			yield return new WaitForSeconds(UnityEngine.Random.value);
		}
	}


//	public AbilityMenuPanel menu;
//
//	void Start ()
//	{
//		menu.Load ("Action", "Move", "Ability", "Wait");
//		menu.SetLocked(1, true);
//	}

//	public ConversationPanel cp;
//
//	IEnumerator Start ()
//	{
//		yield return new WaitForSeconds(1);
//		Queue<string> pages = new Queue<string>();
//		pages.Enqueue("Here is some text to display.  Read it Okay?");
//		pages.Enqueue("Thanks, I appreciate that you read it.  Keep going...");
//		pages.Enqueue("Hi five!  You made it!");
//		cp.Display(pages);
//	}

//	public Unit attacker, defender;
//	
//	void Start () 
//	{
//		for (int i = 0; i < 10; ++i)
//		{
//			attacker.Pos = new Point( UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6) );
//			defender.Pos = new Point( UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6) );
//			
//			if (attacker.Pos == defender.Pos)
//				continue;
//			
//			defender.Dir = (Directions)UnityEngine.Random.Range(0, 4);
//			Facings f = attacker.AngleOfAttack(defender);
//			Debug.Log( string.Format("Attacker at {0}, Defender at {1} with Direction {2} == attack from the {3}", attacker.Pos.ToString (), defender.Pos.ToString(), defender.Dir, f ));
//		}
//	}
}
