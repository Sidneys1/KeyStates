using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using KeyStates;

namespace WpfTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			PassiveKeyboardMonitor.KeyDown += args =>
			{
				Dispatcher.Invoke(DispatcherPriority.Input, new KeyEvent(ProcessKeyDown), args);
			};

			PassiveKeyboardMonitor.KeyUp += args =>
			{
				Dispatcher.Invoke(DispatcherPriority.Input, new KeyEvent(ProcessKeyUp), args);
			};

			PassiveKeyboardMonitor.KeyPressed += args =>
			{
				Dispatcher.Invoke(DispatcherPriority.Input, new KeyEvent(ProcessKeyPress), args);
			};

			PassiveKeyboardMonitor.Start();
		}

		private void ProcessKeyDown(KeyEventArgs keyEventArgs)
		{
			AddItem(string.Format("Key Dw\t{0}", keyEventArgs.Key), Colors.Gray);
		}

		private void ProcessKeyUp(KeyEventArgs keyEventArgs)
		{
			AddItem(string.Format("Key Up\t{0}", keyEventArgs.Key), Colors.Gray);
		}

		private void ProcessKeyPress(KeyEventArgs keyEventArgs)
		{
			AddItem(string.Format("Key Ps\t{0}", keyEventArgs.Key), Colors.Green);

			var text = keyEventArgs.Key.ToChar(PassiveKeyboardMonitor.IsShiftPressed, PassiveKeyboardMonitor.IsAltGrPressed);
			if (text == '\b')
				TextBlock.Text = TextBlock.Text.Remove(TextBlock.Text.Length - 1, 1);
			else if (text != '\0' && text != '\r')
				TextBlock.Text += text;
		}

		private void AddItem(string format, Color green)
		{
			ListView.Items.Add(new DisplayItem(format, green));
			ListView.UpdateLayout();
			ListView.ScrollIntoView(ListView.Items[ListView.Items.Count - 1]);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PassiveKeyboardMonitor.Stop();
		}
	}
}
