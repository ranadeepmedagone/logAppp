
using System.Security.Claims;
using logapp.DTOs;
using logapp.Models;
using logapp.Repositories;
using logapp.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace logapp.Controllers;


[ApiController]
[Route("api/Log")]


public class LogController : ControllerBase
{

    private readonly ILogger<LogController> _logger;
    private readonly ILogRepository _Log;
    private readonly ITagRepository _Tag;

    // public bool PartiaLlyDeleted { get; private set; }

    public LogController(ILogger<LogController> logger,
    ILogRepository Log, ITagRepository Tag)
    {
        _logger = logger;
        _Log = Log;
        _Tag = Tag;
    }



    [HttpGet]



    public async Task<ActionResult<List<LogDTO>>> GetAllLogs([FromQuery] QDateFilterDTO dateFilter = null)
    {

        var IsSuperUser = User.Claims.FirstOrDefault(c => c.Type == UserConstants.IsSuperUser)?.Value;
        var userId = User.Claims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value;
        var Id = int.Parse(userId);
        if (bool.Parse(IsSuperUser))
        {
            var LogsList = await _Log.GetAllLogs(dateFilter);

            var dtoList = LogsList.Select(x => x.asDto);

            return Ok(dtoList);
        }
        if (bool.Parse(IsSuperUser) == false)
        {

            var LogsList = await _Log.GetAllLogsforUser(Id, dateFilter);

            var dtoList = LogsList.Select(x => x.asDto);

            return Ok(dtoList);
        }
        else
        {
            return BadRequest("logs not found");
        }


    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Log>> GetLogById([FromRoute] int id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value;
        var Id = int.Parse(userId);

        var res = await _Log.GetLogById(id);
        await _Log.StoreSeenId(Id, res.Id);


        if (res is null)
            return NotFound("No Log found with given id");
        var dto = res.asDto;
        // dto.UpdatedByUserId = Id;
        dto.ListOfTags = (await _Log.GetLogTagsById(id)).Select(x => x.asDto).ToList();
        // dto.UpdatedByUser = (await _Log.LogUpdatedByUser(id)).Select(x => x.asDto).ToList();



        // if(){
        //        var result = await _Log.StoreSeenId(userId);

        //  if (result is null)
        //     return NotFound("No Log found with given id");
        // }



        // dto.LogSeenBy = (await _Log.LogSeen(id)).Select(x => x.asDto).ToList();



        return Ok(dto);
    }
    [HttpPost]
    public async Task<ActionResult<LogDTO>> CreateLog([FromBody] CreateLogDTO Data)
    {
        // if (!(new string[] { "male", "female" }.Contains(Data.Gender.Trim().ToLower())))
        //     return BadRequest("Gender value is not recognized");

        //    / var subtractDate = DateTimeOffset.Now - Data.DateOfBirth;
        //     if (subtractDate.TotalDays / 365 < 18.0)
        //         return BadRequest("Log must be at least 18 years old");

        var toCreateLog = new Log
        {
            Title = Data.Title.Trim(),
            Description = Data.Description.Trim().ToLower(),
            StackTrace = Data.StackTrace
        };

        var createdLog = await _Log.CreateLog(toCreateLog);

        return StatusCode(StatusCodes.Status201Created, createdLog.asDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLog([FromRoute] int id,
    [FromBody] UpdateLogDTO Data)
    {
        var Id = GetUserIdFromClaims(User.Claims);

        var existing = await _Log.GetLogById(id);


        //    await _Tag.UpdateTagByLog();
        if (existing is null)
            return NotFound("No Log found with given id");

        var toUpdateLog = existing with
        {
            Description = Data.Description?.Trim() ?? existing.Description,
            UpdatedByUserId = Id,

        };

        var didUpdate = await _Log.UpdateLog(toUpdateLog, Data.Tags);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Log");

        return NoContent();
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLog([FromRoute] int id)
    {

        // if (PartiaLlyDeleted == false)
        //     return BadRequest("User not found");
        var existing = await _Log.GetLogById(id);
        if (existing is null)
            return NotFound("No Log found with given Log name");



        var didDelete = _Log.DeleteLog(id);

        return NoContent();
    }



    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }

    // private int GetCurrentUserId()
    // {

    //     return Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value);
    // }

}