using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace TodoTask.Core
{	/// <summary>
	/// Todo task repository XML. The repository is responsible for providing an abstraction to actual data storage mechanism 
	/// whethe it be SQLite, XML or some other method
	/// </summary>
	public class TodoTaskRepositoryXML 
	{
		static string storeLocation= null;
		static List<TodoTask> tasks = new List<TodoTask>();
		static TodoTaskRepositoryXML(){

		}

		public static String DatabaseFilePath{
			get{
				var storeFileName = "TodoTaskDB.xml";
				string libraryPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				var path = Path.Combine (libraryPath, storeFileName);
				return path;
			}
		}
		public static TodoTask GetTodoTask(int id){
			for (var task = 0; task < tasks.Count; task++) {
				if (tasks [task].ID == id) {
					return tasks [task];
				}
			}
			return new TodoTask (){ ID = id };
		}

		public static IEnumerable<TodoTask> GetAllTodoTasks(){
			return tasks;
		}

		public static int SaveTodoTask(TodoTask item){
			var max_counter = 0;
			if (tasks.Count > 0) {
				max_counter = tasks.Max (x => x.ID);
			}
			if (item.ID == 0) {
				item.ID = ++max_counter;
				tasks.Add (item);
			} else {
				var i = tasks.Find (x => x.ID == item.ID);
				i = item;
			}
			WriteToXml ();
			return max_counter;
		}

		public static int DeleteTodoTask(int id){
			for (var t = 0; t < tasks.Count; t++) {
				if (tasks [t].ID == id) {
					tasks.RemoveAt (t);
					WriteToXml ();
					return 1;
				}
			}
			return -1;
		}

		public static void WriteToXml(){
			var serializer = new XmlSerializer(typeof(List<TodoTask>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, tasks);
			}
		}
	}
}

