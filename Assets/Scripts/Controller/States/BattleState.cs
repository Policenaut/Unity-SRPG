using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State
{
	#region Properties
	protected BattleController owner;

	// For convenience sake, make the state wrap the owners properties as if it
	// is a part of the class.
	public AttackPanel attackerPanel { get { return owner.attackerPanel; }}
	public AttackPanel defenderPanel { get { return owner.defenderPanel; }}
	public AbilityMenuPanel abilityMenuPanel { get { return owner.abilityMenuPanel; }}
	public FacingIndicator facingIndicator { get { return owner.facingIndicator; }}
	public HitSuccessGauge hitSuccessGauge { get { return owner.hitSuccessGauge; }}
	public Unit current { get { return owner.current; }}
	public SkillSet skillSet { get { return owner.skillSet; }}
	public Ability ability { get { return owner.ability; } set { owner.ability = value; }}
	public List<Unit> targets { get { return owner.targets; }}
	public List<Unit> heroes { get { return owner.heroes; }}
	public List<Unit> enemies { get {return owner.enemies; }}
	public Point cursor { get { return owner.cursor; }}
	public Board board { get { return owner.board; }}
	public Tile tile { get { return owner.tile; }}
	public bool HasUnitMoved { get { return owner.HasUnitMoved; } set { owner.HasUnitMoved = value; }}
	public bool HasUnitActed { get { return owner.HasUnitActed; } set { owner.HasUnitActed = value; }}
	public bool LockMove { get { return owner.LockMove; } set { owner.LockMove = value; }}
	#endregion

	#region MonoBehaviour
	protected virtual void Awake ()
	{
		owner = GetComponent<BattleController>();
	}

	protected virtual void OnDestroy ()
	{
		RemoveListeners();
	}
	#endregion

	#region EventHandlers
	protected virtual void OnMove (object sender, InfoEventArgs<Point> e)
	{

	}
	
	protected virtual void OnFire (object sender, InfoEventArgs<int> e)
	{

	}
	#endregion

	#region Public
	public override void Enter ()
	{
		AddListeners();
	}

	public override void Exit ()
	{
		RemoveListeners();
	}
	#endregion

	#region Protected
	protected virtual void AddListeners ()
	{
		InputController.moveEvent += OnMove;
		InputController.fireEvent += OnFire;
	}

	protected virtual void RemoveListeners ()
	{
		InputController.moveEvent -= OnMove;
		InputController.fireEvent -= OnFire;
	}

	protected virtual void SnapToCurrent ()
	{
		owner.SetCursor(current.tile);
		attackerPanel.ShowStats(current, heroes.Contains(current));
	}
	#endregion
}
