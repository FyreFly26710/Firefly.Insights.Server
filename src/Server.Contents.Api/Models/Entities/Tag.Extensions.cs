using System;

namespace Server.Contents.Api.Models.Entities;

public partial class Tag
{

    public TagDto ToTagDto() => new TagDto()
    {
        TagId = Id,
        Name = Name,
        Type = Type.ToString()
    };
}
