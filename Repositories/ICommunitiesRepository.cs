﻿using Reddit.Models;

namespace Reddit.Repositories
{
    public interface ICommunitiesRepository
    {
        public Task<PagedList<Community>> GetCommunities(int page, int pageSize, string? SearchKey, string? SortTerm, bool? isAscending = true);
    }
}
