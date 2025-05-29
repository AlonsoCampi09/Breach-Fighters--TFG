using Godot;
using System;

public partial class ManaBar : TextureProgressBar{
	
	private Fighter owner;
	
	private Texture2D textureNormal;
	bool showManaBar;
	
	public override void _Ready(){
		owner = GetParent() as Fighter;
		textureNormal = GD.Load<Texture2D>("res://assets/ui_related/bar_mana.png");
		Visible = owner.GetEntityData().ControlPlayer && owner.GetEntityData().entityType != EntityType.Subdito;
	}

	public void UpdateManaBar(){
		if (!Visible){
			return;
		}
		Value = owner.GetEntityData().giveMP()*100/owner.GetEntityData().giveMAXMP();
	}
}
