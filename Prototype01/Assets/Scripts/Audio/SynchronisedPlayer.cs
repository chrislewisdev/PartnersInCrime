using UnityEngine;
using System.Collections;

/// <summary>
/// SynchronisedPlayer listens for animation events and plays a certain sound when they are triggered.
/// </summary>
[RequireComponent(typeof(tk2dSpriteAnimator))]
public class SynchronisedPlayer : MonoBehaviour {
	
	public string soundName;
	public AudioClip sound;
	
	private tk2dSpriteAnimator animations;

	// Use this for initialization
	void Start ()
	{
		animations = GetComponent<tk2dSpriteAnimator>();
		animations.AnimationEventTriggered += AnimationEventHandler;
	}
	
	public void AnimationEventHandler(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.frames[frame].eventInfo == soundName && sound != null)
		{
			AudioSource.PlayClipAtPoint(sound, transform.position);
		}
	}
}
