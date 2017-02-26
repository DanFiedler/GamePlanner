using GamePlannerModel;

namespace GamePlanner
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

        public EventRegistration PlayerTwo { get; set; }
    }
}