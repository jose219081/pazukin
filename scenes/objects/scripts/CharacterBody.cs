using Godot;

public partial class CharacterBody : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private Direction _facing = Direction.Right;
	public Direction Facing { get => _facing; }

	public Sprite2D Sprite { get => GetNode<Sprite2D>("Sprite2D"); }

	public RayCast2D FrontRayCast { get => GetNode<RayCast2D>("rcFront"); }

	public ShapeCast2D LowShapeCast { get => GetNode<ShapeCast2D>("rcLow"); }

	public Character Parent {
		get {
			return GetParent<Character>();
		}
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor()) {
			velocity.Y += gravity * (float)delta;
		}

		float direction = Input.GetAxis("left", "right");
		if (direction != 0)
		{
			Sprite.FlipH = direction < 0;
			FrontRayCast.TargetPosition = Util.OrientX(direction, FrontRayCast.TargetPosition);
			LowShapeCast.TargetPosition = Util.OrientX(direction, LowShapeCast.TargetPosition);
			_facing = direction > 0 ? Direction.Right : Direction.Left;
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
