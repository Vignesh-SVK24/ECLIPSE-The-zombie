namespace Eclipse.AI
{
    public interface IZombieState
    {
        ZombieStateId StateId { get; }
        void Enter();
        void Update();
        void PhysicsUpdate();
        void Exit();
    }
}
