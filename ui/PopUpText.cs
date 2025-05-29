using Godot;
using System;

public partial class PopUpText : Control{
	[Export] public float Lifetime = 1.0f;
	private float _timer = 0f;
	private CustomSignals customSignals;

	public void TheDamage(int amount){
		GetNode<Label>("Label").Text = amount.ToString();
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}
	
	public void TheEffect(string text){
		GetNode<Label>("Label").Text = text;
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	public override void _Process(double delta){
		_timer += (float)delta;
		Position += new Vector2(0, -30) * (float)delta; // se mueve hacia arriba
		if (_timer >= Lifetime){
			customSignals.EmitSignal(nameof(CustomSignals.OnPopUpExpired));
			GD.Print($"PopUp Expired.");
			QueueFree();
		}
	}
}
