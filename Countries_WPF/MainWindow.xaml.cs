using Countries_WPF.Models;
using Countries_WPF.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Countries_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributos

        //Declarar
        private List<Country> countries; //Troquei a propriedade por este atributo

        private NetworkService networkService;

        private ApiService apiService;

        private DownloadFlag downloadFlag;  //Coloquei no ecrã principal para a IProgress

        private DialogService dialogService;

        private DataService dataService;

        bool _onLine = true;

        #endregion Atributos



        public MainWindow()
        {
            InitializeComponent();

            //Instanciar
            networkService = new NetworkService();
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            downloadFlag = new DownloadFlag();

            LoadCountries(); //Carregar os Paises
        }


        /// <summary>
        /// Refresh Progress bar
        /// EXERCICO - TIM COREY ASYNC 2 IProgress bar
        /// https://www.youtube.com/watch?v=jLqTWvqI-Pg
        /// </summary>
        private void ProcessingProgress_Flag(int percentage)
        {
            pb_ProgressBar_Flags1.Progress = percentage;  // SynkFusion - ProgressBar Bar
            pb_ProgressBar_Flags2.Progress = percentage;  // SynkFusion - ProgressBar Circle
            lbl_progressBar1.Content = $"{percentage}";
            if (percentage > 48) (lbl_progressBar1).Foreground = Brushes.White;
        }


        /// <summary>
        /// Load Countries from API or SQL Lite
        /// </summary>
        private async void LoadCountries()
        {
            _onLine = false;  // false to try the offline Mode -------------------------------------------------------------------------

            //ProgressBars Local 
            //Receive % de conclusão from Async dataService.GetData() OFFLINE
            var progress = new Progress<int>(value =>
            {
                pb_ProgressBar_Flags1.Progress = value;
                pb_ProgressBar_Flags2.Progress = value;
                lbl_progressBar1.Content = $"{value}";
                if (value > 48)(lbl_progressBar1).Foreground = Brushes.White;

            });


            //Bandeiras Para Offline 
            //Receive bandeiras from Async dataService.GetData() OFFLINE
            var bandeiras = new Progress<string>(value1 =>
            {
                //Show Flag From Properties/From Local
                Country xpto = new Country();

                Uri resorceURI = null;
                string teste1 = value1;
                resorceURI = new Uri(teste1);
                image_flag1.Source = new BitmapImage(resorceURI);
            });


            //ProgressBars API
            var ProgressReport_Flags = new Progress<int>(ProcessingProgress_Flag);
            var stopwatch = new Stopwatch();

            // Check connection to Internet. If you hane Internet -> IsSuccess=true
            var connection = networkService.CheckConnection();

            //Dependendo do estado da conexão
            //Se não há conexão - Carrego os dados Locais
            double time = 0;
            if (!connection.IsSuccess || _onLine == false ) // comentado !
            {
                lbl_message.Content = "Downloading Data From Local DB...";
                ProgressbarVisible(); // ProgressBar Visibility = Visible

                stopwatch.Start();

                // Qual a forma correta de Fazer 1 versão
                countries = await Task.Run ( () => dataService.GetData(progress, bandeiras)); //Exemplo 1 - como deve ser feito???
                time = stopwatch.ElapsedMilliseconds / 1000.0;

                lbl_status.Content = string.Format($"Offline: Data Loaded From SQLite at {DateTime.Now}.");
                lbl_message.Content = $"Ready... Loaded {countries.Count()} Countries in {time} seconds.";
                
                MessageBox.Show($"Download done in {time} seconds.");

                _onLine = false;

                //lbl_progressBar1.Visibility = Visibility.Visible;
                ProgressbarHide(); // ProgressBar Visibility = Hidden

            }
            else // se há conexão Carrego os dados da API
            {
                time = 0;
                ProgressbarVisible();  // ProgressBar Visibility = Visible

                lbl_message.Content = "Downloading API Data...";
                
                stopwatch.Start();

                //Vou ler a API e descarregar para a lista
                await LoadApiCountries();

                //Download Flags
                // Qual a forma correta de Fazer 2 versão
                await DownloadFlags1(ProgressReport_Flags);

                time = stopwatch.ElapsedMilliseconds / 1000.0;               
                
                lbl_status.Content = string.Format($"Load {countries.Count()} Countries from API {DateTime.Now:F}");
                lbl_message.Content = $"All data are Downloaded in {time} seconds.";

                MessageBox.Show($"Download done in {time} seconds.");
                _onLine = true;
            }
            try
            {
                if (countries.Count == 0 || countries == null || countries?.Any() != true) //Se a Lista não foi carregada vinda da API ou Localmente
                {
                    lbl_result.Content = "No Internet Connection" + Environment.NewLine +
                        "and the countries were not previously loaded." + Environment.NewLine +
                        "Try Again Later!";

                    lbl_status.Content = "For the First startup, you must have internet connection";
                    return;
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage("Error downloading flags", ex.Message);
                return;
            }

            ///Order List and Enable comboBox
            countries = countries.OrderBy(x => x.Name).ToList();

            cb_country.ItemsSource = countries;
            cb_country.IsEnabled = true;

            // After de Load: API & Flags, hide the progressbars
            ProgressbarHide();
        }

        /// <summary>
        /// Passar para a Classe: Services:DowloadFlags.cd
        /// Download Flags from API
        /// </summary>
        private async Task DownloadFlags1(IProgress<int> progress = null)
        {
            lbl_message.Content = "Downloading Data From API...";
            int taskResoved = 0;

            //TO DO - Move Download Flags to the DataService Class
            //await downloadFlag.GetFlags(countries);

            await Task.Run(() =>
            {
                try
                {
                    foreach (Country country in countries)
                    {
                        string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                        //cria pasta data se não existir
                        if (!Directory.Exists("Flags"))
                        {
                            Directory.CreateDirectory("Flags");
                        }

                        if (country.Name != null)
                        {
                            baseDir = baseDir + "\\Flags\\" + country.Name + ".png";

                            //Download Flags
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile(country.Flags.png, baseDir);
                            }

                            //Show Flags after Download the Flags
                            this.Dispatcher.Invoke(() =>
                            {
                                //Show Flag From Properties/From Local
                                Uri resorceURI = null;
                                string teste1 = country.Bandeira;
                                resorceURI = new Uri(teste1);
                                image_flag1.Source = new BitmapImage(resorceURI);
                            });

                            //IProgress
                            if (progress != null)
                            {
                                taskResoved++;
                                var percentage = (double)taskResoved / countries.Count;
                                percentage = percentage * 100;
                                var percentageInt = (int)Math.Round(percentage, 0);
                                progress.Report(percentageInt);
                            }
                        }
                    }
            }
                catch (Exception ex)
            {
                dialogService.ShowMessage("Error downloading flags", ex.Message);
            }
        });         
            image_flag1.Source = null;
        }


        /// <summary>
        /// When I Change Country in ComboBox
        /// </summary>
        private void cb_country_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cb_country.SelectedItem == null)
            {
                cb_country.SelectedIndex = 0;
            }
            var country = (Country)cb_country.SelectedItem;

            lbl_flag.Visibility = Visibility.Visible;
            lbl_name.Content = country.Name;
            lbl_capitalName.Content = country.Capital;
            lbl_region.Content = country.Region;
            lbl_subRegion.Content = country.Subregion;

            lbl_population.Content = String.Format("{0:n0}", Convert.ToUInt32(country.Population));

            //lbl_gini.Content = country.Gini + " %";
            lbl_gini.Content = country.Gini == "not available" ? "not available" : country.Gini+" %";

            //Show the Flags From Propertie 
            Uri resorceURI = null;
            string teste1 = country.Bandeira;
            resorceURI = new Uri(teste1);
            image_flag1.Source = new BitmapImage(resorceURI);
        }


        /// <summary>
        /// Async carrega dados da API para a List Countries da classe Country
        /// </summary>
        private async Task LoadApiCountries()
        {
            //Load API
            var response = await apiService.GetCountries("https://restcountries.com", "/v3.1/all");

            countries = (List<Country>)response.Result; //Coloca os dados da API na Lista Country da Classe Country

            //SQLite
            //Se há ligação à net, vou refrescar o SQLite com os dados da API          
            dataService.DeleteData();           // delete rates from SQLite
            dataService.SaveData(countries);    //passa valores da Lista countries da classe Country para o SQLite
       
            lbl_result.Content = "Países Atualizados...";
        }


        /// <summary>
        /// EXIT BUTTON
        /// </summary>
        private void btn_Exit_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Close Aplication", "Do you want close the aplecations?", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Close();
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return;
            }
        }

        /// <summary>
        /// Hidden Progress Bar
        /// </summary>
        void ProgressbarHide() // ProgressBar Visibility = Hidden
        {
            pb_ProgressBar_Flags1.Visibility = Visibility.Hidden; // SynkFusion - ProgressBar
            pb_ProgressBar_Flags2.Visibility = Visibility.Hidden; // SynkFusion - ProgressBar Circle
            lbl_progress.Visibility = Visibility.Hidden;
            lbl_flag.Visibility = Visibility.Hidden;
            lbl_progressBar1.Visibility = Visibility.Hidden;
            loader1.Visibility = Visibility.Hidden;
            image_flag1.Source = null;
        }

        /// <summary>
        /// Visible Progress Bar
        /// </summary>
        void ProgressbarVisible() // ProgressBar Visibility = Visible
        {
            pb_ProgressBar_Flags1.Visibility = Visibility.Visible; // SynkFusion - ProgressBar
            pb_ProgressBar_Flags2.Visibility = Visibility.Visible; // SynkFusion - ProgressBar Circle
            lbl_progress.Visibility = Visibility.Visible;
            lbl_flag.Visibility = Visibility.Visible;
            lbl_progressBar1.Visibility = Visibility.Visible;
            loader1.Visibility = Visibility.Visible;
        }



        ///// <summary>
        ///// INOPERATIONAL -> Offline Button
        ///// </summary>
        private void btn_OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string senderOnOffLine = btn_OnOffline.Content.ToString();

            if (senderOnOffLine == "Go Off-line")
            {
                btn_OnOffline.Content = "Go On-line";
                _onLine = false;
            }
            else if (senderOnOffLine == "Go On-line")
            {
                btn_OnOffline.Content = "Go Off-line";
                _onLine = true;
            }
        }

        /// <summary>
        /// INOPERATIONAL -> Load Data From API or Local DB SQLite
        /// </summary>
        private void btn_GetDataManul_Click(object sender, RoutedEventArgs e)
        {
            LoadCountries(); //Carregar os Paises
        }
    }
}
