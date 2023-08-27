extends Node2D

@export var goal: bool = false
@export var pazukin = {
	'green': 0,
	'red': 0,
}
@onready var body = get_node("body")
@onready var Aim = get_node('body/aim')
@onready var Line = get_node("body/Line2D")
@onready var rcAim = body.get_node('rcAim')
var valid_pos: bool = true

var current_pazukin = 'green'

func collides_with(collider: ShapeCast2D, cond: String) -> bool:
	if collider.is_colliding():
		var col = collider.get_collider(0)
		if cond == 'ground':
			return not col.is_in_group('Player') and not col.is_in_group('GreenPazukin')
		if cond == 'ladder':
			return col.is_in_group('GreenPazukin') and col.kind == 'ladder'
		if cond == 'side':
			return (col.is_in_group('GreenPazukin') and col.kind == 'bridge') or (not col.is_in_group('Player') and not col.is_in_group('GreenPazukin'))
	return false

func render_aim():
	var position = get_global_mouse_position() + Vector2(-8,8)
	Aim.global_position = position.snapped(Vector2(16,16))
	print('mouse pos ', position, ' snap ', Aim.global_position)
	rcAim.target_position = Aim.position + Vector2(8,-8)
	var collider_not_player = false
	var aim_collider = Aim.get_node('rc')
	for i in range(aim_collider.get_collision_count()):
		var collider = aim_collider.get_collider(i)
		if collider:
			if not collider.is_in_group('Player') and collider.is_in_group('GreenPazukin'):
				collider_not_player = true
				break
	
	if (collider_not_player
		or rcAim.is_colliding()):
#		print('cond ', Aim.get_node('rc').is_colliding(), not Aim.get_node('rc').get_collider(0).is_in_group('Player'))
		Aim.get_node('Sprite2D').texture = preload("res://resources/sprites/no-aim.png")
		self.valid_pos = false
	else:
		var passed = false
		for cond in [[Aim.get_node('scDown'), 'ground'], [Aim.get_node('scDown'), 'ladder'], [Aim.get_node('scLeft'), 'side'], [Aim.get_node('scRight'), 'side']]:
			if collides_with(cond[0], cond[1]):
				passed = true
				break
		if passed:
			Aim.get_node('Sprite2D').texture = preload("res://resources/sprites/aim.png")
			self.valid_pos = true
		else:
			Aim.get_node('Sprite2D').texture = preload("res://resources/sprites/no-aim.png")
			self.valid_pos = false
		
	Line.clear_points()
	Line.add_point(rcAim.position)
	Line.add_point(rcAim.target_position)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_released("deploy"):
		if self.valid_pos:
			var green = preload("res://scenes/objects/GreenPazukin.tscn").instantiate()
			green.position = rcAim.target_position + Vector2(0, -4)
			
			var green_collider = null
			var aim_collider = Aim.get_node('rc')
			for i in range(aim_collider.get_collision_count()):
				var collider = aim_collider.get_collider(i)
				if collider and collider.is_in_group('GreenPazukin'):
					green_collider = collider
					break
			
			if green_collider:
				green_collider.add(green)
			else:
				self.add_child(green)
	render_aim()
