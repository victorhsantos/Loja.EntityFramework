﻿namespace Alura.Loja.Testes.ConsoleApp
{
    public class Endereco
    {
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public Cliente Cliente { get; set; }
    }
}