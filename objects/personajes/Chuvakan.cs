using Godot;

public partial class Chuvakan : CharacterBody2D
{
	private CharacterData data;
	private AnimatedSprite2D animSprite;
	
	public void ready(){
		data = new ChuvakanData();
		animSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print(data.Health);
		data.Health = 1;
	}
	public override void _Process(double delta)
	{
		if(data.Health > (int) data.TrueHealth / 4){
			animSprite.Play("idle");
		}
		else{
			if(data.Health == 0){
				animSprite.Play("fainted");
			}else{
				animSprite.Play("idle_low");
			}
		}
	}
}
