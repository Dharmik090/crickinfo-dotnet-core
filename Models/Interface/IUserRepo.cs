namespace crickinfo_mvc_ef_core.Models.Interface
{
    public interface IUserRepo
    {
        User GetUserById(int id);

        User GetUserByEmail(string email);
        IEnumerable<User> GetUsers();
        User Add(User user);
        User Update(int id,User user);
        User Delete(int id);
    }
}
