namespace Voyage.InGame.Player
{
    internal enum MotionState
    {
        IDLE,
        WALK,
        RUN,
        JUMP,
        FALLING,
    }

    internal enum JumpState
    {
        NONE,
        WAITING,
        RISING,
        FALL,
        LANDING,
    }
}