using UnityEngine;
using System.Collections;

/// <summary>
/// ReactionMethod is a base for classes that describe a way of reacting to the player,
/// e.g. by shooting at them.
/// </summary>
public abstract class ReactionMethod : MonoBehaviour {
	
	/// <summary>
	/// Called when alert state is normal.
	/// </summary>
	public abstract void OnNormal();
	/// <summary>
	/// Called when alert state is suspicious.
	/// </summary>
	public abstract void OnSuspicious();
	/// <summary>
	/// Called when alert state is aggressive.
	/// </summary>
	public abstract void OnAggressive();
}
