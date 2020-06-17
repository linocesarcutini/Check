using System;

namespace check
{
    public class Usuario
    {
        string _nome;
        string _usuario;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string User
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        string _macAddress { get; set; }

        public string MacAddress
        {
            get { return _macAddress; }

            set { _macAddress = value; }
        }

        string _ativacao { get; set; }

        public string Ativacao
        {
            get { return _ativacao; }

            set { _ativacao = value; }
        }
    }
}