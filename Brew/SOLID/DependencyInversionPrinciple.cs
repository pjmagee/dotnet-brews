using System.Collections.Generic;
using System.Linq;

namespace Brew.SOLID;

public class DependencyInversionPrinciple : IBrew
{
    #region Shared

    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public List<string> Items { get; set; }
    }

    public class OrderRepository
    {
        private ICollection<Order> _orders;

        public OrderRepository(ICollection<Order> orders)
        {
            _orders = orders;
        }

        public virtual void Add(Order order) => _orders.Add(order);

        public virtual void Remove(Order order) => _orders.Remove(order);

        public virtual Order GetById(int id) => _orders.First(x => x.Id == id);
    }

    #endregion

    #region Without

    public class OrderServiceConcrete
    {
        private OrderRepository _orderRepository;

        public OrderServiceConcrete()
        {
            _orderRepository = new OrderRepository(new List<Order>()); // concrete implementation
        }

        public void GetOrder()
        {

        }

        public void AddOrder(Order order) => _orderRepository.Add(order);
    }

    public void Before()
    {
        OrderServiceConcrete serviceConcrete = new OrderServiceConcrete();
        serviceConcrete.AddOrder(new Order());
    }

    #endregion

    #region With

    public void After()
    {
        // Allows us to swap out implementation details through the constructor
        OrderServiceFlexible serviceFlexible = new OrderServiceFlexible(new OrderRepository(new List<Order>()));
        serviceFlexible.AddOrder(new Order());
    }

    public class OrderServiceFlexible
    {
        private OrderRepository _orderRepository;

        public OrderServiceFlexible(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddOrder(Order order) => _orderRepository.Add(order);

        public void GetOrder()
        {

        }
    }

    #endregion
}
