using Godot;
using System;
using System.Collections.Generic;

public partial class Fighter : Node2D
{
    public Entity data;
    public AnimatedSprite2D sprites;
    private Tween parpadeoTween;
    private PackedScene DamagePopupScene = GD.Load<PackedScene>("res://ui/damage_popUp.tscn");

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

    public override void _Ready()
    {
        sprites = GetNode<AnimatedSprite2D>("Sprites");
        this.prepareFighter(1);
    }

    public override void _Process(double delta)
    {
       /* if (data.Health > (int)data.TrueHealth[data.Level - 1] / 4)
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
        }*/
    }



    public void prepareFighter(int level)
    {
        this.prepareData(level);
    }

    public virtual Entity passData()
    {
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
        //GD.Print("sprites.Name = " + sprites.Animation);

        if (data != null)
        {
            if (this.data.Health > (int)this.data.TrueHealth[this.data.Level - 1] / 3)
            {
                PlayAnimationSafe("idle");
                GD.Print("animacion idle");

            }
            else
            {
                if (data.Health <= 0)
                {
                    PlayAnimationSafe("fainted");
                    GD.Print("animacion muerto");
                }
                else
                {
                    if (!sprites.Animation.Equals("idle_low") && this is FighterAliados)
                    {

                        if (CustomSignals.activado)
                        {
                            CustomSignals.Instance.repetir += this.data.Name + "se estremeze de dolor";

                            DisplayServer.TtsSpeak(this.data.Name + "se estremeze de dolor", CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
                        }
                    }
                    GD.Print("animacion idle_low");

                    PlayAnimationSafe("idle_low");

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
        parpadeoTween.TweenProperty(sprites, "modulate", new Color(0.4f, 0.4f, 0.4f), 0.15f)
                     .SetTrans(Tween.TransitionType.Sine)
                     .SetEase(Tween.EaseType.InOut);
        // Volver a normal
        parpadeoTween.TweenProperty(sprites, "modulate", new Color(1f, 1f, 1f), 0.15f)
                     .SetTrans(Tween.TransitionType.Sine)
                     .SetEase(Tween.EaseType.InOut);
    }
    public void PlayAnimationSafe(string animationName)
    {
        if (sprites != null && sprites.SpriteFrames != null)
        {
            if (sprites.SpriteFrames.HasAnimation(animationName))
            {

                sprites.Play(animationName);
            }
            else
            {
                GD.Print($"[WARNING] Animación '{animationName}' no encontrada para {data?.Name ?? "Fighter"}.");
                // Si quieres reproducir "idle" por defecto si no existe la animación:
                if (sprites.SpriteFrames.HasAnimation("idle"))
                    sprites.Play("idle");
            }
        }
    }

    public void ReceiveDamage(int damage)
    {
        // Aplica el dano
        data.removeHP(damage);
        // Mostrar el numero de dano
        ShowDamagePopup(damage);
    }
    private void ShowDamagePopup(int damage)
    {
        var popup = (DamagePopup)DamagePopupScene.Instantiate();
        GetTree().CurrentScene.AddChild(popup);

        // Posicionarlo encima del personaje
        Vector2 globalPosition = GetGlobalPosition();
        popup.GlobalPosition = globalPosition + new Vector2(0, -50); // un poco arriba del personaje
        popup.SetDamage(damage);
    }
    private void ShowEffectPopup(string text)
    {
        var popup = (DamagePopup)DamagePopupScene.Instantiate();
        GetTree().CurrentScene.AddChild(popup);

        // Posicionarlo encima del personaje
        Vector2 globalPosition = GetGlobalPosition();
        popup.GlobalPosition = globalPosition + new Vector2(0, -50); // un poco arriba del personaje
        popup.SetDamage(text);
    }
    public void DetenerParpadeo()
    {
        if (parpadeoTween != null && parpadeoTween.IsValid())
        {
            parpadeoTween.Kill(); // Detiene el tween
            parpadeoTween = null;

            // (opcional) Restaurar color normal
            sprites.Modulate = new Color(1f, 1f, 1f);
        }
    }
}
