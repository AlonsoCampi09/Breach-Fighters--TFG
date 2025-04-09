using Godot;
using System;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

public partial class FighterAliados : Fighter
{
	
	public override void _Ready(){
		animSprite = GetNode<AnimatedSprite2D>("Sprites");
		this.prepareFighter(1);
	}
	
	public override void _Process(double delta)
	{
		if(data.Health > (int) data.TrueHealth[data.Level-1]/4){
			animSprite.Play("idle");
		}else{
			if(data.Health == 0){
				animSprite.Play("fainted");
			}else{
				animSprite.Play("idle_low");
			}
		}
	}

    /*public void prepareFighter(int level)
	{
		this.prepareData(level);
	}
	
	public Entity passData(){
		return this.data;
	}*/

    public override async System.Threading.Tasks.Task myTrun()
    {
       
       // GD.Print("esperando senal");
        await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn));
        //GD.Print("senal recibida");

    }


    public override void attack(int pos)
    {
        int dano;
        /*while (listas.Aliados[x].ID != ID && x < listas.Aliados.Count)
        {
            x++;
        }*/
        dano = Battle.enemieslist[pos].getAtacado(this.data.TrueAttack[this.data.Level-1], 10, this.data.ATQBuf, this.data.ATQDeBuf); //potenci = 10
        EnemyEntity DataE = (EnemyEntity)Battle.enemieslist[pos].passData();

        string Message = this.data.Name.ToString() + "ha atacado a " + DataE.Name.ToString() + "y le ha quitado " + dano + "puntos de vida";
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId);
        GD.Print("dano de aliado a enemigo = " + dano);

        //DataE.Health = DataE.Health - dano;
        GD.Print("Vida de atacado = " + DataE.Health);
        aux();
    }
    public void aux()
    {

    }

}
