using Dapper;
using logapp.DTOs;
using logapp.Models;
using logapp.Repositories;
using logapp.Utilities;

namespace logapp.Repositories;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTags(QTagFilterDTO tagFilter);
    Task<Tag> GetTagById(int Id);
    Task<Tag> CreateTag(Tag Item);
    Task<bool> UpdateTag(Tag Item);
    Task DeleteTag(int Id);
    Task<List<Log>> GetTagLogsById(int Id);
    Task<List<TagType>> GetTagTypesByTagId(int id);
    // Task GetAllLogsByTagName(string typeName);

    // Task<List<Tag>> GetTagTagsById(int Id);

}

public class TagRepository : BaseRepository, ITagRepository
{
    public TagRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Tag> CreateTag(Tag Item)
    {
        var query = $@"INSERT INTO ""{TableNames.tag}"" (name) 
       VALUES (@Name)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Tag>(query, Item);
            return res;
        }
    }

    public async Task DeleteTag(int Id)
    {
        var query = $@"DELETE FROM ""{TableNames.tag}"" WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<Tag>> GetAllTags(QTagFilterDTO tagfilter)
    {
        List<Tag> res;
        var query = $@"SELECT * FROM ""{TableNames.tag}"" ";


        if (tagfilter.Name is not null)
        {
            query += "WHERE name = @Name";
        }


        // var toAdd = QueryBuilder.AddWhereClauses(whereClauses);
        // query += toAdd;
        var paramsObj = new
        {
            Name = tagfilter?.Name,
        };
        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<Tag>(query, paramsObj)).AsList();
        }
        return (res);

        // using (var con = NewConnection)
        //     return (await con.QueryAsync<Tag>(query)).AsList();
    }

    public async Task<List<Log>> GetTagLogsById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.log}"" l LEFT JOIN ""{TableNames.log_tag}"" lt ON lt.tag_id = l.id  WHERE  lt.log_id = @Id ";

        using (var con = NewConnection)
        {
            var res = (await con.QueryAsync<Log>(query, new { Id })).AsList();
            return res;
        }
    }

    // public async Task<List<Tag>> GetTagTagsById(int Id)
    // {
    //     // SELECT * FROM tag t LEFT JOIN Tag_tag lt ON lt.tag_id  = t.id  where lt.Tag_id = 1
    //     var query = $@"SELECT * FROM ""{TableNames.tag}"" t LEFT JOIN ""{TableNames.tag_type}"" tt ON tt.id = t.id  WHERE  tt.id = @Id ";

    //     using (var con = NewConnection) 
    //     {
    //         var res = (await con.QueryAsync<Tag>(query, new { Id })).AsList();
    //         return res;
    //     }
    // }

    public async Task<Tag> GetTagById(int Id)
    {
        // var query = $@"SELECT t.typename AS TagType * FROM ""{TableNames.tag}"" t LEFT JOIN ""{TableNames.tag_type}"" tt ON tt.id = t.type_id WHERE t.id = @Id";
        var query = $@"SELECT * FROM ""{TableNames.tag}""  WHERE id = @Id";

        // var query = $@"SELECT tt.* FROM ""{TableNames.tag}"" t LEFT JOIN ""{TableNames.tag_type}"" tt ON tt.id = t.type_id WHERE t.id = @Id";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Tag>(query, new { Id });
    }

    public async Task<bool> UpdateTag(Tag Item)
    {
        var query = $@"UPDATE ""{TableNames.tag}"" SET name = @Name WHERE id = @Id";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }

    public async Task<List<TagType>> GetTagTypesByTagId(int id)
    {
        var query = $@"SELECT * FROM ""{TableNames.tag_type}"" tt LEFT JOIN ""{TableNames.tag}"" t ON t.type_id = tt.id  WHERE  t.id = @Id ";

        using (var con = NewConnection)
        {
            var res = (await con.QueryAsync<TagType>(query, new { Id = id })).AsList();
            return res;
        }
    }

    // public  async Task GetAllLogsByTagName(string typeName)
    // {
    //     var query = $@"SELECT * FROM ""{TableNames.tag}"" ORDER BY created_at DESC ";

    //     using (var con = NewConnection)
    //         return (await con.QueryAsync<Tag>(query)).AsList();
    // }
}