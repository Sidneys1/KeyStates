using System.Windows.Media;

namespace WpfTest
{
	public class DisplayItem
	{
		public string Text { get; set; }
		public Color Color { get; set; }

		public DisplayItem(string text, Color color)
		{
			Text = text;
			Color = color;
		}
	}
}