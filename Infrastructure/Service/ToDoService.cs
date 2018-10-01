namespace Infrastructure.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Repository.Interfaces;
    using models = Infrastructure.Models;

    public class ToDoService
    {
        private readonly IListRepository listRepository;
        private readonly ITaskRepository taskRepository;

        public ToDoService(IListRepository listRepository, ITaskRepository taskRepository)
        {
            this.listRepository = listRepository;
            this.taskRepository = taskRepository;
        }

        public async Task<List<models.List>> GetLists(string searchValue, int skip = 0, int take = 100)
        {
            List<models.List> lists = new List<models.List>();
            try
            {
                lists = this.listRepository.GetLists(searchValue, skip, take);

                if (lists?.Any() == true)
                {
                    // Let get the tasks
                    List<Task> taskList = null;
                    int maxParallelTasks = 10; // TODO: This should be a configuration
                    int processed = 0;

                    while (processed <= lists.Count)
                    {
                        taskList = new List<Task>();

                        foreach (var list in lists.Skip(processed).Take(maxParallelTasks))
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                list.Tasks = this.taskRepository.GetTasks(list.Id);
                            }));

                            processed++;
                        }

                        if (taskList?.Any() == true)
                        {
                            await Task.WhenAll(taskList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: log the exception
                return null;
            }

            return lists;
        }
    }
}