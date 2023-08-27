using Godot;
using System;

static class Util {
    public static Vector2 OrientX(float direction, Vector2 v) {
        return new Vector2(Mathf.Sign(direction) * Mathf.Abs(v.X), v.Y);
    }

    public static Vector2 AsVector(this Direction direction) {
        switch (direction) {
            case Direction.Left: return Vector2.Left;
            case Direction.Up: return Vector2.Up;
            case Direction.Right: return Vector2.Right;
            case Direction.Down: return Vector2.Down;
            default: throw new NotImplementedException();
        }
    }
}