using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Rule {
	public string name { get; protected set; }
	public List<string> constraints { get; protected set; }

	abstract public bool isThisRule (string key);
	abstract public void implement(GameObject room);
	abstract public void parse(List<string> line);
}
