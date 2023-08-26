extends Node2D

@export var amount: int = 0
@export var type: String = 'green'

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_area_2d_body_entered(body):
	if body.is_in_group('Player'):
		var player = body.get_parent()
		player.pazukin[self.type] += self.amount
		self.queue_free()
	
