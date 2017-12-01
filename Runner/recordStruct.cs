using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public struct Record : IComparable<Record>
    {
        public string GameDate { get; }

        public int Collisions { get; }

        public Record(string date, int collisions)
        {
            GameDate = date;
            Collisions = collisions;
        }

        public override string ToString()
        {
            return $"{GameDate.ToString()}, Collisions: {Collisions}";
        }

        int IComparable<Record>.CompareTo(Record other)
        {
            if(other.GameDate == other.GameDate)
            {
                return this.Collisions.CompareTo(other.Collisions); 
            }

            return other.GameDate.CompareTo(this.GameDate);
        }
        public bool Equals(Record other)
        {
            if (GameDate == other.GameDate && other.Collisions == Collisions)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + GameDate.GetHashCode();
            result = 31 * result + (int)Collisions;
            return result;
        }
    }
}
