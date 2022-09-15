namespace MetricsControl.Service;

public interface IRepository<T> where T : class
{
    IList<T> GetAll();
    T Get(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
}
