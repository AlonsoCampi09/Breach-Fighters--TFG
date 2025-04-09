using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Battle;
public partial class MenuBatalla : Control
{
	private Button attack;
	private Button special;
	private Button bag;
	private Button guard;
	private Panel actingMenu;
	private Panel specialMenu;
	private Button mov1;
	private Button mov2;
	private Button mov3;
	private Button mov4;

	List<Fighter> allylist;
	List<Fighter> enemieslist;
	
	private bool selectingTarget;
	private Flecha flecha; // Nodo de la flecha
	private int currentTargetIndex = 0; // Índice del enemigo seleccionado
    public int ID_turno { get; set; }


    public override void _Ready()
	{
		attack = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Attack");
		special = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Special");
		bag = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Bag");
		guard = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Guard");
        attack.FocusEntered += _OnAttackFocusEntered;
        special.FocusEntered += _OnSpecialFocusEntered;
        bag.FocusEntered += _OnObjectFocusEntered;
        guard.FocusEntered += _OnGuardFocusEntered;
        actingMenu = GetNode<Panel>("Battle_Action");
		specialMenu = GetNode<Panel>("Special_Action");
		mov1 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov1");
		mov2 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov2");
		mov3 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov3");
		mov4 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov4");
		
		flecha = GetNode<Flecha>("Flecha");
		flecha.ShowArrow(false);
		
		attack.Pressed += OnAttackButtonDown;
		special.Pressed += OnSpecialButtonDown;
		bag.Pressed += OnBagButtonDown;
		guard.Pressed += OnGuardButtonDown;
		
		attack.GrabFocus();
		actingMenu.Visible = true;
		specialMenu.Visible = false;
		
		selectingTarget = false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_cancel"))
		{
			ChangeMenu(0);
		}
		if (this.selectingTarget)
		{
			// Cambiar objetivo con izquierda/derecha
			if (Input.IsActionJustPressed("ui_left"))
			{
				GD.Print("Flecha visible moviendose a la izq");
				this.ChangeTarget(-1);
			}
			else if (Input.IsActionJustPressed("ui_right"))
			{
				GD.Print("Flecha visible moviendose a la der");
				this.ChangeTarget(1);
			}
			// Confirmar selección con "ui_accept"
			if (Input.IsActionJustPressed("ui_accept"))
			{
				this.ConfirmTarget();
			}
		}
	}

	private void ChangeMenu(int c)
	{
		switch (c)
		{
			case 0:
				actingMenu.Visible = true;
				specialMenu.Visible = false;
				flecha.ShowArrow(false);
				this.selectingTarget = false;
				attack.GrabFocus();
				break;
			case 1:
				specialMenu.Visible = true;
				actingMenu.Visible = false;
				flecha.ShowArrow(false);
				this.selectingTarget = false;
				mov1.GrabFocus();
				break;
			case 2:
				actingMenu.Visible = false;
				specialMenu.Visible = false;
				this.StartTargetSelection();
				break;
		}
	}

	private void OnAttackButtonDown()
	{
		GD.Print("Atacar");
		ChangeMenu(2);
		
	}

	private void OnSpecialButtonDown()
	{
		GD.Print("Especial");
		ChangeMenu(1);
	}

	private void OnBagButtonDown()
	{
		GD.Print("Bolsa");
	}

	private void OnGuardButtonDown()
	{
		GD.Print("Guardia");
	}

    // Funciones de respuesta a enfoque en botones 
    private void _OnAttackFocusEntered()
    {
        var correctSound = GD.Load("res://assets/Sounds/attack_sound.mp3") as AudioStream;
        var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioPlayer.Stream = correctSound;
        audioPlayer.Play();

    }

    private void _OnSpecialFocusEntered()
    {
        var correctSound = GD.Load("res://assets/Sounds/magic_sound.mp3") as AudioStream;
        var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioPlayer.Stream = correctSound;
        audioPlayer.Play();
    }

    private void _OnObjectFocusEntered()
    {
        var correctSound = GD.Load("res://assets/Sounds/inventory_sound.mp3") as AudioStream;
        var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioPlayer.Stream = correctSound;
        audioPlayer.Play();
    }

    private void _OnGuardFocusEntered()
    {
        var correctSound = GD.Load("res://assets/Sounds/shield_sound.mp3") as AudioStream;
        var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioPlayer.Stream = correctSound;
        audioPlayer.Play();
    }

    public void receiveLists(List<Fighter> ene, List<Fighter> ally){
		this.allylist = ally;
		this.enemieslist = ene;
	}
	
	private void StartTargetSelection(){
		if (Battle.enemieslist.Count == 0){
			GD.PrintErr("No hay enemigos disponibles.");
			return;
		}
		else{
			GD.Print("So far so good");
		}
		this.selectingTarget = true;
		// Seleccionar el primer enemigo por defecto
		currentTargetIndex = 0;
		this.MoveArrowToCurrentTarget();
	}
	
	private void ChangeTarget(int direction)
	{
		if (Battle.enemieslist.Count == 0) return;

		// Mover en el array de enemigos (circular)
		currentTargetIndex = (currentTargetIndex + direction + Battle.enemieslist.Count) % Battle.enemieslist.Count;
		MoveArrowToCurrentTarget();
        GD.Print("currentTargetIndex = " + currentTargetIndex);

    }

    private void MoveArrowToCurrentTarget()
	{
		if (Battle.enemieslist.Count == 0) return;

		Fighter selectedEnemy = Battle.enemieslist[currentTargetIndex];
		// Mover la flecha sobre el enemigo
		flecha.MoveArrow(selectedEnemy.GetPosition() + new Vector2(0, 100)); // Ajustar la posición
		GD.Print("Flecha visible");
		flecha.ShowArrow(true);
	}

	private void ConfirmTarget()
	{
		GD.Print($"¡Enemigo {Battle.enemieslist[currentTargetIndex].Name} seleccionado!");
        //GD.Print("currentTargetIndex = " + currentTargetIndex);

        selectingTarget = false;
		flecha.ShowArrow(false);
		ChangeMenu(0);

        for (int j = 0; j < Battle.allylist.Count; j++)
        {
            if (ID_turno == Battle.allylist[j].data.ID)
            {

                Battle.allylist[j].attack(currentTargetIndex);
                GD.Print("senal emitida");

                CustomSignals.Instance.EmitSignal(nameof(CustomSignals.PassTurn));
            }
        }
    }
    public void SetID_turno(int ID)
    {
        ID_turno = ID;
    }
}
