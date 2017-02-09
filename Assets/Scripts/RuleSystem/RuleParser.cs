using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public static class RuleParser {

	/**
	 *  Apply the rules contained in the specified string on the specified game object.
	 * @param room - GameObject - the object to apply the rules to.
	 * @param rules - string - the rules to apply to the GameObject.
	 * @param ruleLines - List - a list of strings of one rule per entry.
	 */
	public static void implement(GameObject room, string rules)
	{
		// implement each rule on the specified room.
		parse (rules).ForEach ((Rule r) => r.implement(room));
	}
	public static void implement(GameObject room, List<string> ruleLines)
	{
		// implement each rule on the specified room.
		parse (ruleLines).ForEach ((Rule r) => r.implement(room));
	} 

	/**
	 *  Return the rules specified within the parsed string.
	 * @param rules - string - a string containing rules.
	 * @param ruleLines - List - a list of strings of one rule per entry.
	 * @return result - List - a list of all of the found rules.
	 */
	public static List<Rule> parse(string rules)
	{
		// split lines into single lines.
		List<string> lines = new List<string>(rules.Split('\n'));
		return parse (lines);

	}
	public static List<Rule> parse(List<string> ruleLines)
	{
		// split lines into sections.
		List<List<string>> sections = new List<List<string>>(ruleLines.Count);
		ruleLines.ForEach ((string l) => sections.Add(new List<string>(l.Split(':'))));
		// output list.
		List<Rule> result = new List<Rule>(ruleLines.Count);
		
		// loop through rules to check which applies for each line.
		int i = 0;
		foreach(string l in ruleLines)
		{ // loop through all rule lines.
			foreach (Rule r in AppliedRules.RULES)
			{ // loop through all applied rules.
				if(r.isThisRule(sections[0][0]))
				{ // if rule found.
					// create rule instance for specified line.
					Rule currentRule = (Rule)Activator.CreateInstance(r.GetType());
					
					currentRule.parse(sections[i]); // parse sections to rule.
					result.Add(currentRule); // add rule to results.
					
					break; // stop looking for a rule for this line.
				}
			}
			i++;
		}
		
		return result;
	}
}
