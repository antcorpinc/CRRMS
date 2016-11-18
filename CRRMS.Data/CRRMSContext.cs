using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRRMS.Entities;

namespace CRRMS.Data
{
    public class CRRMSContext: DbContext
    {
        public CRRMSContext()
        {

        }

        public DbSet<Account> AccountSet
        {
            get;
            set;
        }

        public DbSet<Car> CarSet
        {
            get;
            set;
        }
    }
}
