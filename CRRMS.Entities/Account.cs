using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// Info: Each of these entities map to a database table
//Dataaccess layer will provide ORM mapping 
namespace CRRMS.Entities
{
    public class Account : IIdentifiableEntity, IAccountOwnedEntity
    {
       
        public Guid Id { get; set; }

       
        public string LoginEmail { get; set; }

       
        public string FirstName { get; set; }

      
        public string LastName { get; set; }

        
        public string Address { get; set; }

      
        public string City { get; set; }

        
        public string State { get; set; }

       
        public string ZipCode { get; set; }

       
        public string CreditCard { get; set; }

       
        public string ExpDate { get; set; }

        
        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }

        public Guid OwnerAccountId
        {
            get { return Id; }
        }
    }
}
