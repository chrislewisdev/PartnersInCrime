﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility {

	private static Dictionary<string, bool> logValues = new Dictionary<string, bool>();
	
	public static void LogChangedValue(string valueName, bool val)
	{
		//Check if the value already exists in our dictionary
		if (logValues.ContainsKey (valueName))
		{
			//If it does, we only want to log and update it if it doesn't match the existing value
			if (logValues[valueName] != val)
			{
				logValues[valueName] = val;
				Debug.Log (valueName + ": " + val);
			}
		}
		else
		{
			//Otherwise, add it and log it regardless
			logValues.Add (valueName, val);
			Debug.Log (valueName + ": " + val);
		}
	}
}