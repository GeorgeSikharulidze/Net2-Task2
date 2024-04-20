using Microsoft.EntityFrameworkCore;
using Reddit.Models;
using System.Linq.Expressions;

namespace Reddit.Repositories
{
    public class CommunitiesRepository : ICommunitiesRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunitiesRepository(ApplicationDbContext applcationDBContext)
        {
            _context = applcationDBContext;
        }

        public async Task<PagedList<Community>> GetCommunities(int page, int pageSize, string? SearchKey, string? SortTerm, bool? isAscending = true)
        {
            var Communities = _context.Communities.AsQueryable();

            if (isAscending == false)
            {
                Communities = Communities.OrderByDescending(GetSortExpression(SearchKey));
            }
            else
            {
                Communities = Communities.OrderBy(GetSortExpression(SearchKey));
            }


            if (!string.IsNullOrEmpty(SearchKey))
            {
                Communities = Communities.Where(community =>
                     community.Name.Contains(SearchKey) || community.Description.Contains(SearchKey));
            }

            return await PagedList<Community>.CreateAsync(Communities, page, pageSize);
        }

        public Expression<Func<Community, object>> GetSortExpression(string? sortTerm)
        {
            sortTerm = sortTerm?.ToLower();
            return sortTerm switch
            {
                "createdAt" => Community => Community.CreateAt,
                "PostsCount" => Community => Community.Posts.Count(),
                "subscribersCount" => Community => Community.Subscribers.Count(),
                _ => Community => Community.Id
            };
        }
    }
}
