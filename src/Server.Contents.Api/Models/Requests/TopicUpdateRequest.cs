namespace Server.Contents.Api.Models.Requests;

public class TopicUpdateRequest
{
    public long TopicId { get; set; }
    public required string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public required long CategoryId { get; set; }
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; } = false;
}
