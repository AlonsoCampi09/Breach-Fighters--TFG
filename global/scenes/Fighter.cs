using Godot;
using System;
using System.Collections.Generic;

public partial class Fighter : Node2D
{
	public Entity data;
    public AnimatedSprite2D animSprite;
    private Tween parpadeoTween;
    private void prepareData(int level)
    {
        string name = this.Name.ToString();
        if (name.Contains("Chuvakan"))
        {
            this.data = new ChuvakanData(level);
        }
        else if (name.Contains("Cassandra"))
        {
            this.data = new CassandraData(level);
        }
        else if (name.Contains("Ishimondo"))
        {
            this.data = new IshimondoData(level);
        }
        else if (name.Contains("bils"))
        {
            this.data = new VylsData(level);
        }
        else if (name.Contains("Blue_Slime"))
        {
            this.data = new SlimeInocenteData(level);
        }
        else if (name.Contains("Purple_Slime"))
        {
            this.data = new SlimeVagoData(level);
        }
        else if (name.Contains("Orange_Slime"))
        {
            this.data = new SlimeAgresivoData(level);
        }
        else if (name.Contains("Grey_Slime"))
        {
            this.data = new SlimeTristeData(level);
        }
        else
        {
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
        GD.PrintErr("myTrun fighter");
        // await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn1));
        if (this.data.Name.Contains("Chuvakan"))
        {
            await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn1));
        }
        else if (this.data.Name.Contains("Cassandra"))
        {
            await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn2));
        }
        else if (this.data.Name.Contains("bils"))
        {
            GD.Print("espera bils");
            await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn3));
        }
        else if (this.data.Name.Contains("Ishimondo"))
        {
            await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn4));
        }
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

    public void ActualizarIconosEstado()
    {
        List<Estado> estados = this.data.estadoManager.GetEstados();
        List<Texture2D> iconos = new List<Texture2D>();

        foreach (Estado kvp in estados)
        {
            Texture2D icono = EstadoHelper.GetIcono(kvp);
            if (icono != null)
            {
                iconos.Add(icono);
            }


        }
        GetNode<EstadoDisplay>("Estado_Display").SetIconos(iconos);
    }


    public void Parpadear()
    {
        // Si ya hay un tween activo, lo eliminamos
        if (parpadeoTween != null && parpadeoTween.IsValid())
        {
            parpadeoTween.Kill();
            parpadeoTween = null;
        }
        parpadeoTween = GetTree().CreateTween();
        parpadeoTween.SetLoops(-1);
        // Oscurecer
        parpadeoTween.TweenProperty(animSprite, "modulate", new Color(0.4f, 0.4f, 0.4f), 0.15f)
                     .SetTrans(Tween.TransitionType.Sine)
                     .SetEase(Tween.EaseType.InOut);
        // Volver a normal
        parpadeoTween.TweenProperty(animSprite, "modulate", new Color(1f, 1f, 1f), 0.15f)
                     .SetTrans(Tween.TransitionType.Sine)
                     .SetEase(Tween.EaseType.InOut);
    }

    public void DetenerParpadeo()
    {
        if (parpadeoTween != null && parpadeoTween.IsValid())
        {
            parpadeoTween.Kill(); // Detiene el tween
            parpadeoTween = null;

            // (opcional) Restaurar color normal
            animSprite.Modulate = new Color(1f, 1f, 1f);
        }
    }
}
