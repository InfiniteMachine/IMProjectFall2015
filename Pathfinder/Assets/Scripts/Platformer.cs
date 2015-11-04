using UnityEngine;
using System.Collections;

public class Platformer : MonoBehaviour {

	[System.Serializable]
	public class Controller
	{
		public Stick[] sticks;
		public Button[] buttons;


	}

	[System.Serializable]
	public class Stick
	{
		public Stick()
		{}

		virtual public float returnThrottle()
		{return 0f;}
	}

	[System.Serializable]
	public class AnalogStick : Stick
	{
		public string leftInput;
		public string rightInput;

		public AnalogStick(string newLeft, string newRight)
		{
			leftInput = newLeft;
			rightInput = newRight;
		}

		override public float returnThrottle()
		{
			if (Input.GetKey (rightInput))
				return 1f;
			else if (Input.GetKey (leftInput))
				return -1f;
			else
				return 0f;
		}
	}

	public class DigitalStick : Stick
	{
		public string stickInput;
	}

	public class Button
	{
		public Button(){}
	}

	[System.Serializable]
	public class Acceleration
	{
		public float factorLerp;
		public float flatLerp;

		public float lerpStrength(float difference)
		{
			return (Mathf.Abs(difference) * factorLerp) + flatLerp;
		}
	}

	[System.Serializable]
	public class Jumpset
	{
		public Jump[] jumps;

		public Jumpset(Jump[] a)
		{
			jumps = a;
		}

		public int Length()
		{
			return jumps.Length;
		}
	}

	[System.Serializable]
	public class Jump
	{
		public float power;
		public float time;
		public bool hover = true;
		public bool applyOverTime = false;
		public Acceleration timeAcceleration;

		public Jump(float a, float b, bool groundedJump = true)
		{
			power = a;
			time = b;
			if(groundedJump)
			{
				hover = true;
				applyOverTime = false;
			}
			else
			{
				hover = false;
				applyOverTime = true;
			}
		}
	}
}
