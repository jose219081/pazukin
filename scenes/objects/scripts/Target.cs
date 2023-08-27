using Godot;

partial class Target : Area2D {
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player")) {
			System.Console.WriteLine("world is a fuck");
			((CharacterBody)body).Parent.HasGoal = true;
			GetParent().QueueFree();
		}
	}
}
