extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
var facing = 'right'
var _position = Vector2.ZERO

@onready var Aim = get_node("../aim")

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("left", "right")
	var rc_front = get_node("rcFront")
	var rc_low = get_node("rcLow")
	if direction:
		velocity.x = direction * SPEED
		var sprite = self.get_node("Sprite2D")
		rc_front.target_position.x = abs(rc_front.target_position.x) * sign(direction)
		rc_low.target_position.x = abs(rc_low.target_position.x) * sign(direction)
		sprite.flip_h = direction < 0
		facing = 'right' if direction > 0 else 'left'
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()
