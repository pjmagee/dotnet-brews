namespace Brew.Models;

public class GenericRepository<T> : IRepository<T> where T : Record
{
    private readonly List<T> repository = new List<T>();

    public IEnumerable<T> List => repository.AsEnumerable();

    public void Add(T t)
    {
        repository.Add(t);
    }

    public void Delete(T t) => repository.Remove(t);

    public T? FindById(Guid id) => repository.Find(x => x.Id == id);

    public void Update(T t)
    {
        repository.Remove(t);
        repository.Add(t);
    }
}