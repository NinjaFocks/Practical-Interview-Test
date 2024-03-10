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
            MessageBox.Show(SearchInput.Text);
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await InitialiseGrid();

            _windowLoaded = true;
        }

        public async Task InitialiseGrid()
        {
            var hashResult = await _source.GetHashModelsAsync();
            DataGrid.ItemsSource = hashResult.Value;
            PageTotalCount.Content = $"of {hashResult.Key}";
        }

        private async void PageNumberUpdated(object sender, TextChangedEventArgs e)
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
                var hashResult = await _source.GetHashModelsAsync(pageNumber: pageNumber);
                DataGrid.ItemsSource = hashResult.Value;
            }
        }

        private void Next_Clicked(object sender, RoutedEventArgs e)
        {
            var convertSuccess = int.TryParse(PageNumberInput.Text, out int pageNumber);

            if (!convertSuccess)
            {
                MessageBox.Show("Issue with page number, resetting to first page.");
                PageNumberInput.Text = "1";
            }
            else
            {
                var newPageNumber = pageNumber + 1;
                PageNumberInput.Text = newPageNumber.ToString();
            }            
        }

        private void Prev_Clicked(object sender, RoutedEventArgs e)
        {
            var convertSuccess = int.TryParse(PageNumberInput.Text, out int pageNumber);

            if (!convertSuccess)
            {
                MessageBox.Show("Issue with page number, resetting to first page.");
                PageNumberInput.Text = "1";
            }
            else if (pageNumber <= 1)
            {
                MessageBox.Show("Page number must be greater than or equal to 1.");
                PageNumberInput.Text = "1";
            }
            else
            {
                var newPageNumber = pageNumber - 1;
                PageNumberInput.Text = newPageNumber.ToString();
            }   
        }

        private async void SearchChanged(object sender, TextChangedEventArgs e)
        {
            var hashResult = await _source.GetHashModelsAsync(search: SearchInput.Text, pageNumber: 1);
            DataGrid.ItemsSource = hashResult.Value;
            PageNumberInput.Text = 1.ToString();
            PageTotalCount.Content = $"of {hashResult.Key}";

            if (SearchInput.Text.Length == 40 && !SearchInput.Text.Contains(",") && hashResult.Value.Count == 1)
            {
                MessageBox.Show($"The hash {SearchInput.Text} currently exists in the database.");
            }
        }
    }
}
