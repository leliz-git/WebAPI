using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record UserDTO(string userName, string firstName, string lastName, List<OrderDTO> Orders);
}
