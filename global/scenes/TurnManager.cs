using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TurnManager{
	
	List<Fighter> allylist;
	List<Fighter> enemieslist;
	
	public List<Fighter> turnOrder = new List<Fighter>();
    int turnIndex = 0;

    public TurnManager(List<Fighter> ally, List<Fighter> ene){
		allylist = ally;
		enemieslist = ene;
		prepareTurnOrder();
	}
	
	public void prepareTurnOrder(){
		// Combinar listas
		turnOrder = allylist.Concat(enemieslist).ToList();
		// Ordenar por velocidad total (TrueSpeed[level] + (buffs - debuffs)
		turnOrder = turnOrder.OrderByDescending(e => 
		{
			Entity data = e.passData();
			int baseSpeed = data.TrueSpeed[data.Level - 1]; 
			int buffedSpeed = baseSpeed + (baseSpeed * (data.VELBuf - data.VELDeBuf) / 100); 
			return buffedSpeed;
		}).ToList();
		// Debug: Imprimir el orden de turnos
		GD.Print("Orden de turnos:");
		foreach (Fighter f in turnOrder)
		{
			Entity entity = f.passData();
			GD.Print($"{entity.Name} - Velocidad: {entity.TrueSpeed[entity.Level-1]}");
		}
	}

    public void updateTurns()
    {
        //GD.Print("update turns");

        bool encontrado = false;
        for (int i = 0; i < turnOrder.Count; i++)
        {
            encontrado = false;
            for (int x = 0; x < Battle.enemieslist.Count && !encontrado; x++)
            {
                if (turnOrder[i].data.ID == Battle.enemieslist[x].data.ID)
                {
                    encontrado = true;
                    if (Battle.enemieslist[x].data.Health > 0)
                    {
                        turnOrder[i] = Battle.enemieslist[x];
                    }
                    else
                    {
                        Battle.enemieslist[x].animSprite.Visible = false;
                        string Message = Battle.enemieslist[x].Name.ToString() + " ha sido derrotado";
                        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
                        Battle.enemieslist.RemoveAt(x);
                        turnOrder.RemoveAt(i);
                        GD.Print("enemigo muerto");

                    }
                }
            }
            for (int x = 0; x < Battle.allylist.Count && !encontrado; x++)
            {


                if (turnOrder[i].data.ID == Battle.allylist[x].data.ID)
                {
                    encontrado = true;

                    if (Battle.allylist[x].data.Health > 0)
                    {
                        turnOrder[i] = Battle.allylist[x];
                    }
                    else
                    {
                        string Message = Battle.allylist[x].Name.ToString() + " ha sido derrotado";
                        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
                        Battle.allylist.RemoveAt(x);
                        turnOrder.RemoveAt(i);
                        GD.Print("aliado muerto");

                    }
                }
            }
        }
    }
    public void updateTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(e =>
        {
            Entity data = e.passData();
            int baseSpeed = data.TrueSpeed[data.Level - 1];
            int buffedSpeed = baseSpeed + (baseSpeed * (data.VELBuf - data.VELDeBuf) / 100);
            return buffedSpeed;
        }).ToList();
    }
}


