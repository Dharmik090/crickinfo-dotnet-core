using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLTeamsRepo : ITeamsRepo
    {
        private CrickInfoContext _context;
        private IUnitOfWork _unitOfWork;
        //public SQLTeamsRepo(CrickInfoContext context)
        //{
        //    _context = context;
        //}
        public SQLTeamsRepo(CrickInfoContext context, IUnitOfWork unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); // Ensure unitOfWork is not null
        }
        Team ITeamsRepo.Add(Team team, int tournament_id)
        {
            //add team into TeamTournamet.cs..
            _context.Teams.Add(team);
            _context.SaveChanges();

            TeamTournament teamTournament = new TeamTournament
            {
                TeamId = team.TeamId,
                TournamentId = tournament_id,
                Team = team,
                Tournament = _context.Tournaments.Find(tournament_id),
                DateJoined = DateTime.Now,
            };
            
            // Add the TeamTournament entry using the UnitOfWork method
            _unitOfWork.AddTeamTournament(teamTournament);

            // Save changes to both Teams and TeamTournaments
            _unitOfWork.Save();

            return team;
        }

        Team ITeamsRepo.GetTeamById(int id)
        {
            return _context.Teams.Find(id);
        }

        Team ITeamsRepo.Update(Team team)
        {
            var t = _context.Teams.Attach(team);
            t.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return team;
        }

        Team ITeamsRepo.Delete(int id)
        {
            Team t = _context.Teams.Find(id);
            if(t != null)
            {
                _context.Teams.Remove(t);
                _context.SaveChanges();
            }
            return t;
        }

        IEnumerable<Team> ITeamsRepo.GetAllTeams()
        {
            //return _context.Teams.ToList();
            return _context.Teams.Include(t => t.TeamTournaments).ToList();
        }

        IEnumerable<Team> ITeamsRepo.GetTeamsByTournamentId(int tournament_id)
        {
            var teams = _context.TeamTournaments
              .Where(tt => tt.TournamentId == tournament_id)
              .Select(tt => tt.Team) // Get the associated team
              .ToList();
            return teams;
        }


    }
}
