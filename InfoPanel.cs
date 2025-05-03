using Godot;
using System;

public partial class InfoPanel : Panel{
	
	private MarginContainer container;
	
	public override void _Ready(){
		container = GetNode<MarginContainer>("MarginContainer");
	}
	public void UpdatePanelSize(){
		Vector2 contentSize = container.Size;
		this.Size = contentSize;
	}
	
}
