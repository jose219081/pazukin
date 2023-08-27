extends Node2D

@export var parent: Node2D = null
@export var kind: String = "ladder"
@export var direction: String = 'up'
var children = []

"""
add

remove

bridge
	

ladder
"""

const DIRS = {
	'up': Vector2i(0, -1),
	'right': Vector2i(1, 0),
	'down': Vector2i(0, 1),
	'left': Vector2i(-1, 0),
}

const TILE_SIZE = 16

# Called when the node enters the scene tree for the first time.
func _ready():
#	var new_atlas = AtlasTexture.new()
#	new_atlas.
	if not self.get_node('Area2D/rcDown').is_colliding():
		self.kind = 'bridge'
	if kind == 'bridge':
		self.get_node('Area2D/Sprite2D').texture.region = Rect2(96,80,16,16)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func add(green_pazukin) -> bool:
	if self.parent:
		return self.parent.add()
		
	var last = self.children[-1] if self.children else self
	if self.get_node('rc' + self.direction.capitalize()).is_colliding():
		return false

	green_pazukin.parent = self
	green_pazukin.kind = self.kind
	green_pazukin.position = last.position + self.DIRS[self.direction] * self.TILE_SIZE
	self.children.add(green_pazukin)
	return true

func remove() -> int:
	var result = 0
	if not self.parent:
		result = len(self.children)
		for child in self.children:
			child.queue_free()
	else:
		result = self.parent.remove()
	self.queue_free()
	return result


