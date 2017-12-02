using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public struct Record : IComparable<Record>
    {
        public int Player { get; }

        public int Collisions { get; }

        public Record(int player, int collisions)
        {
            Player = player;
            Collisions = collisions;
        }

        public override string ToString()
        {
            return $"Player: {Player.ToString()}, Collisions: {Collisions}";
        }

        int IComparable<Record>.CompareTo(Record other)
        {
            if(other.Player == other.Player)
            {
                return this.Collisions.CompareTo(other.Collisions); 
            }

            return other.Player.CompareTo(this.Player);
        }
        public bool Equals(Record other)
        {
            if (Player == other.Player && other.Collisions == Collisions)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + Player.GetHashCode();
            result = 31 * result + (int)Collisions;
            return result;
        }
    }
}
