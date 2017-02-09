using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rule_LightPath : Rule {
	private string key;
	private List<string> values;

	public override bool isThisRule (string key)
	{
		// return true if key starts with "lightPath".
		return key.Split(',')[0].Trim().Equals("lightPath");
	}
	
	public override void implement (GameObject room)
	{
		GameObject previous = null;
		GameObject next, current;
		LightCheck lc;

		// initialise gameObject pointers.
		current = room.transform.Find(values[0]).gameObject;
		if (values.Count < 2) next = null;
		else next = room.transform.Find(values[1]).gameObject;

		for(int i = 0; i < values.Count; i++)
		{ // loop through lightpath objects.
			// get light check script for current light path object.
			lc = current.GetComponent<LightCheck>();
			// set previous and next light path objects.
			lc.previousMirror = previous;
			lc.nextMirror = next;
			// initialise lightCheck stuff.
			lc.setup();

			// set pointers ready for next iteration.
			//previous.
			previous = current.gameObject;
			// current.
			if(next == null) current = next;
			else current = next.gameObject;
			// next.
			if(i+2 >= values.Count) next = null;
			else next = room.transform.Find(values[i+2]).gameObject;
		}
	}
	
	public override void parse (System.Collections.Generic.List<string> line)
	{
		// if line section count doesn't equale two segments something has gone wrong.
		if (line.Count != 2)
			Debug.LogError ("LightPath line has to many parsed arguments.");

		// map key and values to variables.
		key = line [0].Trim();
		values = new List<string>(line [1].Split(','));

		// remove surrounding whitespace.
		for (int i = 0; i < values.Count; i++)
			values [i] = values[i].Trim ();
	}
}
