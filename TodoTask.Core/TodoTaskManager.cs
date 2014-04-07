using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TodoTask.Core
{
	public static class TodoTaskManager
	{
		static TodoTaskManager(){
			
		}

		public static TodoTask GetTodoTask(int id){
			return TodoTaskRepositoryADO.GetTodoTask (id);
		}

		public static List<TodoTask> GetAllTodoTasks(){
			return new List<TodoTask> (TodoTaskRepositoryADO.GetAllTodoTasks());
		}

		public static int SaveTask(TodoTask item){
			return TodoTaskRepositoryADO.SaveTodoTask (item);
		}

		public static int DeteleTask(int id){
			return TodoTaskRepositoryADO.DeleteTodoTask (id);
		}
	}
}

