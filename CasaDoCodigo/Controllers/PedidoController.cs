using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private IConfiguration _configuration;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IConfiguration configuration, IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository)
        {
            // Acessamos o serviço de configuração que foi configurado na classe Startup
            // via injeção de dependência
            _configuration = configuration;
            _produtoRepository = produtoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(_produtoRepository.GetProdutos());
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                // Se o código for preenchido vamos adicioanr um item ao nosso pedido
                _pedidoRepository.AddItem(codigo);
            }

            Pedido p = _pedidoRepository.GetPedido();
            return View(p.Itens);
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Resumo()
        {
            Pedido pedido = _pedidoRepository.GetPedido();
            return View(pedido);
        }

        [HttpPost]
        public IActionResult UpdateQuantidade([FromBody]UpdateQuantidadeRequest updateQuantidadeRequest) {
            return Ok();
        }

        /**
         * Método criado para testar o carregamento de seções do arquivo appsettings
         * Nele mostramos como os itens podem ser carregados de diversas formas
        */
        public IActionResult TesteConfigurationSection()
        {
            // Configuração usando a string de chave
            var keyString = _configuration["TestSection:Test"];

            // Configuração usando método GetSection

            var getSectionOnly =
                _configuration.GetSection("TestSection").GetSection("Test").Value;

            // Configuração usando GetSection e string de chave ao mesmo tempo
           var keyStringAndGetSection =
                _configuration.GetSection("TestSection")["Test"];

            return Ok(new { keyString, getSectionOnly, keyStringAndGetSection });
        }
    }
}
