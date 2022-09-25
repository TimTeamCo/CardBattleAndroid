/// Anything which provides itself to a Locator can then be globally accessed. This should be a single access point for things that *want* to be singleton (that is,
/// when they want to be available for use by arbitrary, unknown clients) but might not always be available or might need alternate flavors for tests, logging, etc.
/// (See http://gameprogrammingpatterns.com/service-locator.html to learn more.)
public class Locator : LocatorBase
{
    private static Locator s_instance;

    public static Locator Get
    {
        get
        {
            if (s_instance == null)
                s_instance = new Locator();
            return s_instance;
        }
    }

    protected override void FinishConstruction()
    {
        s_instance = this;
    }
}
