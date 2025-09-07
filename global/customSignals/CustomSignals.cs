using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class CustomSignals : Node
{
	[Signal]
	public delegate void OnPrepareRoomEventHandler();
	[Signal]
	public delegate void OnGenerateEnemyTeamForRoomEventHandler();
	[Signal]
	public delegate void OnStartTeamBattleEventHandler();
	[Signal]
	public delegate void OnNextRoomEventHandler(int teamExp, int teamCoins);
	[Signal]
	public delegate void OnVictoryEventHandler(int teamExp, int teamCoins);
	
	
	[Signal]
	public delegate void OnShowFighterInActionEventHandler(Fighter f);
	[Signal]
	public delegate void OnShowBattleMenuEventHandler();
	[Signal]
	public delegate void OnPrepareBattleMenuForFighterEventHandler(Fighter f, FighterTeam allies, FighterTeam enemies);
	[Signal]
	public delegate void OnShowDialogEventHandler(string dialog);
	[Signal]
	public delegate void OnDialogIsOverEventHandler();
	[Signal]
	public delegate void OnPopUpExpiredEventHandler();
	[Signal]
	public delegate void OnEffectIsDoneEventHandler();
	[Signal]
	public delegate void OnStatusEffectsProcessedEventHandler();
	[Signal]
	public delegate void OnStatusEffectsConcludedEventHandler();
	
	[Signal]
	public delegate void OnTargetsConfirmedEventHandler(Fighter actor, Fighter[] targets, Skill skill);
	[Signal]
	public delegate void OnAffectedByMultipleEffectsEventHandler(string name, StatusEffect[] effects);
	
	[Signal]
	public delegate void OnTransitionFinishedEventHandler();
	
	[Signal]
	public delegate void OnRestRechargeEventHandler(GameState gs, FighterTeam allies, RocksRest rest);
	[Signal]
	public delegate void OnShowRestHUDEventHandler();
	[Signal]
	public delegate void OnRestFinishedEventHandler();
	
	[Signal]
	public delegate void OnUnpausedEventHandler();
}
