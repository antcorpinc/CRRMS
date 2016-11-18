using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRRMS.Entities
{
    public class Reservation : IIdentifiableEntity, IAccountOwnedEntity
    {
       
        public Guid Id { get; set; }

       
        public Guid AccountId { get; set; }

       
        public Guid CarId { get; set; }

       
        public DateTime RentalDate { get; set; }

       
        public DateTime ReturnDate { get; set; }

        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }

        public Guid OwnerAccountId
        {
            get
            {
                return AccountId;
            }
        }
    }
}
