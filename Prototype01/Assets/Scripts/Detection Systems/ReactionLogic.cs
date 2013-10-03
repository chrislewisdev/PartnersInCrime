using UnityEngine;
using System.Collections;

/// <summary>
/// ReactionLogic is a simple base class for defining a method by which an enemy will respond to detecting a player.
/// I'm unsure if this will be 100% necessary, but its main intention at the moment is to unify reaction logic
/// between different enemy types (e.g. guards and cameras) without code duplication.
/// This WOULD be implemented as an interface, HOWEVER defining it as an abstract class allows us to use it with
/// GetComponent() and related functions
/// </summary>
public abstract class ReactionLogic : MonoBehaviour {
	
	/// <summary>
	/// Raises the intruder in view event- to be called every loop in which an intruder has been seen.
	/// </summary>
	public abstract void OnIntruderInSight();
	/// <summary>
	/// Raises the intruder out of sight event- to be called every loop in which an intruder is NOT seen.
	/// </summary>
	public abstract void OnIntruderOutOfSight();
	/// <summary>
	/// Determines the alertness of the object at this time. Use to determine what state an enemy should be in.
	/// </summary>
	/// <returns>
	/// The alertness level.
	/// </returns>
	public abstract Alertness DetermineAlertness();
}

public enum Alertness
{
	Normal,
	Suspicious,
	Aggressive
}