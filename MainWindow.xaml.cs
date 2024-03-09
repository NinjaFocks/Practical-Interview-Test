using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Rdessoy_MCMS_Practical_Interview_Test.Data;

namespace Rdessoy_MCMS_Practical_Interview_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IDataSource _source;

        public IList<HashModel> _models;
        private bool _windowLoaded = false;

        public MainWindow(IDataSource source)
        {
            _source = source;

            InitializeComponent();

            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Search.Text);
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await InitialiseGrid();

            _windowLoaded = true;
        }

        public async Task InitialiseGrid()
        {
            DataGrid.ItemsSource = await _source.GetHashModelsAsync();

            var count = await _source.GetPageCountAsync();
            PageTotalCount.Content = $"of {count}";
        }

        private async void TextBoxUpdated(object sender, TextChangedEventArgs e)
        {
            if (!_windowLoaded) return;

            TextBox textBox = sender as TextBox;

            var convertSuccess = int.TryParse(textBox.Text, out int pageNumber);

            if (!convertSuccess)
            {
                MessageBox.Show($"{textBox.Text} is not a valid pageNumber.");                
            }
            else
            {
                DataGrid.ItemsSource = await _source.GetHashModelsAsync(pageNumber: pageNumber);
            }
        }

        private void Next_Clicked(object sender, RoutedEventArgs e)
        {
            var convertSuccess = int.TryParse(PageNumberTextBox.Text, out int pageNumber);

            if (!convertSuccess)
            {
                MessageBox.Show("Issue with page number, resetting to first page.");
                PageNumberTextBox.Text = "1";
            }
            else
            {
                var newPageNumber = pageNumber + 1;
                PageNumberTextBox.Text = newPageNumber.ToString();
            }            
        }

        private void Prev_Clicked(object sender, RoutedEventArgs e)
        {
            var convertSuccess = int.TryParse(PageNumberTextBox.Text, out int pageNumber);

            if (!convertSuccess)
            {
                MessageBox.Show("Issue with page number, resetting to first page.");
                PageNumberTextBox.Text = "1";
            }
            else if (pageNumber <= 1)
            {
                MessageBox.Show("Page number must be greater than or equal to 1.");
                PageNumberTextBox.Text = "1";
            }
            else
            {
                var newPageNumber = pageNumber - 1;
                PageNumberTextBox.Text = newPageNumber.ToString();
            }
        }
    }
}
