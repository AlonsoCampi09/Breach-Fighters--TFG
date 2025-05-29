using Godot;
using System;

public enum Behaviour {Aleatorio, Agresivo, Tactico, Apoyo}
public enum EntityType {Subdito, Elite, Jefe}

[GlobalClass]
public partial class Entity : Resource{

	[Export] public Behaviour beh_type { get; protected set; }
	[Export] public EntityType entityType { get; protected set; }
	
	[Export]
	public Texture2D SpriteSheet { get; set; }
	[Export]
	public int NumFrames { get; set; }
	[Export]
	public int FrameLength { get; set; }
	[Export]
	public int FrameWidth { get; set; }
	
	[Export] public string Name {  get; set; } = "";
	
	[Export] public int Level {  get; set; } = 1;
	
	[Export] public int Health {  get; set; }= 1;
	[Export] public int Mana {  get; set; }= 1;
	
	[Export] public int ATQBuf {  get; set; }= 0;
	[Export] public int ATQDeBuf {  get; set; }= 0;
	
	[Export] public int DEFBuf {  get; set; }= 0;
	[Export] public int DEFDeBuf {  get; set; }= 0;
	
	[Export] public int VELBuf {  get; set; }= 0;
	[Export] public int VELDeBuf {  get; set; }= 0;
	
	[Export] public bool ControlPlayer {  get; set; } = false;
	[Export] public bool Turn {  get; set; } = false;
	[Export] public bool Passed {  get; set; } = false;
	
	[Export] public int[] TrueHealth;
	[Export] public int[] TrueAttack;
	[Export] public int[] TrueDefense;
	[Export] public int[] TrueMana;
	[Export] public int[] TrueSpeed;
	
	[Export] public int BaseExp;
	[Export] public int ExpIncrease;
	[Export] public int BaseCoin;
	[Export] public int CoinIncrease;
	
	public void levelUp(){
		Level++;
		Health += TrueHealth[Level] - TrueHealth[Level-1];
		Mana += TrueMana[Level] - TrueMana[Level-1];
	}
	public void AssignLevel(int l){
		Level = l;
		Health = TrueHealth[Level-1];
		Mana = TrueMana[Level-1];
	}
	
	public bool isControlled(){
		return ControlPlayer;
	}
	public void isMyTurn(){
		Turn = true;
	}
	public void passTurn(){
		Turn = false;
		Passed = true;
		//estadoManager.AvanzarTurno();
	}
	public void restartPassed(){
		Passed = false;
	}
	
	public int GetEffectiveSpeed()
	{
		int baseSpeed = TrueSpeed[Level - 1];
		float modifier = (VELBuf - VELDeBuf) / 100f;
		return Mathf.RoundToInt(baseSpeed + (baseSpeed * modifier));
	}
	
	public int giveDMG(){
		return TrueAttack[Level-1];
	}
	public int giveDEF(){
		return TrueDefense[Level-1];
	}
	public int giveDMGBuf(){
		return ATQBuf;
	}
	public int giveDMGDeBuf(){
		return ATQDeBuf;
	}
	public int giveDEFBuf(){
		return DEFBuf;
	}
	public int giveDEFDeBuf(){
		return DEFDeBuf;
	}
	public int giveMAXHP(){
		return TrueHealth[Level-1];
	}
	public int giveHP(){
		return Health;
	}
	public int giveMAXMP(){
		return TrueMana[Level-1];
	}
	public int giveMP(){
		return Mana;
	}
	public int giveSP(){
		return TrueSpeed[Level-1];
	}
	public EntityType giveType(){
		return entityType;
	}
	
	public void restoreMP(int m){
		if(this.Mana + m > TrueMana[Level-1]){
			this.Mana = TrueMana[Level-1];
		}else{
			this.Mana += m;
		}
	}
	public void restoreHP(int h){
		if(this.Health + h > TrueHealth[Level-1]){
			this.Health = TrueHealth[Level-1];
		}else{
			this.Health += h;
		}
	}
	public void removeMP(int m){
		if(this.Mana - m < 0){
			this.Mana = 0;
		}else{
			this.Mana -= m;
		}
	}
	public void removeHP(int h){
		if(this.Health - h < 0){
			this.Health = 0;
		}else{
			this.Health -= h;
		}
	}
	public bool haveEnoughMP(int m){
		return Mana >= m;
	}
	public bool criticalHealth(){
		return Health < TrueHealth[Level]/4 && Health > 0;
	}
	public bool isDead(){
		return Health <= 0;
	}
	
	public bool newATQBuffIsBetter(int n){
		return this.ATQBuf <= n;
	}
	public bool newATQDeBuffIsBetter(int n){
		return this.ATQDeBuf <= n;
	}
	public bool newDEFBuffIsBetter(int n){
		return this.DEFBuf <= n;
	}
	public bool newDEFDeBuffIsBetter(int n){
		return this.DEFDeBuf <= n;
	}
	public bool newVELBuffIsBetter(int n){
		return this.VELBuf <= n;
	}
	public bool newVELDeBuffIsBetter(int n){
		return this.VELDeBuf <= n;
	}
	
	public void addBuffDMG(int ptg){
		this.ATQBuf = ptg;
	}
	public void addDeBuffDMG(int ptg){
		this.ATQDeBuf = ptg;
	}
	public void addBuffDEF(int ptg){
		this.ATQBuf = ptg;
	}
	public void addDeBuffDEF(int ptg){
		this.DEFDeBuf = ptg;
	}
	public void addBuffVEL(int ptg){
		this.VELBuf = ptg;
	}
	public void addDeBuffVEL(int ptg){
		this.VELDeBuf = ptg;
	}
	public void removeBuffDMG(){
		this.ATQBuf = 0;
	}
	public void removeDeBuffDMG(){
		this.ATQDeBuf = 0;
	}
	public void removeBuffDEF(){
		this.DEFDeBuf = 0;
	}
	public void removeDeBuffDEF(){
		this.DEFDeBuf = 0;
	}
	public void removeBuffVEL(){
		this.VELDeBuf = 0;
	}
	public void removeDeBuffVEL(){
		this.VELDeBuf = 0;
	}
	
	public int giveCoins(){
		return BaseCoin + CoinIncrease * Level;
	}
	public int giveExp(){
		return BaseExp + ExpIncrease * Level;
	}
	
}
