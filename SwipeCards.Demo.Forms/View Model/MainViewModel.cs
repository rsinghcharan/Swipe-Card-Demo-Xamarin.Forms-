using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;
using SwipeCards.Demo.Forms.Models;

namespace SwipeCards.Demo.Forms
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Properties
        private JsonSerializer _serializer = new JsonSerializer();
        public ICommand SelectCardCommand { get; private set; }
        ObservableCollection<SwipeCard> tempCards;

        ObservableCollection<SwipeCard> swipeCards;

        public ObservableCollection<SwipeCard> SwipeCards
        {
            get { return swipeCards; }
            set { swipeCards = value; RaisePropertyChanged(); }
        }
        private bool _isActivityVisible;
        public bool IsActivityVisible
        {
            get { return _isActivityVisible; }
            set { _isActivityVisible = value; RaisePropertyChanged(); }
        }
        private bool _isButtonVisible;
        public bool IsButtonVisible
        {
            get { return _isButtonVisible; }
            set { _isButtonVisible = value; RaisePropertyChanged(); }
        }
        #endregion


        #region Constructor
        public MainViewModel()
        {
            //Initialize Card Selection Command
            SelectCardCommand = new Command<object>(GetCardInfoCommand);
            //Initialize swipe card object
            SwipeCards = new ObservableCollection<SwipeCard>();
            //Taking temp date to store all values and then after assigned it's value to main Item Source object
            tempCards = new ObservableCollection<SwipeCard>();

            //Get Swipe Card Data Via REST API Call
            GetSwipeCardData().GetAwaiter();

        }
        #endregion

        #region Private Methods
        private void GetCardInfoCommand(object selectedItem)
        {
            var item = selectedItem as SwipeCard;
            Application.Current.MainPage.DisplayAlert("Swipe Card Demo", "You have selected " + item.name + "'s card and please contact him via email at " + item.email, "OK");
        }
        private async Task GetSwipeCardData()
        {
            IsActivityVisible = true;
            using (var c = new HttpClient())
            {
                var client = new HttpClient();
                var response = await client.GetAsync(string.Format("https://randomuser.me/api/?results=10"));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new StreamReader(stream))
                        using (var json = new JsonTextReader(reader))
                        {
                            var result = _serializer.Deserialize<Cards>(json);
                            if (result.results.Count > 0)
                            {
                                foreach (var item in result.results)
                                {
                                    tempCards.Add(new SwipeCard()
                                    {
                                        name = (item.name.title + " " + item.name.first + " " + item.name.last).ToUpper(),
                                        email = item.email,
                                        url = item.picture.medium
                                    });
                                }
                                SwipeCards = tempCards;
                                //Hide the Activity Indicator once we got API Response
                                IsActivityVisible = false;
                                //Show Reset button once we got API Response
                                IsButtonVisible = true;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }

        }
        #endregion

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

}
