using Sample;

namespace NGO
{
    /// Something that will handle player input while in the game.
    public interface IInGameInputHandler : IProvidable<IInGameInputHandler>
    {
        void OnPlayerInput(ulong playerId, SampleNetworkObject selectedIcon);
    }
}