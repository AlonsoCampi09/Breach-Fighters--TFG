using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.X86;
using static Battle;


public partial class BarrasVida : Control
{
    struct Bars
    {
        public TextureProgressBar healtbar;
        public TextureProgressBar manabar;
        public int ID;
    }
    private List<Bars> bars = new List<Bars>();
   
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void setHealthBars()
    {
        Texture2D textureUnder = GD.Load<Texture2D>("res://assets/sprites/bar.png"), textureProgress;
        GD.Print("barras de vida set");
        for (int i = 0; i < Battle.enemieslist.Count; i++) // se crean los botones que seran los enemigos
        {
            TextureProgressBar healthBar = new TextureProgressBar();
          //  healthBar.ShowPercentage = true;
            healthBar.MinValue = 0;
           // textureUnder = GD.Load<Texture2D>("\"res://assets/sprites/bar.png");
            healthBar.TextureUnder = textureUnder;
            textureProgress = GD.Load<Texture2D>("res://assets/sprites/bar_health_normal.png");
            healthBar.TextureProgress = textureProgress;
            //Texture2D aux =
            //esta cojiendo la data de fighter y no la data de FighterEnemigos por lo que es nula, habira que pasa la data de enemigos que es distinta a la de fighter 
            healthBar.MaxValue = Battle.enemieslist[i].data.TrueHealth[Battle.enemieslist[i].data.Level - 1];

            healthBar.Value = Battle.enemieslist[i].data.Health;
            healthBar.Visible = true;
            // healthBar.SetSize(new Vector2(200, 200));
            healthBar.CustomMinimumSize = new Vector2(175, 0);
            healthBar.Position = Battle.enemieslist[i].GetPosition() + new Vector2(-55, 0);
            Bars bar_aux = new Bars();
            bar_aux.healtbar = healthBar;
            bar_aux.ID = Battle.enemieslist[i].data.ID;

            bars.Add(bar_aux);
            //Enemigos_bars.AddChild(enemigos_healtbar[i]);
            //enemigos_healtbar[i].Position = Battle.enemieslist[i].GetPosition() + new Vector2(0, -100);
            AddChild(bar_aux.healtbar);


        }

        for (int i = 0; i < Battle.allylist.Count; i++) // se crean los botones que seran los enemigos
        {

            TextureProgressBar healthBar = new TextureProgressBar();
            TextureProgressBar manaBar = new TextureProgressBar();
       //     textureUnder = GD.Load<Texture2D>("\"res://assets/sprites/bar.png");
            healthBar.TextureUnder = textureUnder;
            textureProgress = GD.Load<Texture2D>("res://assets/sprites/bar_health_normal.png");
            healthBar.TextureProgress = textureProgress;

            manaBar.TextureUnder = textureUnder;
            textureProgress = GD.Load<Texture2D>("res://assets/sprites/bar_mana.png");
            manaBar.TextureProgress = textureProgress;

            //   healthBar.ShowPercentage = true;
            healthBar.MinValue = 0;
           // manaBar.ShowPercentage = true;
            manaBar.MinValue = 0;
            healthBar.MaxValue = Battle.allylist[i].data.TrueHealth[Battle.allylist[i].data.Level-1];
            manaBar.MaxValue = Battle.allylist[i].data.TrueMana[Battle.allylist[i].data.Level - 1];

            healthBar.Value = Battle.allylist[i].data.Health;
            manaBar.Value = Battle.allylist[i].data.Mana;

            healthBar.Visible = true;
            manaBar.Visible = true;

            // healthBar.SetSize(new Vector2(200, 200));
            healthBar.CustomMinimumSize = new Vector2(175, 0);
            healthBar.Position = Battle.allylist[i].GetPosition() + new Vector2(-15, 240);
            manaBar.CustomMinimumSize = new Vector2(175, 0);
            manaBar.Position = Battle.allylist[i].GetPosition() + new Vector2(-15, 275);

            Bars bar_aux = new Bars();
            bar_aux.healtbar = healthBar;
            bar_aux.manabar = manaBar;
            bar_aux.ID = Battle.allylist[i].data.ID;

            bars.Add(bar_aux);
            AddChild(bar_aux.healtbar);
            AddChild(bar_aux.manabar);


        }

    }

    public void actualizar()
    {
        bool encontrado = false;
        for (int i = 0; i < bars.Count; i++)
        {
            //GD.Print("update barras");
            encontrado = false;
            for (int x = 0; x < Battle.enemieslist.Count && !encontrado; x++)
            {
                //EnemyEntity DataE = (EnemyEntity)Battle.enemieslist[x].passData();
                if (bars[i].ID == Battle.enemieslist[x].data.ID)
                {
                   // GD.Print("encontrado");

                    if (Battle.enemieslist[x].data.Health > 0)
                    {
                        bars[i].healtbar.Value = Battle.enemieslist[x].data.Health;
                    }
                    else
                    {
                        //RemoveChild(GetChild(i+1, false));
                        RemoveChild(FindChild(bars[i].healtbar.Name, false, false));

                       // GD.Print("se quita una barra enemiga (N enemigos)= " + x);
                       // GD.Print("se quita el hijo= " + i);
                        encontrado = true;
                        bars.Remove(bars[i]);
                        i--;
                    }
                }
               
            }
            for (int x = 0; x < Battle.allylist.Count && !encontrado; x++)
            {
                if(bars[i].ID == Battle.allylist[x].data.ID)
                {
                    //GD.Print("encontrado");
                    if (Battle.allylist[x].data.Health > 0)
                    {
                        bars[i].healtbar.Value = Battle.allylist[x].data.Health;
                        bars[i].manabar.Value = Battle.allylist[x].data.Mana;

                    }
                    else
                    {
                        //RemoveChild(GetChild(i+1, false));
                        RemoveChild(FindChild(bars[i].healtbar.Name, false, false));
                        RemoveChild(FindChild(bars[i].manabar.Name, false, false));

                        //RemoveChild(GetChild(i + 2, false));
                        encontrado = true;
                        i--;
                        bars.Remove(bars[i]);
                       // GD.Print("se quita una barra aliada (N aliados)= " + x);
                    }
                }
                
            }
        }

    }
}
