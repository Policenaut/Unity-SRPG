using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine
{
	#region Properties
	[SerializeField] GameObject heroPrefab;
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] Transform tileSelectionIndicator;
	public static BattleController instance { get; private set; }

	public AttackPanel attackerPanel;
	public AttackPanel defenderPanel;
	public AbilityMenuPanel abilityMenuPanel;
	public FacingIndicator facingIndicator;
	public HitSuccessGauge hitSuccessGauge;
	public RoundController turnController;
	public IEnumerator turn;
	
	public Unit current { get; private set; }
	public SkillSet skillSet;
	public Ability ability;
	public List<Unit> targets;
	public List<Unit> heroes = new List<Unit>();
	public List<Unit> enemies = new List<Unit>();
	public Point cursor { get; private set; }
	public Board board { get; private set; }
	public Tile tile { get { return board.GetTile(cursor); }}
	public bool HasUnitMoved;
	public bool HasUnitActed;
	public bool LockMove;

	private Tile _startTile;
	private Directions _startDir;
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		board = GetComponentInChildren<Board>();
		turnController = GetComponent<RoundController>();
		Populate ();
		turn = turnController.Tick();
		turn.MoveNext();
		current = (Unit)turn.Current;
		ChangeState<PreTurnState>();
	}
	#endregion

	#region Public
	public void CompletedTurn ()
	{
		turn.MoveNext();
		current = (Unit)turn.Current;
		ChangeState<PreTurnState>();
	}

	public void MoveCursor (Point offset)
	{
		Tile target = board.GetTile(cursor + offset);
		if (target != null)
			SetCursor(target);
	}

	public void SetCursor (Tile target)
	{
		cursor = target.pos;
		tileSelectionIndicator.localPosition = target.center;
	}

	public void ShowAttackerStats (Unit unit)
	{
		attackerPanel.ShowStats(unit, heroes.Contains(unit));
	}

	public void ShowDefenderStats (Unit unit)
	{
		defenderPanel.ShowStats(unit, heroes.Contains(unit));
	}

	public void MarkPlacement ()
	{
		HasUnitMoved = false;
		HasUnitActed = false;
		LockMove = false;
		_startTile = current.tile;
		_startDir = current.Dir;
	}

	public void UndoMove ()
	{
		HasUnitMoved = false;
		current.Place(_startTile);
		current.Dir = _startDir;
		current.Match();
	}
	#endregion

	#region Private
	void Populate ()
	{
		List<Tile> openTiles = new List<Tile>(board.tiles.Values);
		Populate(Alliances.Hero, heroes, openTiles);
		Populate(Alliances.Monster, enemies, openTiles);
	}

	void Populate (Alliances alliance, List<Unit> team, List<Tile> openTiles)
	{
		for (int i = 0; i < 6; ++i)
		{
			int rnd = UnityEngine.Random.Range(0, openTiles.Count);
			Tile tile = openTiles[rnd];
			openTiles.RemoveAt(rnd);

			int lvl = UnityEngine.Random.Range(7, 11);
			Unit unit = alliance == Alliances.Hero ? UnitFactory.CreateHero(lvl) : UnitFactory.CreateMonster(lvl);
			unit.alliance = alliance;
			team.Add(unit);

			unit.Place(tile);
			unit.Dir = (Directions)UnityEngine.Random.Range(0, 4);
			unit.Match();

			SkillSet temp = new SkillSet();
			temp.name = "Black Magic";
			temp.skills.Add( Resources.Load<Ability>("Abilities/Fire") );
			unit.capability.Add( temp );

			turnController.AddUnit(unit);
		}
	}
	#endregion
}