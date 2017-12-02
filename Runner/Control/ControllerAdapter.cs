using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using BrandonPotter.XBox;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Runner.Control
{

    /// <summary>
    /// The ControllerAdapter handles all input from any XBox Controller and allows information from them to be retrieved in a simple manner.
    /// It implements the singleton design pattern.
    /// </summary>
    class ControllerAdapter
    {
        private static ControllerAdapter instance = null;
        public GamePadState Controller { get; set; }

        private ControllerAdapter()
        {
            Controller = GamePad.GetState(PlayerIndex.One);
            
        }

        public static ControllerAdapter GetInstance()
        {
            if (instance == null)
                instance = new ControllerAdapter();

            return instance;
        }
        
    }
    
    /// <summary>
    /// An exception that can be used if you want to enforce the usage of the controller.
    /// </summary>
    public class ControllerNotFoundException : Exception
    {
        public ControllerNotFoundException () 
            : base()
        {

        }
    }

    //enum XButton
    //{
    //    A, B, X, Y,
    //    RightTrigger, LeftTrigger, RightBumper, LeftBumper,
    //    DpadUp, DpadDown, DpadLeft, DpadRight,
    //    Select, Start,
    //    LeftStickClick, RightStickClick
    //}

    //enum XStickDirection
    //{
    //    LeftStickUp,    LeftStickDown,  LeftStickRight, LeftStickLeft,
    //    RightStickUp,   RightStickDown, RightStickLeft, RightStickRight
    //}

}
