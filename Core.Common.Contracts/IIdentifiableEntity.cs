using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Info: INterface for identofying the Entity Id 
namespace Core.Common.Contracts
{
    public interface IIdentifiableEntity
    {
        Guid  EntityId { get; set; }
    }
}
