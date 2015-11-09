using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine 
{
	public GameObject heroPrefab;
	public CameraRig cameraRig;
	public Board board;
	public LevelData levelData;
	public Transform tileSelectionIndicator;
	public Point pos;
	public Tile currentTile { get { return board.GetTile(pos); }}
	public AbilityMenuPanelController abilityMenuPanelController;
	public StatPanelController statPanelController;
	public HitSuccessIndicator hitSuccessIndicator;
	public Turn turn = new Turn();
	public List<Unit> units = new List<Unit>();
	public IEnumerator round;

	void Start ()
	{
		ChangeState<InitBattleState>();
	}
}