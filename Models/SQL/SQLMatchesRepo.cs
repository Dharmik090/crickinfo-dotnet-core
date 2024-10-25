using crickinfo_mvc_ef_core.Models.Interface;
using System.Text.RegularExpressions;

namespace crickinfo_mvc_ef_core.Models.SQL
{
    public class SQLMatchesRepo : IMatchesRepo
    {

        private CrickInfoContext _context;
        private IUnitOfWork _unitOfWork;

        public SQLMatchesRepo(CrickInfoContext context, IUnitOfWork unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); // Ensure unitOfWork is not null
        }
        Matches IMatchesRepo.Add(Matches match, int tournamentId)
        {
            //add team into TeamTournamet.cs..
            _context.Matches.Add(match);
            _context.SaveChanges();

            //TeamTournament teamTournament = new TeamTournament
            //{
            //    TeamId = team.TeamId,
            //    TournamentId = tournament_id,
            //    Team = team,
            //    Tournament = _context.Tournaments.Find(tournament_id),
            //    DateJoined = DateTime.Now,
            //};

            //// Add the TeamTournament entry using the UnitOfWork method
            //_unitOfWork.AddTeamTournament(teamTournament);

            //// Save changes to both Teams and TeamTournaments
            //_unitOfWork.Save();
            ////_unitOfWork.Team.Add(team, tournament_id);
            ////_context.TeamTournaments.Add(teamTournament);

            return match;
        }

        Matches IMatchesRepo.GetMatchesById(int id)
        {
            return _context.Matches.Find(id);
        }

        IEnumerable<Matches> IMatchesRepo.GetMatchesByTournamentId(int id) {
            return _context.Matches.Where(tournament => tournament.TournamentId == id);
        }

        Matches IMatchesRepo.Update(int id, Matches match)
        {
            // Retrieve the existing match entity from the database by Id
            var existingMatch = _context.Matches.Find(id);

            if (existingMatch != null)
            {
                // Manually update the properties, excluding the Id
                existingMatch.MatchDate = match.MatchDate;
                existingMatch.TeamAId = match.TeamAId;
                existingMatch.TeamBId = match.TeamBId;
                existingMatch.Result = match.Result;
                // Add other properties that you want to update

                // Save the changes to the database
                _context.SaveChanges();

                return existingMatch;
            }

            return null; // Handle the case where the match was not found
        }




        Matches IMatchesRepo.Delete(int id)
        {
            Matches t = _context.Matches.Find(id);
            if (t != null)
            {
                _context.Matches.Remove(t);
                _context.SaveChanges();
            }
            return t;
        }

        IEnumerable<Matches> IMatchesRepo.GetAllMatches()
        {
            return _context.Matches;
        }

    }
}
