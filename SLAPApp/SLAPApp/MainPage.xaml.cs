using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SLAPApp
{
    public partial class MainPage : ContentPage
    {
        string UserEmail;
        string UserPassword;
        string UserName;

        bool happyFlag = false;
        string NameOfDesk = "";

        int condition = 0;
        Button tmpButton = new Button();
        Dictionary<Frame, Board> desksDict = new Dictionary<Frame, Board>();

        Button tmpButtonName = new Button();
        List<Tasks> tasks = new List<Tasks>();
        public ICommand tapGesture => new Command(tapGestureA);

        /// <summary>
        /// Конструктор класса отвечающий за инициализацию начального окна
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            LoadStartConfig();
            collapseAllGrids();
            MainRegAuthGrid.IsVisible = true;
            tmpButton.BackgroundColor = Color.FromHex("#E35930");
        }
        /// <summary>
        /// Внутренний метод для работы со страницами
        /// </summary>
        public void collapseAllGrids()
        {
            MainPanelGrid.IsVisible = false;
            AddBoardPageGrid.IsVisible = false;
            MainRegAuthGrid.IsVisible = false;
            AddColumnPageGrid.IsVisible = false;
            AddTaskPageGrid.IsVisible = false;
            ProfileGrid.IsVisible = false;
            AddUsersFromBoardPageGrid.IsVisible = false;
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

                        UserEmail = AuthEmailEntry.Text;
                        UserPassword = AuthPasswordEntry.Text;
                        UserName = User.name;

                        userNameLabel.Text = UserName;
                        UserEmailLabel.Text = UserEmail;

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

                            UserEmail = AuthEmailEntry.Text;
                            UserPassword = AuthPasswordEntry.Text;
                            UserName = User.name;

                            userNameLabel.Text = UserName;
                            UserEmailLabel.Text = UserEmail;
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
                        AuthGrid.IsVisible = true;
                        RegGrid.IsVisible = false;
                    }
                }
                else
                {
                    DisplayAlert("Ошибка", "Поля не заполнены", "Ок");
                }
            }
            catch
            {
                DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, попробуйте снова или повторите попытку позже", "Ок");
            }
        }
        /// <summary>
        /// Метод отвечающий за добавление новых досок в меню
        /// </summary>
        /// <param name="backgroudColor">Цвет доски</param>
        /// <param name="description">Название доски</param>
        /// <param name="group">Маркер отвечающий за выбор типа доски(Личные - False, Групповые - True)</param>
        public void AddNewBoardMain(Color backgroudColor, string description, bool group)
        {
            NameOfDesk = description;
            Frame frame = new Frame();
            frame.WidthRequest = 160;
            frame.BackgroundColor = backgroudColor;
            frame.Margin = new Thickness(10);
            frame.CornerRadius = 15;
            frame.Padding = new Thickness(0, -20, 0, 0);
            frame.IsVisible = true;

            Grid grid = new Grid();
            
            Label label = new Label();
            label.TextColor = Color.White;
            label.FontSize = 26;
            label.Text = description;
            label.HorizontalOptions = LayoutOptions.End;
            label.VerticalOptions = LayoutOptions.End;
            label.HorizontalTextAlignment = TextAlignment.End;
            label.Padding = new Thickness(20, 20);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                collapseAllGrids();
                AddColumnPageGrid.IsVisible = true;
                NameOfDeskLabel.Text = NameOfDesk;
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

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
        /// <summary>
        /// Метод отвечающий за добавления новых столбцов в доску
        /// </summary>
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
        /// <summary>
        /// Метод отвечающий за изменение параметров доски
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void saveChangesButton_Clicked(object sender, EventArgs e)
        {
            if(!groupCheckBox.IsChecked)
            {
                AddNewBoardMain(tmpButton.BackgroundColor, deskNameEditor.Text, groupCheckBox.IsChecked);
                collapseAllGrids();
                MainPanelGrid.IsVisible = true;
            }
            else
            {
                collapseAllGrids();
                AddUsersFromBoardPageGrid.IsVisible = true;
            }
        }
        /// <summary>
        /// Метод отвечающий за выбор цвета доски в редакторе досок
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void SelectColorButton_Clicked(object sender, EventArgs e)
        {
            tmpButton.BorderWidth = 0;
            tmpButton = sender as Button;
            tmpButton.BorderColor = Color.FromHex("#142453");
            tmpButton.BorderWidth = 3;
            deskFrame.BackgroundColor = tmpButton.BackgroundColor;
            deskEditorFrame.BackgroundColor = tmpButton.BackgroundColor;
        }
        /// <summary>
        /// Метод отвечающий за создание графической составляющей таска
        /// </summary>
        /// <param name="TaskName"></param>
        /// <returns></returns>
        public Frame AddNewTask(string TaskName = "Task") 
        {
            Frame taskFrame = new Frame();
            taskFrame.HeightRequest = 40;
            taskFrame.HasShadow = true;
            taskFrame.BorderColor = Color.Gray;

            Button TaskButton = new Button();
            TaskButton.TextColor = Color.FromHex("#142453");
            TaskButton.Text = TaskName;
            TaskButton.FontSize = 22;
            TaskButton.HorizontalOptions = LayoutOptions.Start;
            TaskButton.VerticalOptions = LayoutOptions.Center;
            TaskButton.Margin = new Thickness(-20);
            TaskButton.BackgroundColor = Color.Transparent;

            TaskButton.Clicked += (s, e) => {
                tmpButtonName = new Button();
                tmpButtonName = s as Button;
                collapseAllGrids();
                AddTaskPageGrid.IsVisible = true;
            };

            taskFrame.Content = TaskButton;
            return taskFrame;
        }
        /// <summary>
        /// Промежуточный метод отвечающий за добавление новых тасков в столбец
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void addTask3Button_Clicked(object sender, EventArgs e)
        {
            column3StackLayout.Children.Add(AddNewTask());
        }
        /// <summary>
        /// Промежуточный метод отвечающий за добавление новых тасков в столбец
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void addTask1Button_Clicked(object sender, EventArgs e)
        {
            column1StackLayout.Children.Add(AddNewTask());
        }
        /// <summary>
        /// Промежуточный метод отвечающий за добавление новых тасков в столбец
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void addTask2Button_Clicked(object sender, EventArgs e)
        {
            column2StackLayout.Children.Add(AddNewTask());
        }
        /// <summary>
        /// Метод отвечающий за добавление новых столбцов в доску
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void AddColumn_Clicked(object sender, EventArgs e)
        {
            if(Column1.IsVisible)
            {
                Column2.IsVisible = true;
            }
            else if(Column2.IsVisible)
            {
                Column3.IsVisible = true;
            }
        }
        /// <summary>
        /// Метод отвечающий за возвращение к предыдущей странице в стеке
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            AddColumnPageGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за удаление таска
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void DeleteTaskButton_Clicked(object sender, EventArgs e)
        {
            //tmpButtonName.Parent.ClearValue( );
        }
        /// <summary>
        /// Метод отвечающий за сохранение изменений в таске
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void saveTaskChangesButton_Clicked(object sender, EventArgs e)
        {
            tmpButtonName.Text = TaskNameEditor.Text;
            tasks.Add(new Tasks { 
                name = TaskNameEditor.Text, 
                desc = descriptionTask.Text, 
                start_date = TaskDateStartDatePicker.Date.ToShortDateString(), 
                end_date = TaskDateEndDatePicker.Date.ToShortDateString()
            });
        }
        /// <summary>
        /// Метод отвечающий за возвращение к предыдущей странице в стеке
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void ProfileToMainBackButton_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за удаление аккаунта
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private async void DeleteAccountButton_Clicked(object sender, EventArgs e)
        {
            bool flag = await DisplayAlert("Уведомление", "Вы уверены, что хотите удалить аккаунт?", "Да", "Нет");
            if(flag)
            {
                collapseAllGrids();
                MainRegAuthGrid.IsVisible = true;
                StartPageGrid.IsVisible = true;
                AuthGrid.IsVisible = false;
                RegGrid.IsVisible = false;
                MainPanelGrid.IsVisible = false;
            }
        }
        /// <summary>
        /// Метод отвечающий за выход из аккаунта на страницу авторизации
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void exitFromAccountButton_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            MainRegAuthGrid.IsVisible = true;
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
            RegGrid.IsVisible = false;
            MainPanelGrid.IsVisible = false;
        }
        /// <summary>
        /// Метод отвечающий за сохранение изменений аккаунта
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void saveAccountChangesButton_Clicked(object sender, EventArgs e)
        {
            UserName = UserNameEntry.Text;
            userNameLabel.Text = UserName;
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за открытия страницы профиля
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void userDataButton_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            ProfileGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за возвращение к предыдущей странице в стеке
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void ButtonBackToMainAddBoard_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            AddBoardPageGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за добавление новых пользователей в групповую доску
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void addUserInDeskButton_Clicked(object sender, EventArgs e)
        {
            if(!happyFlag)
            {
                Thread.Sleep(2000);
                DisplayAlert("Уведомление", "Пользователь не хочет к вам присоединяться", "Жаль");
                happyFlag = true;
            }
            else
            {
                Thread.Sleep(1500);
                DisplayAlert("Уведомление", "Пользователь успешно добавлен", "Ок");

            }
                
        }
        /// <summary>
        /// Метод отвечающий за завершение процедуры добавления новых пользователей в групповую доску
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void doneAddUsersButton_Clicked(object sender, EventArgs e)
        {
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
            AddColumn_Clicked(sender, e);
            KostilEditor.Text = "Работа";
            column1StackLayout.Children.Add(AddNewTask("Добить проект"));
            column1StackLayout.Children.Add(AddNewTask("Выгореть"));

            AddNewBoardMain(tmpButton.BackgroundColor, deskNameEditor.Text, groupCheckBox.IsChecked);
            collapseAllGrids();
            MainPanelGrid.IsVisible = true;
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
        public List<Tasks> tasks { get; set; }
    }

    public class Root
    {
        public List<Board> boards { get; set; }
    }

    public class Tasks
    {
        public string name { get; set; }
        public string desc { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
