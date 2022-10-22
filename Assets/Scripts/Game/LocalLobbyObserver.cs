using TTInfrastructure;
using TTLobbyLogic;

namespace TTGame
{
    // Holds a LocalLobby value and notifies all subscribers when it has been changed.
    // Check the GameManager in the mainScene for the list of observers being used in the project.
    public class LocalLobbyObserver : ObserverBehaviour<LocalLobby> { }
}