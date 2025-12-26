using System;

namespace Server.Contents.Api.Models.Responses;

public class TagDto
{
    public long TagId { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";

}
