using System;
using System.Collections.Generic;
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
		public Vector2 Direction { get; private set; } 

		/// <summary>
		/// Process an input if a warp has been called
		/// </summary>
		public bool Warp { get; private set; }

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

			Direction = Vector2.Zero;
			//Get position from Keyboard
			if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A))
			{
				Direction += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D))
			{
				Direction += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W))
			{
				Direction += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
			}
			if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S))
			{
				Direction += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
			}

			
			if (currentGamepadState.DPad.Left == ButtonState.Pressed)
			{
				Direction += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
			}


			Direction += currentGamepadState.ThumbSticks.Right * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			#endregion

			#region Warp Input
			Warp = false;

			if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
			{
				Warp = true;
			}

			if (currentGamepadState.IsButtonDown(Buttons.A) && priorGamepadState.IsButtonUp(Buttons.A))
			{
				Warp = true;
			}

			if (currentMouseState.LeftButton == ButtonState.Pressed && PriorMouseState.LeftButton == ButtonState.Released)
			{
				Warp = true;
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
