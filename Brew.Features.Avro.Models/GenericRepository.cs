namespace Brew.Models;

public class GenericRepository<T> : IRepository<T> where T : Record
{
    private readonly List<T> _repository = new List<T>();

    public IEnumerable<T> List => _repository.AsEnumerable();

    public void Add(T t)
    {
        _repository.Add(t);
    }

    public void Delete(T t) => _repository.Remove(t);

    public T? FindById(Guid id) => _repository.Find(x => x.Id == id);

    public void Update(T t)
    {
        _repository.Remove(t);
        _repository.Add(t);
    }
}