using Dapper;
using logapp.DTOs;
using logapp.Models;
using logapp.Utilities;

namespace logapp.Repositories;

public interface ILogRepository
{
    Task<List<Log>> GetAllLogs(QDateFilterDTO dateFilter, String tagtypename = null);
    Task<Log> GetLogById(int Id);
    Task<Log> CreateLog(Log Item);
    Task<bool> UpdateLog(Log Item);
    Task DeleteLog(int Id);

    Task<List<Tag>> GetLogTagsById(int Id);
    // Task<List<User>> LogUpdatedByUser(int id);
    Task StoreSeenId(int Id, long id);
    // Task<List<LogSeen>> LogSeen(int id);
    // Task<Log> StoreSeenId(int userid, long id);
}

public class LogRepository : BaseRepository, ILogRepository
{
    public LogRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Log> CreateLog(Log Item)
    {
        var query = $@"INSERT INTO ""{TableNames.log}"" (title, description, stack_trace) 
       VALUES (@Title, @Description, @StackTrace)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Log>(query, Item);
            return res;
        }
    }

    public async Task DeleteLog(int Id)
    {
        var query = $@"DELETE FROM ""{TableNames.log}"" WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<Log>> GetAllLogs(QDateFilterDTO dateFilter, String tagtypename = null)
    {
        List<Log> res;



        var query = $@"SELECT * FROM ""{TableNames.log}"" ";


        if (dateFilter is not null && (dateFilter.FromDate.HasValue || dateFilter.ToDate.HasValue))
        {
            if (dateFilter.FromDate is null) dateFilter.FromDate = DateTimeOffset.MinValue;
            if (dateFilter.ToDate is null) dateFilter.ToDate = DateTimeOffset.Now;
            query += "WHERE created_at BETWEEN  @FromDate AND  @ToDate";
        }

        // var toAdd = QueryBuilder.AddWhereClauses(whereClauses);
        // query += toAdd;
        var paramsObj = new
        {
            FromDate = dateFilter?.FromDate,
            ToDate = dateFilter?.ToDate,
        };
        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<Log>(query, paramsObj)).AsList();
        }
        return (res);
    }

    public async Task<List<Tag>> GetLogTagsById(int Id)
    {
        // SELECT * FROM tag t LEFT JOIN log_tag lt ON lt.tag_id  = t.id  where lt.log_id = 1
        var query = $@"SELECT * FROM ""{TableNames.tag}"" t LEFT JOIN ""{TableNames.log_tag}"" lt ON lt.tag_id = t.id  WHERE  lt.log_id = @Id ";

        using (var con = NewConnection)
        {
            var res = (await con.QueryAsync<Tag>(query, new { Id })).AsList();
            return res;
        }
    }

    public async Task<Log> GetLogById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.log}"" WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Log>(query, new { Id = Id });
    }

    public async Task<bool> UpdateLog(Log Item)
    {
        var query = $@"UPDATE ""{TableNames.log}"" SET description = @Description WHERE id = @Id";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }

    // public async Task<List<User>> LogUpdatedByUser(int id)
    // {
    //     var query = $@"SELECT * FROM ""{TableNames.user}"" u LEFT JOIN ""{TableNames.log}"" l ON l.updated_by_user_id = u.id  WHERE  l.id = @Id ";

    //     using (var con = NewConnection)
    //     {
    //         var res = (await con.QueryAsync<User>(query, new { id })).AsList();
    //         return res;
    //     }
    // }

    public async Task StoreSeenId(int Id, long id)
    {
        var query = $@"INSERT INTO ""{TableNames.log_seen}"" (user_id, log_id) 
      VALUES (@UserId, @LogId)
        RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync(query, new { UserId = Id, LogId = id });

        }
    }

    // public async Task<List<LogSeen>> LogSeen(int id)
    // {
    //     var query = $@"SELECT * FROM ""{TableNames.log}"" l LEFT JOIN ""{TableNames.log_seen}"" ls ON ls.log_id = l.id ";
    //     using (var con = NewConnection)
    //     {
    //         var res = (await con.QueryAsync<LogSeen>(query, new { id })).AsList();
    //         return res;
    //     }
    // }

    // public  async Task<Log> StoreSeenId(int userid, long id)
    // {
    //     var query = $@"INSERT INTO ""{TableNames.log_seen}"" (user_id, log_id) 
    //    VALUES (@UserId, @LogId)
    //    RETURNING *";


    //     using (var con = NewConnection)
    //     {
    //         var res = await con.QuerySingleOrDefaultAsync<Log>(query, userid,   new{id} );
    //         return null;
    //     }
    // }
}