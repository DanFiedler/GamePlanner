using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlanner.Model
{
    public enum RotationType
    {
        MovePlayer,
        TradePlayers,
        NoRotationPossible
    }

    public class Rotation
    {
        public static Rotation NoRotation = new Rotation( RotationType.NoRotationPossible );

        public Rotation(RotationType type) { RotationType = type; }
        public RotationType RotationType { get; set; }

        public Player PlayerTwo { get; set; }
    }
}