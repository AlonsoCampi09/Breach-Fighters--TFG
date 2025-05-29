using Godot;
using System;
using System.Threading.Tasks;

public partial class TransitionScreen : CanvasLayer{
	private ColorRect rect;
	public AnimationPlayer anim;
	
	private CustomSignals customSignals;
	
	public override void _Ready(){
		rect = GetNode<ColorRect>("Rect");
		anim = GetNode<AnimationPlayer>("Anim");
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		rect.Visible = false;
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public async Task fade_to_black(){
		rect.Visible = true;
		anim.Play("fade_to_black");
		await ToSignal(anim, "animation_finished");
	}
	
	public async Task fade_to_normal(){
		anim.Play("fade_to_normal");
		await ToSignal(anim, "animation_finished");
		rect.Visible = false;
	}
}
