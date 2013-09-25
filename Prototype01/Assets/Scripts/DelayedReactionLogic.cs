using UnityEngine;
using System.Collections;

/// <summary>
/// DelayedReactionLogic encapsulates the logic that causes an enemy to react over the course of a second or so to the
/// player.
/// </summary>
public class DelayedReactionLogic : ReactionLogic {
	
	public float suspiciousThreshold;
	public float aggressionThreshold;
	
	private float detectionTimer = 0;

	public override void OnIntruderInSight()
	{
		detectionTimer += Time.deltaTime;
		if (detectionTimer > aggressionThreshold) detectionTimer = 1f;
	}
	
	public override void OnIntruderOutOfSight()
	{
		detectionTimer -= (Time.deltaTime / (int)DetermineAlertness());
		if (detectionTimer < 0) detectionTimer = 0;
	}
	
	public override Alertness DetermineAlertness()
	{
		if (detectionTimer < suspiciousThreshold)
		{
			//Enemy is oblivious
			return Alertness.Normal;
		}
		else if (detectionTimer < aggressionThreshold)
		{
			//Enemy is suspicious
			return Alertness.Suspicious;
		}
		else
		{
			//Enemy is alert!
			return Alertness.Aggressive;
		}
	}
}
