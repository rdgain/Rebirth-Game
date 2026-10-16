using UnityEngine;
using System.Collections.Generic;
    /// <summary>Defines the base contract for parsing and applying a room rule.</summary>

public abstract class Rule {
	public string name { get; protected set; }
	public List<string> constraints { get; protected set; }

	abstract public bool isThisRule (string key);
	abstract public void implement(GameObject room);
	abstract public void parse(List<string> line);
}
