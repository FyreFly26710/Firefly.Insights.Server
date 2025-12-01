using System;
using Server.Common.Types;
using Server.Contents.Api.Models.Requests;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Application.Queries;

public interface IArticleQueries
{
    Task<ArticleDto> GetArticleById(long articleId);
    Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request);
}
