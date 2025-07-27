using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private Camera3D camera {  get; set; }
	private float pitch { get; set; } = 0.0f;
	private float yaw { get; set; } = 0.0f;
	private float mouseSensitivity = 0.005f;
	private float movementSpeed = 1000.0f;
	private Vector3 gravityVector = new Vector3(0, -19.6f, 0);
	private Vector3 jumpVector = new Vector3(0, 10, 0);
	private Vector3 verticalVelocityVector = new Vector3(0, 0, 0);

    private float PITCH_YAW_BUFFER_CONSTANT = 0.005f;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		camera = GetNode<Camera3D>("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        ProcessKeyboardInput(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
		//HandleMovement(delta);
    }

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMotion)
		{
			pitch -= mouseMotion.Relative.Y * mouseSensitivity;
			yaw -= mouseMotion.Relative.X * mouseSensitivity;

			pitch = Mathf.Clamp(pitch, (-Mathf.Pi / 2) + PITCH_YAW_BUFFER_CONSTANT, (Mathf.Pi / 2) - PITCH_YAW_BUFFER_CONSTANT);

			camera.Rotation = new Vector3(pitch, yaw, 0);
		}
    }

	public void ProcessKeyboardInput(double delta)
	{
		if (Input.IsActionJustPressed("escape"))
		{
			Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}

		if (Input.IsActionJustPressed("fullscreen"))
		{
			if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Maximized)
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            } else
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
            }
		}

		HandleMovement(delta);
	}

	private void HandleMovement(double delta)
	{
        var cameraDirection = camera.GlobalTransform.Basis.Z.Normalized();
        var movementDirection = new Vector3(0, 0, 0);
        if (Input.IsActionPressed("move_forward"))
        {
            movementDirection += new Vector3(-cameraDirection.X, 0, -cameraDirection.Z);
        }
        if (Input.IsActionPressed("move_back"))
        {
            movementDirection += new Vector3(cameraDirection.X, 0, cameraDirection.Z);
        }
        if (Input.IsActionPressed("move_left"))
        {
            movementDirection += new Vector3(-cameraDirection.Z, 0, cameraDirection.X);
        }
        if (Input.IsActionPressed("move_right"))
        {
            movementDirection += new Vector3(cameraDirection.Z, 0, -cameraDirection.X);
        }

        Velocity = movementDirection.Normalized() * (float)delta * movementSpeed * (Input.IsActionPressed("sprint") ? 2.0f : 1.0f);
		
		verticalVelocityVector += gravityVector * (float)delta;
        if (IsOnFloor())
        {
            verticalVelocityVector = Vector3.Zero;
        }
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
            verticalVelocityVector += jumpVector;
		}

		Velocity += verticalVelocityVector;

		MoveAndSlide();
    }
}
