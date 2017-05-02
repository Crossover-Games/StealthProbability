using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMultipleCommands : IActionCommand {
	IEnumerable<IActionCommand> commands;

	public ExecuteMultipleCommands (IEnumerable<IActionCommand> commands) {
		this.commands = commands;
	}

	public void Execute () {
		foreach (IActionCommand iac in commands) {
			iac.Execute ();
		}
	}
}
