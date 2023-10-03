using Godot;
using System;

public partial class GameMaster : Node
{
	/// <summary>
	/// Export Variables
	/// </summary>
	[ExportGroup("In-Game Time Settings")]
	[Export]
	private bool timePassing = true;
	[Export]
	private float timeSpeed = 1.0f;
	[ExportGroup("Readonly In-Game Time Values")]
	[Export]
	private float currentTime = 0f;
	[Export]
	private string readableCurrentTime = "";
	/// <summary>
	/// Private Variables
	/// </summary>
	private int hour = 0;
	private int minute = 0;

	
	

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Put the time float into a more readable 24h format
		CalculateReadableTime();
		//Restart the day if it gets to hour 24
		if (currentTime >= 2400)
			currentTime = 0f;
		//Make Time Pass
		if(timePassing)
		{
			currentTime += ((float)delta * timeSpeed);
		}
	}

	private void CalculateReadableTime()
	{
		float timeInMin = currentTime;

		int r_hours = (int)(currentTime / 100);
		int r_min = Mathf.RoundToInt(((timeInMin % 100) * .01f) * 59);

        this.hour = r_hours;
		this.minute = r_min;

		//String Formatting
        string ampm = r_hours > 12 ? "PM" : "AM";
		string s_hours;
		string s_min;

		if (r_hours > 12)
			r_hours -= 12;

        s_hours = r_hours.ToString();
        s_min = r_min.ToString();

        if (r_hours == 0)
            s_hours = "12";
		if (r_min < 10)
			s_min = "0" + s_min;


        this.readableCurrentTime = string.Format("{0}:{1}{2}", s_hours, s_min, ampm);
    }
}
