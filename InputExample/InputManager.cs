using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputExample
{
	public class InputManager
	{
		KeyboardState currentKeyboardState;
		KeyboardState pastKeyboardState;

		MouseState currentMouseState;
		MouseState PriorMouseState;

		GamePadState currentGamepadState;
		GamePadState priorGamepadState;

		/// <summary>
		/// Current direction of position
		/// </summary>
		public Vector2[] Directions { get; private set; } = new Vector2[3];

		/// <summary>
		/// Process an input if a warp has been called
		/// </summary>
		public bool[] Warp { get; private set; } = new bool[3];

		/// <summary>
		/// Process an input to exit the game
		/// </summary>
		public bool Exit { get; private set; }

		public void update(GameTime gameTime) 
		{
			#region State Updating
			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();
			PriorMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();
			priorGamepadState = currentGamepadState;
			currentGamepadState = GamePad.GetState(0);
			#endregion

			#region Direction Input

			int i = 0;
			foreach (Vector2 vector in Directions)
			{
				Directions[i] = Vector2.Zero;
				i++;
			}
			//Get position from Keyboard
			if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A))
			{
				Directions[0] += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D))
			{
				Directions[0] += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W))
			{
				Directions[0] += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S))
			{
				Directions[0] += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
			}

			Directions[1] = new Vector2(currentMouseState.X, currentMouseState.Y);

			if (currentGamepadState.DPad.Left == ButtonState.Pressed)
			{
				Directions[2] += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}


			Directions[2] += currentGamepadState.ThumbSticks.Right * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			#endregion

			#region Warp Input
			int j = 0;
			foreach (bool type in Warp) 
			{
				Warp[j] = false;
				j++;
			}

			if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
			{
				Warp[0] = true;
			}

			if (currentMouseState.LeftButton == ButtonState.Pressed && PriorMouseState.LeftButton == ButtonState.Released)
			{
				Warp[1] = true;
			}

			if (currentGamepadState.IsButtonDown(Buttons.A) && priorGamepadState.IsButtonUp(Buttons.A))
			{
				Warp[2] = true;
			}


			#endregion

			#region Exit Input
			if (currentGamepadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
			{
				Exit = true;
			}
			#endregion
		}

	}
}
