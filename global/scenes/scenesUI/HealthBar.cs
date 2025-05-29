using Godot;
using System;

public partial class HealthBar : TextureProgressBar{
	
	private Fighter owner;
	
	private Texture2D textureNormal;  
	private Texture2D texturePoison;  
	private Texture2D textureDead;  
	private Texture2D textureBleeding;  
	
	public override void _Ready(){
		owner = GetParent() as Fighter;
		textureNormal = GD.Load<Texture2D>("res://assets/ui_related/bar_health_normal.png");
		texturePoison = GD.Load<Texture2D>("res://assets/ui_related/bar_health_poison.png");
		textureDead = GD.Load<Texture2D>("res://assets/ui_related/bar_health_dead.png");
		textureBleeding = GD.Load<Texture2D>("res://assets/ui_related/bar_health_bleeding.png");
	}

	public void UpdateHealthBar(){
		if(owner.IsPoisoned()){
			TextureProgress = texturePoison; 
		}else if(owner.IsDead()){
			if(owner.GetEntityData().isControlled()){
				TextureProgress = textureDead;
			}else{
				Visible = false;
			}
		}else if(owner.IsBleeding()){
			TextureProgress = textureBleeding;
		}else{
			TextureProgress = textureNormal;
		}
		Value = owner.GetEntityData().giveHP()*100/owner.GetEntityData().giveMAXHP();
	}
}
