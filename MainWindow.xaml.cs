using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WPF_TreeView
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Default Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}
		#endregion
		
		#region  loaded
		/// <summary>
		/// when the application first loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Get every logical drive on the machine
			var dirs = Directory.GetLogicalDrives();
			foreach(var drive in dirs)
			{
				// create a new item fot it
				var item = new TreeViewItem();
				// set the header
				item.Header = drive;
				// Add the full path
				item.Tag = drive;

				// add a dummy item
				item.Items.Add(null);

				// Listen out item being expanded
				item.Expanded += FolderExpanded;

				// Add it to the main tree-view	
				FolderView.Items.Add(item);
			}
			
		}
		#endregion

		#region Folder Expanded
		/// <summary>
		/// while the folder expanded find sub folders/fieles
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FolderExpanded(object sender, RoutedEventArgs e)
		{
			var item = (TreeViewItem)sender;
			if (item.Items.Count != 1 || item.Items[0] != null) return;
			item.Items.Clear();

			#region Get folders
			// Get the full path
			var fullpath = (string)item.Tag;
			// Ceate a blank list for directories
			var directories = new List<string>();
			// try and get directories from the folder
			// ignoring any issues doing so
			try
			{
				var dirs = Directory.GetDirectories(fullpath);
				if (dirs.Length > 0)
					directories.AddRange(dirs);

			}
			catch { }

			// For each directory
			directories.ForEach(directorypath =>
			{
				// Create direcory item
				var subItem = new TreeViewItem()
				{
					// set header as folder name
					Header = GetFileFolderName(directorypath),
					// And tag as fullpath
					Tag = directorypath
				};
				// Add a dummy item so we can expand folder
				subItem.Items.Add(null);

				// Handle expanding
				subItem.Expanded += FolderExpanded;

				// Add subitem to parent item
				item.Items.Add(subItem);

			});
			#endregion

			#region Get files


			// Ceate a blank list for directories
			var files = new List<string>();
			// try and get directories from the folder
			// ignoring any issues doing so
			try
			{
				var fs = Directory.GetFiles(fullpath);
				if (fs.Length > 0)
					files.AddRange(fs);

			}
			catch { }

			// For each file
			files.ForEach(filepath =>
			{
				// Create file item
				var subItem = new TreeViewItem()
				{
					// set header as file name
					Header = GetFileFolderName(filepath),
					
					// And tag as fullpath
					Tag = filepath
					
				};

				// Add subitem to parent item
				item.Items.Add(subItem);

			});
			#endregion


		}

		#endregion

		#region Helpers
		/// <summary>
		/// Get file and folder name from a path
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static  string GetFileFolderName(string path)
		{
			// If we have no path,retun empty string
			if (string.IsNullOrEmpty(path))
				return string.Empty;
			
			// Make all slashes back slash
			var normalize = path.Replace('/', '\\');
			var lastIndex = normalize.LastIndexOf('\\');

			// If we don't find a backslash,the return the path itself
			if (lastIndex <= 0)
				return path;

			//Return after the name after backslash
			return normalize.Substring(lastIndex + 1);

		}
		#endregion
	}
}
