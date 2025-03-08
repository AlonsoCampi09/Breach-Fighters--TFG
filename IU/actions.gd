extends Control
@onready var attack: Button = $HBoxContainer/Panel/MarginContainer/HBoxContainer/Attack
@onready var special: Button = $HBoxContainer/Panel/MarginContainer/HBoxContainer/Special
@onready var bag: Button = $HBoxContainer/Panel/MarginContainer/HBoxContainer/Object
@onready var guard: Button = $HBoxContainer/Panel/MarginContainer/HBoxContainer/Guard
@onready var acting_menu: HBoxContainer = $HBoxContainer
@onready var special_menu: HBoxContainer = $HBoxContainer2
@onready var special_1: Button = $HBoxContainer2/Panel/MarginContainer/HBoxContainer/Special1

func change_menu(c: int):
	match c:
		0:
			acting_menu.visible=true
			special_menu.visible=false
			attack.grab_focus()
		1:  
			special_menu.visible=true
			acting_menu.visible=false
			special_1.grab_focus()
			
func close_menu():
	acting_menu.visible=false
	special_menu.visible=false
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	attack.grab_focus()
	acting_menu.visible=true
	special_menu.visible=false


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Input.is_action_pressed("ui_battle_back"):
		change_menu(0)
	pass


func _on_attack_button_down() -> void:
	print("Atacar")


func _on_special_button_down() -> void:
	change_menu(1)


func _on_bag_button_down() -> void:
	print("Bolsa")


func _on_guard_button_down() -> void:
	print("Guardia")

			
