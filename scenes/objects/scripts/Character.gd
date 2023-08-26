extends Node2D

@export var goal: bool = false
@export var pazukin = {
	'green': 0,
	'red': 0,
}

var current_pazukin = 'green'

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_released("deploy"):
		var body = get_node("body")
		var facing = body.facing
		var position = body.position
		var tilemap = get_parent().get_node("TileMap")
		
		var direction = 1 if facing == 'right' else -1
		
		var front = body.get_node("rcFront").is_colliding()
		var low = body.get_node("rcLow").is_colliding()
		
		print("deploying " + current_pazukin + " facing " + facing + " at " + str(position.x) + ", " + str(position.y) + " with racyasts " + str(front) + " and " + str(low))
