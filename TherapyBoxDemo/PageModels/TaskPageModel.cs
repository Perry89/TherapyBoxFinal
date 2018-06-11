using FreshMvvm;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TherapyBoxDemo.Services;

namespace TherapyBoxDemo.PageModels
{
    public class TaskPageModel : FreshBasePageModel
    {
		const string comdText = "DROP TABLE TaskTable";
		const string cmdText = "Select * FROM sqlite_master WHERE type = 'table' AND name = ?";
		private List<TaskItems> list;
		public TaskPageModel()
		{
			try
			{
				list = new List<TaskItems>();
				string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "task.db3");
				var db = new SQLiteConnection(dpPath);
				var cmd = db.CreateCommand(cmdText, typeof(TaskTable).Name);
				var result = cmd.ExecuteScalar<string>();
				if (result == null)
				{
					list.Add(new TaskItems { TaskName = "", TaskToggle = false });
				}
				else
				{
					var data = db.Table<TaskTable>();
					foreach (var item in data)
					{
						TaskNameEntry = item.TaskName;
						if (item.TaskToggle == 0)
						{
							TaskToggled = false;
						}
						else
						{
							TaskToggled = true;
						}
						list.Add(new TaskItems { TaskName = TaskNameEntry, TaskToggle = TaskToggled });
					}
				}
				TaskList = list;
			}
			catch (Exception e)
			{
				var exc = e.ToString();
			}
		}

		private string _taskNameEntry;
		public string TaskNameEntry
        {
            get
            {
				return _taskNameEntry;
            }
            set
            {
				_taskNameEntry = value;
				RaisePropertyChanged("TaskNameEntry");
            }
        }
		private bool _taskToggled;
        public bool TaskToggled
        {
            get
            {
				return _taskToggled;
            }
            set
            {
				_taskToggled = value;
				RaisePropertyChanged("TaskToggled");
            }
        }
        private List<TaskItems> _taskList;
        public List<TaskItems> TaskList
        {
            get
            {
                return _taskList;
            }
            set
            {
                _taskList = value;
				RaisePropertyChanged("TaskList");
            }
        }

		private FreshAwaitCommand _saveTasksCommand;
        public FreshAwaitCommand SaveTasksCommand
        {
            get
            {
				if (_saveTasksCommand == null)
                {
					_saveTasksCommand = new FreshAwaitCommand(async (tcs) =>
                    {
						await SaveTasksCommandHandler();
                        tcs.SetResult(true);
                    });
                }
				return _saveTasksCommand;
            }

        }
		private FreshAwaitCommand _addNewTaskCommand;
        public FreshAwaitCommand AddNewTaskCommand
        {
            get
            {
				if (_addNewTaskCommand == null)
                {
					_addNewTaskCommand = new FreshAwaitCommand(async (tcs) =>
                    {
						await AddNewTaskCommandHandler();
                        tcs.SetResult(true);
                    });
                }
				return _addNewTaskCommand;
            }

        }
		public async Task AddNewTaskCommandHandler()
        {
            try
            {
				List<TaskItems> items = new List<TaskItems>();
				items = TaskList;
				items.Add(new TaskItems { TaskName = " ", TaskToggle = false });
				TaskList = items;

            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Task", ex.ToString(), "OK");
            }
        }
		public async Task SaveTasksCommandHandler()
        {
            try
            {
				if (TaskList.Count > 0)
                {
                    string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "task.db3");
                    var db = new SQLiteConnection(dpPath);

                    var cmd = db.CreateCommand(cmdText, typeof(TaskTable).Name);
                    var result = cmd.ExecuteScalar<string>();
					if (result != null)
					{
						
						var cmddrop = db.CreateCommand(comdText, typeof(TaskTable).Name);
						var resultdrop = cmddrop.ExecuteScalar<string>();
					}
                    db.CreateTable<TaskTable>();
					TaskTable tbl = new TaskTable();
					foreach (var item in TaskList)
					{
						if (!string.IsNullOrEmpty(item.TaskName))
						{
							tbl.TaskName = item.TaskName;
							if (item.TaskToggle == false)
							{
								tbl.TaskToggle = 0;

							}
							else
							{
								tbl.TaskToggle = 1;
							}
						}
						db.Insert(tbl);
					}
                    
                    await CoreMethods.DisplayAlert("Task", "Tasks have been sucesfull added", "OK");
					await CoreMethods.PopPageModel();
                }
                else
                {
                    await CoreMethods.DisplayAlert("Tasks", "There was a problem addding Tasks to Database", "Try again");
                }

            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Registration", ex.ToString(), "OK");
            }
        }
    }
    public class TaskItems
    {
        public string TaskName { get; set; }
        public bool TaskToggle { get; set; }
    }
}
