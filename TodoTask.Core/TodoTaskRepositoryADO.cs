using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TodoTask.Core
{	/// <summary>
	/// Todo task repository ADO. Applying the singleton pattern for building the TodoTaskRepository
	/// </summary>
	public class TodoTaskRepositoryADO
	{
		TodoTaskDatabase db = null;
		protected static String dbLocation;
		protected static TodoTaskRepositoryADO me;

		static TodoTaskRepositoryADO(){
			me = new TodoTaskRepositoryADO ();
		}

		protected TodoTaskRepositoryADO(){
			dbLocation = DatabaseFilePath;
			db = new TodoTaskDatabase (dbLocation);
		}

		public static String DatabaseFilePath{
			get{
				var sqliteFileName = "TodoDB.db3";
				string libraryPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				var path = Path.Combine (libraryPath, sqliteFileName);
				return path;
			}
		}

		public static TodoTask GetTodoTask(int id){
			return me.db.GetItem (id);
		}

		public static IEnumerable<TodoTask> GetAllTodoTasks(){
			return me.db.GetItems ();
		}

		public static int SaveTodoTask(TodoTask item){
			return me.db.SaveItem (item);
		}

		public static int DeleteTodoTask(int id){
			return me.db.DeleteItem (id);
		}
	}
}

