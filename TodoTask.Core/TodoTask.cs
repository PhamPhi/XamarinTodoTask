
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
/// <summary>
/// Author: Larry Pham. The TodoTask Domain class which have represented the data-domain for application
/// This class contains the attributes fields such as: ID, Name, Notes , Done(Status of modifying)
/// </summary>
namespace TodoTask.Core
{
	public class TodoTask
	{
		public TodoTask()
		{

		}
		public int ID { get; set; }
		public String Name{ get; set; }
		public String Notes{ get; set; }
		public bool Done{ get; set; }
	}
}

