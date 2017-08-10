using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWPFBinding
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new MonkeyViewModel();
			this.DataContext = _viewModel;

		}

		public MonkeyViewModel _viewModel;

		private void _addMonkeyRight_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.AddMonkeyToRight();
		}

		private void _addMonkeyLeft_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.AddMonkeyToLeft();
		}

		private void _addMonkeyRight_LostFocus(object sender, RoutedEventArgs e)
		{
			_viewModel.MoveMonkey();
		}

		private void _addMonkeyLeft_LostFocus(object sender, RoutedEventArgs e)
		{
			_viewModel.MoveMonkey();
		}
	}
}
