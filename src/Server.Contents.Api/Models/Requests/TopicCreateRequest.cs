using System;

namespace Server.Contents.Api.Models.Requests;

public class TopicCreateRequest
{
    public required string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public required long CategoryId { get; set; }
    public string ImageUrl { get; set; } = "";
    public bool IsHidden { get; set; } = false;
}
