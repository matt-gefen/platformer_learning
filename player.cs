using Godot;
using System;

public partial class player : CharacterBody2D
{
	AnimationPlayer anim;
	AnimatedSprite2D sprite;
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private float friction = 0.1f;
	private float accelleration = 0.5f;
	private float DashVelocity = 900.0f;
	private bool isDashing = false;
	private float dashTimer = 0.2f;
	private float dashTimerReset = 0.2f;

    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    public override void _PhysicsProcess(double delta)
	{
		// physics variables
		Vector2 velocity = Velocity;
		int direction = 0;

		// GD.Print("Velocity", velocity.X);

		// Add the gravity.
		if (!IsOnFloor())
			{
				
				if (velocity.Y > 0) {
					anim.Play("fall");
				} else {
					anim.Play("jump");
				}
			}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			{
				anim.Play("pre_jump");
				velocity.Y = JumpVelocity;
				
			}

		// handle movement direction
		if(Input.IsActionPressed("ui_right")) {
			direction += 1;
			sprite.FlipH = false;
		} else if(Input.IsActionPressed("ui_left")) {
			direction -= 1;
			sprite.FlipH = true;
		}

		// movement if not dashing
		if (!isDashing) {
			if (direction != 0)
			{
				velocity.X = Mathf.Lerp(velocity.X, direction * Speed, accelleration);
			if (velocity.Y == 0)
				{
          anim.Play("run");
        }
			} else
			{
				if (velocity.Y == 0)
					{
						anim.Play("idle");
					}
				velocity.X = Mathf.Lerp(Velocity.X, 0, friction);
			}
		}
	// dash handling
		if (Input.IsActionJustPressed("dash")) {
			isDashing = true;
			anim.Play("dash");
			// dashng from still
			// if(sprite.FlipH == false) {
			// 	velocity.X = DashVelocity;
			// } else {
			// 	velocity.X = -DashVelocity;
			// }

			if(Input.IsActionPressed("ui_right")) {
				velocity.X = DashVelocity;
			}
			if(Input.IsActionPressed("ui_left")) {
				velocity.X = -DashVelocity;
			}
			if(Input.IsActionPressed("ui_up")) {
				velocity.Y = -DashVelocity;
			}
			if(Input.IsActionPressed("ui_down")) {
				velocity.Y = DashVelocity;
			}
			if(Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_left")) {
				velocity.Y = -DashVelocity;
				velocity.X = -DashVelocity;
			}
			if(Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_right")) {
				velocity.Y = -DashVelocity;
				velocity.X = DashVelocity;
			}
			if(Input.IsActionPressed("ui_down") && Input.IsActionPressed("ui_left")) {
				velocity.Y = DashVelocity;
				velocity.X = -DashVelocity;
			}
			if(Input.IsActionPressed("ui_down") && Input.IsActionPressed("ui_right")) {
				velocity.Y = DashVelocity;
				velocity.X = DashVelocity;
			}

			if (direction != 0)
			{
				velocity.X = direction * DashVelocity;
			}
			dashTimer = dashTimerReset;
		}
		
		if(isDashing) {
			dashTimer -=  (float)delta;
					if(dashTimer <= 0) {
						isDashing = false;
						velocity = Vector2.Zero;
					}
		} else {
			velocity.Y += gravity * (float)delta;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
