namespace MetricsAgent.Service;

public interface IRepository<T> where T : class
{
    IList<T> GetAll();
    T Get(int id);
    IList<T> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
}
