public interface IProvidable<T>
{
    void OnReProvided(T previousProvider);
}
