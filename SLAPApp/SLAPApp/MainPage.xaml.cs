using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]

namespace SLAPApp
{
    public partial class MainPage : ContentPage
    {
        int condition = 0;
        Button tmpButton = new Button();
        Dictionary<Frame, Board> desksDict = new Dictionary<Frame, Board>();
        public ICommand tapGesture => new Command(tapGestureA);

        /// <summary>
        /// Конструктор класса отвечающий за инициализацию начального окна
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            LoadStartConfig();
            collapseAllGrids();
            AddColumnPageGrid.IsVisible = true;
            tmpButton.BackgroundColor = Color.FromHex("#E35930");
        }

        public void collapseAllGrids()
        {
            MainPanelGrid.IsVisible = false;
            AddBoardPageGrid.IsVisible = false;
            MainRegAuthGrid.IsVisible = false;
            AddColumnPageGrid.IsVisible = false;
        }

        /// <summary>
        /// Внутренний метод для обработки события нажатия на desc
        /// </summary>
        public void tapGestureA()
        {
            DisplayAlert("Уведомление", "Пришло новое сообщение", "ОK");
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки добавить доску
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            condition = 1;
            collapseAllGrids();
            AddBoardPageGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки домой
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonHome_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки открыть календарь
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonCalendar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CalendarPage());
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки регистрации
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void StartRegistrationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            RegGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки начать авторизацию
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void StartAutentificationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки вернуться в главное меню
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void ButtonBackToMain_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за загрузку стартовой конфигурации приложения
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void LoadStartConfig()
        {
            object email;
            object password;

            StartPageGrid.IsVisible = true;
            AuthGrid.IsVisible = false;
            RegGrid.IsVisible = false;
            MainPanelGrid.IsVisible = false;

            if (App.Current.Properties.TryGetValue("email", out email) &&
               App.Current.Properties.TryGetValue("password", out password))
            {
                WebClientClass clientClass = new WebClientClass();
                UserClass User;

                try
                {
                    User = clientClass.GetUserData(email.ToString());
                    if (User.hash_pass == CreateMD5(password.ToString()))
                    {
                        collapseAllGrids();
                        MainPanelGrid.IsVisible = true;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, повторите попытку снова", "Ок");
                }
            }
            else if (App.Current.Properties.TryGetValue("email", out email))
            {
                StartPageGrid.IsVisible = false;
                AuthGrid.IsVisible = true;
                RegGrid.IsVisible = false;
            }
            else
            {
                StartPageGrid.IsVisible = true;
                AuthGrid.IsVisible = false;
                RegGrid.IsVisible = false;
            }
        }
        /// <summary>
        /// Метод отвечающий за хеширование строки алгоритмом MD5
        /// </summary>
        /// <param name="input">Строка, которую необходимо преобразовать</param>
        /// <returns>Возвращает захешированную строку</returns>
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки авторизоваться
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void LoginAccountButton_Clicked(object sender, EventArgs e)
        {
            object tmp;
            if (AuthEmailEntry.Text != null && AuthPasswordEntry.Text != null)
            {
                WebClientClass clientClass = new WebClientClass();
                UserClass User;

                try
                {
                    User = clientClass.GetUserData(AuthEmailEntry.Text);
                    if (User.hash_pass == CreateMD5(AuthPasswordEntry.Text))
                    {
                        if(SwitchKeepLoggedMe.IsToggled == true)
                        {
                            if (App.Current.Properties.TryGetValue("email", out tmp))
                                App.Current.Properties["email"] = AuthEmailEntry.Text;
                            else
                                App.Current.Properties.Add("email", AuthEmailEntry.Text);

                            if (App.Current.Properties.TryGetValue("password", out tmp))
                                App.Current.Properties["password"] = AuthPasswordEntry.Text;
                            else
                                App.Current.Properties.Add("password", AuthPasswordEntry.Text);
                        }
                        else if(SwitchKeepLoggedMe.IsToggled == false)
                        {
                            if (App.Current.Properties.TryGetValue("email", out tmp))
                                App.Current.Properties["email"] = AuthEmailEntry.Text;
                            else
                                App.Current.Properties.Add("email", AuthEmailEntry.Text);
                        }
                        collapseAllGrids();
                        MainPanelGrid.IsVisible = true;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, повторите попытку снова", "Ок");
                }
            }
            else
            {
                DisplayAlert("Ошибка", "Не все поля заполнены", "Ок");
            }
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки забыл пароль
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ForgotPassowordButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Ошибка", "Не реализованно", "Ок");
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки перейти в меню авторизации
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void EnterAccountButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
            RegGrid.IsVisible = false;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки зарегистрировать пользователя
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void RegisterAccountButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (RegEmailEntry.Text != null && RegPasswordEntry.Text != null && RegPasswordCopyEntry.Text != null)
                {
                    if (RegPasswordEntry.Text == RegPasswordCopyEntry.Text)
                    {
                        WebClientClass webClient = new WebClientClass();
                        webClient.postRegUserData(RegEmailEntry.Text, CreateMD5(RegPasswordEntry.Text));
                    }
                }
                else
                {
                    DisplayAlert("Ошибка", "Поля не заполнены", "Ок");
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("Ошибка", ex.ToString(), "Ок");
            }
        }

        public void AddNewBoardMain(Color backgroudColor, string description, bool group)
        {
            Frame frame = new Frame();
            frame.WidthRequest = 160;
            frame.BackgroundColor = backgroudColor;
            frame.Margin = new Thickness(10);
            frame.CornerRadius = 15;
            frame.Padding = new Thickness(0, -20, 0, 0);
            frame.IsVisible = true;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                collapseAllGrids();
                AddColumnPageGrid.IsVisible = true;
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            Grid grid = new Grid();
            
            Label label = new Label();
            label.TextColor = Color.White;
            label.FontSize = 26;
            label.Text = description;
            label.HorizontalOptions = LayoutOptions.End;
            label.VerticalOptions = LayoutOptions.End;
            label.HorizontalTextAlignment = TextAlignment.End;
            label.Padding = new Thickness(20, 20);

            grid.Children.Add(label);
            frame.Content = grid;

            desksDict.Add(frame, new Board());

            if (condition == 1)//Создать доску
            {
                if (!group)
                    PersonalDescs.Children.Add(frame);
                else
                    GroupDescs.Children.Add(frame);

                AddNewColumns();

            }
            else if (condition == 2)//Изменить доску
            {

            }
            condition = 0;
        }

        public void AddNewColumns()
        {
            Frame frame = new Frame();
            frame.BackgroundColor = Color.White;
            frame.CornerRadius = 10;
            frame.HorizontalOptions = LayoutOptions.Center;
            frame.VerticalOptions = LayoutOptions.Center;
            frame.HasShadow = true;
            frame.WidthRequest = 290;
            frame.BorderColor = Color.Gray;

            StackLayout stackLayout = new StackLayout();
            StackLayout stackLayout1 = new StackLayout();

            Frame iternalFrame = new Frame();
            iternalFrame.BackgroundColor = Color.FromHex("#E35930");
            iternalFrame.HeightRequest = 40;
            iternalFrame.Margin = new Thickness(-20);

            Label iternalLabel = new Label();
            iternalLabel.TextColor = Color.White;
            iternalLabel.Text = "Column";
            iternalLabel.FontSize = 22;
            iternalLabel.HorizontalOptions = LayoutOptions.Start;
            iternalLabel.VerticalOptions = LayoutOptions.Center;

            iternalFrame.Content = iternalLabel;

            Grid iternalGrid = new Grid();
            iternalGrid.HeightRequest = 20;

            ScrollView iternalScrollView = new ScrollView();
            iternalScrollView.Orientation = ScrollOrientation.Vertical;

            StackLayout iternalStackLayout = new StackLayout();
            iternalStackLayout.ClassId = "TasksStackLayout";

            Frame taskFrame = new Frame();
            taskFrame.HeightRequest = 40;
            taskFrame.HasShadow = true;
            taskFrame.BorderColor = Color.Gray;

            Button TaskButton = new Button();
            TaskButton.TextColor = Color.FromHex("#142453");
            TaskButton.FontSize = 22;
            TaskButton.HorizontalOptions = LayoutOptions.Start;
            TaskButton.VerticalOptions = LayoutOptions.Center;
            TaskButton.Margin = new Thickness(-20);
            TaskButton.BackgroundColor = Color.Transparent;

            taskFrame.Content = TaskButton;

            iternalStackLayout.Children.Add(taskFrame);
            iternalScrollView.Content = iternalStackLayout;
            stackLayout1.Children.Add(iternalFrame);
            stackLayout1.Children.Add(iternalGrid);
            stackLayout1.Children.Add(iternalScrollView);

            Button AddButton = new Button();
            AddButton.Text = "+";
            AddButton.TextColor = Color.FromHex("#142453");
            AddButton.HorizontalOptions = LayoutOptions.Center;
            AddButton.FontSize = 44;
            AddButton.FontFamily = "Roboto";
            AddButton.BackgroundColor = Color.Transparent;
            AddButton.Clicked += (s, e) => {
                iternalStackLayout.Children.Add(taskFrame);
            };

            stackLayout.Children.Add(stackLayout1);
            stackLayout.Children.Add(AddButton);

            frame.Content = stackLayout;

            columnsStackLayout.Children.Add(frame);
            Grid tmpGrid = new Grid();
            tmpGrid.WidthRequest = 40;
            columnsStackLayout.Children.Add(tmpGrid);
        }

        private void saveChangesButton_Clicked(object sender, EventArgs e)
        {
            AddNewBoardMain(tmpButton.BackgroundColor, deskNameEditor.Text, groupCheckBox.IsChecked);
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
        }
        private void SelectColorButton_Clicked(object sender, EventArgs e)
        {
            tmpButton.BorderWidth = 0;
            tmpButton = sender as Button;
            tmpButton.BorderColor = Color.FromHex("#142453");
            tmpButton.BorderWidth = 3;
            deskFrame.BackgroundColor = tmpButton.BackgroundColor;
            deskEditorFrame.BackgroundColor = tmpButton.BackgroundColor;
        }
    }

    public class Board
    {
        public int board_id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string isGroup { get; set; }
        public string role { get; set; }
        public List<Column> columns { get; set; }
    }

    public class Column
    {
        public int column_id { get; set; }
        public string name { get; set; }
        public Tasks tasks { get; set; }
    }

    public class Root
    {
        public List<Board> boards { get; set; }
    }

    public class Tasks
    {
        public int task_id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
