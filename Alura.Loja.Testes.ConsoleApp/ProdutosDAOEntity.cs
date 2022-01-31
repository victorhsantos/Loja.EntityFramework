using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class ProdutosDAOEntity : IProdutoDAO, IDisposable
    {
        private LojaContext contexto;

        public ProdutosDAOEntity()
        {
            this.contexto = new LojaContext();
        }

        public void Adicionar(Produto p)
        {
            contexto.Add(p);
            contexto.SaveChanges();
        }

        public void Atualizar(Produto p)
        {
            contexto.Update(p);
            contexto.SaveChanges();
        }

        public void Dispose()
        {
            contexto.Dispose();
        }

        public IList<Produto> Produtos()
        {
            return contexto.Produtos.ToList();
        }

        public void Remover(Produto p)
        {
            contexto.Remove(p);
            contexto.SaveChanges();
        }
    }
}
