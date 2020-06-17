using System;

namespace check
{
    public class User
    {
        public User()
        {
        }

        string _name { get; set; }

        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }

        string _username { get; set; }

        public string UserName
        {
            get { return _username; }

            set { _username = value; }
        }

        string _serialAutocad { get; set; }

        public string SerialAutocad
        {
            get { return _serialAutocad; }

            set { _serialAutocad = value; }
        }

        string _userIp { get; set; }

        public string IPAddress
        {
            get { return _userIp; }

            set { _userIp = value; }
        }

        DateTime _date { get; set; }

        public DateTime Date
        {
            get { return _date; }

            set { _date = value; }
        }

        string _ativacao { get; set; } = "0";

        public string Ativacao
        {
            get { return _ativacao; }

            set { _ativacao = value; }
        }

    }
}
