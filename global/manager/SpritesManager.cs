using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpritesManager : Node
{
	private Dictionary<Fighter, Tween> blinkingTweens = new Dictionary<Fighter, Tween>();

	public SpriteFrames GenerateSpriteFrames(Entity data_Info, Texture2D spriteSheet){
		SpriteFrames frames = new SpriteFrames();
		string[] animationNames = { "idle", "acting", "damaged", "idle_low_health", "fainted" };
		
		int frameWidth = data_Info.FrameWidth; 
		int frameHeight = data_Info.FrameLength; 

		int columns = spriteSheet.GetWidth() / frameWidth;
		int rows = spriteSheet.GetHeight() / frameHeight;

		int currentAnimation = 0;
		int framesPerAnimation = 1; // O los que correspondan, según cómo esté organizada tu spritesheet.
		int framesAdded = 0;

		for (int row = 0; row < rows; row++){
			for (int col = 0; col < columns; col++){
				Rect2 frameRect = new Rect2(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
				AtlasTexture frameTexture = new AtlasTexture
				{
					Atlas = spriteSheet,
					Region = frameRect
				};
				string animationName = animationNames[currentAnimation];
				if (!frames.HasAnimation(animationName))
					frames.AddAnimation(animationName);
				frames.AddFrame(animationName, frameTexture);
				framesAdded++;
				// Pasar a la siguiente animación si corresponde
				if (framesAdded >= framesPerAnimation){
					framesAdded = 0;
					currentAnimation++;
					if (currentAnimation >= animationNames.Length){
						// Si ya asignamos todas las animaciones, salimos
						return frames;
					}
				}
			}
		}
		return frames;
	}
	
	public void StartBlinking(Fighter target, float duration = 0.25f){
		if (blinkingTweens.ContainsKey(target))
			return; // Ya está parpadeando

		Tween tween = target.GetTree().CreateTween();
		tween.SetLoops(-1);
		tween.TweenProperty(target, "modulate", new Color(0.4f, 0.4f, 0.4f), duration)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.InOut);
		tween.TweenProperty(target, "modulate", new Color(1f, 1f, 1f), duration)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.InOut);

		blinkingTweens[target] = tween;
	}

	public void StopBlinking(Fighter target){
		if (blinkingTweens.TryGetValue(target, out Tween tween)){
			tween.Kill(); // Detiene y elimina el Tween
			blinkingTweens.Remove(target);
		}

		if (target is CanvasItem canvasItem)
			canvasItem.Modulate = new Color(1f, 1f, 1f); // Restaurar visual
	}
	
	public async void PlayDeathTween(Fighter target){
		Tween tween = target.GetTree().CreateTween();
		
		// Asegúrate de tener modulate disponible (en el nodo con Sprite u otro CanvasItem)
		AnimatedSprite2D sprite = target.GetNode<AnimatedSprite2D>("Sprites");

		// Paso 1: se aclara un poco (blanquear)
		Color originalColor = sprite.Modulate;
		Color whiteColor = new Color(1, 1, 1, 1); // Blanco completo
		tween.TweenProperty(sprite, "modulate", whiteColor, 0.2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);

		// Espera al final del primer tween
		await ToSignal(tween, "finished");

		// Paso 2: desvanecerse a transparente
		tween = GetTree().CreateTween();
		Color transparentColor = new Color(1, 1, 1, 0); // Blanco pero transparente
		tween.TweenProperty(sprite, "modulate", transparentColor, 0.5).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);

		await ToSignal(tween, "finished");

		// Opcional: ocultar o eliminar el nodo
		target.Visible = false; 
	}
}
