using crickinfo_mvc_ef_core.Models.Interface;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLTournamentRepo : ITournamentRepo
    {
        private readonly CrickInfoContext _context;

        public SQLTournamentRepo(CrickInfoContext context)
        {
            _context = context;
        }

       Tournament ITournamentRepo.Add(Tournament tournament,int uid)
        {
            User user = _context.Users.Find(uid);
            tournament.User = user;
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
            return tournament;
        }


        Tournament ITournamentRepo.GetTournamentById(int id)
        {
            return _context.Tournaments.Find(id);
        }

        IEnumerable<Tournament> ITournamentRepo.GetTournamentByUserId(int id)
        {
            return _context.Tournaments.Where(tournament => tournament.UserId == id).ToList();
        }

        Tournament ITournamentRepo.Update(Tournament tournament)
        {
            var t = _context.Tournaments.Attach(tournament);
            t.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return tournament;
        }

        Tournament ITournamentRepo.Delete(int id)
        {
            Tournament t = _context.Tournaments.Find(id);
            if (t != null)
            {
                _context.Tournaments.Remove(t);
                _context.SaveChanges();
            }
            return t;
        }

        IEnumerable<Tournament> ITournamentRepo.GetAllTournaments()
        {
            return _context.Tournaments;
        }

        public Tournament GetTournamentById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
