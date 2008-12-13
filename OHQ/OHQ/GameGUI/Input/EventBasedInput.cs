﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OHQ.GameGUI.Input
{
    #region Delegates
    /// <summary>
    /// This delegate is called when a mouse button is clicked
    /// </summary>
    /// <param name="e"></param>
    public delegate void MouseButtonClickHandler(MouseEventArgs e);
    /// <summary>
    /// The delegate is called when a mouse button is released
    /// </summary>
    /// <param name="e"></param>
    public delegate void MouseButtonReleaseHandler(MouseEventArgs e);

    /// <summary>
    /// This delegate is called when a Keyboard key is pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KBKeyPressHandler(object sender, KeyboardEventArgs e);
    /// <summary>
    /// This delegate is called when a Keyboard button is released
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KBKeyReleaseHandler(object sender, KeyboardEventArgs e);

    /// <summary>
    /// This delegate is called when a XBox360 button, joystick, trigger, or shoulder
    /// is pressed on the gamepad
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GamepadButtonReleaseHandler(object sender, GamepadEventArgs e);
    /// <summary>
    /// This delegate is called when a XBox360 button, joystick, trigger, or shoulder
    /// is released on the gamepad
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GamepadButtonPressHandler(object sender, GamepadEventArgs e);

    public delegate void NextTabHandler(CancelEventArgs e);
    public delegate void PrevTabHandler(CancelEventArgs e);

    /// <summary>
    /// This delegate is called when a player connects and XBox360 controller
    /// </summary>
    /// <param name="playerIndex">The index of the player that was connected</param>
    public delegate void PlayerConnectHandler(PlayerIndex playerIndex);
    /// <summary>
    /// This delegate is called when a player disconnects a XBox360 controller
    /// </summary>
    /// <param name="playerIndex">This index of the player that awas disconnected</param>
    public delegate void PlayerDisconnectHandler(PlayerIndex playerIndex);

    /// <summary>
    /// This delegate is a generalization of selecting a menu option and
    /// rather than having a OnKbDownEventDown or Up handler this is a 
    /// generic event that can be used for the keyboard, mouse, or gamepad
    /// </summary>
    /// <param name="sender">The control</param>
    public delegate void MenuSelectHandler(object sender);
    /// <summary>
    /// This delegate is a generalization of go backwards on a menu option and
    /// rather than having a OnKbDownEventDown or Up handler this is a 
    /// generic event that can be used for the keyboard, mouse, or gamepad
    /// </summary>
    /// <param name="sender">the object</param>
    public delegate void MenuBackHandler(object sender);

    #endregion

    public class ControllerBase
    {
        private IEventBasedInputService m_EbiService;
        public IEventBasedInputService EbiService
        {
            get { return m_EbiService; }
            set
            {
                m_EbiService = value;
                OnEbiServiceSet();
            }
        }
        public virtual void Update(GameTime gameTime){}
        protected virtual void OnEbiServiceSet(){}
    }

    public interface IEventBasedInputService
    {
        #region Properties
        float RepeatInterval { get; set; }
        Player AllowedPlayers { get; set; }
        IFocusable Focusable { get; set; }
        List<Keys> TabKeys { get; set; }
        List<GamepadButton> TabNext { get; set; }
        List<GamepadButton> TabPrev { get; set; }
        #endregion

        #region Interface Events
        event PlayerConnectHandler OnPlayerConnect;
        event PlayerDisconnectHandler OnPlayerDisconnect;
        event MouseButtonClickHandler OnRequestingFocus;
        event MouseButtonClickHandler OnMouseButtonClick;
        event MouseButtonReleaseHandler OnMouseButtonRelease;
        event KBKeyPressHandler OnKeyboardKeyPress;
        event KBKeyReleaseHandler OnKeyboardKeyRelease;
        event GamepadButtonPressHandler OnGamepadButtonPress;
        event GamepadButtonReleaseHandler OnGamepadButtonRelease;
        event MenuSelectHandler OnMenuSelectPressed;
        event MenuSelectHandler OnMenuSelectReleased;
        event MenuBackHandler OnMenuBackPressed;
        event MenuBackHandler OnMenuBackReleased;
        #endregion
    }

    public sealed class EventBasedInput<T> 
        : GameComponent, IEventBasedInputService where T : ControllerBase, new()
    {
        #region Properties
        private ControllerBase m_Controller = null;
        private float m_RepeatInterval = 250.0f;
        private bool m_TabEnabled = true;
        private Player m_AllowedPlayers = Player.One;
        private IFocusable m_Focus = null;
        private List<Keys> m_TabKeys = new List<Keys>();
        private List<GamepadButton> m_TabNext = new List<GamepadButton>();
        private List<GamepadButton> m_TabPrev = new List<GamepadButton>();
        private List<GamepadButton> m_SelectNext = new List<GamepadButton>();
        private List<GamepadButton> m_SelectPrev = new List<GamepadButton>();
        #endregion

        #region Events

        public event PlayerConnectHandler OnPlayerConnect;
        public event PlayerDisconnectHandler OnPlayerDisconnect;
        public event MouseButtonClickHandler OnRequestingFocus;
        public event MouseButtonClickHandler OnMouseButtonClick;
        public event MouseButtonReleaseHandler OnMouseButtonRelease;
        public event KBKeyPressHandler OnKeyboardKeyPress;
        public event KBKeyReleaseHandler OnKeyboardKeyRelease;
        public event GamepadButtonPressHandler OnGamepadButtonPress;
        public event GamepadButtonReleaseHandler OnGamepadButtonRelease;
        public event MenuSelectHandler OnMenuSelectPressed;
        public event MenuSelectHandler OnMenuSelectReleased;
        public event MenuBackHandler OnMenuBackPressed;
        public event MenuBackHandler OnMenuBackReleased;
        #endregion

        public EventBasedInput(Game game)
            : base(game)
        {
            // Create the controller
            m_Controller = new T();
            m_Controller.EbiService = this;

            // Add the tab keys
            m_TabKeys.Add(Keys.Tab);
            m_TabNext.Add(GamepadButton.LeftStickRight);
            m_TabPrev.Add(GamepadButton.LeftStickLeft);
            m_SelectNext.Add(GamepadButton.A);
            m_SelectPrev.Add(GamepadButton.B);
        }
    }
}