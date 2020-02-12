using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPF_TreeView
{
	/// <summary>
	/// Convert fullpath to a specific image type of a drive,foldeer or file
	/// </summary>
	[ValueConversion(typeof(string),typeof(BitmapImage))]
	public class HeaderToImageConverter : IValueConverter
	{
		public static HeaderToImageConverter Instance = new HeaderToImageConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Get the full path
			var path = (string)value;

			// If the path is null,ignore
			if (path == null)
				return null;

			// Define a image to store the image path
			string image = "Images/file.png";

			// Get the name of file/folder
			var name = MainWindow.GetFileFolderName(path);

			//if the name is blank,we assume it as a drive,as we cannot have a blank file or folder name
			if (string.IsNullOrEmpty(name))
				image = "Images/drive.png";
			else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
				image = "Images/folder_closed.png";
			//else if(new FileInfo(path).Attributes.HasFlag(FileAttributes))
			//
			return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
		

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
