using Godot;
using System;

public partial class Fighter : Node2D
{
	public Entity data;
    public AnimatedSprite2D animSprite;

    public virtual void prepareData(int level){
		string name = this.Name.ToString();
		if(name.Contains("Chuvakan")){
			this.data = new ChuvakanData(level);
		}else if(name.Contains("Cassandra")){
			this.data = new CassandraData(level);
		}else if(name.Contains("Blue_Slime")){
			this.data = new SlimeInocenteData(level);
		}else if(name.Contains("Purple_Slime")){
			this.data = new SlimeVagoData(level);
		}else if(name.Contains("Orange_Slime")){
			this.data = new SlimeAgresivoData(level);
		}else if(name.Contains("Grey_Slime")){
			this.data = new SlimeTristeData(level);
		}else{
			GD.Print("No deberia entrar aqui.");
		}
	}
	
	public override void _Ready(){
		animSprite = GetNode<AnimatedSprite2D>("Sprites");
		this.prepareFighter(1);
	}
	
	public override void _Process(double delta)
	{
		/*if(data.Health > (int) data.TrueHealth[data.Level-1]/4){
			animSprite.Play("idle");
		}else{
			if(data.Health == 0){
				animSprite.Play("fainted");
			}else{
				animSprite.Play("idle_low");
			}
		}*/
	}



    public void prepareFighter(int level)
	{
		this.prepareData(level);
	}
	
	public virtual Entity passData(){
		return this.data;
	}

    public virtual async System.Threading.Tasks.Task myTrun()
    {
        await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn1));
    }

    public virtual int getAtacado(int stat_ataque, int potencia, int Buf_Atq, int Deb_Atq)
    {
        int dano;
        dano = potencia + Math.Max(stat_ataque * (1 + (Buf_Atq - Deb_Atq) / 100) - this.data.TrueDefense[this.data.Level] * (1 + (this.data.DEFBuf - this.data.DEFDeBuf) / 100), 0);
		this.data.Health = this.data.Health - dano;
        return dano;
    }

    public virtual void attack(int pos)
    {
        GD.PrintErr("atacar de Fighter");

    }

	public void changeSprite()
	{
		if(data != null)
		{
            if (this.data.Health > (int)this.data.TrueHealth[this.data.Level - 1] / 4)
            {
                animSprite.Play("idle");
            }
            else
            {
                if (data.Health <= 0)
                {
                    animSprite.Play("fainted");
                }
                else
                {
                    animSprite.Play("idle_low");
                }
            }
        }
    }
}
