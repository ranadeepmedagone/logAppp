

using Microsoft.AspNetCore.Mvc;
using logapp.Models;
using logapp.Repositories;
using logapp.DTOs;

namespace logapp.Controllers;


[ApiController]
[Route("api/Tag")]


public class TagController : ControllerBase
{

    private readonly ILogger<TagController> _Tagger;
    private readonly ITagRepository _Tag;

    public TagController(ILogger<TagController> Tagger,
    ITagRepository Tag)
    {
        _Tagger = Tagger;
        _Tag = Tag;
    }



    [HttpGet]

    public async Task<ActionResult<List<TagDTO>>> GetAllTags([FromQuery] QTagFilterDTO tagFilter = null)
    {
        var TagsList = await _Tag.GetAllTags(tagFilter);

        // Tag -> TagDTO
        var dtoList = TagsList.Select(x => x.asDto);

        return Ok(dtoList);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Tag>> GetTagById([FromRoute] int id)
    {
        var res = await _Tag.GetTagById(id);

        if (res is null)
            return NotFound("No Tag found with given id");

        var dto = res.asDto;
        dto.ListOfLogs = (await _Tag.GetTagLogsById(id)).Select(x => x.asDto).ToList();
        dto.TagTypes = (await _Tag.GetTagTypesByTagId(id)).Select(x => x.asDto).ToList();




        return Ok(dto);
    }
    [HttpPost]
    public async Task<ActionResult<TagDTO>> CreateTag([FromBody] CreateTagDTO Data)
    {
        // if (!(new string[] { "male", "female" }.Contains(Data.Gender.Trim().ToLower())))
        //     return BadRequest("Gender value is not recognized");

        //    / var subtractDate = DateTimeOffset.Now - Data.DateOfBirth;
        //     if (subtractDate.TotalDays / 365 < 18.0)
        //         return BadRequest("Tag must be at least 18 years old");

        var toCreateTag = new Tag
        {
            Name = Data.Name.Trim(),
            TypeId = Data.TypeId,

        };

        var createdTag = await _Tag.CreateTag(toCreateTag);

        return StatusCode(StatusCodes.Status201Created, createdTag.asDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag([FromRoute] int id,
    [FromBody] UpdateTagDTO Data)
    {
        var existing = await _Tag.GetTagById(id);
        if (existing is null)
            return NotFound("No Tag found with given id");

        var toUpdateTag = existing with
        {
            Name = Data.Name?.Trim() ?? existing.Name,

        };

        var didUpdate = await _Tag.UpdateTag(toUpdateTag);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Tag");

        return NoContent();
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag([FromRoute] int id)
    {
        var existing = await _Tag.GetTagById(id);
        if (existing is null)
            return NotFound("No Tag found with given Tag name");

        var didDelete = _Tag.DeleteTag(id);

        return NoContent();
    }
}