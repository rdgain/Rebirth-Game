using UnityEngine;
using System.Collections.Generic;
using System;
    /// <summary>Registers the rule handlers supported by the room rule parser.</summary>

public static class AppliedRules {
	public static List<Rule> RULES = new List<Rule>
			{
				new Rule_LightPath(),
				new Rule_Win()
			};
}
