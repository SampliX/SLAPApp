using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SLAPApp
{
    internal class WebClientClass
    {
        private UserClass User = new UserClass();

        internal UserClass getUserData(string email)
        {
            //AsyncGetUserData(email);
            string jsonUserLine = getData("http://194.87.99.112:8080/users?email=" + email);

            User = JsonConvert.DeserializeObject<UserClass>(jsonUserLine);

            return User;
        }

        private void AsyncGetUserData(string email)
        {
            //string jsonUserLine = await Task.Run(() => );

            //User = JsonConvert.DeserializeObject<UserClass>(jsonUserLine);
        }

        private string getData(string url)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.119 YaBrowser/22.3.0.2430 Yowser/2.5 Safari/537.36");
            Stream data = wc.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string line = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return line;
        }

        public void postRegUserData(string email, string password)
        {
            RegUserClass regUserClass = new RegUserClass();
            regUserClass.email = email;
            regUserClass.hashPass = password;
            regUserClass.name = "User" + new Random().Next(100000,1000000).ToString();
            string json = JsonConvert.SerializeObject(regUserClass);
            postData(json);
        }

        private void postData(string json, string url = "http://194.87.99.112:8080/users")
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = wc.UploadString(url, json);
        }
    }

    internal class UserClass
    {
        public int user_id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string hash_pass { get; set; }
    }

    internal class RegUserClass
    {
        public string email { get; set; }
        public string name { get; set; }
        public string hashPass { get; set; }
    }
}
