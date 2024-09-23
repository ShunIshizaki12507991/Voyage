namespace Voyage.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using UniRx;

    public enum UpdateGroup
    {
        Units,
        Ui,
    }

    public static class UpdateTaskRegistry
    {
        private sealed class TaskGroup
        {
            public UpdateGroup UpdateGroup { get; }
            public IUpdateTaskComponent Task { get; }

            public TaskGroup(UpdateGroup group, IUpdateTaskComponent task)
            {
                UpdateGroup = group;
                Task = task;
            }
        }

        private static readonly ConcurrentQueue<TaskGroup> ms_RegisteredTasks;
        private static readonly ConcurrentDictionary<UpdateGroup, List<IUpdateTaskComponent>> ms_Tasks;

        static UpdateTaskRegistry()
        {
            ms_RegisteredTasks = new();
            ms_Tasks = new();
        }

        public static IDisposable Register(IUpdateTaskComponent task, UpdateGroup group)
        {
            ms_RegisteredTasks.Enqueue(new TaskGroup(group, task));
            return Disposable.CreateWithState(task, static task => task.SetActivate(false));
        }

        public static void Run()
        {
            while (ms_RegisteredTasks.TryDequeue(out var t))
            {
                if (!ms_Tasks.ContainsKey(t.UpdateGroup))
                    ms_Tasks[t.UpdateGroup] = new();
                t.Task.SetActivate(true);
                ms_Tasks[t.UpdateGroup].Add(t.Task);
            }

            foreach (var (_, tasks) in ms_Tasks)
                foreach (var task in tasks)
                    if (task.IsEnable)
                        task.OnUpdate();
        }
    }
}