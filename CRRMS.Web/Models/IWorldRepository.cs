using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRRMS.Web.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        //Trip GetTripByName(string tripName);
        Trip GetTripByName(string tripName,string username);
        void AddTrip(Trip trip) ;


        Task<bool> SaveChangesAsync();
        void AddStop(string tripName, string username, Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}