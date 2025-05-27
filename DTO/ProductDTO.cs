using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record ProductDTO(string ProductName, double Price, int CategoryId, string Description, string ImageUrl);
}
