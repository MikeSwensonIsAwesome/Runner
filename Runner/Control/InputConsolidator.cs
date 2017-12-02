using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Runner.Control
{

    /// <summary>
    /// The InputConsolidator binds keys and buttons from different input methods to the same actions.
    /// For instance, Jump could be bound to the [Spacebar] on a keyboard and [A Button] on a XInput controller
    /// 
    /// It implements the singleton design pattern.
    /// </summary>
    class InputConsolidator
    {

        private static InputConsolidator instance = null;
        private static GamePadState Controller = ControllerAdapter.GetInstance().Controller;
        private static KeyboardAdapter Keyboard = KeyboardAdapter.GetInstance();

        private InputConsolidator()
        {

        }

        public static InputConsolidator GetInstance()
        {
            if (instance == null)
                instance = new InputConsolidator();

            return instance;
        }

        /// <summary>
        /// Returns true if a given XInput button is pressed
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private bool XButtonPressed(Buttons btn)
        {
            return (Controller.Buttons.btn == ButtonState.Pressed);
        }

    }



}
