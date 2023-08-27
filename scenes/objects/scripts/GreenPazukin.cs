using Godot;
using System.Collections.Generic;

partial class GreenPazukin : Node2D
{
	public GreenPazukin Parent { get; set; } = null;

	public Direction Direction { get; set; } = Direction.Up;

	public GreenPazukinKind Kind { get; set; } = GreenPazukinKind.Ladder;

	public RayCast2D RayCastDown { get => GetNode<RayCast2D>("Area2D/rcDown"); }

	public Sprite2D Sprite { get => GetNode<Sprite2D>("Area2D/Sprite2D"); }

	private List<GreenPazukin> _children = default;

	public override void _Ready()
	{
		base._Ready();
		if (!RayCastDown.IsColliding())
		{
			Kind = GreenPazukinKind.Bridge;
		}
		if (Kind == GreenPazukinKind.Bridge)
		{
			Sprite.RegionRect = new Rect2(96, 80, 16, 16);
		}
	}

	public bool Add(GreenPazukin child)
	{
		if (Parent != null)
		{
			return Parent.Add(child);
		}

		var last = _children.Count > 0 ? _children[_children.Count - 1] : this;
		if (GetNode<RayCast2D>("rc" + Direction.ToString()).IsColliding())
		{
			return false;
		}

		child.Parent = this;
		child.Kind = Kind;
		child.Position = last.Position + Direction.AsVector() * 16;
		_children.Add(child);
		return true;
	}

	public int Remove()
	{
		int result = 0;
		if (Parent == null) {
			result = _children.Count;
			_children.ForEach(it => it.QueueFree());
		} else {
			result = Parent.Remove();
		}
		QueueFree();
		return result;
	}
}
