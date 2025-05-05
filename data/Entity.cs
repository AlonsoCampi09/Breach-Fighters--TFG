using Godot;
using System;

[GlobalClass]
public partial class Entity : Resource
{
    public int ID { get; set; }

    public enum Behaviour { Aleatorio, Agresivo, Tactico, Apoyo }

    [Export] public Behaviour beh_type { get; protected set; }

    public static int ID_generator = 0;
    [Export] public string Name { get; set; } = "";

    [Export] public int Level { get; set; } = 1;

    [Export] public int Health { get; set; } = 1;
    [Export] public int Mana { get; set; } = 1;

    [Export] public int ATQBuf { get; set; } = 0;
    [Export] public int ATQDeBuf { get; set; } = 0;

    [Export] public int DEFBuf { get; set; } = 0;
    [Export] public int DEFDeBuf { get; set; } = 0;

    [Export] public int VELBuf { get; set; } = 0;
    [Export] public int VELDeBuf { get; set; } = 0;

    [Export] public bool ControlPlayer { get; set; } = false;
    [Export] public bool Turn { get; set; } = false;
    [Export] public bool Passed { get; set; } = false;

    [Export] public int[] TrueHealth;
    [Export] public int[] TrueAttack;
    [Export] public int[] TrueDefense;
    [Export] public int[] TrueMana;
    [Export] public int[] TrueSpeed;

    public EstadoManager estadoManager;

    public Movimiento atqBasico;
    public Movimiento mov1;
    public Movimiento mov2;
    public Movimiento mov3;
    public Movimiento mov4;
    public Movimiento defBasico;

    public virtual void levelUp()
    {
        Level++;
    }

    public void isMyTurn()
    {
        Turn = true;
    }
    public void passTurn()
    {
        Turn = false;
        Passed = true;
        estadoManager.AvanzarTurno();
    }
    public void restartPassed()
    {
        Passed = false;
    }

    public int giveDMG()
    {
        return TrueAttack[Level - 1];
    }
    public int giveDEF()
    {
        return TrueDefense[Level - 1];
    }
    public int giveDMGBuf()
    {
        return ATQBuf;
    }
    public int giveDMGDeBuf()
    {
        return ATQDeBuf;
    }
    public int giveDEFBuf()
    {
        return DEFBuf;
    }
    public int giveDEFDeBuf()
    {
        return DEFDeBuf;
    }
    public int giveMAXHP()
    {
        return TrueHealth[Level - 1];
    }
    public int giveHP()
    {
        return Health;
    }
    public int giveMAXMP()
    {
        return TrueMana[Level - 1];
    }
    public int giveMP()
    {
        return Mana;
    }
    public int giveSP()
    {
        return TrueSpeed[Level - 1];
    }

    public void restoreMP(int m)
    {
        if (this.Mana + m > TrueMana[Level - 1])
        {
            this.Mana = TrueMana[Level - 1];
        }
        else
        {
            this.Mana += m;
        }
    }
    public void restoreHP(int h)
    {
        if (this.Health + h > TrueHealth[Level - 1])
        {
            this.Health = TrueHealth[Level - 1];
        }
        else
        {
            this.Health += h;
        }
    }
    public void removeMP(int m)
    {
        if (this.Mana - m < 0)
        {
            this.Mana = 0;
        }
        else
        {
            this.Mana -= m;
        }
    }
    public void removeHP(int h)
    {
        if (this.Health - h < 0)
        {
            this.Health = 0;
        }
        else
        {
            this.Health -= h;
        }
    }
    public bool haveEnoughMP(int m)
    {
        return Mana >= m;
    }
    public bool isDead()
    {
        return Health <= 0;
    }

    public bool hadATQBuff()
    {
        return this.ATQBuf != 0;
    }
    public bool hadATQDeBuff()
    {
        return this.ATQDeBuf != 0;
    }
    public bool hadDEFBuff()
    {
        return this.DEFBuf != 0;
    }
    public bool hadDEFDeBuff()
    {
        return this.DEFDeBuf != 0;
    }

    public bool newATQBuffIsBetter(int n)
    {
        return this.ATQBuf <= n;
    }
    public bool newATQDeBuffIsBetter(int n)
    {
        return this.ATQDeBuf <= n;
    }
    public bool newDEFBuffIsBetter(int n)
    {
        return this.DEFBuf <= n;
    }
    public bool newDEFDeBuffIsBetter(int n)
    {
        return this.DEFDeBuf <= n;
    }

    public void addBuffDMG(int ptg)
    {
        this.ATQBuf = ptg;
    }
    public void addDeBuffDMG(int ptg)
    {
        this.ATQDeBuf = ptg;
    }
    public void addBuffDEF(int ptg)
    {
        this.ATQBuf = ptg;
    }
    public void addDeBuffDEF(int ptg)
    {
        this.DEFDeBuf = ptg;
    }
    public void removeBuffDMG()
    {
        this.ATQBuf = 0;
    }
    public void removeDeBuffDMG()
    {
        this.ATQDeBuf = 0;
    }
    public void removeBuffDEF()
    {
        this.DEFDeBuf = 0;
    }
    public void removeDeBuffDEF()
    {
        this.DEFDeBuf = 0;
    }

}
