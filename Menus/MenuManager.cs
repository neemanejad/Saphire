using Godot;
using System;

public partial class MenuManager : Node
{
	[Export]
	public PackedScene PauseMenuScene { get; set; }
	private PauseMenu pauseMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		pauseMenu = PauseMenuScene.Instantiate<PauseMenu>();
        pauseMenu.Visible = false;
        pauseMenu.ProcessMode = ProcessModeEnum.Always;

		AddChild(pauseMenu);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape"))
		{
            pauseMenu.Visible = !pauseMenu.Visible;
			GetTree().Paused = !GetTree().Paused;
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
        }
	}


}
