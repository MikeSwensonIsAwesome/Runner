using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    interface IEntity
    {

        Vector2 Position { get; }
        Team Team { get; }

        void OnCollision(IEntity entity);

    }

    /// <summary>
    /// Various sides
    /// </summary>
    enum Team { Player, Enemy, Neutral, Background }
}
