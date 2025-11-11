namespace Brew.Features.Solid.DependencyInversion.Shared;

/// <summary>
/// Abstraction that both high-level and low-level modules depend on
/// This is the key to Dependency Inversion Principle
/// </summary>
public interface IOrderRepository
{
    void Add(Order order);
    void Remove(Order order);
    Order GetById(int id);
}
