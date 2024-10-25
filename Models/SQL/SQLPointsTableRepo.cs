using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.EntityFrameworkCore;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLPointsTableRepo : IPointsTableRepo
    {
        private readonly CrickInfoContext _context;

        public SQLPointsTableRepo(CrickInfoContext context)
        {
            _context = context;
        }

        PointsTable IPointsTableRepo.Add(PointsTable ptable)
        {
            _context.PointsTables.Add(ptable);
            _context.SaveChanges();
            return ptable;
        }

        PointsTable IPointsTableRepo.GetPointTableById(int id)
        {
            return _context.PointsTables.Find(id);
        }

        PointsTable IPointsTableRepo.GetPointsTableByTeamIdAndTournamentId(int teamId, int tournamentId)
        {
            return _context.PointsTables.Where(entry => entry.TeamId==teamId && entry.TournamentId==tournamentId).First();
        }

        IQueryable<PointsTable> IPointsTableRepo.GetPointsTableByTournamentId(int id) {
            return _context.PointsTables.Where(entry => entry.TournamentId == id).OrderByDescending(entry => entry.Points);
        }

        PointsTable IPointsTableRepo.Update(PointsTable ptable)
        {
            var pt = _context.PointsTables.Attach(ptable);
            pt.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return ptable;
        }

       PointsTable IPointsTableRepo.Delete(int id)
        {
            PointsTable pt = _context.PointsTables.Find(id);
            if(pt != null)
            {
                _context.PointsTables.Remove(pt);
                _context.SaveChanges();
            }
            return pt;
        }

        IEnumerable<PointsTable> IPointsTableRepo.GetAllPointsTable()
        {
            return _context.PointsTables;
        }
    }
}
