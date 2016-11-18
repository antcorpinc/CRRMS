using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Info: interface for identifying the specific user account
namespace Core.Common.Contracts
{
   public interface IAccountOwnedEntity
    {
        Guid OwnerAccountId { get; }
    }
}
