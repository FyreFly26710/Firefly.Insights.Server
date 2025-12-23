namespace Server.Contents.Api.Application.Queries;

public interface IArticleQueries
{
    Task<ArticleDto> GetArticleById(long articleId);
    Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request);
}
