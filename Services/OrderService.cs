using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrder(OrderDTO order)
        {
            var order1 = _mapper.Map<Order>(order);
            var order2= await _orderRepository.CreateOrder(order1);

            return _mapper.Map<OrderDTO>(order2);
        }

    }
}
