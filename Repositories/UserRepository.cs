using Dapper;
using logapp.Models;
using logapp.Utilities;

namespace logapp.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUserById(int Id);
    Task<User> CreateUser(User Item);
    Task<bool> UpdateUser(User Item);
    Task DeleteUser(int Id);

    Task<User> GetByEmail(string Email);


    Task<List<Tag>> GetUserTagsById(int Id);
    // object DeleteUser(int id);
}

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<User> CreateUser(User Item)
    {
        var query = $@"INSERT INTO ""{TableNames.user}"" (name, hash_password, email) 
       VALUES (@Name, @HashPassword, @Email)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, Item);
            return res;
        }
    }

    public async Task<User> GetByEmail(string Email)
    {
        {
            var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE email = @Email";

            using (var con = NewConnection)

                return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
        }


    }

    public async Task DeleteUser(int Id)
    {
        var query = $@"UPDATE  ""{TableNames.user}"" SET status = false WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<User>> GetAllUsers()
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<User>(query)).AsList();
    }

    public async Task<User> GetUserById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Id });
    }

    public async Task<List<Tag>> GetUserTagsById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.tag}"" t LEFT JOIN ""{TableNames.user_tag}"" ut ON ut.tag_id = t.id  WHERE  ut.user_id = @Id ";

        using (var con = NewConnection)
        {
            var res = (await con.QueryAsync<Tag>(query, new { Id })).AsList();
            return res;
        }
    }

    public async Task<bool> UpdateUser(User Item)
    {
        var query = $@"UPDATE ""{TableNames.user}"" SET name = @Name, hash_password = @HashPassword WHERE id = @Id";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }

}