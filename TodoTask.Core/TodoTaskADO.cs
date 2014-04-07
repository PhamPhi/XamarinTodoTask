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
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
namespace TodoTask.Core
{
	public class TodoTaskDatabase
	{
		static object locker = new object();
		public SqliteConnection connection;
		public String path;
		/// <summary>
		/// Initializes a new instance of the <see cref="TodoTask.Core.TodoTaskDatabase"/> class.
		/// If the database doesn't exist, it will create the new database and all the tables
		/// </summary>
		public TodoTaskDatabase(string dbPath){
			var output = "";
			path = dbPath;
			bool exists = File.Exists (dbPath);	
			if (!exists) {
				connection = new SqliteConnection ("Data Source= " + dbPath);
				var commands = new String[]{" CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Notes NTEXT, Done INTEGER);" };
				foreach (var command in commands) {
					using (var cmd = connection.CreateCommand ()) {
						cmd.CommandText = command;
						var i = cmd.ExecuteNonQuery ();
					}
				}
			} else {
				// It've already existed, do nothing
			}
			Console.WriteLine (output);
		}
		/// <summary>
		/// Froms the reader. Using this method for handling the reading the Data by DataReader
		/// </summary>
		/// <returns>The reader.</returns>
		/// <param name="inReader">In reader.</param>
		public TodoTask FromReader (SqliteDataReader inReader){
			var task = new TodoTask ();
			task.ID = Convert.ToInt32(inReader["_id"]);
			task.Name = inReader ["Name"].ToString ();
			task.Notes = inReader ["Notes"].ToString ();
			task.Done = Convert.ToInt32 (inReader ["Done"]) == 1 ? true : false;
			return task;
		}
		/// <summary>
		/// Gets the items. This method used to get the enum of items which stored into the TodoTask database
		/// </summary>
		/// <returns>The items, Enum of Item.</returns>
		public IEnumerable<TodoTask> GetItems(){
			var taskList = new List<TodoTask> ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items]"; 
					var reader = contents.ExecuteReader ();
					while (reader.Read ()) {
						taskList.Add (FromReader (reader));
					}
				}
				connection.Close ();
			}
			return taskList;
		}
		public TodoTask GetItem(int id){
			var task = new TodoTask ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = " SELECT [_id],[name], [Notes], [Done] from [Items] WHERE [_id] = ?";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					var reader = command.ExecuteReader ();
					while (reader.Read ()) {
						task = FromReader (reader);
						break;
					}
				}
				connection.Close ();
			}
			return task;
		}
		/// <summary>
		/// Saves the item. It used to update the TodoTask record to the Table Items
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="item">Item. The TodoTask record </param>
		public int SaveItem(TodoTask item){
			int reader;
			lock (locker) {
				if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source= " + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = " UPDATE [Items] SET [Name]= ?, [Notes] = ?, [Done]= ? WHERE [_id] = ?; ";
						command.Parameters.Add (new SqliteParameter (DbType.String){ Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String){ Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32){ Value = item.Done });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						reader = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return reader;
				}else{
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = " INSERT INTO [Items] ([Name], [Notes], [Done] VALUE (?, ?, ? )";
						command.Parameters.Add (new SqliteParameter (DbType.String){ Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String){ Value = item.Notes});
						command.Parameters.Add (new SqliteParameter (DbType.Int32){ Value = item.Done });
						reader = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return reader;
				}
			}
		}
		/// <summary>
		/// Method DeleteItem(). It used to remove the item from the Table 'Items' with the given id
		/// </summary>
		/// <returns>The item. a record of table Items</returns>
		/// <param name="id">Identifier. The given identifier of current record </param>
		public int DeleteItem( int id ){
			lock (locker) {
				int result; 
				connection = new SqliteConnection (" Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "DELETE FROM [Items] WHERE [_id]= ?;";
					command.Parameters.Add (new SqliteParameter (DbType.Int32){ Value = id });
					result = command.ExecuteNonQuery ();
				}
				connection.Close ();
				return result;
			}
		}


	}
}

