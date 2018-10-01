namespace Infrastructure.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Models;

    public interface ITaskRepository
    {
        bool AddTask(Task task);

        Task GetTask(Guid taskId, Guid listId);

        List<Task> GetTasks(Guid listId);

        bool UpdateTask(Task task);
    }
}