using Godot;

partial class PazukinEgg : Node2D {
	[Export]
	public int Amount { get; set; }

	[Export]
	public PazukinType Type { get; set; }

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player")) {
			((CharacterBody)body).Parent.Pazukin[Type] += Amount;
			QueueFree();
		}
	}
}