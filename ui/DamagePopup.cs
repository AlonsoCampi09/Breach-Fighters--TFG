using Godot;
using System;

public partial class DamagePopup : Control
{
	[Export] public float Lifetime = 1.0f;
	private float _timer = 0f;
	private CustomSignals customSignals;

	public void SetDamage(int amount)
	{
		GetNode<Label>("Label").Text = amount.ToString();
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}
	public void SetDamage(string text)
	{
		GetNode<Label>("Label").Text = text;
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	public override void _Process(double delta)
	{
		_timer += (float)delta;
		Position += new Vector2(0, -30) * (float)delta; // se mueve hacia arriba
		if (_timer >= Lifetime){
			CustomSignals.Instance.EmitSignal(nameof(CustomSignals.OnMoveResolved));
			QueueFree();
		}
	}
}
