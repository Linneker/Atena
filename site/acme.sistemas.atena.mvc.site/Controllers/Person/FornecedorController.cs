using acme.atena.application.Handler.Comum.Commands.Response;
using acme.atena.application.Handler.Fornecedores.Commands.Request;
using acme.atena.application.Handler.Fornecedores.Commands.Response;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Service.Person;
using acme.sistemas.atena.mvc.site.Filtler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace acme.sistemas.atena.mvc.site.Controllers.Person
{
    public class FornecedorController : Controller
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedorController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        public IActionResult Index()
        {
            List<FornecedorQuery> comando = new List<FornecedorQuery>();

            var todosFornecedores = _fornecedorService.GetAll();
            todosFornecedores.ForEach(t =>
            {
                var novoFornecedor = new FornecedorQuery()
                {
                    DataCriacao = t.DataCriacao,
                    Id = t.Id,
                    Celular = t.Celular,
                    Email = t.Email,
                    Nome = t.Nome,
                    CpfCnpj = t.CpfCnpj,
                    InscricaoMunicipal = t.InscricaoMunicipal,
                    NomeFantasia = t.NomeFantasia,
                    DataNascimento = t.DataNascimento
                };
                comando.Add(novoFornecedor);
            });

            return View(comando);
        }

        [HttpGet]
        public async Task<IActionResult> Editar()
        {

            return await Task.FromResult(View());
        }


        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            return await Task.FromResult(View());
        }

        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Criar(FornecedorCommand fornecedorCommand)
        {
            Fornecedor fornecedor = new Fornecedor()
            {
                Celular = fornecedorCommand.Celular,
                CpfCnpj = fornecedorCommand.CpfCnpj,
                Email = fornecedorCommand.Email,
                InscricaoMunicipal = fornecedorCommand.InscricaoMunicipal,
                Nome = fornecedorCommand.Nome,
                NomeFantasia = fornecedorCommand.NomeFantasia,
                DataNascimento = fornecedorCommand.DataNascimento
            };

            await _fornecedorService.AddAsync(fornecedor);

            return RedirectToAction(nameof(Index));
        }
    }
}
