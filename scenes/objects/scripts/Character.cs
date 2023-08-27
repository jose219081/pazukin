using Godot;
using System.Collections.Generic;

public partial class Character : Node2D
{
    public bool HasGoal { get; set; }

    public Dictionary<PazukinType, int> Pazukin = new Dictionary<PazukinType, int> {
        {PazukinType.Green, 0},
        {PazukinType.Red, 0}
    };

    public CharacterBody Body { get => GetNode<CharacterBody>("body"); }
    public Area2D Aim { get => GetNode<Area2D>("body/aim"); }
    public RayCast2D AimRayCast { get => GetNode<RayCast2D>("body/rcAim"); }
    public Line2D Line { get => GetNode<Line2D>("body/Line2D"); }

    private PazukinType _currentPazukin = PazukinType.Green;

    private PackedScene _greenPazukinScene;
    private Texture2D _noAimTexture;
    private Texture2D _aimTexture;

    private bool _validPos = true;

    interface ICollisionRule
    {
        bool CollidesWith(Node collider);
    }

    class GroundCollisionRule : ICollisionRule
    {
        private GroundCollisionRule() { }

        public static GroundCollisionRule Instance = new GroundCollisionRule();

        public bool CollidesWith(Node collider)
        {
            if (collider.IsInGroup("Player")) return false;
            if (collider.IsInGroup("GreenPazukin")) return false;
            return true;
        }
    }

    class LadderCollisionRule : ICollisionRule
    {
        private LadderCollisionRule() { }

        public static LadderCollisionRule Instance = new LadderCollisionRule();

        public bool CollidesWith(Node collider)
        {
            if (!(collider is GreenPazukin)) return false;
            if (((GreenPazukin)collider).Kind != GreenPazukinKind.Ladder) return false;
            return true;
        }
    }

    class BridgeCollisionRule : ICollisionRule
    {
        private BridgeCollisionRule() { }

        public static BridgeCollisionRule Instance = new BridgeCollisionRule();

        public bool CollidesWith(Node collider)
        {
            return IsBridge(collider) || IsOther(collider);
        }

        private bool IsBridge(Node collider)
        {
            return collider is GreenPazukin && ((GreenPazukin)collider).Kind == GreenPazukinKind.Bridge;
        }

        private bool IsOther(Node collider)
        {
            return !collider.IsInGroup("Player") && !collider.IsInGroup("GreenPazukin");
        }
    }

    private bool CollidesWith(ShapeCast2D shapeCast, ICollisionRule rule)
    {
        if (!shapeCast.IsColliding()) return false;
        return rule.CollidesWith((Node)shapeCast.GetCollider(0));
    }

    private Texture2D ResolveAimTexture()
    {
        var colliderNotPlayer = false;
        var aimCollider = Aim.GetNode<ShapeCast2D>("rc");
        var n = aimCollider.GetCollisionCount();
        for (int i = 0; i < n; i++)
        {
            var collider = (Node)aimCollider.GetCollider(i);
            if (!collider.IsInGroup("Player") && !collider.IsInGroup("GreenPazukin"))
            {
                colliderNotPlayer = true;
                break;
            }
        }

        if (colliderNotPlayer || AimRayCast.IsColliding())
        {
            _validPos = false;
            return _noAimTexture;
        }

        ShapeCast2D scDown = Aim.GetNode<ShapeCast2D>("scDown"),
            scRight = Aim.GetNode<ShapeCast2D>("scRight"),
            scLeft = Aim.GetNode<ShapeCast2D>("scLeft");
        var collision =
                CollidesWith(scDown, GroundCollisionRule.Instance)
            || CollidesWith(scDown, LadderCollisionRule.Instance)
            || CollidesWith(scLeft, BridgeCollisionRule.Instance)
            || CollidesWith(scRight, BridgeCollisionRule.Instance);
        if (collision)
        {
            return _aimTexture;
        }
        else
        {
            return _noAimTexture;
        }
    }

    private void RenderAim()
    {
        var position = GetGlobalMousePosition() + new Vector2(-8, 8);
        Aim.GlobalPosition = position.Snapped(new Vector2(16, 16));
        AimRayCast.TargetPosition = Aim.Position + new Vector2(8, -8);

        Aim.GetNode<Sprite2D>("Sprite2D").Texture = ResolveAimTexture();

        Line.ClearPoints();
        Line.AddPoint(AimRayCast.Position);
        Line.AddPoint(AimRayCast.TargetPosition);
    }

    private void Deploy()
    {
        switch (_currentPazukin)
        {
            case PazukinType.Green:
                var green = _greenPazukinScene.Instantiate<GreenPazukin>();
                green.Position = AimRayCast.TargetPosition + new Vector2(0, -4);

                GreenPazukin greenCollider = null;
                var aimCollider = Aim.GetNode<ShapeCast2D>("rc");
                var n = aimCollider.GetCollisionCount();
                for (int i = 0; i < n; i++)
                {
                    var collider = (Node)aimCollider.GetCollider(i);
                    if (collider is GreenPazukin)
                    {
                        greenCollider = (GreenPazukin)collider;
                        break;
                    }
                }
                if (greenCollider != null)
                {
                    greenCollider.Add(green);
                }
                else
                {
                    AddChild(green);
                }
                break;

            case PazukinType.Red:
                break;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        _greenPazukinScene = GD.Load<PackedScene>("res://scenes/objects/GreenPazukin.tscn");
        _aimTexture = ResourceLoader.Load<Texture2D>("res://resources/sprites/aim.png", cacheMode: ResourceLoader.CacheMode.Reuse);
        _noAimTexture = ResourceLoader.Load<Texture2D>("res://resources/sprites/no-aim.png", cacheMode: ResourceLoader.CacheMode.Reuse);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("deploy") && _validPos)
        {
            Deploy();
        }
        RenderAim();
    }
}
