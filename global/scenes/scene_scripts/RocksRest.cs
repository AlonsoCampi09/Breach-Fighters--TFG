using Godot;
using System;

public partial class RocksRest : Node2D{
	
	private AnimatedSprite2D alex;
	private AnimatedSprite2D cass;
	private AnimatedSprite2D vyls;
	private AnimatedSprite2D ishi;
	private Sprite2D cape;
	
	private SpritesManager sManager;
	
	private Timer a_timer;
	private Timer c_timer;
	private Timer v_timer;
	private Timer i_timer;
	
	private Label a_label;
	private Label c_label;
	private Label v_label;
	private Label i_label;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		sManager = GetNode<SpritesManager>("/root/SpritesManager");
		alex = GetNode<AnimatedSprite2D>("alex_rest");
		cass = GetNode<AnimatedSprite2D>("cass_rest");
		vyls = GetNode<AnimatedSprite2D>("vyls_rest");
		ishi = GetNode<AnimatedSprite2D>("ishi_rest");
		cape = GetNode<Sprite2D>("cape");
		a_timer = GetNode<Timer>("a_timer");
		c_timer = GetNode<Timer>("c_timer");
		v_timer = GetNode<Timer>("v_timer");
		i_timer = GetNode<Timer>("i_timer");
		a_label = GetNode<Label>("a_label");
		c_label = GetNode<Label>("c_label");
		v_label = GetNode<Label>("v_label");
		i_label = GetNode<Label>("i_label");
		
		alex.AnimationFinished += OnAlexAnimationFinished;
		cass.AnimationFinished += OnCassAnimationFinished;
		vyls.AnimationFinished += OnVylsAnimationFinished;
		ishi.AnimationFinished += OnIshiAnimationFinished;
		
		a_timer.Timeout += OnATimerTimeout;
		c_timer.Timeout += OnCTimerTimeout;
		v_timer.Timeout += OnVTimerTimeout;
		i_timer.Timeout += OnITimerTimeout;
		
		StartTimers();
	}
	
	public override void _Process(double delta) {
		a_label.Text = $"Alex: {a_timer.TimeLeft:F2}s";
		c_label.Text = $"Cass: {c_timer.TimeLeft:F2}s";
		v_label.Text = $"Vyls: {v_timer.TimeLeft:F2}s";
		i_label.Text = $"Ishi: {i_timer.TimeLeft:F2}s";
	}
	
	public void StartBlink(int i){
		switch(i){
			case 0:
				sManager.StartBlinking(alex,new Color(1f, 1f, 0.35f), 0.5f);
				break;
			case 1:
				sManager.StartBlinking(cass,new Color(1f, 1f, 0.35f), 0.5f);
				sManager.StartBlinking(cape,new Color(1f, 1f, 0.35f), 0.5f);
				break;
			case 2:
				sManager.StartBlinking(vyls,new Color(1f, 1f, 0.35f), 0.5f);
				break;
			case 3:
				sManager.StartBlinking(ishi,new Color(1f, 1f, 0.35f), 0.5f);
				break;
		}
	}

	public void StopBlink(int i){
		switch(i){
			case 0:
				sManager.StopBlinking(alex);
				break;
			case 1:
				sManager.StopBlinking(cass);
				sManager.StopBlinking(cape);
				break;
			case 2:
				sManager.StopBlinking(vyls);
				break;
			case 3:
				sManager.StopBlinking(ishi);
				break;
		}
	}
	
	private void StopTimers(){
		a_timer.Stop();
		c_timer.Stop();
		v_timer.Stop();
		i_timer.Stop();
	}
	
	private void StartTimers(){
		a_timer.Start();
		c_timer.Start();
		v_timer.Start();
		i_timer.Start();
	}
	
	private void OnATimerTimeout(){
		Random rand = new Random();
		int n = rand.Next(0, 10);
		if(n < 5){
			alex.Play("Blinks");
		}
		else{
			alex.Play("sketches");
		}
			
		
	}
	private void OnCTimerTimeout(){
		Random rand = new Random();
		int n = rand.Next(0, 10);
		if(n < 4){
			cass.Play("blinks");
		}
		else{
			cass.Play("magic");
		}
		
	}
	private void OnVTimerTimeout(){
		vyls.Play("blinks");
	}
	private void OnITimerTimeout(){
		Random rand = new Random();
		int n = rand.Next(0, 10);
		if(n < 8){
			ishi.Play("blinks");
		}
		else{
			ishi.Play("sleeps");
		}
		
	}
	
	private void OnAlexAnimationFinished() {
		// Cuando termina la animación, reanuda el timer
		a_timer.Start();
	}
	private void OnCassAnimationFinished() {
		// Cuando termina la animación, reanuda el timer
		c_timer.Start();
	}
	private void OnVylsAnimationFinished() {
		// Cuando termina la animación, reanuda el timer
		v_timer.Start();
	}
	private void OnIshiAnimationFinished() {
		// Cuando termina la animación, reanuda el timer
		i_timer.Start();
	}
	
}
