using crickinfo_mvc_ef_core.Models.Interface;

namespace crickinfo_mvc_ef_core.Models.SQL
{
	public class SQLUserRepo : IUserRepo
	{
		private readonly CrickInfoContext _context;
		public SQLUserRepo(CrickInfoContext context)
		{
			_context = context;
		}

		User IUserRepo.Add(User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
			return user;
		}

		User IUserRepo.GetUserById(int id)
		{
			return _context.Users.Find(id);
		}

		User IUserRepo.GetUserByEmail(string email)
		{
			return _context.Users.FirstOrDefault(user => user.Email == email);
		}

		User IUserRepo.Update(int id,User user)
		{
			user.Id = id;
			var u = _context.Users.Attach(user);
			
			u.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_context.SaveChanges();
			return user;
		}

		User IUserRepo.Delete(int id)
		{
			User u = _context.Users.Find(id);
			if (u != null)
			{
				_context.Users.Remove(u);
				_context.SaveChanges();
			}
			return u;	
		}

		IEnumerable<User> IUserRepo.GetUsers()
		{
			return _context.Users;
		}
	}

}
