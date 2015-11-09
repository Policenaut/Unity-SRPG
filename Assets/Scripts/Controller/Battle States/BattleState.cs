using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State 
{
	protected BattleController owner;
	public CameraRig cameraRig { get { return owner.cameraRig; }}
	public Board board { get { return owner.board; }}
	public LevelData levelData { get { return owner.levelData; }}
	public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
	public Point pos { get { return owner.pos; } set { owner.pos = value; }}
	public Tile currentTile { get { return owner.currentTile; }}
	public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; }}
	public StatPanelController statPanelController { get { return owner.statPanelController; }}
	public HitSuccessIndicator hitSuccessIndicator { get { return owner.hitSuccessIndicator; }}
	public Turn turn { get { return owner.turn; }}
	public List<Unit> units { get { return owner.units; }}

	protected virtual void Awake ()
	{
		owner = GetComponent<BattleController>();
	}

	protected override void AddListeners ()
	{
		InputController.moveEvent += OnMove;
		InputController.fireEvent += OnFire;
	}
	
	protected override void RemoveListeners ()
	{
		InputController.moveEvent -= OnMove;
		InputController.fireEvent -= OnFire;
	}

	protected virtual void OnMove (object sender, InfoEventArgs<Point> e)
	{
		
	}
	
	protected virtual void OnFire (object sender, InfoEventArgs<int> e)
	{
		
	}

	protected virtual void SelectTile (Point p)
	{
		if (pos == p || !board.tiles.ContainsKey(p))
			return;

		pos = p;
		tileSelectionIndicator.localPosition = board.tiles[p].center;
	}

	protected virtual Unit GetUnit (Point p)
	{
		Tile t = board.GetTile(p);
		GameObject content = t != null ? t.content : null;
		return content != null ? content.GetComponent<Unit>() : null;
	}

	protected virtual void RefreshPrimaryStatPanel (Point p)
	{
		Unit target = GetUnit(p);
		if (target != null)
			statPanelController.ShowPrimary(target.gameObject);
		else
			statPanelController.HidePrimary();
	}

	protected virtual void RefreshSecondaryStatPanel (Point p)
	{
		Unit target = GetUnit(p);
		if (target != null)
			statPanelController.ShowSecondary(target.gameObject);
		else
			statPanelController.HideSecondary();
	}
}