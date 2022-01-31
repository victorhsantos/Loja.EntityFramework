using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //var cliente = contexto
                //    .Clientes
                //    .Include(c => c.EnderecoDeEntrega)
                //    .FirstOrDefault();

                //Console.WriteLine($"Endereco de entrega: {cliente.EnderecoDeEntrega.Logradouro}");

                var produto = contexto
                    .Produtos
                    .Where(p => p.Id == 3002)
                    .FirstOrDefault();

                contexto.Entry(produto)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 10)
                    .Load();

                Console.WriteLine($"Mostrando as compras do Produto: {produto.Nome}");

                foreach (var item in produto.Compras)
                {
                    Console.WriteLine(item);
                }

            }
        }

        private static void IncluirCompra()
        {
            using (var contexto = new LojaContext())
            {
                //var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                //var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                //loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var p1 = new Produto() { Categoria = "Alimentos", Nome = "Pao", PrecoUnitario = 1.19, Unidade = "Unidade" };
                var compra = new Compra()
                {
                    Produto = contexto.Produtos.Where(p => p.Id == 3002).Single(),
                    Quantidade = 1,
                    Preco = 1 * p1.PrecoUnitario
                };

                contexto.Compras.Add(compra);
                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();

                //var cliente = contexto
                //    .Clientes
                //    .Include(c => c.EnderecoDeEntrega)
                //    .FirstOrDefault();

                //Console.WriteLine($"Endereco de entrega: {cliente.EnderecoDeEntrega.Logradouro}");


                //var produto = contexto
                //    .Produtos
                //    .Where(p => p.Id == )
                //    .FirstOrDefault();
            }
        }

        private static void ExibeProdutosDaPromocao()
        {
            using (var contexto2 = new LojaContext())
            {
                var promocao = contexto2
                    .Promocoes
                    .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                    .FirstOrDefault();

                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());


                var promocao = new Promocao()
                {
                    Descricao = "Queima Janeiro",
                    DataInicio = new DateTime(2021, 1, 1),
                    DataTermino = new DateTime(2021, 1, 30)
                };

                var produtos = contexto
                    .Produtos
                    .Where(p => p.Categoria == "Chocolates")
                    .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluirProduto(item);
                }

                contexto.Promocoes.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();
            }
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }

        private static void MuitoParaMuitos()
        {
            var p1 = new Produto() { Nome = "Laka", Categoria = "Chocolates", PrecoUnitario = 5.69, Unidade = "Gramas" };
            var p2 = new Produto() { Nome = "Diamante Negro", Categoria = "Chocolates", PrecoUnitario = 4.76, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Ouro Branco", Categoria = "Chocolates", PrecoUnitario = 1.99, Unidade = "Gramas" };

            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Páscoa Feliz";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);


            promocaoDePascoa.IncluirProduto(p1);
            promocaoDePascoa.IncluirProduto(p2);
            promocaoDePascoa.IncluirProduto(p3);

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());


                //contexto.Promocoes.Add(promocaoDePascoa);
                //ExibeEntries(contexto.ChangeTracker.Entries());

                var promocao = contexto.Promocoes.Find(1);
                contexto.Promocoes.Remove(promocao);
                //ExibeEntries(contexto.ChangeTracker.Entries());


                contexto.SaveChanges();

            }
        }

        private static void UmParaUm()
        {
            var fulano = new Cliente();
            fulano.Nome = "Victor Santos";
            fulano.EnderecoDeEntrega = new Endereco()
            {
                Logradouro = "Rua Olhos Dagua",
                Numero = 310,
                Bairro = "Araguaia",
                Cidade = "Belo Horizonte",
                Complemento = "Casa"
            };

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Clientes.Add(fulano);

                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();


            }
        }
    }
}
