using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
	Colour,
}

public class Effect
{
	public EffectType type;
	public float t, rate;
	public object target;
	
	public Effect(EffectType type, float effectRate, object target)
	{
		this.t = 0;
		this.type = type;
		this.rate = effectRate;
		this.target = target;
	}
}

[RequireComponent(typeof(tk2dBaseSprite))]
public class SpriteEffects : MonoBehaviour {
	
	private tk2dBaseSprite sprite;
	private Color baseColour;
	private List<Effect> runningEffects = new List<Effect>();

	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<tk2dBaseSprite>();
		baseColour = sprite.color;
	}
	
	void Update()
	{
		foreach (Effect effect in runningEffects)
		{
			switch (effect.type)
			{
			case EffectType.Colour:
			{
				Color targetColour = (Color)effect.target;
				effect.t += effect.rate * Time.deltaTime;
				sprite.color = Color.Lerp (targetColour, baseColour, effect.t);
				break;
			}
			}
		}
		
		runningEffects.RemoveAll (delegate (Effect e) 
		{
			return e.t >= 1;	
		});
	}
	
	public void FlashColour(Color colour, float fadeTime)
	{
		runningEffects.RemoveAll (delegate (Effect e)
		{
			return e.type == EffectType.Colour;
		});
		runningEffects.Add (new Effect(EffectType.Colour, 1 / fadeTime, colour));
	}
}
