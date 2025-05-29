using Godot;
using System;

public partial class InfoPanel : Panel{
	
	private VBoxContainer container;
	
	public override void _Ready(){
		container = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
	}
	public void UpdatePanelSize(){
		Vector2 contentSize = container.Size + new Vector2 (5,5);
		this.Size = contentSize;
	}
	
}
