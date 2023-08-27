using Godot;

partial class Spawn : Area2D {
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player") && ((CharacterBody)body).Parent.HasGoal) {
			System.Console.WriteLine("born to die");
		}
	}
}
