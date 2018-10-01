namespace Infrastructure.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using models = Infrastructure.Models;

    public interface IListRepository
    {
        bool AddList(models.List list);

        models.List GetList(Guid listId);

        List<models.List> GetLists(string searchValue, int skip, int take);

        bool UpdateList(models.List list);
    }
}