using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class AppliedRules {
	public static List<Rule> RULES = new List<Rule>
			{
				new Rule_LightPath(),
				new Rule_Win()
			};
}
