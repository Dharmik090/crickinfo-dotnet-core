namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface IPointsTableRepo
	{
		PointsTable GetPointTableById(int id);
		PointsTable GetPointsTableByTeamIdAndTournamentId(int teamId, int tournamentId);

		IQueryable<PointsTable> GetPointsTableByTournamentId(int id);
		PointsTable Add(PointsTable pointsTable);
		IEnumerable<PointsTable> GetAllPointsTable();
		PointsTable Update(PointsTable pointsTable);
		PointsTable Delete(int id);

	}
}
