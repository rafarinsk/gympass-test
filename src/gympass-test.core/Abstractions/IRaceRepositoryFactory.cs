namespace gympass_test.core.Abstractions
{
    public interface IRaceRepositoryFactory
    {
        IRaceRepository CreateRaceRepository(string connectionString);
    }
}
