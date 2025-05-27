using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public record UserRegisterDTO(string password, string userName, string firstName, string lastName);
}
