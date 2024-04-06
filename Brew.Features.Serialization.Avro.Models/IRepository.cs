namespace Brew.Features.Serialization.Avro.Models;

public interface IRepository<T>
{
    IEnumerable<T> List { get; }
    void Add(T t);
    void Update(T t);
    void Delete(T t);
    T? FindById(Guid id);
}