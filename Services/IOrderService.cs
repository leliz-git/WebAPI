using DTO;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrder(OrderDTO order);
    }
}