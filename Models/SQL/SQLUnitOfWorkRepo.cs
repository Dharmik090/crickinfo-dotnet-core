using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLUnitOfWorkRepo : IUnitOfWork
    {
        private readonly CrickInfoContext _context;
        private ITournamentRepo _tournamentRepo;
        private ITeamsRepo _teamsRepo;
        public SQLUnitOfWorkRepo(CrickInfoContext context) {
                    _context = context;
        }

        public ITournamentRepo Tournament => throw new NotImplementedException();

        //public ITeamsRepo Team => throw new NotImplementedException();
        public ITeamsRepo Team
        {
            get
            {
                return _teamsRepo = _teamsRepo ?? new SQLTeamsRepo(_context,this);
            }
        }

        TeamTournament IUnitOfWork.AddTeamTournament(TeamTournament teamTournament)
        {
            _context.TeamTournaments.Add(teamTournament);
            return teamTournament;
        }
        //public void AddTeamTournament(TeamTournament teamTournament)
        //{
        //    // Since we don't have a DbSet for TeamTournament, we handle it directly
        //    _context.Entry(teamTournament).State = EntityState.Added;
        //}
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
